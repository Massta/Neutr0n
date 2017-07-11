using CocosSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutr0n.Shared.Models
{
    public class Box : GameItem
    {
        public int Counter { get; set; }
        public CCColor4B BoxColor { get; set; }
        private CCLabel _label = new CCLabel("Hello SpriteFont", "fonts/arial", 26, CCLabelFormat.SpriteFont);
        public void Draw()
        {
            _label.Color = new CCColor3B(100, 100, 100);
            _label.PositionX = 0;
            _label.PositionY = 0;
            AddChild(_label);
            DrawRect(new CCRect(0, 0, Width, Height), BoxColor);
            DrawRect(new CCRect(5, 5, Width-10, Height-10), CCColor4B.Black);
        }
    }
}
