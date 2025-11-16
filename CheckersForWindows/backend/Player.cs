using Ex._02_Shay_318605342_Eilon_209396837;
using System.Collections.Generic;

namespace CheckersGame
{
    public abstract class Player
    {
        private List<Piece> m_Pieces;
        private Board m_Board;
        public char m_Symbol;

        public Player(Board i_Board, char i_Symbol)
        {
            this.m_Board = i_Board;
            this.m_Symbol = i_Symbol;
            m_Pieces = initializePiecesList();
        }

        public List<Piece> Pieces
        {
            get
            {
                return m_Pieces;
            }
            set
            {
                m_Pieces = value;
            }
        }

        private List<Piece> initializePiecesList()
        {
            char currentSymbol = this.m_Symbol == 'X' ? 'X' : 'O';
            int amountOfPieces = ((m_Board.m_Size - 2) / 2) * (m_Board.m_Size / 2);
            List<Piece> pieces = new List<Piece>(amountOfPieces);

            for (int i = 0; i < m_Board.m_Size; i++)
            {
                for (int j = 0; j < m_Board.m_Size; j++)
                {
                    Piece piece = m_Board.GetPiece(i, j);
                    if (piece != null && piece.m_Symbol == currentSymbol)
                    {
                        pieces.Add(new Piece(ePieceType.Regular, m_Symbol, i, j));
                    }
                }
            }

            return pieces;
        }

        public void RemovePiece(Piece io_Piece)
        {
            m_Pieces.Remove(io_Piece);
        }

        public int GetPieceCount()
        {
            int count = 0;
            foreach (Piece piece in m_Pieces)
            {
                if (piece.IsKing() == true)
                {
                    count += 4;
                }
                else
                {
                    count += 1;
                }
            }

            return count;
        }

        public void ResetPieces(Board i_Board)
        {
            Pieces.Clear();

            for (int row = 0; row < i_Board.m_Size; row++)
            {
                for (int col = 0; col < i_Board.m_Size; col++)
                {
                    Piece piece = i_Board.GetPiece(row, col);
                    if (piece != null && (piece.m_Symbol == m_Symbol || piece.m_Symbol == getKingSymbol()))
                    {
                        Pieces.Add(piece);
                    }
                }
            }
        }

        private char getKingSymbol()
        {
            return m_Symbol == 'X' ? 'K' : 'U';
        }

        public abstract Move GiveMove(Player i_Player, Player i_OppopnentPlayer, ref bool i_IsPlayerQuit);
    }
}