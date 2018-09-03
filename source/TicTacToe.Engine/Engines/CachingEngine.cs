using System.Collections.Generic;

namespace TicTacToe.Engine.Engines
{
    // A decorator about another IEngine
    public class CachingEngine : IEngine
    {
        private static readonly Dictionary<int, int> _moveCache = new Dictionary<int, int>();

        private readonly IEngine _engine;
        //---------------------------------------------------------------------
        public CachingEngine(IEngine engine)
            => _engine = engine ?? throw new System.ArgumentNullException(nameof(engine));
        //---------------------------------------------------------------------
        public int FindBestMove(Board board)
        {
            int key = board.GetKey();

            if (!_moveCache.TryGetValue(key, out int move))
            {
                move = _engine.FindBestMove(board);
                _moveCache.Add(key, move);
            }

            return move;
        }
    }
}
