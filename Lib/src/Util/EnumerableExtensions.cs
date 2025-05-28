namespace TicTacToe.Util;

public static class EnumerableExtensions
{
	public static IEnumerable<int> IndexesWhere<T>(
		this IEnumerable<T> values,
		Func<T, bool> cond
	)
	{
		var i = 0;
		foreach (var e in values)
		{
			if (cond(e))
				yield return i;
			i++;
		}
	}

	public static IEnumerable<int> IndexesWhere<T>(
		this IEnumerable<T> values,
		Func<T, int, bool> cond
	)
	{
		var i = 0;
		foreach (var e in values)
		{
			if (cond(e, i))
				yield return i;
			i++;
		}
	}

	public static int FirstIndex<T>(
		this IEnumerable<T> values,
		Func<T, bool> cond
	)
	{
		var i = 0;
		foreach (var e in values)
		{
			if (cond(e))
				return i;
			i++;
		}

		throw new InvalidOperationException();
	}

	public static int FirstIndex<T>(
		this IEnumerable<T> values,
		Func<T, int, bool> cond
	)
	{
		var i = 0;
		foreach (var e in values)
		{
			if (cond(e, i))
				return i;
			i++;
		}

		throw new InvalidOperationException();
	}

	public static T? FirstOrNullStruct<T>(
		this IEnumerable<T> values,
		Func<T, bool> cond
	)
		where T : struct
	{
		foreach (var e in values)
			if (cond(e))
				return e;

		return null;
	}

	public static T? FirstOrNullClass<T>(
		this IEnumerable<T> values,
		Func<T, bool> cond
	)
		where T : class
	{
		foreach (var e in values)
			if (cond(e))
				return e;

		return null;
	}
}
