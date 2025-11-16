public static class eMoveValidationResult
{
    public enum MoveValidationResult
    {
        Valid,
        OutOfBounds,
        TargetOccupied,
        IncorrectDirection,
        NotDiagonal,
        InvalidDistance,
        MustCapture,
        InvalidCapture,
        JumpWithoutCapture,
        NoPieceAtSource,
        WrongPiece,
        JumpTooFar,
    }

    public static string GetMessage(this MoveValidationResult result)
    {
        string message = "try again.";

        switch (result)
        {
            case MoveValidationResult.Valid:
                message = "The move is valid.";
                break;
            case MoveValidationResult.OutOfBounds:
                message = "The move is out of bounds.";
                break;
            case MoveValidationResult.TargetOccupied:
                message = "The target square is already occupied.";
                break;
            case MoveValidationResult.IncorrectDirection:
                message = "The piece cannot move in this direction.";
                break;
            case MoveValidationResult.NotDiagonal:
                message = "The move must be diagonal.";
                break;
            case MoveValidationResult.InvalidDistance:
                message = "The move distance is invalid.";
                break;
            case MoveValidationResult.MustCapture:
                message = "You must perform a capture if possible.";
                break;
            case MoveValidationResult.JumpWithoutCapture:
                message = "You cannot jump more than one square without capturing. Try again.";
                break;
            case MoveValidationResult.NoPieceAtSource:
                message = "There is no piece at the source square.";
                break;
            case MoveValidationResult.WrongPiece:
                message = "The piece at the source does not belong to you.";
                break;
            case MoveValidationResult.JumpTooFar:
                message = "You cannot jump more than 2 squares.";
                break;
        }

        return message;
    }
}