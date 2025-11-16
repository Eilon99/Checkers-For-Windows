using Ex._02_Shay_318605342_Eilon_209396837;
using System;

namespace CheckersGame
{
    public class Board
    {
        private Piece[,] m_Board;
        public int m_Size;

        public Board(int i_Size)
        {
            this.m_Size = i_Size;
            m_Board = new Piece[i_Size, i_Size];
            initializeBoard();
        }

        private void initializeBoard()
        {
            int amountOfPlayerRows = (m_Size - 2) / 2;

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (i < amountOfPlayerRows && (i + j) % 2 != 0)
                    {
                        m_Board[i, j] = new Piece(ePieceType.Regular, 'O', i, j);
                    }
                    else if (i >= m_Size - amountOfPlayerRows && (i + j) % 2 != 0)
                    {
                        m_Board[i, j] = new Piece(ePieceType.Regular, 'X', i, j);
                    }
                    else
                    {
                        m_Board[i, j] = null;
                    }
                }
            }
        }

        //public void DisplayBoard()
        //{
        //    Console.Clear();
        //    Console.Write("   ");
        //    for (int j = 0; j < m_Size; j++)
        //    {
        //        Console.Write($" {(char)('A' + j)}  ");
        //    }

        //    Console.WriteLine();
        //    Console.WriteLine(new string('=', m_Size * 4 + 4));
        //    for (int i = 0; i < m_Size; i++)
        //    {
        //        Console.Write((char)('a' + i) + " |");
        //        for (int j = 0; j < m_Size; j++)
        //        {
        //            if (m_Board[i, j] == null)
        //            {
        //                Console.Write("   |");
        //            }
        //            else
        //            {
        //                Console.Write($" {m_Board[i, j].m_Symbol} |");
        //            }
        //        }

        //        Console.WriteLine();
        //        Console.WriteLine(new string('=', m_Size * 4 + 4));
        //    }
        //}


       
        public Piece GetPiece(int i_Row, int i_Col)
        {
            Piece piece = null;

            if (i_Row >= 0 && i_Row < m_Board.GetLength(0) && i_Col >= 0 && i_Col < m_Board.GetLength(1))
            {
                piece = m_Board[i_Row, i_Col];
            }

            return piece;
        }

        public void RemovePiece(int i_Row, int i_Col)
        {
            m_Board[i_Row, i_Col] = null;
        }

        public Piece MovePiece(Move io_Move, Player i_Player)
        {
            Piece piece = GetPiece(io_Move.m_CurrentRow, io_Move.m_CurrentCol);

            if (piece != null)
            {
                SetPiece(io_Move.m_TargetRow, io_Move.m_TargetCol, piece);
                RemovePiece(io_Move.m_CurrentRow, io_Move.m_CurrentCol);
                updatePlayerPieces(i_Player);
            }

            return piece;
        }

        public void SetPiece(int i_row, int i_col, Piece io_Piece)
        {
            m_Board[i_row, i_col] = io_Piece;
            if (io_Piece != null)
            {
                io_Piece.m_Row = i_row;
                io_Piece.m_Col = i_col;
            }
        }

        private void updatePlayerPieces(Player i_player)
        {
            i_player.Pieces.Clear();
            for (int row = 0; row < m_Size; row++)
            {
                for (int col = 0; col < m_Size; col++)
                {
                    char playerKing = 'K';
                    if (i_player.m_Symbol == 'O')
                    {
                        playerKing = 'U';
                    }

                    Piece piece = m_Board[row, col];
                    if (piece != null && ((piece.m_Symbol == i_player.m_Symbol) || (piece.m_Symbol == playerKing)))
                    {
                        i_player.Pieces.Add(piece);
                    }
                }
            }
        }

        public void Reset()
        {
            m_Board = new Piece[m_Size, m_Size];

            initializeBoard();
        }
    }
}
