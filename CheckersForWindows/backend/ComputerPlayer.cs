using Ex._02_Shay_318605342_Eilon_209396837;
using System;
using System.Collections.Generic;

namespace CheckersGame
{
    public class ComputerPlayer : Player
    {
        private GameLogic m_GameLogic;
        private Board m_Board;

        public ComputerPlayer(char i_Symbol, Board i_Board) : base(i_Board, i_Symbol)
        {
            this.m_Board = i_Board;
            m_GameLogic = new GameLogic();
        }

        public override Move GiveMove(Player i_Player, Player i_OpponentPlayer, ref bool o_IsPlayerQuit)
        {
            const bool v_AllCaptureMove = true;
            const bool v_AllLegalMove = false;

            Console.WriteLine("Press 'Enter' to see the computer's move.");
            Console.ReadLine();
            Move resultMove = null;
            List<Move> captureMoves = m_GameLogic.GetLegalMoves(m_Board, this, v_AllCaptureMove, this.Pieces, i_OpponentPlayer);
            List<Move> allMoves = m_GameLogic.GetLegalMoves(m_Board, this, v_AllLegalMove, this.Pieces, i_OpponentPlayer);
            if (captureMoves.Count > 0)
            {
                resultMove = getBestCaptureMove(captureMoves, m_Board, i_OpponentPlayer);
            }

            if (resultMove == null)
            {
                List<Move> kingMoves = getMovesToKing(allMoves);
                if (kingMoves.Count > 0)
                {
                    resultMove = kingMoves[0];
                }
            }

            if (resultMove == null)
            {
                List<Move> safeMoves = getSafeMoves(allMoves, m_Board, i_OpponentPlayer);
                if (safeMoves.Count > 0)
                {
                    resultMove = safeMoves[0];
                }
            }

            if (resultMove == null && allMoves.Count > 0)
            {
                resultMove = chooseRandomMove(allMoves);
            }

            string moveString = ParseMoveToString(resultMove);
           

            return resultMove;
        }

        private Move getBestCaptureMove(List<Move> i_CaptureMoves, Board i_Board, Player i_OpponentPlayer)
        {
            Move bestMove = i_CaptureMoves[0];

            foreach (Move move in i_CaptureMoves)
            {
                Board tempBoard = m_GameLogic.CloneBoard(i_Board);
                List<Piece> tempPieces = clonePlayerPieces(this);
                const bool v_AllCaptureMove = true;

                m_GameLogic.ApplyMove(move, tempBoard, this, i_OpponentPlayer);
                List<Move> followUpCaptureMoves = m_GameLogic.GetLegalMoves(tempBoard, this, v_AllCaptureMove, tempPieces, i_OpponentPlayer);
                if (followUpCaptureMoves.Count > 0)
                {
                    bestMove = move;
                    break;
                }
            }

            return bestMove;
        }

        private List<Move> getMovesToKing(List<Move> i_AllMoves)
        {
            List<Move> kingMoves = new List<Move>();

            foreach (Move move in i_AllMoves)
            {
                if (move.m_TargetRow == m_Board.m_Size - 1)
                {
                    kingMoves.Add(move);
                }
            }

            return kingMoves;
        }

        private List<Move> getSafeMoves(List<Move> i_AllMoves, Board i_Board, Player i_OpponentPlayer)
        {
            List<Move> safeMoves = new List<Move>();

            foreach (var move in i_AllMoves)
            {
                Board tempBoard = m_GameLogic.CloneBoard(i_Board);
                bool isCapturePossible = false;
                List<Piece> tempPieces = clonePlayerPieces(this);

                m_GameLogic.ApplyMove(move, tempBoard, this, i_OpponentPlayer);
                for (int row = 0; row < tempBoard.m_Size && isCapturePossible == false; row++)
                {
                    for (int col = 0; col < tempBoard.m_Size && isCapturePossible == false; col++)
                    {
                        Piece opponentPiece = tempBoard.GetPiece(row, col);

                        if (opponentPiece != null && (opponentPiece.m_Symbol == i_OpponentPlayer.m_Symbol || opponentPiece.m_Symbol == (i_OpponentPlayer.m_Symbol == 'X' ? 'K' : 'U')))
                        {
                            int[] rowOffsets = opponentPiece.IsKing() ? new int[] { -2, 2 } : (i_OpponentPlayer.m_Symbol == 'X' ? new int[] { -2 } : new int[] { 2 });
                            int[] colOffsets = { -2, 2 };
                            foreach (int rowOffset in rowOffsets)
                            {
                                foreach (int colOffset in colOffsets)
                                {
                                    int targetRow = row + rowOffset;
                                    int targetCol = col + colOffset;
                                    int middleRow = (row + targetRow) / 2;
                                    int middleCol = (col + targetCol) / 2;
                                    Piece middlePiece = tempBoard.GetPiece(middleRow, middleCol);
                                    Piece targetPiece = tempBoard.GetPiece(targetRow, targetCol);

                                    if (targetRow >= 0 && targetRow < tempBoard.m_Size && targetCol >= 0 && targetCol < tempBoard.m_Size && middlePiece != null && middlePiece.m_Symbol == this.m_Symbol && targetPiece == null)
                                    {
                                        isCapturePossible = true;
                                        break;
                                    }
                                }

                                if (isCapturePossible == true) break;
                            }
                        }
                    }
                }

                if (isCapturePossible == false)
                {
                    safeMoves.Add(move);
                }
            }

            return safeMoves.Count > 0 ? safeMoves : i_AllMoves;
        }

        private Move chooseRandomMove(List<Move> i_LegalMoves)
        {
            Random random = new Random();

            return i_LegalMoves[random.Next(i_LegalMoves.Count)];
        }

        private List<Piece> clonePlayerPieces(Player i_Player)
        {
            List<Piece> clonedPieces = new List<Piece>();

            foreach (var piece in i_Player.Pieces)
            {
                Piece clonedPiece = new Piece(piece.m_Type, piece.m_Symbol)
                {
                    m_Row = piece.m_Row,
                    m_Col = piece.m_Col
                };

                clonedPieces.Add(clonedPiece);
            }

            return clonedPieces;
        }

        public string ParseMoveToString(Move io_Move)
        {
            string moveString = string.Empty;

            if (io_Move != null)
            {
                char currentCol = (char)('A' + io_Move.m_CurrentCol);
                char currentRow = (char)('a' + io_Move.m_CurrentRow);
                char targetCol = (char)('A' + io_Move.m_TargetCol);
                char targetRow = (char)('a' + io_Move.m_TargetRow);

                moveString = $"{currentCol}{currentRow}>{targetCol}{targetRow}";
            }

            return moveString;
        }
    }
}