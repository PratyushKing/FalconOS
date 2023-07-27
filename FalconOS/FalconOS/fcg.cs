using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FalconOS
{
    public class fcg
    {
        private string config = "";
        public fcg(string file)
        {
            this.config = file;
        }

        public void Run()
        {
        rerun:
            bool firstTime = true;
            var title = "Title";
            var description = "Text";
            var color = ConsoleColor.Blue;
            string nextPage = null;
            bool dialog = true;

            var file = config.Split('\n');
            foreach (var line in file)
            {
                if (line.StartsWith(":"))
                {
                    title = line.TrimStart(':');
                }
                else if (line.StartsWith("text: "))
                {
                    description = line.Replace("text: ", "");
                }
                else if (line.StartsWith("!dialog"))
                {
                    dialog = true;
                }
                else if (line.StartsWith("color:"))
                {
                    var pickedcolor = line.Replace("color: ", "");
                    if (pickedcolor == "blue")
                    {
                        color = ConsoleColor.Blue;
                    }
                    else if (pickedcolor == "green")
                    {
                        color = ConsoleColor.Green;
                    }
                    else if (pickedcolor == "red")
                    {
                        color = ConsoleColor.Red;
                    }
                    else if (pickedcolor == "black")
                    {
                        color = ConsoleColor.Black;
                    }
                }
                else if (line.StartsWith("goto "))
                {
                    nextPage = line.Replace("goto ", data.currentDir);
                }
            }

            var select = "ok";

            if (dialog)
            {
            redraw:
                Console.BackgroundColor = color;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2), Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2));
                Console.BackgroundColor = ConsoleColor.White;
                for (var i = 0; i < 14; i++)
                {
                    Console.CursorTop++;
                    Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2);
                    Console.CursorVisible = false;
                    Console.Write(new string(' ', Console.WindowWidth / 2));
                }
                Console.CursorTop = Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2);
                Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2) + 1;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(" " + title + " \n");
                Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2);
                Console.WriteLine("  " + description);
                if (select == "ok")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop += 10;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" <OK> ");
                }
                else if (select == "cancel")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop += 10;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(" <OK> ");
                }
            redrawbtns:
                if (select == "ok")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop -= 1;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" <OK> ");
                }
                else if (select == "cancel")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop -= 1;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine(" <OK> ");
                }
            keyReading:
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    if (select == "ok")
                    {
                        if (nextPage == null)
                        {
                            Console.ResetColor();
                            Console.Clear();
                            return;
                        }
                        else if (File.Exists(nextPage))
                        {
                            this.config = File.ReadAllText(nextPage);
                            Run();
                        }
                    }
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (select == "ok")
                    {
                        select = "cancel";
                        goto redrawbtns;
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (select == "cancel")
                    {
                        select = "ok";
                        goto redrawbtns;
                    }
                }
                else
                {
                    goto keyReading;
                }
                Console.Clear();
            }
        }
    }
}
