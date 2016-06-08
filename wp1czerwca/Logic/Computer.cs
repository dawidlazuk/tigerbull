using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp1czerwca
{
    public partial class Game
    {

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

            foreach(var bull in Bulls)
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell,bull))
                        result[cell.Row, cell.Column] = true;
                }


            foreach (var tiger in Tigers)
            {
                foreach (var move in Enum.GetValues(typeof(Moves)).Cast<Moves>())
                {
                    if (Rules.MoveIsPossible(this, move, out cell,tiger))
                    {
                        switch (move)
                        {
                            case Moves.Down:
                                if (cell.Row != 4 && Rules.CanPottentiallyKill(this, Board[cell.Row + 1, cell.Column],tiger))
                                    result[cell.Row + 1, cell.Column] = false;
                                break;
                            case Moves.Up:
                                if (cell.Row != 0 && Rules.CanPottentiallyKill(this, Board[cell.Row - 1, cell.Column],tiger))
                                    result[cell.Row - 1, cell.Column] = false;
                                break;
                            case Moves.Left:
                                if (cell.Column != 0 && Rules.CanPottentiallyKill(this, Board[cell.Row, cell.Column - 1],tiger))
                                    result[cell.Row, cell.Column - 1] = false;
                                break;
                            case Moves.Right:
                                if (cell.Column != 3 && Rules.CanPottentiallyKill(this, Board[cell.Row, cell.Column + 1],tiger))
                                    result[cell.Row, cell.Column + 1] = false;
                                break;
                        }
                    }
                }
            }

            return result;
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

        private bool TryMoveSafeBullIntoSafe(bool?[,] boardSafety)
        {
            Cell cell;
            foreach (var bull in Bulls)
            {
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
    }
}
