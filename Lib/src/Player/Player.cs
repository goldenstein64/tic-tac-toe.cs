using TicTacToe.Data;

namespace TicTacToe.Player;

/// <summary>
/// represents a player in a game of tic-tac-toe
/// </summary>
public interface IPlayer
{
	/// <summary>
	/// </summary>
	///
	/// <returns>
	/// a position on the board, represented as an integer within
	/// the range of 0-8
	/// </returns>
	int GetMove(Board board, Mark mark);
}
