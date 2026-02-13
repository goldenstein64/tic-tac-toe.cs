using Goldenstein64.TicTacToe.Data.Messages;

namespace Goldenstein64.TicTacToe.Tests.Util;

public class MockConnection : IConnection
{
	public List<Message> Outputs = [];
	public Queue<string> Inputs = [];

	public string Prompt(Message message)
	{
		Outputs.Add(message);
		return Inputs.Dequeue();
	}

	public void Print(Message message) => Outputs.Add(message);
}
