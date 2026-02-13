using Goldenstein64.TicTacToe.Data;

namespace Goldenstein64.TicTacToe.Player;

/// <summary>
/// defines a player in a tic-tac-toe game
/// </summary>
public interface IPlayer
{
	/// <summary>
	/// asks for a move to make
	/// </summary>
	///
	/// <returns>
	/// a position on the board, represented as an integer within
	/// the range of 0-8
	/// </returns>
	int GetMove(Board board, Mark mark);
}
