using TicTacToe.Data;
using TicTacToe.Messages;

namespace TicTacToe.Player;

public class Human(IConnection connection) : IPlayer
{
	public IConnection Conn = connection;

	class MessageException(IOMessages message, params object[] args) : Exception
	{
		public new readonly IOMessages Message = message;
		public readonly object[] Arguments = args;
	}

	int GetMoveOnce(Board board, Mark mark)
	{
		var response = Conn.Prompt(IOMessages.MSG_PromptMove, mark);

		int responseInt;
		try
		{
			responseInt = int.Parse(response);
		}
		catch (Exception e) when (e is FormatException or OverflowException)
		{
			throw new MessageException(IOMessages.ERR_NotANumber);
		}

		if (responseInt is < 1 or > 9)
			throw new MessageException(IOMessages.ERR_NumberOutOfRange);

		responseInt -= 1;
		if (!board.CanMark(responseInt))
			throw new MessageException(IOMessages.ERR_SpaceOccupied);

		return responseInt;
	}

	public int GetMove(Board board, Mark mark)
	{
		while (true)
			try
			{
				return GetMoveOnce(board, mark);
			}
			catch (MessageException e)
			{
				Conn.Print(e.Message, e.Arguments);
			}
	}
}
