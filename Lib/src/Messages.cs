using TicTacToe.Data;
using TicTacToe.Player;

namespace TicTacToe.Messages;

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

	public int? PromptInt(Message2 message) =>
		int.TryParse(Prompt(message), out var result) ? result : null;
}
