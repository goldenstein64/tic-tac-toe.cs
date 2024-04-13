using TicTacToe.Data;
using TicTacToe.Messages;
using TicTacToe.Player;

namespace TicTacToe;

/// <summary>
/// The main class responsible for running a game of tic-tac-toe.
/// </summary>
public class Application(IConnection connection)
{
	public IConnection Conn = connection;

	IPlayer? RespondNull(IOMessages message, params object[] args)
	{
		Conn.Print(message, args);
		return null;
	}

	IPlayer? ChooseComputerOnce(Mark mark) =>
		Conn.Prompt(IOMessages.MSG_PromptComputer, mark) switch
		{
			"E" => new EasyComputer(),
			"M" => new MediumComputer(),
			"H" => new HardComputer(),
			_ => RespondNull(IOMessages.ERR_ComputerInvalid),
		};

	public IPlayer? ChoosePlayerOnce(Mark mark) =>
		Conn.Prompt(IOMessages.MSG_PromptPlayer, mark) switch
		{
			"H" => new Human(Conn),
			"C" => ChooseComputerOnce(mark),
			_ => RespondNull(IOMessages.ERR_PlayerInvalid),
		};

	public IPlayer ChoosePlayer(Mark mark)
	{
		IPlayer? player = null;
		while (player is null)
			player = ChoosePlayerOnce(mark);

		return player;
	}

	public IPlayer[] ChoosePlayers() =>
		[ChoosePlayer(Mark.X), ChoosePlayer(Mark.O)];

	public Mark? PlayGame(Board board, IPlayer[] players)
	{
		var currentIndex = 0;
		var currentMark = Mark.X;
		Conn.Print(IOMessages.MSG_Board, board);
		while (!board.Full())
		{
			var currentPlayer = players[currentIndex];
			var move = currentPlayer.GetMove(board, currentMark);
			board.SetMark(move, currentMark);
			Conn.Print(IOMessages.MSG_Board, board);
			if (board.Won(currentMark))
				return currentMark;

			currentIndex = (currentIndex + 1) % players.Length;
			currentMark = currentMark.Other();
		}

		return null;
	}

	public void DisplayWinner(Mark? winner)
	{
		if (winner is Mark mark)
			Conn.Print(IOMessages.MSG_PlayerWon, mark);
		else
			Conn.Print(IOMessages.MSG_Tied);
	}
}
