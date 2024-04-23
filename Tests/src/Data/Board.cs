using TicTacToe.Data;

namespace TicTacToe.Tests.Data.BoardTests;

public class Won
{
	[Theory]
	[InlineData(["XXX,,,,,,"])]
	[InlineData([",,,XXX,,,"])]
	[InlineData([",,,,,,XXX"])]
	[InlineData(["X,,X,,X,,"])]
	[InlineData([",X,,X,,X,"])]
	[InlineData([",,X,,X,,X"])]
	[InlineData(["X,,,X,,,X"])]
	[InlineData([",,X,X,X,,"])]
	public void DetectsAllLegalMatches(string pattern)
	{
		var oPattern = pattern.Replace('X', 'O');
		var xBoard = new Board(pattern);
		var oBoard = new Board(oPattern);

		Assert.True(xBoard.Won(Mark.X));
		Assert.True(oBoard.Won(Mark.O));
		Assert.False(xBoard.Won(Mark.O));
		Assert.False(oBoard.Won(Mark.X));
	}

	[Theory]
	[InlineData(["XXOOOXXXO"])]
	[InlineData(["XXOOOXXOX"])]
	[InlineData(["XXOOXXXOO"])]
	public void DetectsNoMatch(string pattern)
	{
		var board = new Board(pattern);
		Assert.False(board.Won(Mark.X));
		Assert.False(board.Won(Mark.O));
	}
}

public class Empty
{
	[Fact]
	public void DetectsEmpty()
	{
		Assert.True(new Board().Empty());
		Assert.True(new Board(",,,,,,,,,").Empty());
	}

	[Theory]
	[InlineData(["XO,XO,XO,"])]
	[InlineData(["XXXXXXXX,"])]
	[InlineData([",XXXXXXXX"])]
	[InlineData(["X,,,,,,,,"])]
	[InlineData([",,,,,,,,X"])]
	public void DetectsNonEmpty(string pattern)
	{
		Assert.False(new Board(pattern).Empty());
	}
}

public class Full
{
	[Theory]
	[InlineData(["XXOOOXXXO"])]
	[InlineData(["XXOOOXXOX"])]
	[InlineData(["XXOOXXXOO"])]
	public void DetectsFull(string pattern)
	{
		Assert.True(new Board(pattern).Full());
	}

	[Theory]
	[InlineData(["XO,XO,XO,"])]
	[InlineData(["XXXXXXXX,"])]
	[InlineData([",XXXXXXXX"])]
	[InlineData(["X,,,,,,,,"])]
	[InlineData([",,,,,,,,X"])]
	public void DetectsNotFull(string pattern)
	{
		Assert.False(new Board(pattern).Full());
	}
}

public class IsMarkedWith
{
	[Theory]
	[InlineData([-1, false])]
	[InlineData([0, true])]
	[InlineData([1, true])]
	[InlineData([7, true])]
	[InlineData([8, true])]
	[InlineData([9, false])]
	public void FalseOnBadPosition(int pos, bool expected)
	{
		Assert.Equal(expected, new Board().IsMarkedWith(pos, null));
	}

	[Theory]
	[InlineData([0, Mark.X, true])]
	[InlineData([0, Mark.O, false])]
	[InlineData([0, null, false])]
	[InlineData([1, Mark.X, false])]
	[InlineData([1, Mark.O, true])]
	[InlineData([1, null, false])]
	[InlineData([2, Mark.X, false])]
	[InlineData([2, Mark.O, false])]
	[InlineData([2, null, true])]
	public void MatchesOnMark(int pos, Mark? mark, bool expected)
	{
		var board = new Board("XO,XO,XO,");
		Assert.Equal(expected, board.IsMarkedWith(pos, mark));
	}
}

public class CanMark
{
	[Theory]
	[InlineData([0, false])]
	[InlineData([1, false])]
	[InlineData([2, true])]
	[InlineData([3, false])]
	[InlineData([4, false])]
	[InlineData([5, true])]
	[InlineData([6, false])]
	[InlineData([7, false])]
	[InlineData([8, true])]
	public void MatchesOnNull(int pos, bool expected)
	{
		var board = new Board("XO,XO,XO,");
		Assert.Equal(expected, board.CanMark(pos));
	}
}

public class IndexOperator
{
	[Fact]
	public void ThrowsOnOccupied()
	{
		var board = new Board(",,X,,,,,,");

		Assert.Throws<ArgumentException>(() => board[2] = Mark.O);

		board[1] = Mark.O;
	}

	[Fact]
	public void ThrowsOnBadPosition()
	{
		var board = new Board(",,,,,,,,,");

		Assert.Throws<IndexOutOfRangeException>(() => board[-1] = Mark.X);

		board[0] = Mark.X;
	}

	[Fact]
	public void ChangesState()
	{
		for (var i = 0; i < Board.Size; i++)
		{
			var board = new Board();
			Assert.True(board.CanMark(i));
			board[i] = Mark.X;
			Assert.False(board.CanMark(i));
		}
	}
}
