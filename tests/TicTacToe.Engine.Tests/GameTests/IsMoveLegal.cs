using NUnit.Framework;

namespace TicTacToe.Engine.Tests.GameTests
{
	[TestFixture]
	public class IsMoveLegal
	{
		[Test]
		public void Field_is_empty___true()
		{
			var sut = new Game();

			bool actual = sut.IsMoveLegal(3);

			Assert.IsTrue(actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void Field_is_user___false()
		{
			var sut 	  = new Game();
			sut.Fields[3] = FieldState.User;

			bool actual = sut.IsMoveLegal(3);

			Assert.IsFalse(actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void Field_is_machine___false()
		{
			var sut 	  = new Game();
			sut.Fields[3] = FieldState.Machine;

			bool actual = sut.IsMoveLegal(3);

			Assert.IsFalse(actual);
		}
	}
}
