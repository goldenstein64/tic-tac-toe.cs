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

public class ConsoleConnection(Dictionary<IOMessages, string> formatMap)
	: IConnection
{
	readonly Dictionary<IOMessages, string> FormatMap = formatMap;

	string Format(IOMessages message, params object[] args) =>
		string.Format(FormatMap[message], args);

	public string Prompt(IOMessages message, params object[] args)
	{
		Console.Write(Format(message, args));
		return Console.ReadLine() ?? throw new("EOF");
	}

	public void Print(IOMessages message, params object[] args) =>
		Console.WriteLine(Format(message, args));
}
