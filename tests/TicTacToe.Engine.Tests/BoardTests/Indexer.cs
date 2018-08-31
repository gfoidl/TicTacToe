using NUnit.Framework;

namespace TicTacToe.Engine.Tests.BoardTests
{
	[TestFixture]
	public class Indexer
	{
		[Test]
		public void Empty_board_get___Empty()
		{
			var sut = new Board(true);

			FieldState actual = sut[3];

			Assert.AreEqual(FieldState.Empty, actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void Empty_board_set_and_get___OK([Values(FieldState.User, FieldState.Machine)] FieldState value)
		{
			var sut = new Board(true);
			sut[3] 	= value;

			FieldState actual = sut[3];

			Assert.AreEqual(value, actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void Set_and_get___OK([Values(FieldState.User, FieldState.Machine)] FieldState value)
		{
			var sut = new Board(true);

			for (int i = 0; i < 9; ++i)
				sut[i] = value;

			FieldState expected = value == FieldState.User ? FieldState.Machine : FieldState.User;
			sut[4] 				= expected;

			FieldState actual = sut[4];

			Assert.AreEqual(expected, actual);
		}
	}
}
