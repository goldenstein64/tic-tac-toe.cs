namespace TicTacToe.Messages;

public class ConsoleConnection2(Func<Message2, string> formatFunc)
	: IConnection2
{
	Func<Message2, string> Format = formatFunc;

	public string Prompt(Message2 message)
	{
		Console.Write(Format(message));
		return Console.ReadLine() ?? throw new("EOF");
	}

	public void Print(Message2 message) => Console.Write(Format(message));
}
