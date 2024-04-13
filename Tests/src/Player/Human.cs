using TicTacToe.Data;
using TicTacToe.Messages;
using TicTacToe.Player;
using TicTacToe.Tests.Util;

namespace TicTacToe.Tests.Player.HumanTests;

public class UsesHuman
{
	public MockConsole Connection = new();
	public Human Subject;

	public UsesHuman()
	{
		Subject = new Human(Connection);
	}

	public void AssertPrints(IOMessages message)
	{
		Assert.Contains(message, Connection.Outputs);
	}

	public void AssertDoesNotPrint(IOMessages message)
	{
		Assert.DoesNotContain(message, Connection.Outputs);
	}
}

public class GetMove : UsesHuman
{
	[Fact]
	public void AsksIOForMove()
	{
		var board = new Board(",,,,,,,,,");
		Connection.Inputs = new(["2"]);

		var move = Subject.GetMove(board, Mark.X);
		AssertPrints(IOMessages.MSG_PromptMove);
		AssertDoesNotPrint(IOMessages.ERR_NotANumber);
		AssertDoesNotPrint(IOMessages.ERR_NumberOutOfRange);
		AssertDoesNotPrint(IOMessages.ERR_SpaceOccupied);
		Assert.Equal(1, move);
	}

	[Fact]
	public void PositionIsOccupied_Prints()
	{
		var board = new Board(",XO,,,,,,");
		Connection.Inputs = new(["3", "2", "1"]);

		var move = Subject.GetMove(board, Mark.X);

		AssertPrints(IOMessages.MSG_PromptMove);
		AssertDoesNotPrint(IOMessages.ERR_NotANumber);
		AssertDoesNotPrint(IOMessages.ERR_NumberOutOfRange);
		AssertPrints(IOMessages.ERR_SpaceOccupied);

		Assert.Equal(0, move);
	}

	[Fact]
	public void PositionOutOfRange_Prints()
	{
		var board = new Board(",,,,,,,,,");
		Connection.Inputs = new(["0", "1"]);

		var move = Subject.GetMove(board, Mark.X);

		AssertPrints(IOMessages.MSG_PromptMove);
		AssertDoesNotPrint(IOMessages.ERR_NotANumber);
		AssertPrints(IOMessages.ERR_NumberOutOfRange);
		AssertDoesNotPrint(IOMessages.ERR_SpaceOccupied);

		Assert.Equal(0, move);
	}

	[Fact]
	public void PositionIsNaN_Prints()
	{
		var board = new Board(",,,,,,,,,");
		Connection.Inputs = new(["@", "1"]);

		var move = Subject.GetMove(board, Mark.X);

		AssertPrints(IOMessages.MSG_PromptMove);
		AssertPrints(IOMessages.ERR_NotANumber);
		AssertDoesNotPrint(IOMessages.ERR_NumberOutOfRange);
		AssertDoesNotPrint(IOMessages.ERR_SpaceOccupied);

		Assert.Equal(0, move);
	}
}
