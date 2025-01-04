using TicTacToe.Data;
using TicTacToe.Player;
using TicTacToe.Util.EnumerableExtensions;

namespace TicTacToe.Tests.Player.HardComputerTests;

public class UsesHardComputer
{
	public HardComputer Subject = new();
}

public class Terminal
{
	[Fact]
	public void FullBoard_ReturnsZero()
	{
		var board = new Board("OXXXOOOXX");
		var terminal = HardComputer.Terminal(board);
		Assert.Equal(0, terminal);
	}

	[Fact]
	public void BoardThatXWon_ReturnsOne()
	{
		var board = new Board("X,,XOOX,,");
		var terminal = HardComputer.Terminal(board);
		Assert.Equal(1, terminal);
	}

	[Fact]
	public void FullBoardThatXWon_ReturnsOne()
	{
		var board = new Board("XXXXOOOXO");
		var terminal = HardComputer.Terminal(board);
		Assert.Equal(1, terminal);
	}

	[Fact]
	public void BoardThatOWon_ReturnsNegOne()
	{
		var board = new Board("O,,OXXO,X");
		var terminal = HardComputer.Terminal(board);
		Assert.Equal(-1, terminal);
	}

	[Fact]
	public void FullBoardThatOWon_ReturnsNegOne()
	{
		var board = new Board("XXOXOXOOX");
		var terminal = HardComputer.Terminal(board);
		Assert.Equal(-1, terminal);
	}

	[Fact]
	public void InProgressBoardReturnsNull()
	{
		var board = new Board("XX,OO,XX,");
		var terminal = HardComputer.Terminal(board);
		Assert.Null(terminal);
	}
}

public class ResultOf
{
	static readonly Random RNG = new();

	public static IEnumerable<object[]> GenerateData(int trials)
	{
		var currentTrials = 0;
		while (currentTrials < trials)
		{
			var initialValues = RNG.GetItems<Mark?>(
				[Mark.X, Mark.O, null],
				Board.Size
			);

			var emptyIndexes = initialValues.IndexesWhere((m) => m is null).ToArray();
			if (emptyIndexes.Length == 0)
				continue;

			Assert.InRange(emptyIndexes.Length, 1, Board.Size);
			var chosenIndex = emptyIndexes[RNG.Next(emptyIndexes.Length)];

			Mark?[] expectedValues = [.. initialValues]; // cloning the array
			var chosenMark = RNG.Next(2) == 0 ? Mark.X : Mark.O;
			expectedValues[chosenIndex] = chosenMark;

			var initial = new Board(initialValues);
			var expected = new Board(expectedValues);

			yield return [initial, chosenMark, chosenIndex, expected];
			currentTrials++;
		}
	}

	[Theory]
	[MemberData(nameof(GenerateData), 50)]
	public void IsCalculatedCorrectly(
		Board initial,
		Mark chosenMark,
		int chosenIndex,
		Board expected
	)
	{
		var actual = HardComputer.ResultOf(initial, chosenMark, chosenIndex);

		Assert.Equal(expected, actual);
	}
}
