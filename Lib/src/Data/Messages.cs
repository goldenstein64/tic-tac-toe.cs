namespace TicTacToe.Data.Messages;

public abstract record Message;

public sealed record MSG_PromptPlayer(Mark Mark) : Message;

public sealed record MSG_PromptComputer(Mark Mark) : Message;

public sealed record MSG_PlayerWon(Mark Mark) : Message;

public sealed record MSG_Tied : Message;

public sealed record MSG_Board(Board Board) : Message;

public sealed record ERR_PlayerInvalid : Message;

public sealed record ERR_ComputerInvalid : Message;

public sealed record MSG_PromptMove(Mark Mark) : Message;

public sealed record ERR_NotANumber : Message;

public sealed record ERR_NumberOutOfRange : Message;

public sealed record ERR_SpaceOccupied : Message;

public class MessageException(Message message) : Exception
{
	public new readonly Message Message = message;

	public static T TryUntilOk<T>(IConnection conn, Func<T> body)
	{
		while (true)
		{
			try
			{
				return body();
			}
			catch (MessageException e)
			{
				conn.Print(e.Message);
			}
		}
	}
}

public interface IConnection
{
	public string Prompt(Message message);
	public void Print(Message message);

	public int? PromptInt(Message message) =>
		int.TryParse(Prompt(message), out var result) ? result : null;
}
