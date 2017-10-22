using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nuterra
{
    public static class Utils
    {
        private static int currentWindowID = int.MaxValue;

        public static int GetWindowID()
        {
            return currentWindowID--;
        }
    }
}
