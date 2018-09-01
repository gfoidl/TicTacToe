using System.Collections.Generic;

namespace TicTacToe.Engine
{
	public readonly struct Board
	{
		private readonly FieldState[] _fields;
		//---------------------------------------------------------------------
		public ref FieldState Fields => ref _fields[0];
		//---------------------------------------------------------------------
		public Board(bool init) : this(new FieldState[9]) { }
		public Board(FieldState[] fields) => _fields = fields;
		//---------------------------------------------------------------------
		public FieldState this[int index]
		{
			get => _fields[index];
			set => _fields[index] = value;
		}
		//---------------------------------------------------------------------
		public bool IsMoveLegal(int index) => this[index] == FieldState.Empty;
		//---------------------------------------------------------------------
		public IEnumerable<int> GetEmptyFields()
		{
			FieldState[] fields = _fields;
			for (int i = 0; i < fields.Length; ++i)
				if (fields[i] == FieldState.Empty) yield return i;
		}
	}
}
