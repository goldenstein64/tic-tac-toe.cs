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

	public void AssertPrints(IOMessages message, int count)
	{
		for (var i = 0; i < count; i++)
		{
			Assert.True(Connection.Outputs.Remove(message));
		}
	}

	public void AssertPrints(IOMessages message) => AssertPrints(message, 1);

	public void AssertDoesNotPrint(IOMessages message)
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

		Assert.Equal(
			[IOMessages.MSG_PromptPlayer, IOMessages.MSG_PromptComputer],
			Connection.Outputs
		);
		Assert.IsType<HardComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsComputerOnCM()
	{
		Connection.Inputs = new(["C", "M"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		Assert.Equal(
			[IOMessages.MSG_PromptPlayer, IOMessages.MSG_PromptComputer],
			Connection.Outputs
		);
		Assert.IsType<MediumComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsComputerOnCE()
	{
		Connection.Inputs = new(["C", "E"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		Assert.Equal(
			[IOMessages.MSG_PromptPlayer, IOMessages.MSG_PromptComputer],
			Connection.Outputs
		);
		Assert.IsType<EasyComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsHumanOnH()
	{
		Connection.Inputs = new(["H"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		Assert.Equal([IOMessages.MSG_PromptPlayer], Connection.Outputs);
		Assert.IsType<Human>(chosenPlayer);
	}

	[Fact]
	public void ReturnsNullOnInvalidComputer()
	{
		Connection.Inputs = new(["C", "@"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		Assert.Equal(
			[
				IOMessages.MSG_PromptPlayer,
				IOMessages.MSG_PromptComputer,
				IOMessages.ERR_ComputerInvalid
			],
			Connection.Outputs
		);
		Assert.Null(chosenPlayer);
	}

	[Fact]
	public void ReturnsNullOnInvalidPlayer()
	{
		Connection.Inputs = new(["#"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		Assert.Equal(
			[IOMessages.MSG_PromptPlayer, IOMessages.ERR_PlayerInvalid],
			Connection.Outputs
		);
		Assert.Null(chosenPlayer);
	}
}

public class ChoosePlayer : UsesApplication
{
	[Fact]
	public void RetriesOnInvalidPlayer()
	{
		Connection.Inputs = new(["#", "H"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		Assert.Equal(
			[
				IOMessages.MSG_PromptPlayer,
				IOMessages.ERR_PlayerInvalid,
				IOMessages.MSG_PromptPlayer
			],
			Connection.Outputs
		);
		Assert.IsType<Human>(chosenPlayer);
	}

	[Fact]
	public void RetriesOnInvalidComputer()
	{
		Connection.Inputs = new(["C", "@", "H"]);
		var chosenPlayer = Subject.ChoosePlayer(Mark.X);

		Assert.Equal(
			[
				IOMessages.MSG_PromptPlayer,
				IOMessages.MSG_PromptComputer,
				IOMessages.ERR_ComputerInvalid,
				IOMessages.MSG_PromptPlayer
			],
			Connection.Outputs
		);
		Assert.IsType<Human>(chosenPlayer);
	}
}

public class ChoosePlayers : UsesApplication
{
	[Fact]
	public void RetriesOnInvalidPlayer()
	{
		Connection.Inputs = new(["@", "C", "H", "@", "@", "H"]);

		var players = Subject.ChoosePlayers();

		Assert.Equal(
			[
				IOMessages.MSG_PromptPlayer,
				IOMessages.ERR_PlayerInvalid,
				IOMessages.MSG_PromptPlayer,
				IOMessages.MSG_PromptComputer,
				IOMessages.MSG_PromptPlayer,
				IOMessages.ERR_PlayerInvalid,
				IOMessages.MSG_PromptPlayer,
				IOMessages.ERR_PlayerInvalid,
				IOMessages.MSG_PromptPlayer,
			],
			Connection.Outputs
		);

		Assert.Equal(2, players.Length);
		Assert.IsType<HardComputer>(players[0]);
		Assert.IsType<Human>(players[1]);
	}

	[Fact]
	public void RetriesOnInvalidComputer()
	{
		Connection.Inputs = new(["C", "@", "C", "M", "@", "C", "@", "C", "E"]);

		var players = Subject.ChoosePlayers();

		Assert.Equal(
			[
				IOMessages.MSG_PromptPlayer, // "C"
				IOMessages.MSG_PromptComputer, // "@"
				IOMessages.ERR_ComputerInvalid,
				IOMessages.MSG_PromptPlayer, // "C"
				IOMessages.MSG_PromptComputer, // "M"
				IOMessages.MSG_PromptPlayer, // "@"
				IOMessages.ERR_PlayerInvalid,
				IOMessages.MSG_PromptPlayer, // "C"
				IOMessages.MSG_PromptComputer, // "@"
				IOMessages.ERR_ComputerInvalid,
				IOMessages.MSG_PromptPlayer, // "C"
				IOMessages.MSG_PromptComputer, // "E"
			],
			Connection.Outputs
		);

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

		Assert.Equal([IOMessages.MSG_Tied], Connection.Outputs);
	}

	[Fact]
	public void WinXOnMarkX()
	{
		Subject.DisplayWinner(Mark.X);

		Assert.Equal([IOMessages.MSG_PlayerWon], Connection.Outputs);
	}

	[Fact]
	public void WinOOnMarkO()
	{
		Subject.DisplayWinner(Mark.O);

		Assert.Equal([IOMessages.MSG_PlayerWon], Connection.Outputs);
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

		Assert.Equal(
			[
				IOMessages.MSG_Board,
				IOMessages.MSG_PromptMove, // "1"
				IOMessages.MSG_Board,
				IOMessages.MSG_PromptMove, // "2"
				IOMessages.MSG_Board,
				IOMessages.MSG_PromptMove, // "7"
				IOMessages.MSG_Board,
				IOMessages.MSG_PromptMove, // "4"
				IOMessages.MSG_Board,
				IOMessages.MSG_PromptMove, // "9"
				IOMessages.MSG_Board,
				IOMessages.MSG_PromptMove, // "5"
				IOMessages.MSG_Board,
				IOMessages.MSG_PromptMove, // "8"
				IOMessages.MSG_Board,
			],
			Connection.Outputs
		);
	}

	[Fact]
	public void CanRunBetweenComputers()
	{
		Subject.PlayGame(new Board(), [new MediumComputer(), new MediumComputer()]);

		AssertPrints(IOMessages.MSG_Board);
	}
}
