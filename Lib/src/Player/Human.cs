using TicTacToe.Data;
using TicTacToe.Data.Messages;

namespace TicTacToe.Player;

/// <summary>
/// a Player that prompts the user for input
/// </summary>
/// <param name="connection">a connection to I/O</param>
public class Human(IConnection connection) : IPlayer
{
	public IConnection Conn = connection;

	/// <summary>
	/// prompts the user for input and parses a move from it
	/// </summary>
	/// <param name="board">the current state of the board</param>
	/// <param name="mark">the current player's mark</param>
	/// <returns>the chosen move</returns>
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

	/// <summary>
	/// prompts the user for input and parses a move from it. If the input is
	/// invalid, print a message and prompt the user again.
	/// </summary>
	/// <param name="board">the current state of the board</param>
	/// <param name="mark">the current player's mark</param>
	/// <returns>the chosen move</returns>
	public int GetMove(Board board, Mark mark) =>
		MessageException.TryUntilOk(Conn, () => GetMoveOnce(board, mark));
}
