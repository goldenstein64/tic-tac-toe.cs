namespace TicTacToe.Messages;

public enum IOMessages
{
	ERR_PlayerInvalid,
	MSG_PromptPlayer,
	MSG_PromptComputer,
	ERR_ComputerInvalid,
	MSG_PlayerWon,
	MSG_Tied,
	MSG_Board,

	MSG_PromptMove,
	ERR_NotANumber,
	ERR_NumberOutOfRange,
	ERR_SpaceOccupied,
}

public interface IConnection
{
	public string Prompt(IOMessages message, params object[] args);
	public void Print(IOMessages message, params object[] args);
}
