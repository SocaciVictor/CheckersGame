using CheckersGame.Core;
using CheckersGame.Models;
using CheckersGame.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CheckersGame.ViewModels
{
    public class CheckerPieceViewModel : ViewModel
    {
        public RelayCommand RemoveCheckerCommand { get; set; }

        public CheckerPieceViewModel(CheckerPiece c, RelayCommand command)
        {
            SimpleModelChecker = c;
            RemoveCheckerCommand = command;
        }
        public CheckerPiece SimpleModelChecker { get; set; }

    }
}
