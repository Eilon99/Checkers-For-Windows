namespace Ex._02_Shay_318605342_Eilon_209396837
{
    public class Move
    {
        public int m_CurrentRow { get; set; }
        public int m_CurrentCol { get; set; }
        public int m_TargetRow { get; set; }
        public int m_TargetCol { get; set; }

        public Move(int i_SourceRow, int i_SourceCol, int i_TargetRow, int i_TargetCol)
        {
            m_CurrentRow = i_SourceRow;
            m_CurrentCol = i_SourceCol;
            m_TargetRow = i_TargetRow;
            m_TargetCol = i_TargetCol;
        }
    }
}