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
		return mark == Mark.X ? Mark.O : Mark.X;
	}

	public static string ToString(this Mark mark)
	{
		return mark == Mark.X ? "O" : "X";
	}
}
