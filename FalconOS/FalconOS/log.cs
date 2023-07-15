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
            Console.ResetColor();
            log.sPrint(message);
            return;
        }

        public static void programPrint(string program, string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(program);
            Console.ResetColor();
            log.sPrint(": " + message);
            return;
        }

<<<<<<< HEAD
        public static void sPrint(string text = "\n")
        {
            log.sPrint(text);
            Shell.buffer += text + "\n";
        }
=======
        public static void print(string text = "")
        {
            Console.Write(text + "\n"); Shell.buffer += text; return;
        }

>>>>>>> 15fe22673d59a0d51d432693317421e44efc1937
        public static void drawTitleBar(string title, ConsoleColor back = ConsoleColor.DarkCyan, ConsoleColor fore = ConsoleColor.Black)
        {
            Console.BackgroundColor = back; Console.ForegroundColor = fore;
            Console.SetCursorPosition(0, 0);
            log.sPrint(title + new string(' ', Console.WindowWidth - title.Length));
            Console.ResetColor();
        }

        public static void drawBar(string text, ConsoleColor back = ConsoleColor.DarkCyan, ConsoleColor fore = ConsoleColor.Black, bool newLine = true)
        {
            Console.BackgroundColor = back; Console.ForegroundColor = fore;
            Console.Write(text + new string(' ', Console.WindowWidth - text.Length - 1));
            if (newLine) { Console.Write("\n"); }
            Console.ResetColor();
        }
    }
}
