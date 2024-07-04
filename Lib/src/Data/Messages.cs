namespace TicTacToe.Data.Messages;

public record Message;

public record MSG_PromptPlayer(Mark Mark) : Message;

public record MSG_PromptComputer(Mark Mark) : Message;

public record MSG_PlayerWon(Mark Mark) : Message;

public record MSG_Tied : Message;

public record MSG_Board(Board Board) : Message;

public record ERR_PlayerInvalid : Message;

public record ERR_ComputerInvalid : Message;

public record MSG_PromptMove(Mark Mark) : Message;

public record ERR_NotANumber : Message;

public record ERR_NumberOutOfRange : Message;

public record ERR_SpaceOccupied : Message;

public interface IConnection
{
	public string Prompt(Message message);
	public void Print(Message message);

	public int? PromptInt(Message message) =>
		int.TryParse(Prompt(message), out var result) ? result : null;
}
