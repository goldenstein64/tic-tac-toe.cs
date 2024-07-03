using TicTacToe;
using TicTacToe.Data;
using TicTacToe.Messages;

IConnection2 connection2 = new ConsoleConnection2(
	(msg) =>
		msg switch
		{
			MSG_PromptPlayer(var mark)
				=> $"Is Player {mark} a player or computer? [H/C]: ",
			MSG_PromptComputer(var mark)
				=> $"What is computer {mark}'s difficulty? [E/M/H]: ",
			MSG_PlayerWon(var mark) => $"Player {mark} won!",
			MSG_Tied => "There was a tie!",
			MSG_Board(var board) => $"{board}\n",
			ERR_PlayerInvalid => "This does not match 'H' or 'C'!",
			ERR_ComputerInvalid => "This does not match 'E', 'M' or 'H'!",
			MSG_PromptMove(var mark) => $"Pick a move, Player {mark} [1-9]: ",
			ERR_NotANumber => "This is not a valid number!",
			ERR_NumberOutOfRange => "This is not in the range of 1-9!",
			ERR_SpaceOccupied => "This space is occupied!",
			_ => throw new($"Unknown message type '{msg}'"),
		}
);

IConnection connection = new ConsoleConnection(
	new()
	{
		[IOMessages.ERR_PlayerInvalid] = "This does not match 'H' or 'C'!",
		[IOMessages.MSG_PromptPlayer] =
			"Is Player {0} a player or computer? [H/C]: ",

		[IOMessages.ERR_ComputerInvalid] = "This does not match 'E', 'M' or 'H'!",
		[IOMessages.MSG_PromptComputer] =
			"What is computer {0}'s difficulty? [E/M/H]: ",

		[IOMessages.MSG_PlayerWon] = "Player {0} won!",
		[IOMessages.MSG_Tied] = "There was a tie!",

		[IOMessages.MSG_Board] = "{0}\n",

		[IOMessages.MSG_PromptMove] = "Pick a move, Player {0} [1-9]: ",
		[IOMessages.ERR_NotANumber] = "This is not a valid number!",
		[IOMessages.ERR_NumberOutOfRange] = "This is not in the range of 1-9!",
		[IOMessages.ERR_SpaceOccupied] = "This space is occupied!",
	}
);

var app = new Application(connection, connection2);

var players = app.ChoosePlayers();
var winner = app.PlayGame(new Board(), players);
app.DisplayWinner(winner);
