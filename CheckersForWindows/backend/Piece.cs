namespace Ex._02_Shay_318605342_Eilon_209396837
{
    public class Piece
    {
        public ePieceType m_Type { get; set; }
        public char m_Symbol { get; set; }
        public int m_Row { get; set; }
        public int m_Col { get; set; }

        public Piece(ePieceType i_Type, char i_Ssymbol)
        {
            m_Type = i_Type;
            m_Symbol = i_Ssymbol;
        }

        public Piece(ePieceType i_Type, char i_Symbol, int i_Row, int i_Col)
        {
            m_Type = i_Type;
            m_Symbol = i_Symbol;
            m_Col = i_Col;
            m_Row = i_Row;
        }

        public bool IsKing()
        {
            bool isKing = false;
            isKing = m_Type == ePieceType.King;

            return isKing;
        }
    }
}