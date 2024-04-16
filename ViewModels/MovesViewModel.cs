using CheckersGame.Core;
using CheckersGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.ViewModels
{
    public class MovesViewModel : ViewModel
    {
        public Position ChosenMove { get; set; }

        public string Color { get; set; }
        public List<CheckerPiece>  EatenChecker{ get; set; }

        public RelayCommand GetMovePress {  get; set; }
        public MovesViewModel(Position chosenMove, string color, List<CheckerPiece> eatenChecker = null)
        {
            ChosenMove = chosenMove;
            EatenChecker = eatenChecker;
            Color = color;
        }
    }
}
