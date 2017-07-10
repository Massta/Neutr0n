using CocosSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neutr0n.Shared.Models
{
    public abstract class GameItem : CCDrawNode
    {
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
