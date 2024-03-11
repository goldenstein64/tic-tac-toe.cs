namespace TicTacToe.Util.EnumerableExtensions;

public static class Enumerables
{
	/// <summary>
	/// Add a value between each element in the sequence.
	///
	/// <para>
	/// This does not place the value at the front or back of the sequence. Use
	/// <c>.Prepend</c> and <c>.Append</c> respectively to do this.
	/// </para>
	/// </summary>
	public static IEnumerable<T> Intersperse<T>(
		this IEnumerable<T> values,
		T element
	)
	{
		using var enumerator = values.GetEnumerator();

		enumerator.Reset();
		if (enumerator.MoveNext())
			yield return enumerator.Current;
		else
			yield break;

		while (enumerator.MoveNext())
		{
			yield return element;
			yield return enumerator.Current;
		}
	}

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
