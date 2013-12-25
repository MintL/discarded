using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Discarded
{
    static class Log
    {
        public static void E(string obj, string method, string message)
        {
            LogMessage("E", obj, method, message);
        }

        public static void W(string obj, string method, string message)
        {
            LogMessage("W", obj, method, message);
        }

        public static void I(string obj, string method, string message)
        {
            LogMessage("I", obj, method, message);
        }

        private static void LogMessage(string level, string obj, string method, string message)
        {
            Console.WriteLine(level + " -- " + method + " (" + obj + "): " + message);
        }
    }
}
