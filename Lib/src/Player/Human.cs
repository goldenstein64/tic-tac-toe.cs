using TicTacToe.Data;
using TicTacToe.Messages;

namespace TicTacToe.Player;

public class Human(IConnection connection) : IPlayer
{
	public IConnection Conn = connection;

	int? GetMoveOnce(Board board, Mark mark)
	{
		var result = Conn.PromptInt(IOMessages.MSG_PromptMove, mark);
		if (result is < 1 or > 9)
		{
			Conn.Print(IOMessages.ERR_NumberOutOfRange);
			return null;
		}

		result -= 1;

		if (!board.CanMark(result))
		{
			Conn.Print(IOMessages.ERR_SpaceOccupied);
			return null;
		}

		return result;
	}

	public int GetMove(Board board, Mark mark)
	{
		int? move = null;
		while (move is null)
			move = GetMoveOnce(board, mark);

		return (int)move;
	}
}
