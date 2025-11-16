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

## 🧱 Architecture Overview

```text
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
```
🖼️ Screenshots:

Game Settings

<img width="390" height="448" alt="CheckersValidMove" src="https://github.com/user-attachments/assets/913152c5-5317-49a2-8af3-0bf0cbe31bbf" />


Game Board

<img width="378" height="392" alt="CheckersGameSetting" src="https://github.com/user-attachments/assets/55cab616-27f3-4ec0-9fa8-3ad59c8fb49e" />


Valid Move Highlighting

<img width="390" height="448" alt="CheckersBoard" src="https://github.com/user-attachments/assets/ab73ca48-4057-4bce-b74c-212ac41961aa" />












🚀 Getting Started
Clone
git clone https://github.com/Eilon99/Checkers-For-Windows.git

Open

Open the solution:

CheckersEilonAndShay.sln

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
