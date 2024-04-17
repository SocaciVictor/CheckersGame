using CheckersGame.Core;
using CheckersGame.Models;
using CheckersGame.Services;
using Microsoft.Win32;
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

        public CheckerPieceViewModel pressCheckerPiece { get; set; }

        private double _buttonRedOpacity;

        public double ButtonRedOpacity
        {
            get { return _buttonRedOpacity; }
            set
            {
                _buttonRedOpacity = value;
                OnPropertyChanged(); 
            }
        }

        private double _buttonGreyOpacity;

        public double ButtonGreyOpacity
        {
            get { return _buttonGreyOpacity; }
            set
            {
                _buttonGreyOpacity = value;
                OnPropertyChanged();
            }
        }


        private string _saveNameGame;
        public string SaveNameGame 
        {
            get => _saveNameGame;
            set
            {
                _saveNameGame = value;
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

            if (Logic._currentPlayer.Color == PlayerColor.Red)
            {
                ButtonRedOpacity = 1;
                ButtonGreyOpacity = 0.2;
            }
            else
            {
                ButtonGreyOpacity = 1;
                ButtonRedOpacity = 0.2;
            }

            if (Logic.EndGame())
            {
                MessageBox.Show("Player " + Logic.GetPlayerColor() + " win!");
                Score(Logic.GetPlayerColor());
            }
        }
        public RelayCommand BackToMainMenu {  get; set; }

        public GameViewModel(INavigationService navigation)
        {
            GameBoard = new ObservableCollection<CheckerPieceViewModel>();
            MovesBoard = new ObservableCollection<MovesViewModel>();
            Navigation = navigation;
            BackToMainMenu = new RelayCommand(execute: o => { Navigation.NavigateTo<MainMenuViewModel>(); }, canExecute: o => true);

        }

        public RelayCommand NewGameCommand => new RelayCommand((obj) => InitGame(Logic._forceJump));

        public void InitGame(bool ForceJump)
        {
            Logic = new GameLogic(ForceJump);
            pressCheckerPiece = null;
            SaveNameGame = null;
            GameBoard.Clear();
            MovesBoard.Clear();
            foreach (CheckerPiece piece in Logic.Board.PiecesBoard)
            {
                GameBoard.Add(new CheckerPieceViewModel(piece, GetPiecePressCommand));
            }
            ButtonRedOpacity = 1;
            ButtonGreyOpacity = 0.2;
        }

        public RelayCommand SaveGameCommand => new RelayCommand((obj) => SaveGame());
        private void SaveGame()
        {
            string directoryPath = "../../GameSaves/";
            string filePath = Path.Combine(directoryPath, SaveNameGame + ".xml");

            try
            {
                // Verificăm dacă directorul există, altfel îl creăm
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Salvăm fișierul
                XmlSerializer xmlSer = new XmlSerializer(typeof(GameLogic));
                using (FileStream file = new FileStream(filePath, FileMode.Create))
                {
                    xmlSer.Serialize(file, this.Logic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            SaveNameGame = null;
            MessageBox.Show("Salvarea cu succes!");
        }

        public RelayCommand LoadGameCommand => new RelayCommand(LoadGameFromFile);

        public void LoadGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                LoadGameFromFile(fileName);
            }
        }

        public void LoadGameFromFile(object sender)
        {
            try
            {
                string fileName = (sender as string);
                XmlSerializer xmlser = new XmlSerializer(typeof(GameLogic));
                using (FileStream file = new FileStream(fileName, FileMode.Open))
                {
                    Logic = (xmlser.Deserialize(file) as GameLogic);
                    file.Dispose();
                    pressCheckerPiece = null;
                    GameBoard.Clear();
                    MovesBoard.Clear();
                    for (int i = 0; i < Logic.Board.PiecesBoard.Count; i++)
                    {
                        GameBoard.Add(new CheckerPieceViewModel(Logic.Board.PiecesBoard[i], GetPiecePressCommand));
                    }
                    if (Logic._currentPlayer.Color == PlayerColor.Red)
                    {
                        ButtonRedOpacity = 1;
                        ButtonGreyOpacity = 0.2;
                    }
                    else
                    {
                        ButtonGreyOpacity = 1;
                        ButtonRedOpacity = 0.2;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Score(string winner)
        {
            int redWin = 0;
            int blueWin = 0;
            int numberMaxOfPieces = 0;

            try
            {
                using (StreamReader reader = new StreamReader("statistics.txt"))
                {
                    redWin = int.Parse(reader.ReadLine());
                    blueWin = int.Parse(reader.ReadLine());
                    numberMaxOfPieces = int.Parse(reader.ReadLine());
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Eroare la citirea fișierului: " + e.Message);
            }

            if (winner == "red")
            {
                redWin++;
            }
            else
            {
                blueWin++;
            }

            if (numberMaxOfPieces < GameBoard.Count())
            {
                numberMaxOfPieces = GameBoard.Count();
            }
            using (StreamWriter writer = new StreamWriter("statistics.txt"))
            {
                writer.WriteLine(redWin);
                writer.WriteLine(blueWin);
                writer.Write(numberMaxOfPieces);
            }
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
