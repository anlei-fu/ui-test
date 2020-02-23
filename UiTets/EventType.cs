using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiTets
{
   public enum EventType
    {
        KeyDown,
        KeyUp,
        MouseLeftDown,
        MouseLeftUp,
        MouseMiddleDown,
        MouseMiddleUp,
        MouseRightDown,
        MouseRightUp,
        MouseEvent=MouseLeftDown|MouseLeftUp|MouseRightDown|MouseRightUp|MouseMiddleDown|MouseMiddleUp,
        Sleep,
        Text,
        KeyEvent
    }
}
