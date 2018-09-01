using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TicTacToe.Engine
{
	public class Board
	{
		private int _user;
		private int _machine;
		//---------------------------------------------------------------------
		internal int UserFields	   => _user;
		internal int MachineFields => _machine;
		internal int SetFields 	   => _user | _machine;
		//---------------------------------------------------------------------
		public FieldState this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (Get(_machine, index)) return FieldState.Machine;
				if (Get(_user, index))    return FieldState.User;
				return FieldState.Empty;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (value == FieldState.Machine)
				{
					Set(ref _machine, index);
					UnSet(ref _user, index);
				}
				else if (value == FieldState.User)
				{
					Set(ref _user, index);
					UnSet(ref _machine, index);
				}
				else
				{
					UnSet(ref _machine, index);
					UnSet(ref _user, index);
				}
			}
		}
		//---------------------------------------------------------------------
		public bool IsMoveLegal(int index) => this[index] == FieldState.Empty;
		//---------------------------------------------------------------------
		public IEnumerable<int> GetEmptyFields()
		{
			for (int i = 0; i < 9; ++i)
				if (this[i] == FieldState.Empty) yield return i;
		}
		//---------------------------------------------------------------------
		private static bool Get(int value, int index) 		=> ((value >> index) & 1) > 0;
		private static void Set(ref int value, int index) 	=> value |= 1 << index;
		private static void UnSet(ref int value, int index) => value &= ~(1 << index);
	}
}
