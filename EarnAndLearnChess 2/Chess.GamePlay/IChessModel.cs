namespace Chess.GamePlay
{
    public enum GridCharacter
    {
        Empty = '.',

        WhitePawn = 'p',
        WhiteKing = 'k',
        WhiteQueen = 'q',
        WhiteRook = 'r',
        WhiteBishop = 'b',
        WhiteKnight = 'n',

        BlackPawn = 'P',
        BlackQueen = 'Q',
        BlackKing = 'K',
        BlackRook = 'R',
        BlackKnight = 'N',
        BlackBishop = 'B',
    }

    public enum Player
    {
        White,
        Black,
    }

    /// <summary>
    /// Represent a move from a position to another position.
    /// Stored in this format for convenience in passing paramaters
    /// </summary>
    public struct Move
    {
        // Rows and columns start at 0 and go up to 7
        public Move(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            this.fromRow = fromRow;
            this.fromColumn = fromColumn;
            this.toRow = toRow;
            this.toColumn = toColumn;
        }

        public int fromRow { get; init; }
        public int fromColumn { get; init; }
        public int toRow { get; init; }
        public int toColumn { get; init; }
    }

    public interface IChessModel
    {
        /// <summary>
        /// Test is the specified piece is owned by the specified player.
        /// </summary>
        public bool IsPieceOwnedByPlayer(char piece, Player player);

        /// <summary>
        /// Applies the move to the board.
        /// does not need to check if the move is legal and can assume it is.
        /// </summary>
        public void MovePiece(char[][] board, Move move);

        /// <summary>
        /// Returns if the tiles the move is from and to are inside the 8x8 board
        /// </summary>
        public bool IsMoveWithinBoard(Move move);

        /// <summary>
        /// Returns if the tile the move is from contains a piece owned by the given player
        /// </summary>
        public bool IsMoveFromOwnPiece(char[][] board, Move move, Player player);

        /// <summary>
        /// Returns if the tile the move is to is one that can be moved to by this player.
        /// A player can move to tiles which are empty or have an opponents piece
        /// </summary>
        public bool IsMoveToValidTile(char[][] board, Move move, Player player);


        /* As well as the requirements on all moves to be legal each piece also has additional rules about the moves
         * it is allowed to make, you can read about the rules for each piece here:
         * https://www.chess.com/terms/chess-pieces#howthechesspiecesmove
         * The following methods only need to check for the piece specific rules
         */

        /// <summary>
        /// Returns if the move would be legal for king to make (only the king specific logic)
        /// Kings can move one tile in any direction
        /// </summary>
        public bool IsValidMovementForKing(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns if the move would be legal for a knight to make (only the knight specific logic)
        /// Knights can move in an L shape, 
        /// </summary>
        public bool IsValidMovementForKnight(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns if the move would be legal for a rook to make (only the rook specific logic)
        /// Rooks can move any distance either horizontally or vertically, but cannot move through other pieces
        /// </summary>
        public bool IsValidMovementForRook(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns if the move would be legal for a bishop to make (only the bishop specific logic)
        /// Bishops can move any distance in any one of the four 45 degree diagonals, but cannot move through other pieces
        /// </summary>
        public bool IsValidMovementForBishop(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns if the move would be legal for queen to make (only the queen specific logic)
        /// Queens can move any distance in one of the eight horizontal, vertical and 45 degree diagonal directions
        /// but cannot move over other pieces
        /// </summary>
        public bool IsValidMovementForQueen(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns if the move would be legal for a pawn to make (only the pawn specific logic)
        /// Pawns can do any one of:
        ///  - Move one square forwards, but cannot make a capture
        ///  - Move diagonally one square forwards and one sideways, but only when making a capture
        ///  - Move two squares forwards, but only if they are not making a capture and are on the row which they started
        ///  
        ///  Which direction is forwards and which row pawns start on will depend on which color the pawn is.
        /// </summary>
        public bool IsValidMovementForPawn(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns if a move is Legal for the given player.
        /// To be legal a move must:
        ///  - Not start or end outside of the board
        ///  - Start from one of the current players pieces
        ///  - Not go to one of the current players pieces
        ///  - Meet any piece specific rules (check the "IsValidMovementForPIECENAME" methods)
        /// </summary>
        public bool IsMoveLegal(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns whether the given player is in check.
        /// A player is in check if one of the opponents pieces has a move that would capture the opponents king
        /// </summary>
        public bool IsInCheck(char[][] board, Player turn);

        /// <summary>
        /// Returns whether after the move is made the player is in check.
        /// One way to do this is to make the move record if the player is in check then undo the move.
        /// You should add this check to IsMoveLegal
        /// </summary>
        public bool IsMoveIntoCheck(char[][] board, Move move, Player turn);

        /// <summary>
        /// Returns true if the game is over.
        /// Initially, this will just check for a king missing on the board.
        /// However, later, it should look for a check situation without a legal move to escape check
        /// </summary>
        public bool IsGameOver(char[][] board, Player turn);
    }
}
