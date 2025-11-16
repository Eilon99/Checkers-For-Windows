using Ex._02_Shay_318605342_Eilon_209396837;
using System;

namespace CheckersGame
{
    public class HumanPlayer : Player
    {
        public string m_Name;

        public HumanPlayer(string i_Name, Board i_Board, char i_Symbol) : base(i_Board, i_Symbol)
        {
            this.m_Name = i_Name;
        }

        public override Move GiveMove(Player i_Player, Player i_OpponentPlayer, ref bool io_IsPlayerQuit)
        {
            Move move = null;

            while (move == null)
            {
                Console.WriteLine($"{m_Name}, enter your move (e.g., Aa>Bb):");
                string input = Console.ReadLine();

                if (input == "Q")
                {
                    io_IsPlayerQuit = true;
                }
                else if (input.Length == 5 && input[2] == '>' && char.IsUpper(input[0]) && char.IsLower(input[1]) && char.IsUpper(input[3]) && char.IsLower(input[4]))
                {
                    string source = input.Substring(0, 2);
                    string target = input.Substring(3);

                    int sourceRow = parseRow(source);
                    int sourceCol = parseCol(source);
                    int targetRow = parseRow(target);
                    int targetCol = parseCol(target);

                    move = new Move(sourceRow, sourceCol, targetRow, targetCol);
                }
                else
                {
                    Console.WriteLine("Invalid move format. Use 'Aa>Bb'. Try again.");
                }

                if (io_IsPlayerQuit)
                {
                    break;
                }
            }

            return move;
        }

        private int parseRow(string i_Position)
        {
            return i_Position[1] - 'a';
        }

        private int parseCol(string i_Position)
        {
            return i_Position[0] - 'A';
        }
    }
}