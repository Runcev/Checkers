using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Game
{
    public class MoveAction
    {
        public (int from, int to) Action { get; init; }
    }
}