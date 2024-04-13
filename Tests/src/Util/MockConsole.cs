using TicTacToe.Messages;

namespace TicTacToe.Tests.Util;

public class MockConsole : IConnection
{
	public List<IOMessages> Outputs = [];
	public Queue<string> Inputs = [];

	public string Prompt(IOMessages message, params object[] args)
	{
		Outputs.Add(message);
		return Inputs.Dequeue();
	}

	public void Print(IOMessages message, params object[] args)
	{
		Outputs.Add(message);
	}
}
