using CheckersGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace CheckersGame.Models
{
    [Serializable]
    public enum PlayerColor
    {
        None,
        Grey,
        Red
    }

    [Serializable]
    public class Position
    {
        [XmlElement]
        public int X { get; }
        [XmlElement]
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }


    public class CheckerPiece : ObservableObject
    {
        [XmlIgnore]
        private Position _position;

        [XmlElement]
        public Position Position 
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        [XmlIgnore]
        private bool _isKing;

        [XmlElement]
        public bool IsKing 
        {
            get { return  (_isKing); }
            set
            {
                _isKing = value;
                if (Color == PlayerColor.Red)
                {
                    MyImage = "/Assets/RedKing.png";
                }
                else
                {

                    MyImage = "/Assets/GreyKing.png";
                }
            }
        }

        [XmlAttribute]
        public PlayerColor Color { get; set; }

        [XmlIgnore]
        private string _myImage;

        [XmlElement]
        public string MyImage
        {
            get { return _myImage; }
            set
            {
                _myImage = value;
                OnPropertyChanged(nameof(MyImage));
            }
        }

        public CheckerPiece(PlayerColor playerColor, Position pos)
        {
            _position = pos;
            _isKing = false;
            Color = playerColor;
            if (Color == PlayerColor.Red)
            {
                MyImage = "/Assets/RedChecker.png";
            }
            else 
            {

                MyImage = "/Assets/GreyChecker.png";
            }
        }

    }
}
