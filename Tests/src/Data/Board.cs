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
	[Fact]
	public void FalseOnBadPosition()
	{
		var empty = new Board();

		Assert.False(empty.IsMarkedWith(-1, null));
		Assert.True(empty.IsMarkedWith(0, null));
		Assert.True(empty.IsMarkedWith(1, null));
		Assert.True(empty.IsMarkedWith(7, null));
		Assert.True(empty.IsMarkedWith(8, null));
		Assert.False(empty.IsMarkedWith(9, null));
	}

	[Fact]
	public void MatchesOnMark()
	{
		var board = new Board("XO,XO,XO,");

		Assert.True(board.IsMarkedWith(0, Mark.X));
		Assert.False(board.IsMarkedWith(0, Mark.O));
		Assert.False(board.IsMarkedWith(0, null));

		Assert.False(board.IsMarkedWith(1, Mark.X));
		Assert.True(board.IsMarkedWith(1, Mark.O));
		Assert.False(board.IsMarkedWith(1, null));

		Assert.False(board.IsMarkedWith(2, Mark.X));
		Assert.False(board.IsMarkedWith(2, Mark.O));
		Assert.True(board.IsMarkedWith(2, null));
	}
}

public class CanMark
{
	[Fact]
	public void MatchesOnNull()
	{
		var board = new Board("XO,XO,XO,");

		Assert.False(board.CanMark(0));
		Assert.False(board.CanMark(1));
		Assert.True(board.CanMark(2));
		Assert.False(board.CanMark(3));
		Assert.False(board.CanMark(4));
		Assert.True(board.CanMark(5));
		Assert.False(board.CanMark(6));
		Assert.False(board.CanMark(7));
		Assert.True(board.CanMark(8));
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
