using TicTacToe;
using TicTacToe.App;
using TicTacToe.Data;
using TicTacToe.Data.Messages;
using static TicTacToe.App.Util.EnumerableExtensions;

IConnection connection = new ConsoleConnection(
	(msg) =>
		msg switch
		{
			MSG_PromptPlayer(var mark)
				=> $"Is Player {mark} a player or computer? [H/C]: ",
			MSG_PromptComputer(var mark)
				=> $"What is computer {mark}'s difficulty? [E/M/H]: ",
			MSG_PlayerWon(var mark) => $"Player {mark} won!",
			MSG_Tied => "There was a tie!",
			MSG_Board(var board)
				=> string.Concat(
					board
						.Select((mark, i) => (mark, i))
						.Chunk(3)
						.Select(
							(row) =>
								string.Concat(
									row.Select( // convert each element to a string
											(t) =>
												t.mark is Mark mark
													? mark.ToString()
													: (t.i + 1).ToString() // default to index + 1
										)
										.Intersperse(" | ") // vertical separators
										.Prepend(" ") // padding on left side
										.Append(" ") // padding on right side
								)
						)
						.Intersperse("-----------") // horizontal separators
						.Intersperse("\n") // put new lines between each element
						.Append("\n\n")
				),
			ERR_PlayerInvalid => "This does not match 'H' or 'C'!",
			ERR_ComputerInvalid => "This does not match 'E', 'M' or 'H'!",
			MSG_PromptMove(var mark) => $"Pick a move, Player {mark} [1-9]: ",
			ERR_NotANumber => "This is not a valid number!",
			ERR_NumberOutOfRange => "This is not in the range of 1-9!",
			ERR_SpaceOccupied => "This space is occupied!",
			_ => throw new($"Unknown message type '{msg}'"),
		}
);

var app = new Application(connection);

var players = app.ChoosePlayers();
var winner = app.PlayGame(players);
app.DisplayWinner(winner);
