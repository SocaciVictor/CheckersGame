using CheckersGame.Models;
using CheckersGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CheckersGame.Services
{
    public class Player
    {
        public PlayerColor Color { get; set; }
        public int CheckerPieceCount { get; set; }

        public Player(PlayerColor color)
        {
            Color = color;
            CheckerPieceCount = 12;
        }

    }
    public class GameLogic
    {
        public bool _forceJump;

        public Player _playerRed { get; set; }

        public Player _playerGrey { get; set; }

        public Player _currentPlayer { get; set; }

        public Board Board { get; set; }       


        public GameLogic(bool ForceJump)
        {
            Board = new Board();
            _playerRed = new Player(PlayerColor.Red);
            _playerGrey = new Player(PlayerColor.Grey);
            _currentPlayer = _playerRed;
            _forceJump = ForceJump;
        }

        private CheckerPiece GetCheckerPiece(Position pos)
        {
            foreach (CheckerPiece piece in Board.PiecesBoard) 
            { 
                if (piece.Position.X == pos.X && piece.Position.Y == pos.Y) 
                    return piece; 
            }
            return null;
        }
        public void DoMove(CheckerPiece checkerPiece, MovesViewModel movesView) 
        {
            Board.ColorBoard[checkerPiece.Position.X][checkerPiece.Position.Y] = PlayerColor.None;
            Board.ColorBoard[movesView.ChosenMove.X][movesView.ChosenMove.Y] = checkerPiece.Color;
            checkerPiece.Position = movesView.ChosenMove;

            if (checkerPiece.Color == PlayerColor.Red && checkerPiece.Position.X == 0)
            {
                checkerPiece.IsKing = true;
            }
            if (checkerPiece.Color == PlayerColor.Grey && checkerPiece.Position.X == 7)
            {
                checkerPiece.IsKing = true;
            }

            if (movesView.EatenChecker != null)
            {
                foreach (CheckerPiece eatPiece in movesView.EatenChecker)
                {
                    Board.ColorBoard[eatPiece.Position.X][eatPiece.Position.Y] = PlayerColor.None;
                    Board.PiecesBoard.Remove(eatPiece);
                    if (_playerGrey.Color == eatPiece.Color) _playerGrey.CheckerPieceCount--;
                    if (_playerRed.Color == eatPiece.Color) _playerRed.CheckerPieceCount--;
                }
            }
            SwitchPlayer();
        }
        public List<MovesViewModel> GetMoves(CheckerPiece currentCheckerPiece) 
        {
            int[] dx = { 1, 1, -1, -1 };
            int[] dy = { 1, -1, 1, -1 };
            List<MovesViewModel> _newMoves = new List<MovesViewModel>();

            int possibleMoves = 0;
            int directionMove;

            if (_currentPlayer.Color != currentCheckerPiece.Color) { return _newMoves; }

            if (currentCheckerPiece.IsKing == true)
            {
                possibleMoves = 4;
            }
            else
            {
                possibleMoves = 2;
            }

            if (currentCheckerPiece.Color == PlayerColor.Red)
            {
                directionMove = -1;
            }
            else
            {
                directionMove = 1;
            }

            for (int i = 0; i < possibleMoves; i++)
            {
                Position pos = new Position(currentCheckerPiece.Position.X + dx[i] * directionMove, currentCheckerPiece.Position.Y + dy[i]);
                if (IsInBoard(pos) == true && Board.ColorBoard[pos.X][pos.Y] == PlayerColor.None)
                {
                    _newMoves.Add(new MovesViewModel(pos, "#00FF00"));
                }
            }


            Queue<Position> moves = new Queue<Position>();
            List<CheckerPiece> eatenPieces = new List<CheckerPiece>();
            bool[,] boolEatenMatrix = new bool[8, 8];
            moves.Enqueue(currentCheckerPiece.Position);
            while (moves.Count > 0) 
            {
                Position currentPos = moves.Dequeue();
                for (int i = 0; i < possibleMoves; i++)
                {
                    Position newPos = new Position(currentPos.X + dx[i] * directionMove * 2, currentPos.Y + dy[i] * 2);
                    Position eatenPos = new Position(currentPos.X + dx[i] * directionMove, currentPos.Y + dy[i]);

                    if (!IsInBoard(newPos) || !IsInBoard(eatenPos)) { continue; }
                    if (Board.ColorBoard[eatenPos.X][eatenPos.Y] == PlayerColor.None) { continue; }
                    if (Board.ColorBoard[eatenPos.X][eatenPos.Y] == currentCheckerPiece.Color) { continue; }
                    if (Board.ColorBoard[newPos.X][newPos.Y] != PlayerColor.None) { continue; }
                    if (boolEatenMatrix[eatenPos.X,eatenPos.Y] != false) { continue; }


                    boolEatenMatrix[eatenPos.X, eatenPos.Y] = true;
                    eatenPieces.Add(GetCheckerPiece(eatenPos));
                    _newMoves.Add(new MovesViewModel(newPos, "#FF0000", new List<CheckerPiece>(eatenPieces)));

                    if (_forceJump)
                    {
                        moves.Enqueue(newPos);
                        break;
                    }
                }
            }
            return _newMoves;
        }

        private void SwitchPlayer() 
        {
            if (_currentPlayer == _playerRed)
                _currentPlayer = _playerGrey;
            else 
               _currentPlayer = _playerRed;
        }

        public bool EndGame()
        {
            return _playerGrey.CheckerPieceCount == 0 || _playerRed.CheckerPieceCount == 0;
        }

        private bool IsInBoard(Position pos)
        {
            return pos.X >= 0 && pos.X < 8 && pos.Y >=0 && pos.Y < 8;
        }

        public string GetPlayerColor()
        {
            if (_playerRed.CheckerPieceCount == 0)
                return "Grey";
            if (_playerGrey.CheckerPieceCount == 0)
                return "Red";
            return "None";
        }

    }
}
