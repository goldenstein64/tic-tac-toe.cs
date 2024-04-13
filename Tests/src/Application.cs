using TicTacToe.Data;
using TicTacToe.Messages;
using TicTacToe.Player;
using TicTacToe.Tests.Util;

namespace TicTacToe.Tests.ApplicationTests;

public class UsesApplication
{
	public MockConsole Connection = new();
	public Application Subject;

	public UsesApplication()
	{
		Subject = new Application(Connection);
	}

	public void AssertPrints(IOMessages message)
	{
		Assert.Contains(message, Connection.Outputs);
	}

	public void AssertNotPrint(IOMessages message)
	{
		Assert.DoesNotContain(message, Connection.Outputs);
	}
}

public class ChoosePlayerOnce : UsesApplication
{
	[Fact]
	public void ReturnsComputerOnCH()
	{
		Connection.Inputs = new(["C", "H"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertPrints(IOMessages.MSG_PromptComputer);
		AssertNotPrint(IOMessages.ERR_PlayerInvalid);
		AssertNotPrint(IOMessages.ERR_ComputerInvalid);
		Assert.IsType<HardComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsComputerOnCM()
	{
		Connection.Inputs = new(["C", "M"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertPrints(IOMessages.MSG_PromptComputer);
		AssertNotPrint(IOMessages.ERR_PlayerInvalid);
		AssertNotPrint(IOMessages.ERR_ComputerInvalid);
		Assert.IsType<MediumComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsComputerOnCE()
	{
		Connection.Inputs = new(["C", "E"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertPrints(IOMessages.MSG_PromptComputer);
		AssertNotPrint(IOMessages.ERR_PlayerInvalid);
		AssertNotPrint(IOMessages.ERR_ComputerInvalid);
		Assert.IsType<EasyComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsHumanOnH()
	{
		Connection.Inputs = new(["H"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertNotPrint(IOMessages.MSG_PromptComputer);
		AssertNotPrint(IOMessages.ERR_PlayerInvalid);
		AssertNotPrint(IOMessages.ERR_ComputerInvalid);
		Assert.IsType<Human>(chosenPlayer);
	}

	[Fact]
	public void ReturnsNullOnInvalidComputer()
	{
		Connection.Inputs = new(["C", "@"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertNotPrint(IOMessages.ERR_PlayerInvalid);
		AssertPrints(IOMessages.MSG_PromptComputer);
		AssertPrints(IOMessages.ERR_ComputerInvalid);
		Assert.Null(chosenPlayer);
	}

	[Fact]
	public void ReturnsNullOnInvalidPlayer()
	{
		Connection.Inputs = new(["#"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertPrints(IOMessages.ERR_PlayerInvalid);
		AssertNotPrint(IOMessages.MSG_PromptComputer);
		AssertNotPrint(IOMessages.ERR_ComputerInvalid);
		Assert.Null(chosenPlayer);
	}
}

public class ChoosePlayers : UsesApplication
{
	[Fact]
	public void RetriesOnInvalidPlayer()
	{
		Connection.Inputs = new(["@", "C", "H", "@", "@", "H"]);

		var players = Subject.ChoosePlayers();

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertPrints(IOMessages.ERR_PlayerInvalid);

		Assert.Equal(2, players.Length);
		Assert.IsType<HardComputer>(players[0]);
		Assert.IsType<Human>(players[1]);
	}

	[Fact]
	public void RetriesOnInvalidComputer()
	{
		Connection.Inputs = new(["C", "@", "C", "M", "@", "C", "@", "C", "E"]);

		var players = Subject.ChoosePlayers();

		AssertPrints(IOMessages.MSG_PromptPlayer);
		AssertPrints(IOMessages.MSG_PromptComputer);
		AssertPrints(IOMessages.ERR_PlayerInvalid);
		AssertPrints(IOMessages.ERR_ComputerInvalid);

		Assert.Equal(2, players.Length);
		Assert.IsType<MediumComputer>(players[0]);
		Assert.IsType<EasyComputer>(players[1]);
	}
}

public class DisplayWinner : UsesApplication
{
	[Fact]
	public void TieOnNull()
	{
		Subject.DisplayWinner(null);

		AssertPrints(IOMessages.MSG_Tied);
	}

	[Fact]
	public void WinXOnMarkX()
	{
		Subject.DisplayWinner(Mark.X);

		AssertPrints(IOMessages.MSG_PlayerWon);
	}

	[Fact]
	public void WinOOnMarkO()
	{
		Subject.DisplayWinner(Mark.O);

		AssertPrints(IOMessages.MSG_PlayerWon);
	}
}

public class PlayGame : UsesApplication
{
	[Fact]
	public void CanRunBetweenHumans()
	{
		// a classic corner trap game
		Connection.Inputs = new(["1", "2", "7", "4", "9", "5", "8"]);

		var human1 = new Human(Connection);
		var human2 = new Human(Connection);
		var board = new Board();
		var winner = Subject.PlayGame(board, [human1, human2]);

		Assert.Equal(Mark.X, winner);
		AssertPrints(IOMessages.MSG_PromptMove);
		AssertPrints(IOMessages.MSG_Board);
	}

	[Fact]
	public void CanRunBetweenComputers()
	{
		Subject.PlayGame(new Board(), [new MediumComputer(), new MediumComputer()]);

		AssertPrints(IOMessages.MSG_Board);
	}
}
