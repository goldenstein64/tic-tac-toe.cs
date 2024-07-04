using TicTacToe.Data.Messages;

namespace TicTacToe.App;

public class ConsoleConnection(Func<Message, string> formatFunc) : IConnection
{
	Func<Message, string> Format = formatFunc;

	public string Prompt(Message message)
	{
		Console.Write(Format(message));
		return Console.ReadLine() ?? throw new("EOF");
	}

	public void Print(Message message) => Console.Write(Format(message));
}
