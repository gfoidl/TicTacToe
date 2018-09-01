using NUnit.Framework;

namespace TicTacToe.Engine.Tests.BoardTests
{
	[TestFixture]
	public class HasEmptyFields
	{
		[Test]
		public void New_board___true()
		{
			var sut = new Board();

			bool actual = sut.HasEmptyFields();

			Assert.IsTrue(actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void All_fields_set___false([Values(FieldState.Machine, FieldState.User)] FieldState value)
		{
			var sut = new Board();

			for (int i = 0; i < 9; ++i)
				sut[i] = value;

			bool actual = sut.HasEmptyFields();

			Assert.IsFalse(actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void One_fields_not_set___true([Values(FieldState.Machine, FieldState.User)] FieldState value)
		{
			Assert.Multiple(() =>
			{
				for (int j = 0; j < 9; ++j)
				{
					var sut = new Board();

					for (int i = 0; i < 9; ++i)
						sut[i] = value;

					sut[j] = FieldState.Empty;

					bool actual = sut.HasEmptyFields();

					Assert.IsTrue(actual);
				}
			});
		}
	}
}
