using System;
using Ex._02_Shay_318605342_Eilon_209396837;
using System.Collections.Generic;
using static eMoveValidationResult;

namespace CheckersGame
{
    public class GameLogic
    {
        public eMoveValidationResult.MoveValidationResult IsMoveValid(Move i_Move, Board i_Board, Player i_CurrentPlayer, Player i_OpponentPlayer)
        {
            MoveValidationResult result = MoveValidationResult.Valid;
            int rowDiff = i_Move.m_TargetRow - i_Move.m_CurrentRow;
            int colDiff = i_Move.m_TargetCol - i_Move.m_CurrentCol;

            Piece piece = i_Board.GetPiece(i_Move.m_CurrentRow, i_Move.m_CurrentCol);
            bool isPieceKing = piece != null && piece.IsKing();
            if (i_Move.m_TargetRow < 0 || i_Move.m_TargetCol < 0 || i_Move.m_TargetRow >= i_Board.m_Size || i_Move.m_TargetCol >= i_Board.m_Size)
            {
                result = MoveValidationResult.OutOfBounds;
            }
            else if (i_Board.GetPiece(i_Move.m_TargetRow, i_Move.m_TargetCol) != null)
            {
                result = MoveValidationResult.TargetOccupied;
            }
            else if (piece == null)
            {
                result = MoveValidationResult.NoPieceAtSource;
            }
            else if (piece.m_Symbol != i_CurrentPlayer.m_Symbol && piece.m_Symbol != (i_CurrentPlayer.m_Symbol == 'X' ? 'K' : 'U'))
            {
                result = MoveValidationResult.WrongPiece;
            }
            else if (!isPieceKing && ((i_CurrentPlayer.m_Symbol == 'X' && rowDiff >= 0) || (i_CurrentPlayer.m_Symbol == 'O' && rowDiff <= 0)))
            {
                result = MoveValidationResult.IncorrectDirection;
            }
            else if (!(Math.Abs(rowDiff) == Math.Abs(colDiff)))
            {
                result = MoveValidationResult.NotDiagonal;
            }
            else if (Math.Abs(rowDiff) > 2 || Math.Abs(colDiff) > 2)
            {
                result = MoveValidationResult.JumpTooFar;
            }
            else if (Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 2 && !isCaptureMoveValid(i_Board, i_Move.m_CurrentRow, i_Move.m_CurrentCol, i_Move.m_TargetRow, i_Move.m_TargetCol, i_OpponentPlayer.m_Symbol))
            {
                result = MoveValidationResult.JumpWithoutCapture;
            }
            else if (HasMandatoryCapture(i_CurrentPlayer, i_Board) && Math.Abs(rowDiff) != 2)
            {
                result = MoveValidationResult.MustCapture;
            }

            return result;
        }

        public bool HasMandatoryCapture(Player i_Player, Board i_Board, int i_CurrentRow = -1, int i_CurrentCol = -1)
        {
            bool o_HasMandatoryCapture = false;
            char playerSymbol = i_Player.m_Symbol;
            char opponentSymbol = playerSymbol == 'X' ? 'O' : 'X';

            if (i_CurrentRow != -1 && i_CurrentCol != -1)
            {
                o_HasMandatoryCapture = CanCaptureFromPosition(i_CurrentRow, i_CurrentCol, playerSymbol, opponentSymbol, i_Board);
            }
            else
            {
                for (int row = 0; row < i_Board.m_Size && !o_HasMandatoryCapture; row++)
                {
                    for (int col = 0; col < i_Board.m_Size && !o_HasMandatoryCapture; col++)
                    {
                        Piece piece = i_Board.GetPiece(row, col);

                        char playerKing = playerSymbol == 'O' ? 'U' : 'K';
                        if (piece != null && (piece.m_Symbol == playerSymbol || piece.m_Symbol == playerKing))
                        {
                            if (CanCaptureFromPosition(row, col, playerSymbol, opponentSymbol, i_Board) == true)
                            {
                                o_HasMandatoryCapture = true;
                            }
                        }
                    }
                }
            }

            return o_HasMandatoryCapture;
        }
        
        public static bool CanCaptureFromPosition(int i_Row, int i_Col, char i_PlayerSymbol, char i_OpponentSymbol, Board i_Board)
        {
            Piece piece = i_Board.GetPiece(i_Row, i_Col);
            bool isPieceKing = false;
            bool canCapture = false;

            if (piece != null && piece.IsKing())
            {
                isPieceKing = true;
            }

            int[] rowOffsets;
            int[] colOffsets = { -2, 2 };
            if (isPieceKing == true)
            {
                rowOffsets = new int[] { -2, -2, 2, 2 };
            }
            else
            {
                rowOffsets = i_PlayerSymbol == 'X' ? new int[] { -2 } : new int[] { 2 };
            }

            foreach (int rowOffset in rowOffsets)
            {
                foreach (int colOffset in colOffsets)
                {
                    int targetRow = i_Row + rowOffset;
                    int targetCol = i_Col + colOffset;
                    int middleRow = (i_Row + targetRow) / 2;
                    int middleCol = (i_Col + targetCol) / 2;

                    if (isCaptureMoveValid(i_Board, i_Row, i_Col, targetRow, targetCol, i_OpponentSymbol) == true)
                    {
                        canCapture = true;
                    }
                }
            }

            return canCapture;
        }

