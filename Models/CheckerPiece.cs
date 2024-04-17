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
        [XmlAttribute]
        public int X { get; set; }

        [XmlAttribute]
        public int Y { get; set; }

        public Position() { }
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [Serializable]
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
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        private bool _isKing;

        [XmlAttribute]
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

        [XmlAttribute]
        public string MyImage
        {
            get { return _myImage; }
            set
            {
                _myImage = value;
                OnPropertyChanged(nameof(MyImage));
            }
        }

        public CheckerPiece() { }
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
