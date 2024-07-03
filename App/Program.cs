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

var app = new Application(connection2);

var players = app.ChoosePlayers();
var winner = app.PlayGame(new Board(), players);
app.DisplayWinner(winner);