        public List<Move> GetLegalMoves(Board i_Board, Player i_Player, bool i_CaptureOnly, List<Piece> i_Pieces, Player i_OpponentPlayer)
        {
            List<Move> legalMoves = new List<Move>();
            char playerSymbol = i_Player.m_Symbol;
            char opponentSymbol = playerSymbol == 'X' ? 'O' : 'X';

            foreach (Piece piece in i_Pieces)
            {
                int row = piece.m_Row;
                int col = piece.m_Col;

                int[] rowOffsets = piece.IsKing() ? new int[] { -1, 1 } : (playerSymbol == 'X' ? new int[] { -1 } : new int[] { 1 });
                int[] colOffsets = { -1, 1 };

                foreach (int rowOffset in rowOffsets)
                {
                    foreach (int colOffset in colOffsets)
                    {
                        int targetRow = row + rowOffset;
                        int targetCol = col + colOffset;

                        Move regularMove = new Move(row, col, targetRow, targetCol);
                        MoveValidationResult validationResult = IsMoveValid(regularMove, i_Board, i_Player, i_OpponentPlayer);
                        if (i_CaptureOnly == false && validationResult == MoveValidationResult.Valid)
                        {
                            legalMoves.Add(regularMove);
                        }

                        int captureRow = row + 2 * rowOffset;
                        int captureCol = col + 2 * colOffset;

                        Move captureMove = new Move(row, col, captureRow, captureCol);
                        if (IsMoveValid(captureMove, i_Board, i_Player, i_OpponentPlayer) == MoveValidationResult.Valid)
                        {
                            legalMoves.Add(captureMove);
                        }
                    }
                }
            }

            return legalMoves;
        }

        private static bool isCaptureMoveValid(Board i_Board, int i_Row, int i_Col, int i_TargetRow, int i_TargetCol, char i_OpponentSymbol)
        {
            bool isValid = true;

            if (i_TargetRow < 0 || i_TargetCol < 0 || i_TargetRow >= i_Board.m_Size || i_TargetCol >= i_Board.m_Size)
            {
                isValid = false;
            }

            int middleRow = (i_Row + i_TargetRow) / 2;
            int middleCol = (i_Col + i_TargetCol) / 2;

            Piece middlePiece = i_Board.GetPiece(middleRow, middleCol);
            char opponentPlayerKing = i_OpponentSymbol == 'O' ? 'U' : 'K';
            if (middlePiece == null || (middlePiece.m_Symbol != i_OpponentSymbol && middlePiece.m_Symbol != opponentPlayerKing))
            {
                isValid = false;
            }

            if (i_Board.GetPiece(i_TargetRow, i_TargetCol) != null)
            {
                isValid = false;
            }

            return isValid;
        }

        public Board CloneBoard(Board i_OriginalBoard)
        {
            // Create a new board with the same size as the original
            Board clone = new Board(i_OriginalBoard.m_Size);

            // Loop through each position on the board
            for (int i = 0; i < i_OriginalBoard.m_Size; i++)
            {
                for (int j = 0; j < i_OriginalBoard.m_Size; j++)
                {
                    // Get the piece at the current position on the original board
                    Piece originalPiece = i_OriginalBoard.GetPiece(i, j);

                    if (originalPiece != null)
                    {
                        // Create a cloned piece with the same type and symbol
                        Piece clonedPiece = new Piece(originalPiece.m_Type, originalPiece.m_Symbol);

                        // If the original piece is a king, set the cloned piece to be a king
                        if (originalPiece.IsKing())
                        {
                            clonedPiece.m_Type = ePieceType.King;
                        }

                        // Place the cloned piece on the corresponding position of the cloned board
                        clone.SetPiece(i, j, clonedPiece);
                    }
                }
            }

            return clone;
        }

        public bool ApplyMove(Move i_Move, Board io_Board, Player i_CurrentPlayer, Player i_OpponentPlayer)
        {
            bool isIfPlayerOutOfPieces = false;

            if (Math.Abs(i_Move.m_CurrentRow - i_Move.m_TargetRow) == 2 && Math.Abs(i_Move.m_CurrentCol - i_Move.m_TargetCol) == 2)
            {
                int middleRow = (i_Move.m_CurrentRow + i_Move.m_TargetRow) / 2;
                int middleCol = (i_Move.m_CurrentCol + i_Move.m_TargetCol) / 2;

                Piece capturedPiece = io_Board.GetPiece(middleRow, middleCol);
                io_Board.RemovePiece(middleRow, middleCol);
                i_OpponentPlayer.RemovePiece(capturedPiece);
            }

            io_Board.MovePiece(i_Move, i_CurrentPlayer);
            promoteToKing(i_Move.m_TargetRow, i_Move.m_TargetCol, io_Board, i_CurrentPlayer);
            isIfPlayerOutOfPieces = checkIfPlayerOutOfPieces(i_CurrentPlayer, i_OpponentPlayer);

            return isIfPlayerOutOfPieces;
        }

