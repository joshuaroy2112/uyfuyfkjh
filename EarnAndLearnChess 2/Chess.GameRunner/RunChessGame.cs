using Chess.GamePlay;
using Chess.Players;

namespace Chess.GameRunner;

public class RunChessGame
{
    public static void Main()
    {
        IChessModel model = new IncompleteChessModel();

        IPlayer player = new ConsolePlayer();

        IChessRunner runner = new ConsoleChessRunner();

        char[][] board = InitializeBoard();

        runner.RunChess(model, player, board);
    }

    private static char[][] InitializeBoard()
    {
        String[] boardStrings = new string[]{
            "RNBQKBNR",
            "PPPPPPPP",
            "........",
            "........",
            "........",
            "........",
            "pppppppp",
            "rnbqkbnr",
        };

        char[][] board = new char[8][];
        for (int i = 0; i < 8; i++)
        {
            board[i] = boardStrings[i].ToCharArray();
        }

        return board;
    }
}
