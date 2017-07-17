using System;
using System.Collections.Generic;
using System.Text;

namespace Neutr0n.Shared.Models
{
    public class AIBox : Box
    {
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public bool Dead { get; set; }
    }
}
