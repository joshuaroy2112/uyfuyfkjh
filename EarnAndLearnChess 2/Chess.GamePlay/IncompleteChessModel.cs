using System.Data;

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


        private bool IsPathUnoccupied(char[][] board, int startRow, int startColumn, int incrementRow, int incrementColumn, int moveDisplacement)
        {
            for (int i = 1; i < moveDisplacement; i++) {
                if (board[startRow + (incrementRow * i)][startColumn + (incrementColumn * i)] != '.') //if not a empty space, path is occupied.
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

            if (Math.Abs(rowDifference) == 1 && Math.Abs(columnDifference) == 2) // move 1 U or D and 2 L or R
            {
                return true;
            }
            if (Math.Abs(rowDifference) == 2 && Math.Abs(columnDifference) == 1) // move 2 U or D and 1 L or R
            {
                return true;
            }
            return false;
        }

        public bool IsValidMovementForRook(char[][] board, Move move, Player player) //moves vert or hori
        {
            int rowDifference = move.toRow - move.fromRow;
            int columnDifference = move.toColumn - move.fromColumn;

                if (rowDifference == 0 && columnDifference != 0) //if movement is either row or either column then valid
                {
                    return IsPathUnoccupied(board, move.fromRow, move.fromColumn, 0, Math.Clamp(columnDifference, -1, 1), Math.Abs(columnDifference)); //row doesnt change, column changes by eithe -1, 0, 1 and displacement is the abs value of columndifference
                }
                if (rowDifference != 0 && columnDifference == 0)
                {
                    return IsPathUnoccupied(board, move.fromRow, move.fromColumn, Math.Clamp(rowDifference, -1, 1), 0, Math.Abs(rowDifference)); //column doesnt change, row changes by eithe -1, 0, 1 and displacement is the abs value of rowdifference
            }
                return false;
        }

        public bool IsValidMovementForBishop(char[][] board, Move move, Player player) // moves diag
        {
            int rowDifference = move.toRow - move.fromRow;
            int columnDifference = move.toColumn - move.fromColumn;

            if (Math.Abs(rowDifference) == Math.Abs(columnDifference)) //if bishop moves the same amount then it is going diagonally
            {
                return IsPathUnoccupied(board, move.fromRow, move.fromColumn, Math.Clamp(rowDifference, -1, 1), Math.Clamp(columnDifference, -1, 1), Math.Abs(rowDifference)); //row changes by either -1, 0, 1 , column does same, displacement increments by either rowdiff or columndiff but both are same here.
            }
            return false;
        }

            public bool IsValidMovementForQueen(char[][] board, Move move, Player player) //rook and bishop in 1
        {
            if(IsValidMovementForRook(board, move, player)) return true; //uses rook and bishop rules
            if (IsValidMovementForBishop(board, move, player)) return true;
            return false;
        }

        public bool IsValidMovementForPawn(char[][] board, Move move, Player player)
        {
            int rowDifference = move.toRow - move.fromRow;
            int columnDifference = move.toColumn - move.fromColumn;
         
            if (columnDifference == 0) { //cannot move L or R only forwards/backwards depending on how your looking at board
                if ((rowDifference == -1) && (board[move.toRow][move.toColumn] == '.')) // white moving first "upwards" as board is indexed top to bottom
                {
                    return true;
                }
                else if ((rowDifference == 1) && (board[move.toRow][move.toColumn] == '.')) //black moving 1 down the list.
                {
                    return true;
                }
                else if ((rowDifference == -2) && (move.fromRow == 6)) //white pawn moving 2
                {
                    if (board[move.fromRow - 1][move.fromColumn] == '.' && board[move.fromRow - 2][move.fromColumn] == '.') return true;
                }
                else if ((rowDifference == 2) && (move.fromRow == 1)) //black pawn moving 2
                {
                    if (board[move.fromRow + 1][move.fromColumn] == '.' && board[move.fromRow + 2][move.fromColumn] == '.') return true;
                }

            }
            else if (Math.Abs(columnDifference) == 1 && rowDifference == 1) //capture logic ->if the white pawn moves diagonally to take then its true
            {
                if (board[move.toRow][move.toColumn] != '.' && !IsPieceOwnedByPlayer(board[move.toRow][move.toColumn], player)) return true;
            }
            else if (Math.Abs(columnDifference) == 1 && rowDifference == -1) // black pawn take
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
            int[] kingPosition = FindKingPosition(board, player); //calling function which finds king position
            Player opponent = player == Player.White ? Player.Black : Player.White; //opponent switch

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++) //iterating through entire board
                {
                    if (board[i][j] != '.' && IsPieceOwnedByPlayer(board[i][j], opponent)) //if space is occupied and owned by opposing team - 
                    {
                        if (IsMoveLegal(board, new Move(i, j, kingPosition[0], kingPosition[1]), opponent)) return true; //is the move is legal then the king is in check
                    }
                }
            }
            return false; //otherwise not in check
        }

        public bool IsMoveIntoCheck(char[][] board, Move move, Player player)
        {
            char[][] boardCopy = (char[][])board.Clone(); //thanks sam for Clone() method to clone board
            for (int rows = 0; rows < board.Length; rows++)
            {
                boardCopy[rows] = new char[board[rows].Length];
                for (int columns = 0; columns < board[rows].Length; columns++)
                {
                    boardCopy[rows][columns] = board[rows][columns];
                }
            }

            boardCopy[move.toRow][move.toColumn] = boardCopy[move.fromRow][move.fromColumn];
            boardCopy[move.fromRow][move.fromColumn] = '.'; //copying board

            if (IsInCheck(boardCopy, player))
            {
                return true;
            } //if the next move puts the game into check then that move is in check

            return false;

        }

        public bool IsGameOver(char[][] board, Player player)
        {
            int[] kingPosition = new int[2]; //using king finder function

            if (IsInCheck(board, player))
            {
                for (int i = 0; i < board.Length; i++)
                {
                    for (int j = 0; j < board[i].Length; j++)
                    {
                        if (IsPieceOwnedByPlayer(board[i][j], player)) //iterating through all possible pieces
                        {
                            if (IsMoveLegal(board, new Move(i, j, kingPosition[0], kingPosition[1]), player) && !IsMoveIntoCheck(board, new Move(i, j, kingPosition[0], kingPosition[1]), player))
                            { //if the move is legal but does not put the game into check then the game isnt over
                                return false;
                            }

                        }

                    }
                }

                return true; //otherwise the game is over
            }
            return false; //game is over
        }
        public int[] FindKingPosition(char[][] board, Player player)
        {
            int[] kingPosition = new int[2];

            GridCharacter targetPiece = player == Player.White ? GridCharacter.WhiteKing : GridCharacter.BlackKing; //taken from original IsGameOver()

            // Search for the correct king (targetPiece).
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] == (char)targetPiece)
                    {
                        kingPosition[0] = i;
                        kingPosition[1] = j;
                        return kingPosition;
                    }
                }

            }
            return kingPosition;
        }
    }
}
