using TicTacToe.Data;
using TicTacToe.Messages;

namespace TicTacToe.Player;

public class Human(IConnection connection, IConnection2 connection2) : IPlayer
{
	public IConnection Conn = connection;
	public IConnection2 Conn2 = connection2;

	public int? GetMoveOnce(Board board, Mark mark)
	{
		Conn2.Print(new MSG_PromptMove(mark));
		var @in = Conn.PromptInt(IOMessages.MSG_PromptMove, mark);
		if (@in is null)
		{
			Conn.Print(IOMessages.ERR_NotANumber);
			Conn2.Print(new ERR_NotANumber());
			return null;
		}

		var result = (int)@in;
		if (result is < 1 or > Board.Size)
		{
			Conn.Print(IOMessages.ERR_NumberOutOfRange);
			Conn2.Print(new ERR_NumberOutOfRange());
			return null;
		}

		result -= 1;

		if (!board.CanMark(result))
		{
			Conn.Print(IOMessages.ERR_SpaceOccupied);
			Conn2.Print(new ERR_SpaceOccupied());
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
