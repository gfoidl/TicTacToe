using System.Linq;
using NUnit.Framework;

namespace TicTacToe.Engine.Tests.BoardTests
{
	[TestFixture]
	public class GetEmptyFields
	{
		[Test]
		public void New_board___all_fields()
		{
			var sut = new Board();

			int[] actual = sut.GetEmptyFields().ToArray();

			int[] expected = Enumerable.Range(0, 9).ToArray();
			CollectionAssert.AreEqual(expected, actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void One_empty_field___OK()
		{
			var sut = new Board();

			for (int i = 0; i < 9; ++i)
				sut[i] = FieldState.Machine;

			sut[3] = FieldState.Empty;

			int[] actual = sut.GetEmptyFields().ToArray();

			int[] expected = { 3 };
			CollectionAssert.AreEqual(expected, actual);
		}
		//---------------------------------------------------------------------
		[Test]
		public void Two_empty_fields___OK()
		{
			var sut = new Board();

			for (int i = 0; i < 9; ++i)
				sut[i] = FieldState.Machine;

			sut[0] = FieldState.Empty;
			sut[3] = FieldState.Empty;

			int[] actual = sut.GetEmptyFields().ToArray();

			int[] expected = { 0, 3 };
			CollectionAssert.AreEqual(expected, actual);
		}
	}
}
