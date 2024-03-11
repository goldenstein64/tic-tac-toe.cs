using TicTacToe.Data;
using TicTacToe.Player;
using TicTacToe.Tests.Util;

namespace TicTacToe.Tests.Player.HumanTests;

using ConsoleCapture = MockConsole<Human.IOMessages>;

public class UsesHuman
{
	public ConsoleCapture ConsoleCapture = new();
	public Human Subject;

	public UsesHuman()
	{
		Subject = new Human() { Conn = ConsoleCapture };
	}

	public void AssertPrints(Human.IOMessages message)
	{
		Assert.Contains(message, ConsoleCapture.Outputs);
	}

	public void AssertDoesNotPrint(Human.IOMessages message)
	{
		Assert.DoesNotContain(message, ConsoleCapture.Outputs);
	}
}

public class GetMove : UsesHuman
{
	[Fact]
	public void AsksIOForMove()
	{
		var board = new Board(",,,,,,,,,");
		ConsoleCapture.Inputs = new(["2"]);

		var move = Subject.GetMove(board, Mark.X);
		AssertPrints(Human.IOMessages.MSG_PromptMove);
		AssertDoesNotPrint(Human.IOMessages.ERR_NotANumber);
		AssertDoesNotPrint(Human.IOMessages.ERR_NumberOutOfRange);
		AssertDoesNotPrint(Human.IOMessages.ERR_SpaceOccupied);
		Assert.Equal(1, move);
	}

	[Fact]
	public void PositionIsOccupied_Prints()
	{
		var board = new Board(",XO,,,,,,");
		ConsoleCapture.Inputs = new(["3", "2", "1"]);

		var move = Subject.GetMove(board, Mark.X);

		AssertPrints(Human.IOMessages.MSG_PromptMove);
		AssertDoesNotPrint(Human.IOMessages.ERR_NotANumber);
		AssertDoesNotPrint(Human.IOMessages.ERR_NumberOutOfRange);
		AssertPrints(Human.IOMessages.ERR_SpaceOccupied);

		Assert.Equal(0, move);
	}

	[Fact]
	public void PositionOutOfRange_Prints()
	{
		var board = new Board(",,,,,,,,,");
		ConsoleCapture.Inputs = new(["0", "1"]);

		var move = Subject.GetMove(board, Mark.X);

		AssertPrints(Human.IOMessages.MSG_PromptMove);
		AssertDoesNotPrint(Human.IOMessages.ERR_NotANumber);
		AssertPrints(Human.IOMessages.ERR_NumberOutOfRange);
		AssertDoesNotPrint(Human.IOMessages.ERR_SpaceOccupied);

		Assert.Equal(0, move);
	}

	[Fact]
	public void PositionIsNaN_Prints()
	{
		var board = new Board(",,,,,,,,,");
		ConsoleCapture.Inputs = new(["@", "1"]);

		var move = Subject.GetMove(board, Mark.X);

		AssertPrints(Human.IOMessages.MSG_PromptMove);
		AssertPrints(Human.IOMessages.ERR_NotANumber);
		AssertDoesNotPrint(Human.IOMessages.ERR_NumberOutOfRange);
		AssertDoesNotPrint(Human.IOMessages.ERR_SpaceOccupied);

		Assert.Equal(0, move);
	}
}
