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

	public IPlayer? ChoosePlayer(Mark mark)
	{
		try
		{
			return Conn.Prompt(IOMessages.MSG_PromptPlayer, mark) switch
			{
				"H" => new Human(Conn),
				"C"
					=> Conn.Prompt(IOMessages.MSG_PromptComputer, mark) switch
					{
						"E" => new EasyComputer(),
						"M" => new MediumComputer(),
						"H" => new HardComputer(),
						_
							=> throw new MessageException(
								IOMessages.ERR_ComputerInvalid
							),
					},
				_ => throw new MessageException(IOMessages.ERR_PlayerInvalid),
			};
		}
		catch (MessageException e)
		{
			Conn.Print(e.IOMessage, e.Arguments);
			return null;
		}
	}

	public IPlayer[] ChoosePlayers()
	{
		var currentMark = Mark.X;
		var result = new IPlayer[2];
		for (var i = 0; i < result.Length; i++)
		{
			IPlayer? chosen = ChoosePlayer(currentMark);
			while (chosen is null)
				chosen = ChoosePlayer(currentMark);
			result[i] = chosen;
			currentMark = currentMark.Other();
		}
		return result;
	}

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
