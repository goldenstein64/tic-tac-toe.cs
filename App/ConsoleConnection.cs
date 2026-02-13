using Goldenstein64.TicTacToe.Data.Messages;

namespace Goldenstein64.TicTacToe.App;

public class ConsoleConnection(Func<Message, string> formatFunc) : IConnection
{
	readonly Func<Message, string> Format = formatFunc;

	public string Prompt(Message message)
	{
		Console.Write(Format(message));
		return Console.ReadLine() ?? throw new("EOF");
	}

	public void Print(Message message) => Console.Write(Format(message));
}
