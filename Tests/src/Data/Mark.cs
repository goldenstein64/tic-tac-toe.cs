using TicTacToe.Data;

namespace TicTacToe.Tests.Data.MarkTests;

public class Other
{
	[Fact]
	public void Works()
	{
		Assert.Equal(Mark.O, Mark.X.Other());
		Assert.Equal(Mark.X, Mark.O.Other());
	}
}

public class ToString
{
	[Fact]
	public void Works()
	{
		Assert.Equal("X", Mark.X.ToString());
		Assert.Equal("O", Mark.O.ToString());
	}
}
