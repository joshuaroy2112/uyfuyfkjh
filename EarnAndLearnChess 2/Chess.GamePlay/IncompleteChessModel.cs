namespace Chess.GamePlay
{
    public class IncompleteChessModel : IChessModel
    {
        /// ----------------------------------------------------------------------------------
        /// ----------- No need to edit this introductory code -------------------------------
        /// ----------- (scroll down to see code you need to edit) ---------------------------
        /// ----------------------------------------------------------------------------------

        public IncompleteChessModel() { }

        public bool IsPieceOwnedByPlayer(char piece, Player player)
        {
            GridCharacter[] allWhitePieces = {
                GridCharacter.WhiteQueen,
                GridCharacter.WhiteKing,
                GridCharacter.WhiteBishop,
                GridCharacter.WhiteRook,
                GridCharacter.WhiteKnight,
                GridCharacter.WhitePawn
            };
            GridCharacter[] allBlackPieces = {
                GridCharacter.BlackQueen,
                GridCharacter.BlackKing,
                GridCharacter.BlackBishop,
                GridCharacter.BlackRook,
                GridCharacter.BlackKnight,
                GridCharacter.BlackPawn
            };
            GridCharacter[] allowedPieces = player == Player.White ? allWhitePieces : allBlackPieces;
            return allowedPieces.Contains((GridCharacter)piece);
        }

        public void MovePiece(char[][] board, Move move)
        {
            char sourcePiece = board[move.fromRow][move.fromColumn];
            board[move.fromRow][move.fromColumn] = (char)GridCharacter.Empty;
            board[move.toRow][move.toColumn] = sourcePiece;
        }

        private bool IsIndexInRange(int index)
        {
            return index >= 0 && index < 8;
        }

        public bool IsMoveWithinBoard(Move move)
        {
            return IsIndexInRange(move.fromRow) && IsIndexInRange(move.fromColumn) && IsIndexInRange(move.toRow) && IsIndexInRange(move.toColumn);
        }

        public bool IsMoveFromOwnPiece(char[][] board, Move move, Player player)
        {
            char fromPiece = board[move.fromRow][move.fromColumn];
            return IsPieceOwnedByPlayer(fromPiece, player);
        }

        public bool IsMoveToValidTile(char[][] board, Move move, Player player)
        {
            char toPiece = board[move.toRow][move.toColumn];
            return !IsPieceOwnedByPlayer(toPiece, player);
        }

        public bool IsMoveLegal(char[][] board, Move move, Player turn)
        {
            return IsMoveWithinBoard(move)
                && IsMoveFromOwnPiece(board, move, turn)
                && IsMoveToValidTile(board, move, turn)
                && IsPieceMoveLegal(board, move, turn)
                && !IsMoveIntoCheck(board, move, turn);
        }

        private bool IsPieceMoveLegal(char[][] board, Move move, Player turn)
        {
            char fromPiece = board[move.fromRow][move.fromColumn];
            switch (fromPiece.ToString().ToLower())
            {
                case "k":
                    return IsValidMovementForKing(board, move, turn);
                case "n":
                    return IsValidMovementForKnight(board, move, turn);
                case "p":
                    return IsValidMovementForPawn(board, move, turn);
                case "r":
                    return IsValidMovementForRook(board, move, turn);
                case "b":
                    return IsValidMovementForBishop(board, move, turn);
                case "q":
                    return IsValidMovementForQueen(board, move, turn);
                default:
                    return true;
            }
        }

        public bool IsValidMovementForKing(char[][] board, Move move, Player player)
        {
            int rowDifference = move.fromRow - move.toRow;
            int columnDifference = move.fromColumn - move.toColumn;

            // A valid move must not start and end at the same place
            if (rowDifference == 0 && columnDifference == 0)
            {
                return false;
            }

            // A valid move must not move a distance of 2+ in either direction
            else if (rowDifference < -1 || rowDifference > 1 || columnDifference < -1 || columnDifference > 1)
            {
                return false;
            }

            // Everything else (within 1 square) is legal
            else return true;
        }


        /// ----------------------------------------------------------------------------------
        /// ----------- Edit from here for Part 1: Piece Logic -------------------------------
        /// ----------------------------------------------------------------------------------


        private bool IsPathUnoccupied(char[][] board, int startRow, int startColumn, int moveDistance, int incrementRow, int incrementColumn)
        {
            for (int i = 1; i < moveDistance; i++) {
                if (board[startRow + (incrementRow * i)][startColumn + (incrementColumn * i)] != '.')
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsPathUnoccupiedUp(char[][] board, int startRow, int startColumn, int moveDistance, int incrementRow, int incrementColumn)
        {
            for (int i = 1; i <= moveDistance; i++)
            {
                if (board[startRow + (incrementRow + i)][startColumn + (incrementColumn + i)] != '.')
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsPathUnoccupiedDown(char[][] board, int startRow, int startColumn, int moveDistance, int incrementRow, int incrementColumn)
        {
            for (int i = moveDistance - 1; i >= 0; i--)
            {
                if (board[startRow + (incrementRow + i)][startColumn + (incrementColumn + i)] != '.')
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsValidMovementForKnight(char[][] board, Move move, Player player) //horse that moves in L shape
        {
            int rowDifference = move.toRow - move.fromRow;
            int columnDifference = move.toColumn - move.fromColumn;

            if (Math.Abs(rowDifference) == 1 && Math.Abs(columnDifference) == 2)
            {
                return true;
            }
            if (Math.Abs(rowDifference) == 2 && Math.Abs(columnDifference) == 1)
            {
                return true;
            }
            return false;
        }

        //    // A valid move must not start and end at the same place
        //    if ((rowDifference == 0 && columnDifference == 0)) // || (rowDifference == 0 && columnDifference != 0) || (rowDifference != 0 && columnDifference != 0))
        //    {
        //        return false;
        //    }
        //    // A valid move must move a distance of 1 and 2, 2 and 1, -1 and -2, -1 and 2 or 1 and -2
        //    else if ((rowDifference == 1 && columnDifference == 2) || (rowDifference == 2 && columnDifference == 1) || (rowDifference == -1 && columnDifference == -2) || (rowDifference == -2 && columnDifference == -1) || (rowDifference == -1 && columnDifference == 2) || (rowDifference == -2 && columnDifference == 1) || (rowDifference == 1 && columnDifference == -2) || (rowDifference == 2 && columnDifference == -1))
        //    //else if ((rowDifference <= -1 || rowDifference >= 1) && (columnDifference <= -2 || columnDifference >= 2 ) || (rowDifference <= -2 || rowDifference >= 2) && (columnDifference <= -1 || columnDifference >= 1))
        //    {
        //        return true;
        //    }

        //    // Everything else (within 1 square) is illegal
        //    else return false;
        //}

        public bool IsValidMovementForRook(char[][] board, Move move, Player player) //moves vert or hori
        {
            int rowDifference = move.toRow - move.fromRow;
            int columnDifference = move.toColumn - move.fromColumn;

                if (rowDifference == 0 && columnDifference != 0)
                {
                    return IsPathUnoccupied(board, move.fromRow, move.fromColumn, Math.Abs(columnDifference), 0, Math.Clamp(columnDifference, -1, 1));
                }
                if (rowDifference != 0 && columnDifference == 0)
                {
                    return IsPathUnoccupied(board, move.fromRow, move.fromColumn, Math.Abs(rowDifference), Math.Clamp(rowDifference, -1, 1), 0);
                }
                return false;
        }

        //    // A valid move must not start and end at the same place
        //    if ((rowDifference == 0 && columnDifference == 0) || (rowDifference != 0 && columnDifference != 0))
        //    {
        //        return false;
        //    }

        //    if (rowDifference == 0 && columnDifference != 0) // Horizontal movement
        //    {
        //        int startRow = move.fromRow;
        //        int startColumn = move.fromColumn;
        //        int endColumn = move.toColumn;
        //        int columnDirection = columnDifference > 0 ? -1 : 1;

        //        // Checking if spaces along the horizontal path are occupied
        //        for (int col = startColumn + columnDirection; col != endColumn; col += columnDirection)
        //        {
        //            if (board[startRow][col] != '.')
        //            {
        //                return false; // Obstructed by any piece
        //            }
        //        }
        //    }

        //    else if (columnDifference == 0) // Vertical movement
        //    {
        //        int startRow = move.fromRow;
        //        int startColumn = move.fromColumn;
        //        int endRow = move.toRow;
        //        int rowDirection = rowDifference > 0 ? -1 : 1;

        //        // Checking if spaces along the vertical path are occupied
        //        for (int row = startRow + rowDirection; row != endRow; row += rowDirection)
        //        {
        //            if (board[row][startColumn] != ' ')
        //            {
        //                return false; // Obstructed by any piece
        //            }
        //        }
        //    }
        //    else // Diagonal movement (invalid for a rook)
        //    {
        //        return false;
        //    }

        //    char destinationPiece = board[move.toRow][move.toColumn];
        //    // Checking if the destination piece is empty or owned by the opponent
        //    if (destinationPiece == '.' || IsPieceOwnedByPlayer(destinationPiece, player) == true)
        //    {
        //        return true; // Valid move
        //    }

        //    return false;
        //}

        //    if ((rowDifference == 0 && columnDifference != 0) || (rowDifference != 0 && columnDifference == 0)) // if movement is in either hori or verticle direction
        //    {
        //        int startRow = move.fromRow;
        //        int startColumn = move.fromColumn;
        //        int endRow = move.toRow;
        //        int endColumn = move.toColumn;

        //        if (rowDifference == 0)//horizontally checking if spaces are occupied
        //        {
        //            int columnDirection = columnDifference > 0 ? 1 : -1;
        //            for (int col = startColumn + columnDifference; col != endColumn; col += columnDirection)
        //            {
        //                if (board[startRow][col] != ' ')
        //                {
        //                    return false; //obstructed
        //                }
        //            }
        //        }
        //        else //vertically
        //        {
        //            int rowDirection = rowDifference > 0 ? 1 : -1;
        //            for (int row = startRow + endRow; row != endRow; row += rowDirection)
        //            {
        //                if (board[row][startColumn] != ' ')
        //                {
        //                    return false; //obstructed up way 

        //                }
        //            }
        //        }

        //        char destinationPiece = board[endRow][endColumn];
        //        if (destinationPiece == ' ' || IsPieceOwnedByPlayer(destinationPiece, player))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public bool IsValidMovementForBishop(char[][] board, Move move, Player player) // moves diag
        {
            int rowDifference = move.toRow - move.fromRow;
            int columnDifference = move.toColumn - move.fromColumn;


            if (Math.Abs(rowDifference) == Math.Abs(columnDifference))
            {
                return IsPathUnoccupied(board, move.fromRow, move.fromColumn, Math.Abs(rowDifference), Math.Clamp(rowDifference, -1, 1), Math.Clamp(columnDifference, -1, 1));
            }
            return false;
        }


        //    int startRow = move.fromRow;
        //    int startColumn = move.fromColumn;
        //    int endColumn = move.toColumn;
        //    int endRow = move.toRow;
        //    int columnDirection = columnDifference > 0 ? -1 : 1;
        //    int rowDirection = rowDifference > 0 ? -1 : 1;


        //    // A valid move must not start and end at the same place
        //    if ((rowDifference == 0 && columnDifference == 0))
        //    {
        //        return false;
        //    }
        //    //going up right

        //    if (rowDifference >= 1 && columnDifference >= 1) // up right
        //    {
        //        // Checking if spaces along the horizontal path are occupied
        //        for (int col = startColumn + columnDirection; col != endColumn; col += columnDirection)
        //        {
        //            for (int row = startRow + rowDirection; row != endRow; row += rowDirection)
        //            {
        //                if (board[row][col] != '.')
        //                {
        //                    return false; // Obstructed by any piece
        //                }
        //            }

        //        }

        //    }
        //    else if (rowDifference >= 1 && columnDifference <= 1) // down right movement
        //    {

        //        for (int col = startColumn - columnDirection; col != endColumn; col += columnDirection)
        //        {
        //            for (int row = startRow - rowDirection; row != endRow; row += rowDirection)
        //            {
        //                if (board[row][col] != '.')
        //                {
        //                    return false; // Obstructed by any piece
        //                }
        //            }

        //        }
        //    }
        //    else if (rowDifference <= 1 && columnDifference <= 1)
        //    // down left movement
        //    {
        //        for (int col = startColumn - columnDirection; col != endColumn; col += columnDirection)
        //        {
        //            for (int row = startRow - rowDirection; row != endRow; row += rowDirection)
        //            {
        //                if (board[row][col] != '.')
        //                {
        //                    return false; // Obstructed by any piece
        //                }
        //            }

        //        }
        //    }
        //    else if (rowDifference <= 1 && columnDifference >= 1)
        //    // down left movement
        //    {
        //        for (int col = startColumn - columnDirection; col != endColumn; col += columnDirection)
        //        {
        //            for (int row = startRow - rowDirection; row != endRow; row += rowDirection)
        //            {
        //                if (board[row][col] != '.')
        //                {
        //                    return false; // Obstructed by any piece
        //                }
        //            }

        //        }
        //    }
        //    return true;
        //}

            //            else
            //{
            //    return false;

            //}


            //        else if (columnDifference == 0) // Vertical movement
            //        {
            //            int startRow = move.fromRow;
            //            int startColumn = move.fromColumn;
            //            int endRow = move.toRow;
            //            int rowDirection = rowDifference > 0 ? -1 : 1;

            //            // Checking if spaces along the vertical path are occupied
            //            for (int row = startRow + rowDirection; row != endRow; row += rowDirection)
            //            {
            //                if (board[row][startColumn] != ' ')
            //                {
            //                    return false; // Obstructed by any piece
            //                }
            //            }
            //        }
            //        else // Diagonal movement (invalid for a rook)
            //        {
            //            return false;
            //        }

            //    char destinationPiece = board[move.toRow][move.toColumn];
            //    // Checking if the destination piece is empty or owned by the opponent
            //    if (destinationPiece == '.' || IsPieceOwnedByPlayer(destinationPiece, player))
            //    {
            //        return true; // Valid move
            //    }

            //    return false;
            //}
            //return true;
            //}
            //    return true;
            //}

            public bool IsValidMovementForQueen(char[][] board, Move move, Player player) //rook and bishop in 1
        {
            if(IsValidMovementForRook(board, move, player)) return true;
            if (IsValidMovementForBishop(board, move, player)) return true;
            return false;
        }

        public bool IsValidMovementForPawn(char[][] board, Move move, Player player)
        {
            int rowDifference = move.toRow - move.fromRow;
            int columnDifference = move.toColumn - move.fromColumn;
         
            if (columnDifference == 0) {
                if ((rowDifference == -1) && (board[move.toRow][move.toColumn] == '.'))
                {
                    return true;
                }
                else if ((rowDifference == 1) && (board[move.toRow][move.toColumn] == '.'))
                {
                    return true;
                }
                else if ((rowDifference == 2 * 1) && (move.fromRow == 6)) {
                    if (board[move.fromRow + 1][move.fromColumn] == '.' && board[move.fromRow + 2][move.fromColumn] == '.') return true;
                }
                else if ((rowDifference == 2 * -1) && (move.fromRow == 1))
                {
                    if (board[move.fromRow - 1][move.fromColumn] == '.' && board[move.fromRow + 2][move.fromColumn] == '.') return true;
                }

            }
            else if (Math.Abs(columnDifference) == 1 && rowDifference == 1)
            {
                if (board[move.toRow][move.toColumn] != '.' && !IsPieceOwnedByPlayer(board[move.toRow][move.toColumn], player)) return true;
            }
            else if (Math.Abs(columnDifference) == 1 && rowDifference == -1)
            {
                if (board[move.toRow][move.toColumn] != '.' && !IsPieceOwnedByPlayer(board[move.toRow][move.toColumn], player)) return true;
            }
            return false;
        }

        /// ----------------------------------------------------------------------------------
        /// ----------- Edit from here for Part 2: End game logic ----------------------------
        /// ----------------------------------------------------------------------------------

        public bool IsInCheck(char[][] board, Player player)
        {
            Player opponent = player == Player.White ? Player.Black : Player.White;

            

            for (int i = 0; i < board.Length; i++) {
                for (int j = 0; j < board.Length; j++) {
                    if (board[i][j] != '.' && IsPieceOwnedByPlayer(board[i][j], opponent)) {
                        return IsGameOver(board, player);
                    }
                }
            }
            return true;
        }

        public bool IsMoveIntoCheck(char[][] board, Move move, Player player)
        {
            return false; // todo implement this method.
        }

        public bool IsGameOver(char[][] board, Player player)
        {
            // todo update this method - the current implementation is incorrect and does not refer to the concept of "check".

            GridCharacter targetPiece = player == Player.White ? GridCharacter.WhiteKing : GridCharacter.BlackKing;

            // Search for the correct king (targetPiece).
            for (int i=0; i<board.Length; i++)
            {
                for (int j=0; j<board[i].Length; j++)
                {
                    if (board[i][j] == (char)targetPiece)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
