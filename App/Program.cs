using TicTacToe;
using TicTacToe.Data;
using TicTacToe.Messages;

IConnection connection = new ConsoleConnection(
	new()
	{
		[IOMessages.ERR_PlayerInvalid] = "This does not match 'H' or 'C'!",
		[IOMessages.MSG_PromptPlayer] =
			"Is Player {0} a player or computer? [H/C]: ",

		[IOMessages.ERR_ComputerInvalid] =
			"This does not match 'E', 'M' or 'H'!",
		[IOMessages.MSG_PromptComputer] =
			"What is computer {0}'s difficulty? [E/M/H]: ",

		[IOMessages.MSG_PlayerWon] = "Player {0} won!",
		[IOMessages.MSG_Tied] = "There was a tie!",

		[IOMessages.MSG_Board] = "{0}",

		[IOMessages.MSG_PromptMove] = "Pick a move, Player {0} [1-9]: ",
		[IOMessages.ERR_NotANumber] = "This is not a valid number!",
		[IOMessages.ERR_NumberOutOfRange] = "This is not in the range of 1-9!",
		[IOMessages.ERR_SpaceOccupied] = "This space is occupied!",
	}
);

var app = new Application(connection);

var players = app.ChoosePlayers();
var winner = app.PlayGame(new Board(), players);
app.DisplayWinner(winner);
