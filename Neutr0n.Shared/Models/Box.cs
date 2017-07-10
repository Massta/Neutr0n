using CocosSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutr0n.Shared.Models
{
    public class Box : GameItem
    {
        public int Counter { get; set; }
        public void Draw()
        {
            DrawRect(new CCRect(PositionX, PositionY, Width, Height), CCColor4B.Blue);
            DrawString((int)(PositionX + 10), (int)(PositionY + 10), "{0}", Counter);
        }
    }
}
