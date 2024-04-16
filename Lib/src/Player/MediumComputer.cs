using TicTacToe.Data;
using TicTacToe.Util.EnumerableExtensions;

namespace TicTacToe.Player;

public class MediumComputer : IPlayer
{
	static readonly int[][] WinPatternMap = Enumerable
		.Range(0, 9)
		.Select(
			(i) =>
				Board
					.WinPatterns.IndexesWhere((pattern) => pattern.Contains(i))
					.ToArray()
		)
		.ToArray();

	readonly Random MoveRNG = new();

	const int Center = 4;
	static readonly int[] Corners = [0, 2, 6, 8];
	static readonly int[] Sides = [1, 3, 5, 7];

	static List<int>? GetWinningMoves(Board board, Mark mark)
	{
		var otherMark = mark.Other();
		var result = new List<int>();
		foreach (var pattern in Board.WinPatterns)
		{
			var boardPattern = pattern.Select((i) => board[i]);
			if (boardPattern.Contains(otherMark))
				continue;

			int markCount = 0;
			int? emptyIndex = null;

			foreach (var i in pattern)
			{
				var found = board[i];
				if (found == mark)
					markCount += 1;
				else if (found is null)
					emptyIndex = i;
				else
					goto NextPattern;
			}

			if (markCount == 2 && emptyIndex is not null)
				result.Add(emptyIndex.Value);

			NextPattern: { }
		}

		return result.Count > 0 ? result : null;
	}

	static List<int>? GetBlockingMoves(Board board, Mark mark) =>
		GetWinningMoves(board, mark.Other());

	static List<int>? GetTrappingMoves(Board board, Mark mark)
	{
		var result = new List<int>();
		for (var i = 0; i < Board.Size; i++)
		{
			if (board[i] != null)
				continue;

			var trapCount = 0;
			foreach (var patternIndex in WinPatternMap[i])
			{
				var pattern = Board.WinPatterns[patternIndex];

				var hasEmpty = false;
				var hasMark = false;
				foreach (var j in pattern.Where((j) => j != i))
				{
					var found = board[j];
					if (found == mark)
						hasMark = true;
					else if (found is null)
						hasEmpty = true;
					else
						goto NextWinPattern;
				}

				if (hasEmpty && hasMark)
					trapCount += 1;

				NextWinPattern: { }
			}

			if (trapCount > 1)
				result.Add(i);
		}

		return result.Count > 0 ? result : null;
	}

	static List<int>? GetCenterMove(Board board, Mark mark) =>
		board.CanMark(Center) ? [Center] : null;

	static List<int>? GetCornerMoves(Board board, Mark mark)
	{
		var result = Corners.Where(board.CanMark).ToList();
		return result.Count > 0 ? result : null;
	}

	static List<int>? GetSideMoves(Board board, Mark mark)
	{
		var result = Sides.Where(board.CanMark).ToList();
		return result.Count > 0 ? result : null;
	}

	public int GetMove(Board board, Mark mark)
	{
		var moves =
			GetWinningMoves(board, mark)
			?? GetBlockingMoves(board, mark)
			?? GetTrappingMoves(board, mark)
			?? GetCenterMove(board, mark)
			?? GetCornerMoves(board, mark)
			?? GetSideMoves(board, mark)
			?? throw new("no moves to take!");

		return moves[MoveRNG.Next(moves.Count)];
	}
}
