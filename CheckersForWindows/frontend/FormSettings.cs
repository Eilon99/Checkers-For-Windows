using CheckersGame;
using System.Drawing;
using System.Windows.Forms;

public class FormSettings : Form
{
    private RadioButton m_RadioButtonBoardSize6x6;
    private RadioButton m_RadioButtonBoardSize8x8;
    private RadioButton m_RadioButtonBoardSize10x10;
    private CheckBox m_CheckBoxIsSecondPlayerHuman;
    private TextBox m_TextBoxMainPlayerName;
    private TextBox m_TextBoxSecondPlayerName;
    private Label m_LabelMainPlayer;
    private Label m_LabelPlayers;
    private Button m_ButtonDone;

    public FormSettings()
    {
        initializeComponents();
    }

    private void initializeComponents()
    {
        this.Text = "Game Settings";
        this.Size = new Size(400, 400);

        GroupBox groupBoxBoardSize = new GroupBox
        {
            Text = "Game Board Size",
            Location = new Point(20, 20),
            Size = new Size(350, 50)
        };

        m_RadioButtonBoardSize6x6 = new RadioButton
        {
            Text = "6X6",
            Location = new Point(10, 20),
            Checked = true 
        };

        m_RadioButtonBoardSize8x8 = new RadioButton
        {
            Text = "8X8",
            Location = new Point(120, 20)
        };

        m_RadioButtonBoardSize10x10 = new RadioButton
        {
            Text = "10X10",
            Location = new Point(230, 20)
        };

        groupBoxBoardSize.Controls.Add(m_RadioButtonBoardSize6x6);
        groupBoxBoardSize.Controls.Add(m_RadioButtonBoardSize8x8);
        groupBoxBoardSize.Controls.Add(m_RadioButtonBoardSize10x10);

        m_LabelPlayers = new Label
        {
            Text = "Players:",
            Location = new Point(15, 85)
        };

        m_LabelMainPlayer = new Label
        {
            Text = "Player 1:",
            Location = new Point(20, 115)
        };

        m_TextBoxMainPlayerName = new TextBox
        {
            Location = new Point(m_LabelMainPlayer.Left + m_LabelMainPlayer.Width + 20, 115),
            Width = 150,
            Enabled = true
        };

        m_CheckBoxIsSecondPlayerHuman = new CheckBox
        {
            Text = "Player 2:",
            Location = new Point(20, 155)
        };

        m_TextBoxSecondPlayerName = new TextBox
        {
            Text = "(Computer)",
            Location = new Point(m_TextBoxMainPlayerName.Left, 155), 
            Width = 150,
            Enabled = false
        };

        m_CheckBoxIsSecondPlayerHuman.CheckedChanged += (sender, e) =>
        {
            if (m_CheckBoxIsSecondPlayerHuman.Checked)
            {
                m_TextBoxSecondPlayerName.Text = ""; 
                m_TextBoxSecondPlayerName.Enabled = true; 
            }
            else
            {
                m_TextBoxSecondPlayerName.Text = "(Computer)"; 
                m_TextBoxSecondPlayerName.Enabled = false; 
            }
        };

        m_ButtonDone = new Button
        {
            Text = "Done",
            Location = new Point(150, 300),
            Size = new Size(100, 30)
        };

        m_ButtonDone.Click += (sender, e) =>
        {
            string boardSize = m_RadioButtonBoardSize6x6.Checked ? "6X6" : m_RadioButtonBoardSize8x8.Checked ? "8X8" : "10X10";
            string mainPlayerName = m_TextBoxMainPlayerName.Text.Trim();
            string secondPlayerName = m_CheckBoxIsSecondPlayerHuman.Checked ? m_TextBoxSecondPlayerName.Text.Trim() : "Computer";
            if (string.IsNullOrWhiteSpace(mainPlayerName) == true)
            {
                MessageBox.Show("Player 1 name is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (m_CheckBoxIsSecondPlayerHuman.Checked && string.IsNullOrWhiteSpace(secondPlayerName) == true) 
            {
                MessageBox.Show("Player 2 name is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            startGame(boardSize, mainPlayerName, secondPlayerName);
        };

        this.Controls.Add(groupBoxBoardSize);
        this.Controls.Add(m_LabelPlayers);
        this.Controls.Add(m_LabelMainPlayer);
        this.Controls.Add(m_TextBoxMainPlayerName);
        this.Controls.Add(m_CheckBoxIsSecondPlayerHuman);
        this.Controls.Add(m_TextBoxSecondPlayerName);
        this.Controls.Add(m_ButtonDone);
    }

    private void startGame(string i_BoardSize, string i_MainPlayerName, string i_SecondPlayerName)
    {
        int boardDimension;
        switch (i_BoardSize)
        {
            case "6X6":
                boardDimension = 6;
                break;
            case "8X8":
                boardDimension = 8;
                break;
            case "10X10":
                boardDimension = 10;
                break;
            default:
                boardDimension = 6;
                break;
        }

        bool isPlayer2Human = m_CheckBoxIsSecondPlayerHuman.Checked;
        FormGame mainForm = new FormGame(boardDimension, isPlayer2Human, i_MainPlayerName, i_SecondPlayerName)
        {
            Text = $"{i_MainPlayerName} vs {i_SecondPlayerName}"
        };

        this.Hide();
        mainForm.ShowDialog();
        this.Close();
    }
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new FormSettings());
    }
}