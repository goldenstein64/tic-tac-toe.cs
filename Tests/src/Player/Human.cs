using TicTacToe.Data;
using TicTacToe.Messages;
using TicTacToe.Player;
using TicTacToe.Tests.Util;

namespace TicTacToe.Tests.Player.HumanTests;

public class UsesHuman
{
	public MockConnection Connection = new();
	public Human Subject;

	public UsesHuman()
	{
		Subject = new Human(Connection);
	}

	public void AssertPrints(Message message)
	{
		Assert.Contains(message, Connection.Outputs);
	}

	public void AssertDoesNotPrint(Message message)
	{
		Assert.DoesNotContain(message, Connection.Outputs);
	}
}

public class GetMoveOnce : UsesHuman
{
	[Fact]
	public void AsksIOForMove()
	{
		var board = new Board(",,,,,,,,,");
		Connection.Inputs = new(["2"]);

		var move = Subject.GetMoveOnce(board, Mark.X);
		Assert.Equal([new MSG_PromptMove(Mark.X)], Connection.Outputs);
		Assert.Equal(1, move);
	}

	[Fact]
	public void PrintsPositionIsOccupied()
	{
		var board = new Board(",XO,,,,,,");
		Connection.Inputs = new(["3"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_SpaceOccupied()],
			Connection.Outputs
		);

		Assert.Null(move);
	}

	[Fact]
	public void PrintsPositionOutOfRange()
	{
		var board = new Board(",,,,,,,,,");
		Connection.Inputs = new(["0"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_NumberOutOfRange()],
			Connection.Outputs
		);

		Assert.Null(move);
	}

	[Fact]
	public void PrintsNaN_OnHugeInt()
	{
		var board = new Board(",,,,,,,,,");
		Connection.Inputs = new(["9999999999999999999999999999999"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_NotANumber()],
			Connection.Outputs
		);

		Assert.Null(move);
	}

	[Fact]
	public void PrintsNaN_OnNonInt()
	{
		var board = new Board(",,,,,,,,,");
		Connection.Inputs = new(["@"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_NotANumber()],
			Connection.Outputs
		);

		Assert.Null(move);
	}
}
