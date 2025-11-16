Checkers Game for Windows

A C# Windows Forms implementation of the classic Checkers game.
Features a clean separation between backend logic and frontend UI, following good object-oriented design.

🎮 Features

- Play vs Human

- Play vs Computer (AI)

- Valid move detection

- Forced captures

- King promotions

- Turn indicators

- Score tracking

- Custom board size (6×6, 8×8, 10×10)

🧱 Architecture Overview
CheckersForWindows/
│
├── backend
│   ├── Board.cs
│   ├── GameLogic.cs
│   ├── Move.cs
│   ├── Piece.cs
│   ├── Player.cs
│   ├── ComputerPlayer.cs
│   └── Helpers…
│
└── frontend
    ├── FormGame.cs
    ├── FormSettings.cs
    └── UI files…


Game Settings

<img width="390" height="448" alt="CheckersBoard" src="https://github.com/user-attachments/assets/a3e6c172-ed18-44ca-8b2c-ba24464e655e" />
<img width="390" height="448" alt="CheckersValidMove" src="https://github.com/user-attachments/assets/8376a427-a22b-4fd5-a0b7-08dedce3ddba" />
<img width="378" height="392" alt="CheckersGameSetting" src="https://github.com/user-attachments/assets/6c8d44bd-bc98-4c49-802f-87532c43cb5e" />

Game Board

<img width="390" height="448" alt="CheckersValidMove" src="https://github.com/user-attachments/assets/a34ae525-d2d3-4add-883d-2509af87e545" />
<img width="378" height="392" alt="CheckersGameSetting" src="https://github.com/user-attachments/assets/7a59f0fa-936e-46d0-b0af-093e09a38cc4" />
<img width="390" height="448" alt="CheckersBoard" src="https://github.com/user-attachments/assets/40de9943-1d3c-496a-b06c-fb654e853a01" />

Valid Move Highlighting

<img width="378" height="392" alt="CheckersGameSetting" src="https://github.com/user-attachments/assets/753ded9e-ec0e-4038-9ce1-638f2eb9c83e" />
<img width="390" height="448" alt="CheckersBoard" src="https://github.com/user-attachments/assets/c0c1811f-3e1f-4832-a59e-939917e3bb26" />
<img width="390" height="448" alt="CheckersValidMove" src="https://github.com/user-attachments/assets/e5933185-0f7d-40e5-b61b-dea8504f1ec9" />











🚀 Getting Started
Clone
git clone https://github.com/Eilon99/Checkers-For-Windows.git

Open

Open the solution:

Ex05 Shay 318605342 Eilon 209396837.sln

Run

Press Start ▶ in Visual Studio.

🛠️ Tech Stack
Layer	Technology
Language	C#
UI	Windows Forms
Architecture	Backend + Frontend separation
AI	Simple heuristic-based computer player

📄 License

Academic project for educational use.
Not intended for commercial distribution.
