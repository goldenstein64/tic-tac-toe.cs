using TicTacToe.Data;
using TicTacToe.Util.EnumerableExtensions;

namespace TicTacToe.Player;

public class HardComputer : IPlayer
{
	readonly Random MoveRNG = new();

	static readonly (int a, int b)[] Equalities =
	[
		(0, 2),
		(3, 5),
		(6, 8),
		(0, 6),
		(1, 7),
		(2, 8),
		(1, 3),
		(2, 6),
		(5, 7),
		(3, 7),
		(0, 8),
		(1, 5),
	];

	readonly struct Symmetry(IEnumerable<int> equalities, IEnumerable<int> image)
	{
		public readonly IEnumerable<int> Equalities = equalities;
		public readonly IEnumerable<int> Image = image;
	}

	static readonly Symmetry[] Symmetries =
	[
		new( // rotate 90
			equalities: [0, 1, 2, 6, 7, 8],
			image: [0, 1, 4]
		),
		new( // rotate 180
			equalities: [1, 4, 7, 10],
			image: [0, 1, 2, 3, 4]
		),
		new( // vertical
			equalities: [0, 1, 2],
			image: [0, 1, 3, 4, 6, 7]
		),
		new( // horizontal
			equalities: [3, 4, 5],
			image: [0, 1, 2, 3, 4, 5]
		),
		new( // diagonal down
			equalities: [6, 7, 8],
			image: [0, 1, 2, 3, 4, 6]
		),
		new( // diagonal up
			equalities: [9, 10, 11],
			image: [0, 1, 2, 4, 5, 8]
		),
	];

	static int Controls(Mark mark) =>
		mark switch
		{
			Mark.X => -1,
			Mark.O => 1,
			_ => throw new(),
		};

	static Func<int, int, int> Reconcilers(Mark mark) =>
		mark switch
		{
			Mark.X => Math.Max,
			Mark.O => Math.Min,
			_ => throw new(),
		};

	static bool SymmetryMatches(
		HashSet<int> equalSet,
		IEnumerable<int> symmetry
	) => symmetry.All(equalSet.Contains);

	static IEnumerable<int> FilterImage(Board board, IEnumerable<int> image) =>
		image.Where(board.CanMark);

	static HashSet<int> GetEqualitySet(Board board) =>
		Equalities.IndexesWhere((eq) => board[eq.a] == board[eq.b]).ToHashSet();

	static IEnumerable<int>? SymmetricActions(Board board)
	{
		var equalitySet = GetEqualitySet(board);
		var maybeMatchedSymmetry = Symmetries.FirstOrNullStruct(
			(sym) => SymmetryMatches(equalitySet, sym.Equalities)
		);

		if (maybeMatchedSymmetry is not Symmetry matchedSymmetry)
			return null;

		return FilterImage(board, matchedSymmetry.Image);
	}

	static IEnumerable<int> SimpleActions(Board board) =>
		Enumerable.Range(0, Board.Size).Where(board.CanMark);

	IEnumerable<int> Actions(Board board) =>
		SymmetricActions(board) ?? SimpleActions(board);

	public static Board ResultOf(Board board, Mark mark, int action) =>
		new(board) { [action] = mark };

	public static int? Terminal(Board board)
	{
		if (board.Won(Mark.X))
			return 1;
		else if (board.Won(Mark.O))
			return -1;
		else if (board.Full())
			return 0;

		return null;
	}

	int Judge(Board board, Mark mark)
	{
		var maybeTerminal = Terminal(board);
		if (maybeTerminal is int terminal)
			return terminal;

		var value = Controls(mark);
		var reconcile = Reconcilers(mark);
		var otherMark = mark.Other();
		foreach (var action in Actions(board))
		{
			var newBoard = ResultOf(board, mark, action);
			value = reconcile(value, Judge(newBoard, otherMark));
		}

		return value;
	}

	public List<int> GetMoves(Board board, Mark mark)
	{
		var otherMark = mark.Other();

		var actions = SimpleActions(board);
		var scores = actions
			.Select((action) => ResultOf(board, mark, action))
			.Select((newBoard) => Judge(newBoard, otherMark));

		var reconcile = Reconcilers(mark);
		var bestScore = scores.Aggregate(Controls(mark), reconcile);
		var bestMoves = actions
			.Zip(scores)
			.Cast<(int action, int score)>()
			.Where((tup) => tup.score == bestScore)
			.Select((tup) => tup.action)
			.ToList();

		return bestMoves;
	}

	public int GetMove(Board board, Mark mark)
	{
		if (board.Empty())
		{
			return MoveRNG.Next(Board.Size);
		}
		else
		{
			var moves = GetMoves(board, mark);
			if (moves.Count <= 0)
				throw new("no moves to take!");
			return moves[MoveRNG.Next(moves.Count)];
		}
	}
}
