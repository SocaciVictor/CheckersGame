using CheckersGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace CheckersGame.Models
{
    [Serializable]
    public class Board : ObservableObject
    {
        [XmlArray]
        public List<CheckerPiece> PiecesBoard { get; set; }

        [XmlArray]
        public List<List<PlayerColor>> ColorBoard { get; set; }
        public Board()
        {
            PiecesBoard = new List<CheckerPiece>();
            ColorBoard = new List<List<PlayerColor>>();
            InitBoard();
        }


        public void InitBoard()
        {
            
            for (int row = 0; row < 8; row++)
            {
                ColorBoard.Add(new List<PlayerColor>());    
                for (int col = 0; col < 8; col++)
                {
                    if ((row + col) % 2 == 1)
                    {
                        if (row < 3)
                        {
                            ColorBoard[row].Add(PlayerColor.Grey);
                            PiecesBoard.Add(new CheckerPiece(PlayerColor.Grey, new Position(row, col)));
                        }
                        else if (row > 4)
                        {
                            ColorBoard[row].Add(PlayerColor.Red);
                            PiecesBoard.Add(new CheckerPiece(PlayerColor.Red, new Position(row, col)));
                        }
                        else
                        {
                            ColorBoard[row].Add(PlayerColor.None);
                           
                        }
                    }
                    else
                    {
                        ColorBoard[row].Add(PlayerColor.None);
                  
                    }
                }
            }
        }
    }
}
