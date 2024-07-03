using TicTacToe.Data;
using TicTacToe.Messages;
using TicTacToe.Player;
using TicTacToe.Tests.Util;

namespace TicTacToe.Tests.Player.HumanTests;

public class UsesHuman
{
	public MockConnection2 Connection2 = new();
	public Human Subject;

	public UsesHuman()
	{
		Subject = new Human(Connection2);
	}

	public void AssertPrints2(Message2 message)
	{
		Assert.Contains(message, Connection2.Outputs);
	}

	public void AssertDoesNotPrint2(Message2 message)
	{
		Assert.DoesNotContain(message, Connection2.Outputs);
	}
}

public class GetMoveOnce : UsesHuman
{
	[Fact]
	public void AsksIOForMove()
	{
		var board = new Board(",,,,,,,,,");
		Connection2.Inputs = new(["2"]);

		var move = Subject.GetMoveOnce(board, Mark.X);
		Assert.Equal([new MSG_PromptMove(Mark.X)], Connection2.Outputs);
		Assert.Equal(1, move);
	}

	[Fact]
	public void PrintsPositionIsOccupied()
	{
		var board = new Board(",XO,,,,,,");
		Connection2.Inputs = new(["3"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_SpaceOccupied()],
			Connection2.Outputs
		);

		Assert.Null(move);
	}

	[Fact]
	public void PrintsPositionOutOfRange()
	{
		var board = new Board(",,,,,,,,,");
		Connection2.Inputs = new(["0"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_NumberOutOfRange()],
			Connection2.Outputs
		);

		Assert.Null(move);
	}

	[Fact]
	public void PrintsNaN_OnHugeInt()
	{
		var board = new Board(",,,,,,,,,");
		Connection2.Inputs = new(["9999999999999999999999999999999"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_NotANumber()],
			Connection2.Outputs
		);

		Assert.Null(move);
	}

	[Fact]
	public void PrintsNaN_OnNonInt()
	{
		var board = new Board(",,,,,,,,,");
		Connection2.Inputs = new(["@"]);

		var move = Subject.GetMoveOnce(board, Mark.X);

		Assert.Equal(
			[new MSG_PromptMove(Mark.X), new ERR_NotANumber()],
			Connection2.Outputs
		);

		Assert.Null(move);
	}
}
