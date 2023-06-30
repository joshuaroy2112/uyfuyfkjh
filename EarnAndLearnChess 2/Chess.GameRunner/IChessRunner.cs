using Chess.GamePlay;
using Chess.Players;

namespace Chess.GameRunner;

public interface IChessRunner
{
    void RunChess(IChessModel model, IPlayer player, char[][] board);
}