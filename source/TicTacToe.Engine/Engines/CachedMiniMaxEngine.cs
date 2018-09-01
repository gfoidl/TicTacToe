using System.Collections.Generic;

namespace TicTacToe.Engine.Engines
{
    public class CachedMiniMaxEngine : MiniMaxEngine
    {
        private static readonly Dictionary<int, int> _moveCache = new Dictionary<int, int>();
        //---------------------------------------------------------------------
        public override int FindBestMove(Board board)
        {
            int key = board.GetKey();

            if (!_moveCache.TryGetValue(key, out int move))
            {
                move = base.FindBestMove(board);
                _moveCache.Add(key, move);
            }

            return move;
        }
    }
}
