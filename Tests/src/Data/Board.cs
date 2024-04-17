using TicTacToe.Data;

namespace TicTacToe.Tests.Data.BoardTests;

public class Won
{
	[Fact]
	public void DetectsAllLegalMatches()
	{
		string[] winPatterns =
		[
			"XXX,,,,,,",
			",,,XXX,,,",
			",,,,,,XXX",
			"X,,X,,X,,",
			",X,,X,,X,",
			",,X,,X,,X",
			"X,,,X,,,X",
			",,X,X,X,,",
		];

		foreach (var pattern in winPatterns)
		{
			var oPattern = pattern.Replace('X', 'O');
			var xBoard = new Board(pattern);
			var oBoard = new Board(oPattern);
			Assert.True(xBoard.Won(Mark.X));
			Assert.True(oBoard.Won(Mark.O));
			Assert.False(xBoard.Won(Mark.O));
			Assert.False(oBoard.Won(Mark.X));
		}
	}

	[Fact]
	public void DetectsNoMatch()
	{
		string[] tiePatterns = ["XXOOOXXXO", "XXOOOXXOX", "XXOOXXXOO",];

		foreach (var pattern in tiePatterns)
		{
			var board = new Board(pattern);
			Assert.False(board.Won(Mark.X));
			Assert.False(board.Won(Mark.O));
		}
	}
}

public class Empty
{
	[Fact]
	public void DetectsEmpty()
	{
		Assert.True(new Board(",,,,,,,,,").Empty());
	}

	[Fact]
	public void DetectsNonEmpty()
	{
		Assert.False(new Board("XO,XO,XO,").Empty());
		Assert.False(new Board("XXXXXXXX,").Empty());
		Assert.False(new Board(",XXXXXXXX").Empty());
		Assert.False(new Board("X,,,,,,,,").Empty());
		Assert.False(new Board(",,,,,,,,X").Empty());
	}
}

public class Full
{
	[Fact]
	public void DetectsFull()
	{
		Assert.True(new Board("XXOOOXXXO").Full());
		Assert.True(new Board("XXOOOXXOX").Full());
		Assert.True(new Board("XXOOXXXOO").Full());
	}

	[Fact]
	public void DetectsNotFull()
	{
		Assert.False(new Board("XO,XO,XO,").Full());
		Assert.False(new Board("XXXXXXXX,").Full());
		Assert.False(new Board(",XXXXXXXX").Full());
		Assert.False(new Board("X,,,,,,,,").Full());
		Assert.False(new Board(",,,,,,,,X").Full());
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

public class SetMark
{
	[Fact]
	public void ThrowsOnOccupied()
	{
		var board = new Board(",,X,,,,,,");

		Assert.Throws<ArgumentException>(() => board.SetMark(2, Mark.O));

		board.SetMark(1, Mark.O);
	}

	[Fact]
	public void ChangesState()
	{
		for (var i = 0; i < Board.Size; i++)
		{
			var board = new Board();
			Assert.True(board.CanMark(i));
			board.SetMark(i, Mark.X);
			Assert.False(board.CanMark(i));
		}
	}
}
