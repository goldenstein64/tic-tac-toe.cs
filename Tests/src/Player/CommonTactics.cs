using TicTacToe.Data;
using TicTacToe.Player;

namespace TicTacToe.Tests.Player.CommonTacticsTests;

public class CommonTactics
{
	public static IEnumerable<object[]> GenerateComputers() =>
		[
			[new MediumComputer()],
			[new HardComputer()],
		];

	[Theory]
	[MemberData(nameof(GenerateComputers))]
	public void DetectsXWinningMove(IPlayer computer)
	{
		/*
		   0 | X | X
		  ---|---|---
		   3 | O | O
		  ---|---|---
		   6 | 7 | 8
		*/
		var board = new Board(",XX,OO,,,");

		var move = computer.GetMove(board, Mark.X);

		Assert.Equal(0, move);
	}

	[Theory]
	[MemberData(nameof(GenerateComputers))]
	public void DetectsOWinningMove(IPlayer computer)
	{
		/*
		   0 | O | O
		  ---|---|---
		   3 | X | X
		  ---|---|---
		   6 | 7 | X
		*/
		var board = new Board(",OO,XX,,X");

		var move = computer.GetMove(board, Mark.O);

		Assert.Equal(0, move);
	}

	[Theory]
	[MemberData(nameof(GenerateComputers))]
	public void DetectsXBlockingMove(IPlayer computer)
	{
		/*
		   O | 1 | 2
		  ---|---|---
		   O | 4 | X
		  ---|---|---
		   6 | X | 8
		*/
		var board = new Board("O,,O,X,X,");

		var move = computer.GetMove(board, Mark.X);

		Assert.Equal(6, move);
	}

	[Theory]
	[MemberData(nameof(GenerateComputers))]
	public void DetectsOBlockingMove(IPlayer computer)
	{
		/*
		   0 | O | 2
		  ---|---|---
		   X | 4 | 5
		  ---|---|---
		   X | X | O
		*/
		var board = new Board(",O,X,,XXO");

		var move = computer.GetMove(board, Mark.O);

		Assert.Equal(0, move);
	}

	[Theory]
	[MemberData(nameof(GenerateComputers))]
	public void DetectsXTrappingMove(IPlayer computer)
	{
		/*
		   0 | X | 2
		  ---|---|---
		   O | 4 | X
		  ---|---|---
		   6 | O | 8
		*/
		var board = new Board(",X,O,X,O,");

		var move = computer.GetMove(board, Mark.X);

		Assert.Equal(2, move);
	}

	[Theory]
	[MemberData(nameof(GenerateComputers))]
	public void DetectsOTrappingMove(IPlayer computer)
	{
		/*
		   0 | X | 2
		  ---|---|---
		   O | X | X
		  ---|---|---
		   6 | O | 8
		*/
		var board = new Board(",X,OXX,O,");

		var move = computer.GetMove(board, Mark.O);

		Assert.Equal(6, move);
	}

	[Theory]
	[MemberData(nameof(GenerateComputers))]
	public void Matches_XXXOOOX_(IPlayer computer)
	{
		/*
		   0 | X | X
		  ---|---|---
		   X | O | O
		  ---|---|---
		   O | X | 8
		*/
		var board = new Board(",XXXOOOX,");

		var move = computer.GetMove(board, Mark.O);

		Assert.Equal(0, move);
	}
}
