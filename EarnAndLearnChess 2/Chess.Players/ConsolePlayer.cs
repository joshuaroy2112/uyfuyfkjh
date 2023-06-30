using Chess.GamePlay;

namespace Chess.Players
{
    public class ConsolePlayer : IPlayer
    {
        public Move GetMove(char[][] board)
        {
            Console.Write("Enter move (e.g. e2 e4): ");
            string moveString = (Console.ReadLine() ?? "").Trim();
            while (!IsStringValidMove(moveString))
            {
                Console.Write("Invalid move, try again: ");
                moveString = Console.ReadLine() ?? "";
            }

            string from = moveString.Split(' ')[0];
            string to = moveString.Split(' ')[1];
            return new Move(CharToIndex(from[1]), ColumnLetterToNumber(from[0]), CharToIndex(to[1]), ColumnLetterToNumber(to[0]));
        }

        private static bool IsStringValidMove(string moveString)
        {
            string[] squares = moveString.Split(' ');
            if (squares.Length != 2) return false;
            return IsStringValidSquare(squares[0]) && IsStringValidSquare(squares[1]);
        }

        public static bool IsStringValidSquare(string square)
        {
            if (square == null) return false;
            if (square.Length != 2) return false;
            if (!"abcdefgh".Contains(square[0])) return false;
            if (!"12345678".Contains(square[1])) return false;
            return true;
        }

        // converts a column number 'a'-'h' to an int 0-7
        public static int ColumnLetterToNumber(char letter)
        {
            return letter - 'a';
        }

        // converts a character '1'-'8' to an int 7-0
        public static int CharToIndex(char number)
        {
            return '8' - number;
        }
    }
}
