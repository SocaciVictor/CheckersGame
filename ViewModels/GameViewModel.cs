using CheckersGame.Core;
using CheckersGame.Models;
using CheckersGame.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace CheckersGame.ViewModels
{
    public class GameViewModel : Core.ViewModel
    {

        public ObservableCollection<CheckerPieceViewModel> GameBoard { get; set; }

        public ObservableCollection<MovesViewModel> MovesBoard { get; set; }

        private CheckerPieceViewModel pressCheckerPiece { get; set; }

        private string _nameSave;
        public string NameSave 
        {
            get => _nameSave;
            set
            {
                _nameSave = value;
                OnPropertyChanged();
            }
        }


        private GameLogic Logic { get; set; }

        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand GetPiecePressCommand => new RelayCommand(GetPiecePress);
        private void GetPiecePress(object param)
        {
            pressCheckerPiece = (param as CheckerPieceViewModel);
            if (pressCheckerPiece == null) 
            {
                return;
            }
            MovesBoard.Clear();
            foreach (MovesViewModel move in Logic.GetMoves(pressCheckerPiece.SimpleModelChecker))
            {
                MovesBoard.Add(move);
                move.GetMovePress = GetMovePressCommand;
            }
        }

        public RelayCommand GetMovePressCommand => new RelayCommand(GetMovePress);

        private void GetMovePress(object param)
        {
            MovesViewModel move = (param as MovesViewModel);
            if (move == null)
            {
                return;
            }
            MovesBoard.Clear();
            Logic.DoMove(pressCheckerPiece.SimpleModelChecker, move);

            if (move.EatenChecker != null)
            {
                foreach (CheckerPiece eatChecker in move.EatenChecker)
                {
                    GameBoard.Remove(GetCheckerPieceViewModel(eatChecker));
                }
            }

            pressCheckerPiece = null;

            if (Logic.EndGame())
            {
                MessageBox.Show("Player " + Logic.GetPlayerColor() + " win!");
            }
        }


        public GameViewModel(INavigationService navigation)
        {
            GameBoard = new ObservableCollection<CheckerPieceViewModel>();
            MovesBoard = new ObservableCollection<MovesViewModel>();
            Navigation = navigation;
            
        }

        public RelayCommand NewGameCommand => new RelayCommand((obj) => InitGame(Logic._forceJump));

        public void InitGame(bool ForceJump)
        {
            Logic = new GameLogic(ForceJump);
            pressCheckerPiece = null;
            GameBoard.Clear();
            MovesBoard.Clear();
            foreach (CheckerPiece piece in Logic.Board.PiecesBoard)
            {
                GameBoard.Add(new CheckerPieceViewModel(piece, GetPiecePressCommand));
            }
        }

        public RelayCommand SaveGameCommand => new RelayCommand((obj) => SaveGame());
        private void SaveGame()
        {
            MessageBox.Show(NameSave);
            //XmlSerializer xmlSer = new XmlSerializer(typeof());
            //FileStream file = new FileStream(, FileMode.Create);
            //xmlSer.Serialize(file, this);
            //file.Dispose();
        }

        public CheckerPieceViewModel GetCheckerPieceViewModel(CheckerPiece checkerPiece)
        {
            foreach (CheckerPieceViewModel checkerPieceViewModel  in GameBoard)
            {
                if (checkerPieceViewModel.SimpleModelChecker == checkerPiece)
                    return checkerPieceViewModel;
            }
            return null;
        }

    }
}
