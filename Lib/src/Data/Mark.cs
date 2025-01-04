namespace TicTacToe.Data;

public enum Mark
{
	X,
	O
}

public static class MarkExtensions
{
	public static Mark Other(this Mark mark)
	{
		return mark is Mark.X ? Mark.O : Mark.X;
	}

	public static string ToString(this Mark mark)
	{
		return mark is Mark.X ? "O" : "X";
	}
}
