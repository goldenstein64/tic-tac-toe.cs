using TicTacToe.Data;
using TicTacToe.Data.Messages;
using TicTacToe.Player;

namespace TicTacToe;

/// <summary>
/// holds all logic required to run a game of tic-tac-toe
/// </summary>
public class Application(IConnection connection, Board? board = null)
{
	public readonly IConnection Conn = connection;
	public readonly Board Board = board ?? new();

	record EndedResult(Mark? Winner);

	IPlayer ChooseComputerOnce(Mark mark)
	{
		return Conn.Prompt(new MSG_PromptComputer(mark)) switch
		{
			"E" => new EasyComputer(),
			"M" => new MediumComputer(),
			"H" => new HardComputer(),
			_ => throw new MessageException(new ERR_ComputerInvalid()),
		};
	}

	public IPlayer ChoosePlayerOnce(Mark mark)
	{
		return Conn.Prompt(new MSG_PromptPlayer(mark)) switch
		{
			"H" => new Human(Conn),
			"C" => ChooseComputerOnce(mark),
			_ => throw new MessageException(new ERR_PlayerInvalid()),
		};
	}

	public IPlayer ChoosePlayer(Mark mark) =>
		MessageException.TryUntilOk(Conn, () => ChoosePlayerOnce(mark));

	public IPlayer[] ChoosePlayers() =>
		[ChoosePlayer(Mark.X), ChoosePlayer(Mark.O)];

	EndedResult? PlayTurn(IPlayer player, Mark mark)
	{
		var move = player.GetMove(Board, mark);
		Board[move] = mark;
		return Board.Won(mark)
			? new(mark)
			: Board.Full()
				? new(null)
				: null;
	}

	public Mark? PlayGame(IPlayer[] players)
	{
		var currentIndex = 0;
		Conn.Print(new MSG_Board(Board));
		while (true)
		{
			var mark = currentIndex % 2 is 0 ? Mark.X : Mark.O;
			var player = players[currentIndex];
			var maybeResult = PlayTurn(player, mark);
			Conn.Print(new MSG_Board(Board));
			if (maybeResult is EndedResult result)
				return result.Winner;

			currentIndex = (currentIndex + 1) % players.Length;
		}
	}

	public void DisplayWinner(Mark? winner)
	{
		if (winner is Mark mark)
		{
			Conn.Print(new MSG_PlayerWon(mark));
		}
		else
		{
			Conn.Print(new MSG_Tied());
		}
	}
}
