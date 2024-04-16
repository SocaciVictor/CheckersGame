using CheckersGame.Core;
using CheckersGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.ViewModels
{
    public class MainMenuViewModel : Core.ViewModel
    {
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

        public RelayCommand NavigateToStartCommand { get; set; }  

        public RelayCommand NavigateToAboutCommand { get; set; }

        public RelayCommand NavigateToLoadGameCommand { get; set; }

        public MainMenuViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToStartCommand = new RelayCommand(execute:o => { Navigation.NavigateTo<PlayViewModel>(); }, canExecute: o => true);
            NavigateToAboutCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<AboutViewModel>(); }, canExecute: o => true);
            NavigateToLoadGameCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<LoadGameViewModel>(); }, canExecute: o => true);
        }
    }
}
