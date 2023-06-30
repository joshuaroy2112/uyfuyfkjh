using Chess.GamePlay;
using NUnit.Framework;

namespace Chess.Tests
{
    internal class ModelTests
    {
        public IChessModel Model { get; set; }

        [SetUp]
        public void Setup()
        {
            Model = new IncompleteChessModel();
            Console.WriteLine(Directory.GetFiles("./"));
        }

        private static char[][] LoadBoardFromFile(string filename)
        {
            string[] allLines = File.ReadAllLines(filename);
            char[][] board = new char[allLines.Length][];
            for (int i = 0; i < allLines.Length; i++)
            {
                board[i] = allLines[i].ToCharArray();
            }

            return board;
        }

        [TestCase("k", Player.White, true)]
        [TestCase("k", Player.Black, false)]
        [TestCase("K", Player.White, false)]
        [TestCase("K", Player.Black, true)]
        public void TestIsPieceOwnedByPlayer(char piece, Player player, bool expected)
        {
            Assert.That(Model.IsPieceOwnedByPlayer(piece, player), Is.EqualTo(expected));
        }

        [TestCase(3, 3, 0, 0, Player.White)]
        [TestCase(3, 3, 0, 0, Player.Black)]
        public void TestCanMoveFromBlank(int fromRow, int fromCol, int toRow, int toCol, Player turn)
        {
            char[][] board = LoadBoardFromFile("TestFiles/StartBoard.txt");

            Move move = new Move(fromRow, fromCol, toRow, toCol);
            bool canMove = Model.IsMoveLegal(board, move, turn);
            Assert.That(canMove, Is.False);
        }

        [Test]
        public void TestCanMoveStartPawn(
            [Values(1, 2, 3)] int moveDist,
            [Values(Player.Black, Player.White)] Player turn)
        {
            int pawnColumn = 3;
            char[][] board = LoadBoardFromFile("TestFiles/StartBoard.txt");

            Move moveWhitePawn = new Move(6, pawnColumn, 6 - moveDist, pawnColumn);
            bool canMoveWhitePawn = Model.IsMoveLegal(board, moveWhitePawn, turn);
            bool shouldBeAbleToMoveWhite = turn == Player.White && moveDist != 3;
            Assert.That(canMoveWhitePawn, Is.EqualTo(shouldBeAbleToMoveWhite));

            Move moveBlackPawn = new Move(1, pawnColumn, 1 + moveDist, pawnColumn);
            bool canMoveBlackPawn = Model.IsMoveLegal(board, moveBlackPawn, turn);
            bool shouldBeAbleToMoveBlack = turn == Player.Black && moveDist != 3;
            Assert.That(canMoveBlackPawn, Is.EqualTo(shouldBeAbleToMoveBlack));
        }

        // Pawn:
        [TestCase(3, 5, 2, 5, Player.White, true)] // single forwards
        [TestCase(3, 5, 2, 5, Player.Black, false)] // single forwards of opponents piece 
        [TestCase(3, 5, 1, 5, Player.White, false)] // double forwards not start
        [TestCase(3, 5, 2, 4, Player.White, true)] // takes up left 
        [TestCase(3, 5, 2, 6, Player.White, false)] // up right no take
        [TestCase(6, 2, 4, 3, Player.White, false)] // double up right take
        public void TestCanMovePawn(int fromRow, int fromCol, int toRow, int toCol, Player turn, bool expected)
        {
            TestCanMove(fromRow, fromCol, toRow, toCol, turn, expected);
        }

        // Rook:
        [TestCase(3, 3, 0, 3, Player.White, true)] //*DONE*
        [TestCase(3, 3, 0, 3, Player.Black, false)] // opponents piece
        [TestCase(3, 3, 5, 3, Player.White, false)] // over opponent piece *DONE*
        [TestCase(3, 3, 3, 6, Player.White, false)] // capture over own piece *DONE*
        [TestCase(3, 3, 3, 4, Player.White, true)] // capture *DONE*
        [TestCase(3, 3, 4, 4, Player.White, false)] // diagonal *DONE*
        public void TestCanMoveRook(int fromRow, int fromCol, int toRow, int toCol, Player turn, bool expected)
        {
            TestCanMove(fromRow, fromCol, toRow, toCol, turn, expected);
        }

        // King:
        [TestCase(7, 7, 6, 6, Player.White, true)]
        [TestCase(7, 7, 5, 7, Player.White, false)] // too far
        [TestCase(7, 7, 8, 8, Player.White, false)] // outside board
        public void TestCanMoveKing(int fromRow, int fromCol, int toRow, int toCol, Player turn, bool expected)
        {
            TestCanMove(fromRow, fromCol, toRow, toCol, turn, expected);
        }

