using TicTacToe.Data;
using TicTacToe.Data.Messages;

namespace TicTacToe.Player;

public class Human(IConnection connection) : IPlayer
{
	public IConnection Conn = connection;

	public int GetMoveOnce(Board board, Mark mark)
	{
		var result =
			Conn.PromptInt(new MSG_PromptMove(mark))
			?? throw new MessageException(new ERR_NotANumber());

		if (result is < 1 or > Board.Size)
			throw new MessageException(new ERR_NumberOutOfRange());

		result -= 1;
		if (!board.CanMark(result))
			throw new MessageException(new ERR_SpaceOccupied());

		return result;
	}

	public int GetMove(Board board, Mark mark) =>
		MessageException.TryUntilOk(Conn, () => GetMoveOnce(board, mark));
}
