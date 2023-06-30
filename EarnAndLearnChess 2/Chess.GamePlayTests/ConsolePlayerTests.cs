using Chess.Players;
using NUnit.Framework;

namespace Chess.Tests
{
    internal class ConsolePlayerTests
    {
        [TestCase("e4", true)]
        [TestCase("a4", true)]
        [TestCase("a8", true)]
        [TestCase("a3", true)]
        [TestCase("h1", true)]
        [TestCase("h 1", false)]
        [TestCase("h 11", false)]
        [TestCase("811", false)]
        [TestCase("hh1", false)]
        [TestCase("A8", false)]
        [TestCase("a9", false)]
        [TestCase("p2", false)]
        [TestCase("c0", false)]
        public void TestIsValidSquareString(string square, bool expected)
        {
            Assert.That(ConsolePlayer.IsStringValidSquare(square), Is.EqualTo(expected));
        }

        [TestCase('a', 0)]
        [TestCase('h', 7)]
        public void TestColumnLetterToNumber(char letter, int expected)
        {
            Assert.That(ConsolePlayer.ColumnLetterToNumber(letter), Is.EqualTo(expected));
        }

        [TestCase('1', 7)]
        [TestCase('8', 0)]
        public void TestCharToNumber(char number, int expected)
        {
            Assert.That(ConsolePlayer.CharToIndex(number), Is.EqualTo(expected));
        }
    }
}
