using CocosSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutr0n.Shared.Models
{
    public class Box : GameItem
    {
        private int _counter { get; set; }
        public int Counter
        {
            get
            {
                return _counter;
            }
            set
            {
                _counter = value;
                if(_label != null)
                {
                    _label.Text = value.ToString();
                }
            }
        }
        public CCColor4B BoxColor { get; set; }
        private CCLabel _label = new CCLabel("0", "Arial", 66)
        {
            Color = CCColor3B.White
        };
        public void Draw()
        {
            _label.PositionX = Width/2;
            _label.PositionY = Height/2;
            AddChild(_label);
            DrawRect(new CCRect(0, 0, Width, Height), BoxColor);
            DrawRect(new CCRect(5, 5, Width - 10, Height - 10), CCColor4B.Black);
        }
    }
}
