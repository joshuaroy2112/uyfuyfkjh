using Chess.GamePlay;
using Chess.Players;

namespace Chess.GameRunner;

public class ConsoleChessRunner : IChessRunner
{

    /// <summary>
    /// Runs the game of chess.
    /// While the method "IsInCheck" always returns false, it will only end when somebody's king is taken (since that is the definition of "IsGameOver")
    /// ... and it will report the end as stalemate!
    /// However, when IsInCheck and IsGameOver are updated, it will properly terminate the game of chess.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="player"></param>
    /// <param name="board"></param>
    public void RunChess(IChessModel model, IPlayer player, char[][] board)
    {
        Player currentPlayer = Player.White;

        while (!model.IsGameOver(board, currentPlayer))
        {
            PrintBoardOnConsole(board);
            if (model.IsInCheck(board, currentPlayer))
            {
                Console.WriteLine("Check!");
            }
            Console.WriteLine($"{PlayerName(currentPlayer)} to play.");

            Move move = player.GetMove(board);
            while (!model.IsMoveLegal(board, move, currentPlayer))
            {
                Console.WriteLine("That move is illegal, try again: ");
                move = player.GetMove(board);
            }

            model.MovePiece(board, move);
            currentPlayer = OtherPlayer(currentPlayer);

        };

        PrintBoardOnConsole(board);
        Console.WriteLine("\nGame over");

        if (model.IsInCheck(board, currentPlayer))
        {
            Console.WriteLine($"{PlayerName(OtherPlayer(currentPlayer))} wins!");
        }
        else
        {
            Console.WriteLine("Stalemate");
        }
    }

    private static Player OtherPlayer(Player p)
    {
        return p == Player.White ? Player.Black : Player.White;
    }


    private void PrintBoardOnConsole(char[][] board)
    {
        Console.Clear();

        for (int i = 0; i < 8; i++)
        {
            var row = board[i];
            Console.Write(IndexToRowNumber(i) + "|");
            foreach (char square in row)
            {
                Console.Write(square);
            }
            Console.WriteLine();
        }
        Console.Write("  --------\n  ");
        for (int i = 0; i < 8; i++)
        {
            Console.Write(IndexToColumnLetter(i));
        }
        Console.WriteLine("\n");
    }

    private static char IndexToRowNumber(int index)
    {
        return (char)('8' - index);
    }
    private static char IndexToColumnLetter(int index)
    {
        return (char)('a' + index);
    }

    private static string PlayerName(Player player)
    {
        return player == Player.White ? "White" : "Black";
    }
}
