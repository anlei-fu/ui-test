using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UiTets
{
  public  class EventRecord
    {
        public EventType EventType { get; set; }
        public Point Location { get; set; }
        public Keys Key { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }

    }
}
