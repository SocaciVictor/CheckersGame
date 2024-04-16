using CheckersGame.Core;
using CheckersGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.ViewModels
{
    public class AboutViewModel : Core.ViewModel
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
        public AboutViewModel(INavigationService navigation)
        {
            Navigation = navigation;
            NavigateToMainMenuCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<MainMenuViewModel>(); }, canExecute: o => true);
        }
    }
}
