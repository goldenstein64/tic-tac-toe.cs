using TicTacToe.Messages;

namespace TicTacToe.Tests.Util;

public class MockConsole<T> : IConnection<T>
	where T : notnull
{
	public List<T> Outputs = [];
	public Queue<string> Inputs = [];

	public string Prompt(T message, params object[] args)
	{
		Outputs.Add(message);
		return Inputs.Dequeue();
	}

	public void Print(T message, params object[] args)
	{
		Outputs.Add(message);
	}
}
