using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace wp1czerwca
{
    public enum GameMode{
        Player, Computer
    }

    enum GameAction{
        Choose, MoveOrKill
    }

    enum Moves{
        Up, Down, Left, Right
    }

    public partial class Game
    {
        public Cell[,] Board;

        private readonly MainWindow mwContext;

        public static Settings StaticSettings { get; set; }

        public Settings Settings
        {
            get { return StaticSettings; }
            set { StaticSettings = value; }

        }

        public List<Cell> Tigers { get; private set; }
        public List<Cell> Bulls { get; private set; }
        public Animal Round { get; private set; }
        public Cell Choosen { get; private set; }
        public int MoveCounter { get; set; }

        public Game(MainWindow mw, Settings settings)
        {
            Board = new Cell[5, 4];
            Tigers = new List<Cell>();
            Bulls = new List<Cell>();

            mwContext = mw;
            Settings = settings;

            Round = Animal.Tiger;
        }

        public void InitGame()
        {
            mwContext.GameGrid.Children.Clear();
            Tigers.Clear();
            Bulls.Clear();
            for (int i = 0; i < 2; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    Board[i, j] = new Cell(this, i, j, Animal.Bull);
                    Bulls.Add(Board[i, j]);
                }

            for (int i = 1; i < 3; ++i)
            {
                Board[4, i] = new Cell(this, 4, i, Animal.Tiger);
                Tigers.Add(Board[4, i]);
            }
            for (int i = 2; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                    Board[i, j] = new Cell(this, i, j);
            Board[4, 0] = new Cell(this, 4, 0);
            Board[4, 3] = new Cell(this, 4, 3);

            MoveCounter = 0;
            mwContext.DataContext = null;
            mwContext.DataContext = this;
            mwContext.ShowBoard();
        }

        //private void SetNextAction()
        //{
        //    if (Choosen == null)
        //        nextAction = GameAction.Choose;
        //    else
        //        nextAction = GameAction.MoveOrKill;
        //}

        public void PerformAction(Cell cell)
        {
            if (Choosen == null || Choosen.Type == cell.Type)
            {
                if (Rules.CanChoose(this, cell))
                {
                    if (Choosen != null)
                        ErasePossibleMoves();
                    Choosen = cell;
                    LightPossibleMoves();
                }
            }

            else if (Rules.CanMove(this, cell))
            {
                MoveChoosenCell(cell);
                Choosen = null;
                if (Rules.TigersCanMove(this) == false)
                    GameEnd(Animal.Bull);
            }
            else if (Rules.CanKill(this, cell,Choosen))
            {
                KillCell(cell);
                Choosen = null;
                if (Bulls.Count == 2)
                    GameEnd(Animal.Tiger);
            }
        }

        private void LightPossibleMoves()
        {
            Cell cell;
            foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
            {
                if (Rules.MoveIsPossible(this, move, out cell,Choosen))
                {
                    Rectangle rect = new Rectangle
                    {
                        Width = Cell.CellSize.Width,
                        Height = Cell.CellSize.Height,
                        Visibility = Visibility.Visible,
                        Fill = Brushes.GreenYellow
                    };
                    Canvas.SetLeft(rect, Cell.CellSize.Width*cell.Column);
                    Canvas.SetTop(rect,Cell.CellSize.Height*cell.Row);
                    mwContext.Canvas.Children.Add(rect);

                    switch (move)
                    {
                        case Moves.Down:
                            if (cell.Row != 4 && Rules.CanKill(this, Board[cell.Row + 1, cell.Column],Choosen))
                                LightPossibleKill(Board[cell.Row + 1, cell.Column]);
                            break;
                        case Moves.Up:
                            if (cell.Row != 0 && Rules.CanKill(this, Board[cell.Row - 1, cell.Column],Choosen))
                                LightPossibleKill(Board[cell.Row - 1, cell.Column]);
                            break;
                        case Moves.Left:
                            if (cell.Column != 0 && Rules.CanKill(this, Board[cell.Row, cell.Column - 1],Choosen))
                                LightPossibleKill(Board[cell.Row, cell.Column - 1]);
                            break;
                        case Moves.Right:
                            if (cell.Column != 3 && Rules.CanKill(this, Board[cell.Row, cell.Column + 1],Choosen))
                                LightPossibleKill(Board[cell.Row, cell.Column + 1]);
                            break;
                    }
                }
            }
        }

        private void LightPossibleKill(Cell cell)
        {
            Rectangle rect = new Rectangle
            {
                Width = Cell.CellSize.Width,
                Height = Cell.CellSize.Height,
                Visibility = Visibility.Visible,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(rect, Cell.CellSize.Width * cell.Column);
            Canvas.SetTop(rect, Cell.CellSize.Height * cell.Row);
            mwContext.Canvas.Children.Add(rect);
        }

        private void MoveChoosenCell(Cell cell)
        {
            int tempRow = Choosen.Row;
            int tempColumn = Choosen.Column;
            Choosen.Row = cell.Row;
            Choosen.Column = cell.Column;
            cell.Row = tempRow;
            cell.Column = tempColumn;
            Grid.SetRow(Choosen.image, Choosen.Row);
            Grid.SetColumn(Choosen.image, Choosen.Column);
            Grid.SetRow(cell.image, cell.Row);
            Grid.SetColumn(cell.image, cell.Column);

            Board[cell.Row, cell.Column] = cell;
            Board[Choosen.Row, Choosen.Column] = Choosen;

            ErasePossibleMoves();

            if (Choosen.Type == Animal.Tiger)
                MoveCounter++;

            if (Bulls.Count == 2)
            {
                mwContext.DataContext = null;
                mwContext.DataContext = this;
                return;
            }
            Round = Choosen.Type == Animal.Bull ? Animal.Tiger : Animal.Bull;

            if (Settings.Mode == GameMode.Computer && Round == Animal.Bull)
                //mwContext.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(MakeComputerMove));
                MakeComputerMove();
            mwContext.DataContext = null;
            mwContext.DataContext = this;
            Choosen = null;
        }

        public void MakeComputerMove()
        {
            bool?[,] safetyBoard = AnalyseBoard();

            if (TryMoveUnsafeBullintoSafe(safetyBoard) || TryMoveSafeBullIntoSafe(safetyBoard) ||
                TryMoveUnsafeBullintoAny(safetyBoard) || TryMoveAnyBullIntoAny())
                return;

            //Cell cell;
            //Random random = new Random();
            //List<int> bullsInt = new List<int>();
            //List<int> movesInt = new List<int>();

            //for(int i = 0; i < Bulls.Count; ++i)
            //    bullsInt.Add(i);
            //while (bullsInt.Count > 0)
            //{
            //    for (int i = 0; i < 4; ++i)
            //        movesInt.Add(i);
            //    int ind = random.Next(Bulls.Count - 1);
            //    Choosen = Bulls[ind];
            //    bullsInt.Remove(ind);
            //    while (movesInt.Count > 0)
            //    {
            //        ind = movesInt[random.Next(movesInt.Count - 1)];
            //        movesInt.Remove(ind);
            //        if (Rules.MoveIsPossible(this, Enum.GetValues(typeof(Moves)).Cast<Moves>().ElementAt(ind), out cell,Choosen))
            //        {
            //            MoveChoosenCell(cell);
            //            return;
            //        }
            //    }
            //}
        }

        private void ErasePossibleMoves()
        {
            for(int i = 0; i < mwContext.Canvas.Children.Count; ++i)
            {
                if( mwContext.Canvas.Children[i] is Rectangle)
                    mwContext.Canvas.Children.Remove(mwContext.Canvas.Children[i--]);

            }
        }

        private void KillCell(Cell cell)
        {
            Bulls.Remove(cell);
            cell.Erase();
            MoveChoosenCell(cell);
        }

        private void GameEnd(Animal winner)
        {
            switch (winner)
            {
                case Animal.Tiger:
                    MessageBox.Show("Tigers won!");
                    break;
                case Animal.Bull:
                    MessageBox.Show("Bulls won!");
                    break;
            }
            InitGame();
        }

        public void SetMode(GameMode mode)
        {
            Settings.Mode = mode;
            if(Round == Animal.Bull && Settings.Mode == GameMode.Computer)
                MakeComputerMove();
        }

        public void SetAnimalImages()
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = Settings.Bull;
            bmp.EndInit();
            foreach (var bull in Bulls)
            {
                bull.image.Child = new Image
                {
                    Source = bmp,
                    Stretch = Stretch.Fill
                };
            }
            bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = Settings.Tiger;
            bmp.EndInit();
            foreach (var tiger in Tigers)
            {
                tiger.image.Child = new Image
                {
                    Source = bmp,
                    Stretch = Stretch.Fill
                };
            }
        }
    }
}