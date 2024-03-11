using TicTacToe.Data;
using TicTacToe.Player;
using TicTacToe.Tests.Util;

namespace TicTacToe.Tests.ApplicationTests;

using AppConsoleCapture = MockConsole<Application.IOMessages>;
using HumanConsoleCapture = MockConsole<Human.IOMessages>;

public class UsesApplication
{
	public AppConsoleCapture ConsoleCapture = new();
	public Application Subject;

	public UsesApplication()
	{
		Subject = new Application() { Conn = ConsoleCapture };
	}

	public void AssertPrints(Application.IOMessages message)
	{
		Assert.Contains(message, ConsoleCapture.Outputs);
	}
}

public class ChoosePlayer : UsesApplication
{
	[Fact]
	public void ReturnsComputerOnCH()
	{
		ConsoleCapture.Inputs = new(["C", "H"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		AssertPrints(Application.IOMessages.PromptPlayer);
		AssertPrints(Application.IOMessages.PromptComputer);
		Assert.IsType<HardComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsComputerOnCM()
	{
		ConsoleCapture.Inputs = new(["C", "M"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		AssertPrints(Application.IOMessages.PromptPlayer);
		AssertPrints(Application.IOMessages.PromptComputer);
		Assert.IsType<MediumComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsComputerOnCE()
	{
		ConsoleCapture.Inputs = new(["C", "E"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		AssertPrints(Application.IOMessages.PromptPlayer);
		AssertPrints(Application.IOMessages.PromptComputer);
		Assert.IsType<EasyComputer>(chosenPlayer);
	}

	[Fact]
	public void DetectsInvalidComputer()
	{
		ConsoleCapture.Inputs = new(["C", "@"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		AssertPrints(Application.IOMessages.PromptPlayer);
		AssertPrints(Application.IOMessages.PromptComputer);
		AssertPrints(Application.IOMessages.ComputerInvalid);
		Assert.Null(chosenPlayer);
	}

	[Fact]
	public void ReturnsHumanOnH()
	{
		ConsoleCapture.Inputs = new(["H"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		AssertPrints(Application.IOMessages.PromptPlayer);
		Assert.IsType<Human>(chosenPlayer);
	}

	[Fact]
	public void DetectsInvalid()
	{
		ConsoleCapture.Inputs = new(["@"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		AssertPrints(Application.IOMessages.PromptPlayer);
		AssertPrints(Application.IOMessages.PlayerInvalid);
		Assert.Null(chosenPlayer);
	}
}

public class ChoosePlayers : UsesApplication
{
	[Fact]
	public void RetriesOnInvalidPlayer()
	{
		ConsoleCapture.Inputs = new(["@", "C", "H", "@", "@", "H"]);

		var players = Subject.ChoosePlayers();

		AssertPrints(Application.IOMessages.PromptPlayer);
		AssertPrints(Application.IOMessages.PlayerInvalid);

		Assert.Equal(2, players.Length);
		Assert.IsType<HardComputer>(players[0]);
		Assert.IsType<Human>(players[1]);
	}

	[Fact]
	public void RetriesOnInvalidComputer()
	{
		ConsoleCapture.Inputs = new(
			["C", "@", "C", "M", "@", "C", "@", "C", "E"]
		);

		var players = Subject.ChoosePlayers();

		AssertPrints(Application.IOMessages.PromptPlayer);
		AssertPrints(Application.IOMessages.PromptComputer);
		AssertPrints(Application.IOMessages.PlayerInvalid);
		AssertPrints(Application.IOMessages.ComputerInvalid);

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

		AssertPrints(Application.IOMessages.Tied);
	}

	[Fact]
	public void WinXOnMarkX()
	{
		Subject.DisplayWinner(Mark.X);

		AssertPrints(Application.IOMessages.PlayerWon);
	}

	[Fact]
	public void WinOOnMarkO()
	{
		Subject.DisplayWinner(Mark.O);

		AssertPrints(Application.IOMessages.PlayerWon);
	}
}

public class PlayGame : UsesApplication
{
	readonly HumanConsoleCapture HumanConsoleCapture = new();

	void AssertPrints(Human.IOMessages message)
	{
		Assert.Contains(message, HumanConsoleCapture.Outputs);
	}

	[Fact]
	public void CanRunBetweenHumans()
	{
		// a classic corner trap game
		HumanConsoleCapture.Inputs = new(["1", "2", "7", "4", "9", "5", "8"]);

		var human1 = new Human() { Conn = HumanConsoleCapture };
		var human2 = new Human() { Conn = HumanConsoleCapture };
		var board = new Board();
		var winner = Subject.PlayGame(board, [human1, human2]);

		Assert.Equal(Mark.X, winner);
		AssertPrints(Human.IOMessages.MSG_PromptMove);
		AssertPrints(Application.IOMessages.Board);
	}

	[Fact]
	public void CanRunBetweenComputers()
	{
		Subject.PlayGame(
			new Board(),
			[new MediumComputer(), new MediumComputer()]
		);

		AssertPrints(Application.IOMessages.Board);
	}
}
