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
		switch (Conn.Prompt(IOMessages.MSG_PromptPlayer, mark))
		{
			case "H":
				return new Human(Conn);
			case "C":
				switch (Conn.Prompt(IOMessages.MSG_PromptComputer, mark))
				{
					case "E":
						return new EasyComputer();
					case "M":
						return new MediumComputer();
					case "H":
						return new HardComputer();
					default:
						Conn.Print(IOMessages.ERR_ComputerInvalid);
						return null;
				}
			default:
				Conn.Print(IOMessages.ERR_PlayerInvalid);
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
