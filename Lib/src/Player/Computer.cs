using TicTacToe.Data;

namespace TicTacToe.Player;

public abstract class Computer : IPlayer
{
	protected readonly Random MoveRNG = new();

	public abstract List<int> GetMoves(Board board, Mark mark);

	public virtual int GetMove(Board board, Mark mark)
	{
		var moves = GetMoves(board, mark);
		if (moves.Count <= 0)
			throw new("no moves to take!");
		return moves[MoveRNG.Next(moves.Count)];
	}
}