        // Queen:
        [TestCase(5, 4, 2, 4, Player.White, true)] // capture vertical
        [TestCase(5, 4, 0, 4, Player.White, false)] // goes over piece
        [TestCase(5, 4, 4, 3, Player.White, true)] // capture diagonal
        [TestCase(5, 4, 5, 0, Player.White, true)] // move horizontal
        [TestCase(5, 4, 6, 6, Player.White, false)] // move L
        public void TestCanMoveQueen(int fromRow, int fromCol, int toRow, int toCol, Player turn, bool expected)
        {
            TestCanMove(fromRow, fromCol, toRow, toCol, turn, expected);
        }

        // Knight:
        [TestCase(0, 5, 2, 1, Player.White, false)] // too far
        [TestCase(0, 5, 0, 3, Player.White, false)] // not L
        [TestCase(0, 5, 1, 3, Player.White, true)]
        [TestCase(0, 5, 2, 4, Player.White, true)]
        [TestCase(0, 5, 2, 6, Player.White, true)]
        public void TestCanMoveKnight(int fromRow, int fromCol, int toRow, int toCol, Player turn, bool expected)
        {
            TestCanMove(fromRow, fromCol, toRow, toCol, turn, expected);
        }

        // Bishop: 
        [TestCase(4, 0, 2, 2, Player.White, true)]
        [TestCase(4, 0, 5, 1, Player.White, true)] // *DONE*
        [TestCase(4, 0, 4, 2, Player.White, false)] // not diagonal
        [TestCase(4, 0, 7, 3, Player.White, false)] // over piece
        public void TestCanMoveBishop(int fromRow, int fromCol, int toRow, int toCol, Player turn, bool expected)
        {
            TestCanMove(fromRow, fromCol, toRow, toCol, turn, expected);
        }


        public void TestCanMove(int fromRow, int fromCol, int toRow, int toCol, Player turn, bool expected)
        {
            char[][] board = LoadBoardFromFile("TestFiles/Board1.txt");

            Move move = new Move(fromRow, fromCol, toRow, toCol);
            bool canMove = Model.IsMoveLegal(board, move, turn);
            Assert.That(canMove, Is.EqualTo(expected));
        }

        [TestCase("Board1.txt", "Board1After1.txt", 3, 3, 0, 3)]
        public void TestMovePiece(string beforeFile, string expectedFile, int fromRow, int fromCol, int toRow, int toCol)
        {
            Move move = new Move(fromRow, fromCol, toRow, toCol);

            char[][] after = LoadBoardFromFile("TestFiles/" + beforeFile);
            Model.MovePiece(after, move);

            char[][] expected = LoadBoardFromFile("TestFiles/" + expectedFile);
            Assert.That(after, Is.EqualTo(expected));
        }

        [TestCase("Board1After1.txt", Player.Black, true)]
        [TestCase("Board1.txt", Player.Black, false)]
        [TestCase("StartBoard.txt", Player.White, false)]
        [TestCase("StartBoard.txt", Player.Black, false)]
        public void TestIsInCheck(string boardFile, Player turn, bool expected)
        {
            char[][] board = LoadBoardFromFile("TestFiles/" + boardFile);
            bool inCheck = Model.IsInCheck(board, turn);
            Assert.That(inCheck, Is.EqualTo(expected));
        }

        [TestCase("Board1.txt", 0, 0, 0, 1, Player.Black, true)]
        [TestCase("Board1After1.txt", 0, 0, 0, 1, Player.Black, true)]
        [TestCase("Board1.txt", 7, 7, 6, 7, Player.White, false)]
        public void TestIsMoveIntoCheck(string boardFile, int fromRow, int fromCol, int toRow, int toCol, Player player, bool expected)
        {
            char[][] board = LoadBoardFromFile("TestFiles/" + boardFile);
            Move move = new(fromRow, fromCol, toRow, toCol);
            bool inCheck = Model.IsMoveIntoCheck(board, move, player);

            Assert.That(inCheck, Is.EqualTo(expected));
        }

        [TestCase("Board1After1.txt", Player.White, true)]
        [TestCase("Board1After1.txt", Player.Black, false)]
        public void TestHasLegalMove(string boardFile, Player player, bool expected)
        {
            char[][] board = LoadBoardFromFile("TestFiles/" + boardFile);
            bool inCheck = !Model.IsGameOver(board, player);
            Assert.That(inCheck, Is.EqualTo(expected));
        }
    }
}