using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp1czerwca
{
    static class Rules
    {
        public static bool CanChoose(Game game, Cell cell)
        {
            return game.Round == cell.Type;
        }

        public static bool CanMove(Game game, Cell target)
        {
            return target.Type == Animal.None &&
                   (Math.Abs(game.Choosen.Row - target.Row) + Math.Abs(game.Choosen.Column - target.Column) == 1);
        }

        public static bool CanKill(Game game, Cell target, Cell choosen)
        {
            return target.Type == Animal.Bull && choosen.Type == Animal.Tiger &&
                   CanPotentiallyKill(game, target, choosen);
        }

        public static bool CanPotentiallyKill(Game game, Cell target, Cell choosen)
        {
            return (choosen.Column == target.Column && Math.Abs(choosen.Row - target.Row) == 2 &&
                     game.Board[(choosen.Row + target.Row) / 2, choosen.Column].Type == Animal.None) ||
                    (choosen.Row == target.Row && Math.Abs(choosen.Column - target.Column) == 2 &&
                     game.Board[choosen.Row, (choosen.Column + target.Column) / 2].Type == Animal.None);
        }

        public static bool MoveIsPossible(Game game, Moves direction, out Cell cell, Cell choosen)
        {
            bool result = false;
            cell = null;
            switch (direction)
            {
                case Moves.Up:
                    if(result = choosen.Row > 0 && game.Board[choosen.Row - 1, choosen.Column].Type == Animal.None)
                        cell = game.Board[choosen.Row - 1, choosen.Column];
                    break;
                case Moves.Down:
                    if(result = choosen.Row < game.Board.GetLength(0) - 1 &&
                           game.Board[choosen.Row + 1, choosen.Column].Type == Animal.None) 
                        cell = game.Board[choosen.Row + 1, choosen.Column];
                    break;
                case Moves.Left:
                    if(result = choosen.Column > 0 && game.Board[choosen.Row, choosen.Column - 1].Type == Animal.None) 
                        cell = game.Board[choosen.Row, choosen.Column-1];
                    break;
                case Moves.Right:
                   if(result = choosen.Column < game.Board.GetLength(1) - 1 &&
                           game.Board[choosen.Row, choosen.Column + 1].Type == Animal.None)
                       cell = game.Board[choosen.Row, choosen.Column+1];
                    break;
            }
            return result;
        }

        public static bool TigersCanMove(Game game)
        {
            foreach (var tiger in game.Tigers)
            {
                if((tiger.Row > 0 && game.Board[tiger.Row-1,tiger.Column].Type == Animal.None) ||
                   (tiger.Row < game.Board.GetLength(0)-1 && game.Board[tiger.Row+1,tiger.Column].Type == Animal.None) ||
                   (tiger.Column > 0 && game.Board[tiger.Row,tiger.Column-1].Type == Animal.None) ||
                   (tiger.Column < game.Board.GetLength(1)-1 && game.Board[tiger.Row,tiger.Column+1].Type == Animal.None))
                return true;
            }
            return false;
        }
    }
}
