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
			var sut   	  	= new Game();
			ref Board board = ref sut.Board;

			for (int i = 0; i < 9; ++i)
				board[i] = fieldState;

			bool actual = sut.IsFinal();

			Assert.IsTrue(actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void No_winner_no_empty_fields___winner_for_draw_set_and_true()
		{
			var sut 		= new Game();
			ref Board board = ref sut.Board;

			board[0] = FieldState.Machine;
			board[1] = FieldState.User;
			board[2] = FieldState.Machine;
			board[3] = FieldState.Machine;
			board[4] = FieldState.User;
			board[5] = FieldState.Machine;
			board[6] = FieldState.User;
			board[7] = FieldState.Machine;
			board[8] = FieldState.User;

			bool actual = sut.IsFinal();

			Assert.Multiple(() =>
			{
				Assert.AreEqual(Winner.Draw, sut.Winner);
				Assert.IsTrue(actual);
			});
		}
		//---------------------------------------------------------------------
		[Test]
		public void Winner_set_no_empty_fields___winner_not_changed_and_true()
		{
			var sut 		= new Game();
			ref Board board = ref sut.Board;

			board[0] = FieldState.Machine;
			board[1] = FieldState.User;
			board[2] = FieldState.User;
			board[3] = FieldState.Machine;
			board[4] = FieldState.User;
			board[5] = FieldState.Machine;
			board[6] = FieldState.Machine;
			board[7] = FieldState.Machine;
			board[8] = FieldState.User;

			sut.CheckWinner();
			bool actual = sut.IsFinal();

			Assert.Multiple(() =>
			{
				Assert.AreEqual(Winner.Machine, sut.Winner);
				Assert.IsTrue(actual);
			});
		}
	}
}
