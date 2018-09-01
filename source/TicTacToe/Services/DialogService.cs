using System.Windows;
using TicTacToe.Abstractions;

namespace TicTacToe.Services
{
    class DialogService : IDialogService
    {
        public bool QuestionUserFirst()
        {
            MessageBoxResult res = MessageBox.Show(
                "Would you like to go first?",
                App.Current.MainWindow.Title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return res == MessageBoxResult.Yes;
        }
        //---------------------------------------------------------------------
        public void FinalInfo(string msg)
        {
            MessageBox.Show(
                msg + "\n\nPress OK to restart.",
                App.Current.MainWindow.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
