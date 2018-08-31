using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TicTacToe.Engine.Tests.GameTests
{
	[TestFixture]
	public class CheckWinner
	{
		[Test]
		public void Start___no_winner()
		{
			var sut = new Game();

			sut.CheckWinner();

			Assert.AreEqual(Winner.None, sut.Winner);
		}
		//---------------------------------------------------------------------
		[Test]
		public void Only_two_rounds___no_winner()
		{
			var sut   = new Game();
			var board = sut.Board;

			board[0] = FieldState.User;
			board[1] = FieldState.User;
			board[4] = FieldState.Machine;

			sut.CheckWinner();

			Assert.AreEqual(Winner.None, sut.Winner);
		}
		//---------------------------------------------------------------------
		[Test]
		[TestCaseSource(nameof(User_wins___winner_set_TestCases))]
		public void User_wins___winner_set(FieldState[] fieldStates)
		{
			var sut   = new Game();
			var board = sut.Board;

			for (int i = 0; i < 9; ++i)
				board[i] = fieldStates[i];

			sut.CheckWinner();

			Assert.AreEqual(Winner.User, sut.Winner);
		}
		//---------------------------------------------------------------------
		private static IEnumerable<TestCaseData> User_wins___winner_set_TestCases()
		{
			const string pattern = "WWW......|...WWW...|......WWW|W..W..W..|.W..W..W.|..W..W..W|W...W...W|..W.W.W..";
			var rnd 			 = new Random(0);

			foreach (string pat in pattern.Split('|'))
			{
				var fieldStates = new FieldState[9];

				for (int i = 0; i < 9; ++i)
				{
					char c = pat[i];

					if (c == 'W')
						fieldStates[i] = FieldState.User;
					else
						fieldStates[i] = (FieldState)rnd.Next(-1, 1);
				}

				yield return new TestCaseData(fieldStates);
			}
		}
		//---------------------------------------------------------------------
		[Test]
		[TestCaseSource(nameof(Machine_wins___winner_set_TestCases))]
		public void Machine_wins___winner_set(FieldState[] fieldStates)
		{
			var sut   = new Game();
			var board = sut.Board;

			for (int i = 0; i < 9; ++i)
				board[i] = fieldStates[i];

			sut.CheckWinner();

			Assert.AreEqual(Winner.Machine, sut.Winner);
		}
		//---------------------------------------------------------------------
		private static IEnumerable<TestCaseData> Machine_wins___winner_set_TestCases()
		{
			const string pattern = "WWW......|...WWW...|......WWW|W..W..W..|.W..W..W.|..W..W..W|W...W...W|..W.W.W..";
			var rnd 			 = new Random(0);

			foreach (string pat in pattern.Split('|'))
			{
				var fieldStates = new FieldState[9];

				for (int i = 0; i < 9; ++i)
				{
					char c = pat[i];

					if (c == 'W')
						fieldStates[i] = FieldState.Machine;
					else
						fieldStates[i] = (FieldState)rnd.Next(0, 2);
				}

				yield return new TestCaseData(fieldStates);
			}
		}
	}
}
