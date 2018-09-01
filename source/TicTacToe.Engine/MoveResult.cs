namespace TicTacToe.Engine
{
    public readonly struct MoveResult
    {
        private static readonly MoveResult _illegalMove = new MoveResult(false, false);
        //---------------------------------------------------------------------
        public static ref readonly MoveResult IllegalMove => ref _illegalMove;

        public bool MoveIsLegal { get; }
        public bool GameEnded   { get; }
        //---------------------------------------------------------------------
        public MoveResult(bool moveIsLegal, bool gameEnded)
        {
            this.MoveIsLegal = moveIsLegal;
            this.GameEnded   = gameEnded;
        }
    }
}