        private static bool checkIfPlayerOutOfPieces(Player i_CurrentPlayer, Player i_OpponentPlayer)
        {
            bool isIfPlayerOutOfPieces = false;
            if (i_CurrentPlayer.Pieces.Count == 0 || i_OpponentPlayer.Pieces.Count == 0)
            {
                isIfPlayerOutOfPieces = true;
            }

            return isIfPlayerOutOfPieces;
        }

        private void promoteToKing(int i_Row, int i_Col, Board io_Board, Player i_CurrentPlayer)
        {
            Piece piece = io_Board.GetPiece(i_Row, i_Col);

            if (piece != null && !piece.IsKing())
            {
                if ((i_CurrentPlayer.m_Symbol == 'X' && i_Row == 0) || (i_CurrentPlayer.m_Symbol == 'O' && i_Row == io_Board.m_Size - 1))
                {
                    piece.m_Type = ePieceType.King;
                    piece.m_Symbol = i_CurrentPlayer.m_Symbol == 'X' ? 'K' : 'U';
                }
            }
        }

        public bool CheckIfNoValidMovesExists(Player i_Player, Board i_Board, Player i_Opponent)
        {
            List<Move> optionalMoves = GetLegalMoves(i_Board, i_Player, false, i_Player.Pieces, i_Opponent);
            return optionalMoves.Count == 0;
        }

        public void DetermineGameOverState(Player i_Player1, Player i_Player2, Board i_Board, ref string io_Message, ref int o_Player1ScoreChange, ref int o_Player2ScoreChange)
        {
            bool player1NoMoves = CheckIfNoValidMovesExists(i_Player1, i_Board, i_Player2);
            bool player2NoMoves = CheckIfNoValidMovesExists(i_Player2, i_Board, i_Player1);
            o_Player1ScoreChange = 0;
            o_Player2ScoreChange = 0;

            if (player1NoMoves == true && player2NoMoves == true)
            {
                int player1PieceCount = i_Player1.GetPieceCount();
                int player2PieceCount = i_Player2.GetPieceCount();

                if (player1PieceCount > player2PieceCount)
                {
                    int difference = Math.Abs(player1PieceCount - player2PieceCount);
                    o_Player1ScoreChange = difference;
                    io_Message = $"Both players are out of moves. Player 1 wins by having {difference} more points!";
                }
                else if (player2PieceCount > player1PieceCount)
                {
                    int difference = Math.Abs(player2PieceCount - player1PieceCount);
                    o_Player2ScoreChange = difference;
                    io_Message = $"Both players are out of moves. Player 2 wins by having {difference} more points!";
                }
                else
                {
                    io_Message = "Both players are out of moves, and they have an equal number of points. It's a tie!";
                }
            }
            else if (player1NoMoves == true)
            {
                int player1PieceCount = i_Player1.GetPieceCount();
                int player2PieceCount = i_Player2.GetPieceCount();
                int difference = Math.Abs(player2PieceCount - player1PieceCount);

                o_Player2ScoreChange = difference;
                io_Message = $"Player 1 is out of moves. Player 2 wins by {difference} more points!";
            }
            else if (player2NoMoves == true) 
            {
                int player1PieceCount = i_Player1.GetPieceCount();
                int player2PieceCount = i_Player2.GetPieceCount();
                int difference = Math.Abs(player1PieceCount - player2PieceCount);

                o_Player1ScoreChange = difference; 
                io_Message = $"Player 2 is out of moves. Player 1 wins by {difference} more points!";
            }
        }

        public void HandleQuitGame(Player i_QuitPlayer, Player i_OpponentPlayer, ref int o_Player1ScoreChange, ref int o_Player2ScoreChange, ref string io_Message)
        {
            int quitPlayerPieces = i_QuitPlayer.GetPieceCount();
            int opponentPlayerPieces = i_OpponentPlayer.GetPieceCount();
            int difference = Math.Abs(quitPlayerPieces - opponentPlayerPieces);

            if (i_QuitPlayer.m_Symbol == 'X') 
            {
                o_Player2ScoreChange = difference; 
                io_Message = $"Player 1 has quit. Player 2 wins by {difference} more points!";
            }
            else 
            {
                o_Player1ScoreChange = difference; 
                io_Message = $"Player 2 has quit. Player 1 wins by {difference} more points!";
            }
        }
    }
}