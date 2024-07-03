using TicTacToe.Data;
using TicTacToe.Player;

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

public record Message2;

public record MSG_PromptPlayer(Mark Mark) : Message2;

public record MSG_PromptComputer(Mark Mark) : Message2;

public record MSG_PlayerWon(Mark Mark) : Message2;

public record MSG_Tied : Message2;

public record MSG_Board(Board Board) : Message2;

public record ERR_PlayerInvalid : Message2;

public record ERR_ComputerInvalid : Message2;

public record MSG_PromptMove(Mark Mark) : Message2;

public record ERR_NotANumber : Message2;

public record ERR_NumberOutOfRange : Message2;

public record ERR_SpaceOccupied : Message2;

public interface IConnection2
{
	public string Prompt(Message2 message);
	public void Print(Message2 message);
}

public interface IConnection
{
	public string Prompt(IOMessages message, params object[] args);
	public void Print(IOMessages message, params object[] args);

	public int? PromptInt(IOMessages message, params object[] args) =>
		int.TryParse(Prompt(message, args), out var result) ? result : null;
}
