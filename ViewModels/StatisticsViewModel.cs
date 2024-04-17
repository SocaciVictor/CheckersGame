using CheckersGame.Core;
using CheckersGame.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.ViewModels
{
    public class StatisticsViewModel : ViewModel
    {
        private string _redWins;

        public string RedWins
        {
            get { return _redWins; }
            set 
            {
                _redWins = value; 
                OnPropertyChanged();
            }
        }

        private string _greyWins;

        public string GreyWins
        {
            get { return _greyWins; }
            set
            {
                _greyWins = value;
                OnPropertyChanged();
            }
        }

        private string _maxPieces;

        public string MaxPieces
        {
            get { return _maxPieces; }
            set
            {
                _maxPieces = value;
                OnPropertyChanged();
            }
        }


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

        public RelayCommand NavigateToMainMenuCommand { get; set; }
        public StatisticsViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToMainMenuCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<MainMenuViewModel>(); }, canExecute: o => true);
        }

        public void GetStatistics()
        {
            try
            {
                using (StreamReader reader = new StreamReader("statistics.txt"))
                {
                    RedWins = reader.ReadLine();
                    GreyWins = reader.ReadLine();
                    MaxPieces = reader.ReadLine();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Eroare la citirea fișierului: " + e.Message);
            }
        }

    }
}
