using Goldenstein64.TicTacToe.Data;

namespace Goldenstein64.TicTacToe.Player;

public class EasyComputer : Computer
{
	public override List<int> GetMoves(Board board, Mark mark) =>
		Enumerable.Range(0, Board.Size).Where(board.CanMark).ToList();
}
