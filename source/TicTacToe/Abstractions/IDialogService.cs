namespace TicTacToe.Abstractions
{
    public interface IDialogService
    {
        bool QuestionUserFirst();
        void FinalInfo(string msg);
    }
}
