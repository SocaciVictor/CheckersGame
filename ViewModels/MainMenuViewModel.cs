using CheckersGame.Core;
using CheckersGame.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

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

        public RelayCommand LoadGameNavToCommand => new RelayCommand(LoadGameNavTo);

        public MainMenuViewModel(INavigationService navigation) 
        {
            Navigation = navigation;
            NavigateToStartCommand = new RelayCommand(execute:o => { Navigation.NavigateTo<PlayViewModel>(); }, canExecute: o => true);
            NavigateToAboutCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<AboutViewModel>(); }, canExecute: o => true);
            NavigateToLoadGameCommand = new RelayCommand(execute: o => { Navigation.NavigateTo<LoadGameViewModel>(); }, canExecute: o => true);
        }

        private void LoadGameNavTo(object sender)
        {

           
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                sender = (fileName as object);
                Navigation.NavigateTo<MainMenuViewModel>();
                Navigation.NavigateTo<GameViewModel>();
                (Navigation.CurrentView as GameViewModel).LoadGameFromFile(sender as string);

            }
           
        }

        
    }
}
