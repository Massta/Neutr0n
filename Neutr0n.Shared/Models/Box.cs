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
            DrawRect(new CCRect(0, 0, Width, Height), CCColor4B.Blue);
            DrawRect(new CCRect(5, 5, Width-10, Height-10), CCColor4B.Black);
        }
    }
}
