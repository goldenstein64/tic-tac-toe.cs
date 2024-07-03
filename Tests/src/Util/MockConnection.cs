using TicTacToe.Messages;

namespace TicTacToe.Tests.Util;

public class MockConnection2 : IConnection2
{
	public List<Message2> Outputs = [];
	public Queue<string> Inputs = [];

	public string Prompt(Message2 message)
	{
		Outputs.Add(message);
		return Inputs.Dequeue();
	}

	public void Print(Message2 message) => Outputs.Add(message);
}
