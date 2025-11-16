using Ex._02_Shay_318605342_Eilon_209396837;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static eMoveValidationResult;

namespace CheckersGame
{
    public class FormGame : Form
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;
        private Player m_CurrentPlayer;
        private Player m_OpponentPlayer;
        private GameLogic m_GameLogic;
        private Button[,] m_BoardButtons;
        private Move m_CurrentMove;
        private Label m_CurrentPlayerLabel;
        private Label m_ScoreLabel;
        private int m_Player1Score = 0;
        private int m_Player2Score = 0;

        public FormGame(int i_BoardSize, bool i_IsPlayer2Human, string i_Player1Name, string i_Player2Name)
        {
            this.Text = "Checkers Game";
            this.Size = new Size(i_BoardSize * 60 + 50, i_BoardSize * 60 + 100);

            initializeGame(i_BoardSize, i_IsPlayer2Human, i_Player1Name, i_Player2Name);
            initializeLabels();
        }

        private void initializeGame(int i_BoardSize, bool i_IsPlayer2Human, string i_Player1Name, string i_Player2Name)
        {
            m_Board = new Board(i_BoardSize);
            m_Player1 = new HumanPlayer(i_Player1Name, m_Board, 'X');
            if (i_IsPlayer2Human == true) 
            {
                m_Player2 = new HumanPlayer(i_Player2Name, m_Board, 'O');
            }
            else
            {
                m_Player2 = new ComputerPlayer('O', m_Board);
            }

            m_CurrentPlayer = m_Player1;
            m_OpponentPlayer = m_Player2;
            m_GameLogic = new GameLogic();
            initializeBoardUI(i_BoardSize);
            updateBoardUI();
        }

        private void initializeLabels()
        {
            m_CurrentPlayerLabel = new Label
            {
                Text = $"{getPlayerName(m_CurrentPlayer)}'s Turn",
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10)
            };

