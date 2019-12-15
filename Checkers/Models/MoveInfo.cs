using System.Collections.Generic;
using Xamarin.Forms;

namespace Checkers.Models
{
    class MoveInfo
    {
        public Frame whoBeat { get; set; }
        public List<Frame> whomBeat = new List<Frame>();
        public List<Frame> canBeat = new List<Frame>();
        public MoveInfo()   {  }
    }
}
