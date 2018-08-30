using System;

namespace TicTacToe.Engine
{
	internal class GameDebugView
	{
		private readonly Game _game;
		//---------------------------------------------------------------------
		public GameDebugView(Game game) => _game = game;
		//---------------------------------------------------------------------
		public string Fields =>
			$"{this[0]}|{this[1]}|{this[2]}\n" +
			"-+-+-\n" +
			$"{this[3]}|{this[4]}|{this[5]}\n" +
			"-+-+-\n" +
			$"{this[6]}|{this[7]}|{this[8]}";
		//---------------------------------------------------------------------
		private string this[int index] => this.GetFieldSign(index);
		//---------------------------------------------------------------------
		private string GetFieldSign(int index)
		{
			FieldState fieldState = _game.Fields[index];

			switch (fieldState)
			{
				case FieldState.Empty:
					return " ";
				case FieldState.Machine:
					return "O";
				case FieldState.User:
					return "X";
				default:
					throw new NotSupportedException();
			}
		}
	}
}
