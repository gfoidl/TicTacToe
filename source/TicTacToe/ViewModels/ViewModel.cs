using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TicTacToe.Abstractions;
using TicTacToe.Engine;
using TicTacToe.Services;

namespace TicTacToe.ViewModels
{
	public class ViewModel : INotifyPropertyChanged
	{
		private readonly IDialogService _dialogService;
		private Game 					_game;
		//---------------------------------------------------------------------
		public Board Board => _game.Board;
		//---------------------------------------------------------------------
		private bool _isBusy;
		public bool IsBusy
		{
			get => _isBusy;
			set
			{
				if (_isBusy == value) return;

				_isBusy = value;
				this.OnPropertyChanged(nameof(this.IsBusy));
			}
		}
		//---------------------------------------------------------------------
		public ViewModel() : this(new DialogService()) { }
		//---------------------------------------------------------------------
		public ViewModel(IDialogService dialogService)
		{
			_dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

			this.Start();
		}
		//---------------------------------------------------------------------
		private void Start()
		{
			bool userFirst = _dialogService.QuestionUserFirst();
			_game 		   = new Game();
			this.FieldsChanged();

			if (!userFirst)
			{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				this.MachineMoveAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}
		//---------------------------------------------------------------------
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
			=> this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		//---------------------------------------------------------------------
		private void FieldsChanged() => this.OnPropertyChanged(nameof(this.Board));
		//---------------------------------------------------------------------
		private RelayCommand _userMoveCommand;
		public ICommand UserMoveCommand => _userMoveCommand ?? (_userMoveCommand = new RelayCommand(this.FieldTicked));
		//---------------------------------------------------------------------
		private void FieldTicked(object field)
		{
			if (this.IsBusy) return;

			int fieldIdx = int.Parse(field as string);
			MoveResult result = this.UserMove(fieldIdx);

			if (result.GameEnded)
				this.FinalInfo();
			else if (!result.MoveIsLegal)
			{

			}
			else
			{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				this.MachineMoveAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}
		//---------------------------------------------------------------------
		private MoveResult UserMove(int fieldIdx)
		{
			MoveResult result = _game.MakeMove(fieldIdx);
			this.FieldsChanged();

			return result;
		}
		//---------------------------------------------------------------------
		private async Task MachineMoveAsync()
		{
			this.IsBusy 	  = true;
			MoveResult result = await Task.Run(() => _game.MakeMachineMove());
			this.IsBusy 	  = false;
			this.FieldsChanged();

			if (result.GameEnded) this.FinalInfo();
		}
		//---------------------------------------------------------------------
		private void FinalInfo()
		{
			Winner winner = _game.Winner;

			if (winner == Winner.None) return;

			string msg;

			if (winner == Winner.User)
				msg = "You won. This should not be possible!";
			else if (winner == Winner.Machine)
				msg = "AI triumphed.";
			else
				msg = "It was draw.";

			_dialogService.FinalInfo(msg);
			this.Start();
		}
	}
}
