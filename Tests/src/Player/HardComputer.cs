using TicTacToe.Data;
using TicTacToe.Messages.EnumerableExtensions;
using TicTacToe.Player;

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
}

public class ResultOf
{
	static readonly Random RNG = new();

	public static IEnumerable<object[]> GenerateData(int trials)
	{
		var currentTrials = 0;
		while (currentTrials < trials)
		{
			var initialValues = Enumerable
				.Range(0, Board.Size)
				.Select<int, Mark?>(
					(_) =>
						RNG.Next(3) switch
						{
							0 => Mark.X,
							1 => Mark.O,
							_ => null,
						}
				);

			var emptyValues = initialValues.IndexesWhere((m) => m is null);
			if (!emptyValues.Any())
				continue;

			var emptyValuesArray = emptyValues.ToArray();
			Assert.InRange(emptyValuesArray.Length, 1, 9);
			var chosenIndex = emptyValuesArray[
				RNG.Next(emptyValuesArray.Length)
			];

			var initialValuesArray = initialValues.ToArray();
			var chosenMark = RNG.Next(2) == 0 ? Mark.X : Mark.O;
			initialValuesArray[chosenIndex] = chosenMark;

			var initial = new Board(initialValues);
			var expected = new Board(initialValuesArray);

			yield return [initial, chosenIndex, chosenMark, expected];
			currentTrials++;
		}
	}

	[Theory]
	[MemberData(nameof(GenerateData), 50)]
	public void GetsCalculatedCorrectly(
		Board initial,
		int chosenIndex,
		Mark chosenMark,
		Board expected
	)
	{
		var actual = HardComputer.ResultOf(initial, chosenMark, chosenIndex);

		Assert.Equal(expected, actual);
	}
}
