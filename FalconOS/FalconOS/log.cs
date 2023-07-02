using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class log
    {

        public static void print(string topic, string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[ " + topic + " ] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            return;
        }

        public static void drawTitleBar(string title, ConsoleColor back = ConsoleColor.DarkCyan, ConsoleColor fore = ConsoleColor.Black)
        {
            Console.BackgroundColor = back; Console.ForegroundColor = fore;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(title + new string(' ', Console.WindowWidth - title.Length));
            Console.ResetColor();
        }
    }
}
