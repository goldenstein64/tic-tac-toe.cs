using TicTacToe.Data;
using TicTacToe.Data.Messages;
using TicTacToe.Player;
using TicTacToe.Tests.Util;

namespace TicTacToe.Tests.ApplicationTests;

public class UsesApplication
{
	public MockConnection Connection = new();
	public Application Subject;

	public UsesApplication() => Subject = new Application(Connection);

	public void AssertPrints(Message message, int count) =>
		Assert.True(
			Connection.Outputs.Where((msg) => msg == message).Count() >= count
		);

	public void AssertPrints(Message message) =>
		Assert.Contains(message, Connection.Outputs);

	public void AssertDoesNotPrint(Message message) =>
		Assert.DoesNotContain(message, Connection.Outputs);
}

public class ChoosePlayerOnce : UsesApplication
{
	[Fact]
	public void ReturnsComputerOnCH()
	{
		Connection.Inputs = new(["C", "H"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		Assert.Equal(
			[new MSG_PromptPlayer(Mark.X), new MSG_PromptComputer(Mark.X)],
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
			[new MSG_PromptPlayer(Mark.X), new MSG_PromptComputer(Mark.X)],
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
			[new MSG_PromptPlayer(Mark.X), new MSG_PromptComputer(Mark.X)],
			Connection.Outputs
		);
		Assert.IsType<EasyComputer>(chosenPlayer);
	}

	[Fact]
	public void ReturnsHumanOnH()
	{
		Connection.Inputs = new(["H"]);
		var chosenPlayer = Subject.ChoosePlayerOnce(Mark.X);

		Assert.Equal([new MSG_PromptPlayer(Mark.X)], Connection.Outputs);
		Assert.IsType<Human>(chosenPlayer);
	}

	[Fact]
	public void ThrowsOnInvalidComputer()
	{
		Connection.Inputs = new(["C", "@"]);

		var e = Assert.Throws<MessageException>(
			() => Subject.ChoosePlayerOnce(Mark.X)
		);

		Assert.Equal(new ERR_ComputerInvalid(), e.Message);
		Assert.Equal(
			[new MSG_PromptPlayer(Mark.X), new MSG_PromptComputer(Mark.X),],
			Connection.Outputs
		);
	}

	[Fact]
	public void ReturnsNullOnInvalidPlayer()
	{
		Connection.Inputs = new(["#"]);
		var e = Assert.Throws<MessageException>(
			() => Subject.ChoosePlayerOnce(Mark.X)
		);

		Assert.Equal(new ERR_PlayerInvalid(), e.Message);
		Assert.Equal([new MSG_PromptPlayer(Mark.X)], Connection.Outputs);
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
				new MSG_PromptPlayer(Mark.X),
				new ERR_PlayerInvalid(),
				new MSG_PromptPlayer(Mark.X)
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
				new MSG_PromptPlayer(Mark.X),
				new MSG_PromptComputer(Mark.X),
				new ERR_ComputerInvalid(),
				new MSG_PromptPlayer(Mark.X)
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
				new MSG_PromptPlayer(Mark.X), // "@"
				new ERR_PlayerInvalid(),
				new MSG_PromptPlayer(Mark.X), // "C"
				new MSG_PromptComputer(Mark.X), // "H"
				new MSG_PromptPlayer(Mark.O), // "@"
				new ERR_PlayerInvalid(),
				new MSG_PromptPlayer(Mark.O), // "@"
				new ERR_PlayerInvalid(),
				new MSG_PromptPlayer(Mark.O), // "H"
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
				new MSG_PromptPlayer(Mark.X), // "C"
				new MSG_PromptComputer(Mark.X), // "@"
				new ERR_ComputerInvalid(),
				new MSG_PromptPlayer(Mark.X), // "C"
				new MSG_PromptComputer(Mark.X), // "M"
				new MSG_PromptPlayer(Mark.O), // "@"
				new ERR_PlayerInvalid(),
				new MSG_PromptPlayer(Mark.O), // "C"
				new MSG_PromptComputer(Mark.O), // "@"
				new ERR_ComputerInvalid(),
				new MSG_PromptPlayer(Mark.O), // "C"
				new MSG_PromptComputer(Mark.O), // "E"
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

		Assert.Equal([new MSG_Tied()], Connection.Outputs);
	}

	[Fact]
	public void WinXOnMarkX()
	{
		Subject.DisplayWinner(Mark.X);

		Assert.Equal([new MSG_PlayerWon(Mark.X)], Connection.Outputs);
	}

	[Fact]
	public void WinOOnMarkO()
	{
		Subject.DisplayWinner(Mark.O);

		Assert.Equal([new MSG_PlayerWon(Mark.O)], Connection.Outputs);
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
		var winner = Subject.PlayGame([human1, human2]);

		Assert.Equal(Mark.X, winner);
		Assert.Equal(
			[
				new MSG_Board(Subject.Board),
				new MSG_PromptMove(Mark.X), // "1"
				new MSG_Board(Subject.Board),
				new MSG_PromptMove(Mark.O), // "2"
				new MSG_Board(Subject.Board),
				new MSG_PromptMove(Mark.X), // "7"
				new MSG_Board(Subject.Board),
				new MSG_PromptMove(Mark.O), // "4"
				new MSG_Board(Subject.Board),
				new MSG_PromptMove(Mark.X), // "9"
				new MSG_Board(Subject.Board),
				new MSG_PromptMove(Mark.O), // "5"
				new MSG_Board(Subject.Board),
				new MSG_PromptMove(Mark.X), // "8"
				new MSG_Board(Subject.Board),
			],
			Connection.Outputs
		);
	}

	[Fact]
	public void CanRunBetweenComputers()
	{
		Subject.PlayGame([new MediumComputer(), new MediumComputer()]);

		AssertPrints(new MSG_Board(Subject.Board));
	}
}
