using TicTacToe.Data;

namespace TicTacToe.Player;

public class EasyComputer : IPlayer
{
	readonly Random MoveRNG = new();

	public int GetMove(Board board, Mark mark)
	{
		var moves = Enumerable
			.Range(0, Board.Size)
			.Where(board.CanMark)
			.ToArray();

		return moves[MoveRNG.Next(moves.Length)];
	}
}
