namespace Goldenstein64.TicTacToe.Data;

public class Board : IEnumerable<Mark?>
{
	public const int Size = 9;

	readonly Mark?[] Data = new Mark?[Size];

	public Board() => Data = Enumerable.Repeat<Mark?>(null, Size).ToArray();

	public Board(string pattern) =>
		Data = pattern
			.Take(Size)
			.Select<char, Mark?>(
				(c) =>
					c switch
					{
						'X' => Mark.X,
						'O' => Mark.O,
						_ => null,
					}
			)
			.ToArray();

	public Board(IEnumerable<Mark?> data)
	{
		Data = data.Take(Size).ToArray();
	}

	public Mark? this[int index]
	{
		get => Data[index];
		set
		{
			if (Data[index] is not null && value is not null)
				throw new ArgumentException("This position is already marked!");

			Data[index] = value;
		}
	}

	public IEnumerator<Mark?> GetEnumerator() =>
		Data.AsEnumerable().GetEnumerator();

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
		GetEnumerator();

	public static readonly int[][] WinPatterns =
	[
		[0, 1, 2], // 0
		[3, 4, 5], // 1
		[6, 7, 8], // 2
		[0, 3, 6], // 3
		[1, 4, 7], // 4
		[2, 5, 8], // 5
		[2, 4, 6], // 6
		[0, 4, 8], // 7
	];

	public bool PatternWon(Mark mark, int[] pattern) =>
		pattern.All(pos => Data[pos] == mark);

	public bool Won(Mark mark) =>
		WinPatterns.Any(pattern => PatternWon(mark, pattern));

	public bool Full() => Data.All(mark => mark is not null);

	public bool Empty() => Data.All(mark => mark is null);

	public bool IsMarkedWith(int pos, Mark? mark) =>
		pos is >= 0 and < Size && Data[pos] == mark;

	public bool CanMark(int pos) => IsMarkedWith(pos, null);
}
