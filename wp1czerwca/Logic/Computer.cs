using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace wp1czerwca
{
     public enum Difficulty
    {
        Easy, Hard
    }
    public partial class Game
    {
        public const Difficulty DifficultyLevel = Difficulty.Hard;

        public void MakeComputerMove(Difficulty level)
        {
            switch (level)
            {
                case Difficulty.Easy:
                    RandomMove();
                    break;
                case Difficulty.Hard:
                    bool?[,] safetyBoard = AnalyseBoard();

                    if (!(TryMoveUnsafeBullintoSafeBlocking(safetyBoard) || TryMoveUnsafeBullintoSafe(safetyBoard) ||
                          TryMoveSafeBullIntoSafeBlocking(safetyBoard) || TryMoveSafeBullIntoSafe(safetyBoard) ||
                          TryMoveAnyBullIntoAny()))
                        MessageBox.Show("Bulls cannot make move.");
                    break;
            }
        }


        /// <summary>
        /// Fuction analyse board
        /// </summary>
        /// <returns>Board of safe/unsafe positions
        /// true - safe
        /// false - unsafe</returns>
        private bool?[,] AnalyseBoard()
        {
            Cell cell;
            bool?[,] result = new bool?[5,4];

            foreach (var tiger in Tigers)
            {
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell,tiger))
                    {
                        if (result[cell.Row, cell.Column].HasValue == false)
                            result[cell.Row, cell.Column] = true;
                        switch (move)
                        {
                            case Moves.Down:
                                if (cell.Row != 4 && Rules.CanPotentiallyKill(this, Board[cell.Row + 1, cell.Column],tiger))
                                    result[cell.Row + 1, cell.Column] = false;
                                break;
                            case Moves.Up:
                                if (cell.Row != 0 && Rules.CanPotentiallyKill(this, Board[cell.Row - 1, cell.Column],tiger))
                                    result[cell.Row - 1, cell.Column] = false;
                                break;
                            case Moves.Left:
                                if (cell.Column != 0 && Rules.CanPotentiallyKill(this, Board[cell.Row, cell.Column - 1],tiger))
                                    result[cell.Row, cell.Column - 1] = false;
                                break;
                            case Moves.Right:
                                if (cell.Column != 3 && Rules.CanPotentiallyKill(this, Board[cell.Row, cell.Column + 1],tiger))
                                    result[cell.Row, cell.Column + 1] = false;
                                break;
                        }
                    }
                }
            }

            return result;
        }

        private bool TryMoveUnsafeBullintoSafeBlocking(bool?[,] boardSafety)
        {
            Cell cell;
            foreach (var bull in Bulls)
            {
                if (boardSafety[bull.Row, bull.Column] == null)
                    continue;
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell, bull) && boardSafety[cell.Row, cell.Column].HasValue &&
                        boardSafety[cell.Row, cell.Column] == true)
                    {
                        Choosen = bull;
                        MoveChoosenCell(cell);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryMoveUnsafeBullintoSafe(bool?[,] boardSafety)
        {
            Cell cell;
            foreach (var bull in Bulls)
            {
                if (boardSafety[bull.Row, bull.Column] == null)
                    continue;
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell, bull) && boardSafety[cell.Row, cell.Column] != false)
                    {
                        Choosen = bull;
                        MoveChoosenCell(cell);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryMoveSafeBullIntoSafeBlocking(bool?[,] boardSafety)
        {
            Cell cell;
            foreach (var bull in Bulls)
            {
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell, bull) && boardSafety[cell.Row, cell.Column] == true)
                    {
                        Choosen = bull;
                        MoveChoosenCell(cell);
                        return true;
                    }
                }
            }
            return false;
        }
        private bool TryMoveSafeBullIntoSafe(bool?[,] boardSafety)
        {
            Cell cell;
            foreach (var bull in Bulls)
            {
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell, bull) && boardSafety[cell.Row, cell.Column] != false)
                    {
                        Choosen = bull;
                        MoveChoosenCell(cell);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryMoveUnsafeBullintoAny(bool?[,] boardSafety)
        {
            Cell cell;
            foreach (var bull in Bulls)
            {
                if (boardSafety[bull.Row, bull.Column] == null)
                    continue;
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell, bull))
                    {
                        Choosen = bull;
                        MoveChoosenCell(cell);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryMoveAnyBullIntoAny()
        {
            Cell cell;
            foreach (var bull in Bulls)
            {
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell, bull))
                    {
                        Choosen = bull;
                        MoveChoosenCell(cell);
                        return true;
                    }
                }
            }
            return false;
        }

        void RandomMove()
        {
            Cell cell;
            Random random = new Random();
            List<int> bullsInt = new List<int>();
            List<int> movesInt = new List<int>();

            for (int i = 0; i < Bulls.Count; ++i)
                bullsInt.Add(i);
            while (bullsInt.Count > 0)
            {
                for (int i = 0; i < 4; ++i)
                    movesInt.Add(i);
                int ind = random.Next(Bulls.Count - 1);
                Choosen = Bulls[ind];
                bullsInt.Remove(ind);
                while (movesInt.Count > 0)
                {
                    ind = movesInt[random.Next(movesInt.Count - 1)];
                    movesInt.Remove(ind);
                    if (Rules.MoveIsPossible(this, Enum.GetValues(typeof(Moves)).Cast<Moves>().ElementAt(ind), out cell, Choosen))
                    {
                        MoveChoosenCell(cell);
                        return;
                    }
                }
            }
        }
    }
}
