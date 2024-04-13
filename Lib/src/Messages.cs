namespace TicTacToe.Messages;

public enum IOMessages
{
	MSG_PromptPlayer,
	MSG_PromptComputer,
	MSG_PlayerWon,
	MSG_Tied,
	MSG_Board,
	ERR_PlayerInvalid,
	ERR_ComputerInvalid,

	MSG_PromptMove,
	ERR_NotANumber,
	ERR_NumberOutOfRange,
	ERR_SpaceOccupied,
}

public interface IConnection
{
	public string Prompt(IOMessages message, params object[] args);
	public void Print(IOMessages message, params object[] args);

	public int PromptInt(IOMessages message, params object[] args)
	{
		while (true)
		{
			var @in = Prompt(message, args);
			int result;
			try
			{
				result = int.Parse(@in);
			}
			catch (FormatException)
			{
				Print(IOMessages.ERR_NotANumber);
				continue;
			}
			catch (OverflowException)
			{
				Print(IOMessages.ERR_NumberOutOfRange);
				continue;
			}
			return result;
		}
	}
}
