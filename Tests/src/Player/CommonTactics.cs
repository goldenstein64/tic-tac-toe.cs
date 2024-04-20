using TicTacToe.Data;
using TicTacToe.Player;

namespace TicTacToe.Tests.Player.CommonTacticsTests;

public class CommonTactics
{
	public static IEnumerable<object[]> ComputerCases() =>
		[
			[new MediumComputer()],
			[new HardComputer()],
		];

	public static IEnumerable<object[]> TestCases =
	[ // pattern, mark, expected
		[",XX,OO,,,", Mark.X, 0], // X winning move
		[",OO,XX,,X", Mark.O, 0], // O winning move
		["O,,O,X,X,", Mark.X, 6], // X blocking move
		[",O,X,,XXO", Mark.O, 0], // O blocking move
		[",X,O,X,O,", Mark.X, 2], // X trapping move
		[",X,OXX,O,", Mark.O, 6], // O trapping move
		[",XXXOOOX,", Mark.O, 0],
	];

	public static IEnumerable<object[]> GenerateCases()
	{
		foreach (object[] testArgs in TestCases)
		foreach (object[] cpuArgs in ComputerCases())
			yield return [.. cpuArgs, .. testArgs];
	}

	[Theory]
	[MemberData(nameof(GenerateCases))]
	public void PassesAllTests(
		IPlayer computer,
		string pattern,
		Mark mark,
		int expected
	)
	{
		var board = new Board(pattern);

		var move = computer.GetMove(board, mark);

		Assert.Equal(expected, move);
	}
}
