namespace Goldenstein64.TicTacToe.App.Util;

public static class EnumerableExtensions
{
	/// <summary>
	/// Add a value between each element in the sequence.
	///
	/// <para>
	/// This does not place the value at the front or back of the sequence. Use
	/// <see cref="Enumerable.Prepend{T}" /> and <see cref="Enumerable.Append{T}" /> respectively to do this.
	/// </para>
	/// </summary>
	public static IEnumerable<T> Intersperse<T>(
		this IEnumerable<T> values,
		T element
	)
	{
		using var enumerator = values.GetEnumerator();

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
}
