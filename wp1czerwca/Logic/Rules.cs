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

        public static bool CanKill(Game game, Cell target)
        {
            return target.Type == Animal.Bull && game.Choosen.Type == Animal.Tiger &&
                   ((game.Choosen.Column == target.Column && Math.Abs(game.Choosen.Row - target.Row) == 2 &&
                     game.Board[(game.Choosen.Row + target.Row)/2, game.Choosen.Column].Type == Animal.None) ||
                    (game.Choosen.Row == target.Row && Math.Abs(game.Choosen.Column - target.Column) == 2 &&
                     game.Board[game.Choosen.Row, (game.Choosen.Column + target.Column)/2].Type == Animal.None));
        }

        public static bool MoveIsPossible(Game game, Moves direction, out Cell cell)
        {
            bool result = false;
            cell = null;
            switch (direction)
            {
                case Moves.Up:
                    if(result = game.Choosen.Row > 0 && game.Board[game.Choosen.Row - 1, game.Choosen.Column].Type == Animal.None)
                        cell = game.Board[game.Choosen.Row - 1, game.Choosen.Column];
                    break;
                case Moves.Down:
                    if(result = game.Choosen.Row < game.Board.GetLength(0) - 1 &&
                           game.Board[game.Choosen.Row + 1, game.Choosen.Column].Type == Animal.None) 
                        cell = game.Board[game.Choosen.Row + 1, game.Choosen.Column];
                    break;
                case Moves.Left:
                    if(result = game.Choosen.Column > 0 && game.Board[game.Choosen.Row, game.Choosen.Column - 1].Type == Animal.None) 
                        cell = game.Board[game.Choosen.Row, game.Choosen.Column-1];
                    break;
                case Moves.Right:
                   if(result = game.Choosen.Column < game.Board.GetLength(1) - 1 &&
                           game.Board[game.Choosen.Row, game.Choosen.Column + 1].Type == Animal.None)
                       cell = game.Board[game.Choosen.Row, game.Choosen.Column+1];
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
