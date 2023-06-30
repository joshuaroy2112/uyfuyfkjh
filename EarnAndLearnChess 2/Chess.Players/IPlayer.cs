using Chess.GamePlay;

namespace Chess.Players
{
    public interface IPlayer
    {
        public Move GetMove(char[][] board);
    }
}