            m_ScoreLabel = new Label
            {
                Text = $"{getPlayerName(m_Player1)}: {m_Player1Score} | {getPlayerName(m_Player2)}: {m_Player2Score}",
                AutoSize = true,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 40)
            };

            this.Controls.Add(m_CurrentPlayerLabel);
            this.Controls.Add(m_ScoreLabel);
        }

        private void handleGameOver(string i_Message)
        {
            updateScores();
            DialogResult result = MessageBox.Show(
                $"{i_Message}\nWould you like to play again?",
                "Game Over",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                resetGame();
            }
            else
            {
                Application.Exit();
            }
        }

        private void updateScores()
        {
            string io_Message = string.Empty;
            int io_Player1ScoreChange = 0;
            int io_Player2ScoreChange = 0;

            m_GameLogic.DetermineGameOverState(m_Player1, m_Player2, m_Board, ref io_Message, ref io_Player1ScoreChange, ref io_Player2ScoreChange);
            m_Player1Score += io_Player1ScoreChange;
            m_Player2Score += io_Player2ScoreChange;
            m_ScoreLabel.Text = $"{getPlayerName(m_Player1)}: {m_Player1Score} | {getPlayerName(m_Player2)}: {m_Player2Score}";
        }

        private void resetGame()
        {
            m_Board.Reset();
            m_Player1.ResetPieces(m_Board);
            m_Player2.ResetPieces(m_Board);
            m_CurrentPlayer = m_Player1;
            m_OpponentPlayer = m_Player2;
            updateBoardUI();
            m_CurrentPlayerLabel.Text = $"{getPlayerName(m_Player1)}'s Turn";
        }

        private void BoardButton_Click(object sender, EventArgs e)
        {
            if (m_CurrentPlayer is ComputerPlayer || !(sender is Button clickedButton))
            {
                return;
            }

            var position = (Tuple<int, int>)clickedButton.Tag;
            int row = position.Item1;
            int col = position.Item2;

            updateBoardUI();
            Piece piece = m_Board.GetPiece(row, col);

            if (m_CurrentMove == null) 
            {
                if (piece != null && (piece.m_Symbol == m_CurrentPlayer.m_Symbol ||
                     piece.m_Symbol == (m_CurrentPlayer.m_Symbol == 'X' ? 'K' : 'U')))
                {
                    m_CurrentMove = new Move(row, col, -1, -1);
                    clickedButton.BackColor = Color.Blue; 
                    highlightCaptureMoves(piece, row, col, true); 
                }
                else
                {
                    MessageBox.Show("You must select one of your pieces.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                Move move = new Move(m_CurrentMove.m_CurrentRow, m_CurrentMove.m_CurrentCol, row, col);
                MoveValidationResult validationResult = m_GameLogic.IsMoveValid(move, m_Board, m_CurrentPlayer, m_OpponentPlayer);
                if (validationResult == MoveValidationResult.Valid)
                {
                    bool isCapture = Math.Abs(move.m_CurrentRow - move.m_TargetRow) == 2;
                    m_GameLogic.ApplyMove(move, m_Board, m_CurrentPlayer, m_OpponentPlayer);
                    updateBoardUI();

                    if (isCapture == true && GameLogic.CanCaptureFromPosition(move.m_TargetRow, move.m_TargetCol, m_CurrentPlayer.m_Symbol, m_OpponentPlayer.m_Symbol, m_Board))
                    {
                        MessageBox.Show("Another capture is mandatory. Continue with the same piece.", "Chained Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_CurrentMove = new Move(move.m_TargetRow, move.m_TargetCol, -1, -1);
                        highlightCaptureMoves(m_Board.GetPiece(move.m_TargetRow, move.m_TargetCol), move.m_TargetRow, move.m_TargetCol, true);
                    }
                    else if (m_GameLogic.CheckIfNoValidMovesExists(m_OpponentPlayer, m_Board, m_CurrentPlayer) == true) 
                    {
                        handleGameOver($"{getPlayerName(m_CurrentPlayer)} wins!");
                    }
                    else
                    {
                        swapPlayers(); 
                    }
                }
                else
                {
                    MessageBox.Show(validationResult.GetMessage(), "Invalid Move", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                resetButtonColors();
                m_CurrentMove = null;
            }

            if (m_CurrentPlayer is ComputerPlayer)
            {
                performComputerMove();
            }
        }

        private void performComputerMove()
        {
            if (m_CurrentPlayer is ComputerPlayer computerPlayer)
            {
                bool isPlayerQuit = false;
                var move = computerPlayer.GiveMove(m_CurrentPlayer, m_OpponentPlayer, ref isPlayerQuit);

                if (move != null)
                {
                    bool isCapture = Math.Abs(move.m_CurrentRow - move.m_TargetRow) == 2; 

                    m_GameLogic.ApplyMove(move, m_Board, m_CurrentPlayer, m_OpponentPlayer);
                    updateBoardUI();
                    if (isCapture == true && GameLogic.CanCaptureFromPosition(move.m_TargetRow, move.m_TargetCol, m_CurrentPlayer.m_Symbol, m_OpponentPlayer.m_Symbol, m_Board) == true) 
                    {
                        MessageBox.Show("Computer must continue capturing.", "Chained Capture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        performComputerMove(); 

                        return;
                    }
                }

                if (m_GameLogic.CheckIfNoValidMovesExists(m_OpponentPlayer, m_Board, m_CurrentPlayer) == true) 
                {
                    handleGameOver($"{getPlayerName(m_CurrentPlayer)} wins!");
                }
                else
                {
                    swapPlayers(); 
                }
            }
        }

        private void resetButtonColors()
        {
            foreach (Button button in m_BoardButtons)
            {
                if (button.Enabled == true) 
                {
                    var position = (Tuple<int, int>)button.Tag;
                    int row = position.Item1;
                    int col = position.Item2;

                    button.BackColor = (row + col) % 2 == 0 ? Color.DarkGray : Color.Beige;
                }
            }
        }

        private void swapPlayers()
        {
            Player temp = m_CurrentPlayer;

            m_CurrentPlayer = m_OpponentPlayer;
            m_OpponentPlayer = temp;
            m_CurrentPlayerLabel.Text = $"{getPlayerName(m_CurrentPlayer)}'s Turn";
            if (m_CurrentPlayer is ComputerPlayer)
            {
                performComputerMove();
            }
        }

        private void updateBoardUI()
        {
            for (int row = 0; row < m_Board.m_Size; row++)
            {
                for (int col = 0; col < m_Board.m_Size; col++)
                {
                    Piece piece = m_Board.GetPiece(row, col);
                    Button button = m_BoardButtons[row, col];

                    button.Text = piece?.m_Symbol.ToString();
                    button.BackColor = (row + col) % 2 == 0 ? Color.DarkGray : Color.Beige;
                    button.Enabled = true;
                }
            }
        }

        private string getPlayerName(Player i_Player)
        {
            string currentPlayerName= i_Player is HumanPlayer humanPlayer ? humanPlayer.m_Name : "Computer";

            return currentPlayerName;
        }

        private void highlightCaptureMoves(Piece i_Piece, int i_Row, int i_Col, bool i_CaptureOnly)
        {
            resetButtonColors();
            List<Piece> playerPieces = new List<Piece> { i_Piece };
            List<Move> legalMoves = m_GameLogic.GetLegalMoves(m_Board, m_CurrentPlayer, i_CaptureOnly, playerPieces, m_OpponentPlayer);

            foreach (Move move in legalMoves)
            {
                int targetRow = move.m_TargetRow;
                int targetCol = move.m_TargetCol;

                m_BoardButtons[targetRow, targetCol].BackColor = Color.LightGreen;
            }
        }

        private void initializeBoardUI(int i_BoardSize)
        {
            int buttonSize = 50;
            m_BoardButtons = new Button[i_BoardSize, i_BoardSize];

            for (int row = 0; row < i_BoardSize; row++)
            {
                for (int col = 0; col < i_BoardSize; col++)
                {
                    Button button = new Button
                    {
                        Width = buttonSize,
                        Height = buttonSize,
                        Left = col * buttonSize + 10,
                        Top = row * buttonSize + 70,
                        BackColor = (row + col) % 2 == 0 ? Color.DarkGray : Color.Beige,
                        Tag = new Tuple<int, int>(row, col)
                    };

                    button.Enabled = (row + col) % 2 != 0;
                    button.Click += BoardButton_Click;

                    this.Controls.Add(button);
                    m_BoardButtons[row, col] = button;
                }
            }
        }
    }
}