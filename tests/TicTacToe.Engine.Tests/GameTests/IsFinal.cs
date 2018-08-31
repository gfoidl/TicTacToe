using NUnit.Framework;

namespace TicTacToe.Engine.Tests.GameTests
{
	[TestFixture]
	public class IsFinal
	{
		[Test]
		public void No_winner_empty_fields___false()
		{
			var sut = new Game();

			bool actual = sut.IsFinal();

			Assert.IsFalse(actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void Winner_empty_fields___true()
		{
			var sut    = new Game();
			sut.Winner = Winner.User;

			bool actual = sut.IsFinal();

			Assert.IsTrue(actual);
		}
		//---------------------------------------------------------------------
		[Test]
		[TestCase(FieldState.User)]
		[TestCase(FieldState.Machine)]
		public void No_winner_no_empty_fields___true(FieldState fieldState)
		{
			var sut   = new Game();
			var board = sut.Board;

			for (int i = 0; i < 9; ++i)
				board[i] = fieldState;

			bool actual = sut.IsFinal();

			Assert.IsTrue(actual);
		}
	}
}
