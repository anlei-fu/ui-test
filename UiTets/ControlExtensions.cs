using System;
using System.Windows.Forms;

namespace UiTest
{
    public static class ControlExtensions
    {
        public static void InvokeAfterHandleCreated(this Control control, Action act)
        {
            while (!control.IsHandleCreated) ;
            control.Invoke(act);
        }
    }
}
