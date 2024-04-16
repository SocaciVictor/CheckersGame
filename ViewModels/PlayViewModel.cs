using CheckersGame.Core;
using CheckersGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.ViewModels
{
    public class PlayViewModel : Core.ViewModel
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

        public RelayCommand NavigateToMainMenuCommand { get; set; }

        public RelayCommand NavigateToGameCommand { get; set; }

        private bool _forceJump = false;
        public bool ForceJump 
        {
            get => _forceJump;
            set
            {
                _forceJump = value;
                OnPropertyChanged();
            }
        }

        public PlayViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToMainMenuCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<MainMenuViewModel>(); }, canExecute: o => true);
            NavigateToGameCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<GameViewModel>();
                                                                    (Navigation.CurrentView as GameViewModel).InitGame(ForceJump);
                                                                    }, canExecute: o => true);
        }
    }
}
