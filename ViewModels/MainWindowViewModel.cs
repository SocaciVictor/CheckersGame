using CheckersGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.ViewModels
{
    public class MainWindowViewModel: Core.ViewModel
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
    }
}
