using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    public class man
    {
        private string config = "";
        public bool passwd = false;
        public bool output = false;
        public bool darkMode = false;

        private ConsoleColor fore = ConsoleColor.Black;
        private ConsoleColor bFore = ConsoleColor.White;
        private ConsoleColor back = ConsoleColor.White;

        public man(string file)
        {
            this.config = file;
        }

        public void Run()
        {
            bool firstTime; firstTime = true; if (firstTime) { }
            var title = "Title";
            var usage = "";
            var description = "";
            var color = ConsoleColor.Cyan;
            bool dialog = true;

            var file = config.Split('\n');
            foreach (var line in file)
            {
                if (line.StartsWith(":"))
                {
                    title = line.TrimStart(':');
                }
                else if (line.StartsWith("usage: "))
                {
                    usage += line.Replace("usage: ", "");
                }
                else if (line.StartsWith("description: "))
                {
                    description += line.Replace("description: ", "") + "\n";
                }
                else if (line.StartsWith("color:"))
                {
                    var pickedcolor = line.Replace("color: ", "");
                    if (pickedcolor == "blue")
                    {
                        color = ConsoleColor.Cyan;
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
                    //nextPage = line.Replace("goto ", data.currentDir);
                }
            }
            if (dialog)
            {
            redraw:
                Console.BackgroundColor = color;
                Console.ForegroundColor = fore;
                Console.Clear();
                Console.SetCursorPosition(5, 2);
                Console.BackgroundColor = back;
                for (var i = 0; i < Console.WindowHeight - 5; i++)
                {
                    Console.CursorTop++;
                    Console.CursorLeft = 5;
                    Console.Write(new string(' ', Console.WindowWidth - 10));
                }
                Console.CursorLeft = 0;
                Console.CursorTop = Console.WindowHeight - 1;
                Console.BackgroundColor = color;
                Console.ForegroundColor = back;
                Console.Write("Press D for toggling dark mode");
                Console.BackgroundColor = back;
                Console.CursorTop = 2;
                Console.CursorLeft = 5 + 1;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(" Man: ");
                Console.ForegroundColor = fore;
                if (title.Length > 36)
                {
                    var newTitle = "";
                    for (var i = 0; i < title.Length; i++)
                    {
                        if (i == 36)
                        {
                            newTitle += "\n";
                        }
                        newTitle += title[i];
                    }
                    Console.WriteLine(newTitle + " \n");
                }
                else
                {
                    Console.WriteLine(title + " \n");
                }
                Console.CursorLeft = 5;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("  Usage: ");
                Console.ForegroundColor = fore;
                if (usage.Length > Console.WindowWidth - 15)
                {
                    var newusage = "";
                    for (var i = 0; i < usage.Length; i++)
                    {
                        if ((i % 32) == 9)
                        {
                            newusage += "\n";
                        }
                        newusage += usage[i];
                    }
                    Console.WriteLine(" " + newusage);
                }
                else
                {
                    Console.WriteLine("  " + usage);
                }

                Console.CursorLeft = 5;
                Console.CursorTop++;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("  Description:");
                Console.CursorTop++;
                Console.CursorLeft = 8;
                Console.ForegroundColor = fore;
                if (description.Length > Console.WindowWidth - 15)
                {
                    for (var i = 0; i < description.Length; i++)
                    {
                        var tempI = description[i];
                        if (Convert.ToChar(description[i]) == '\n')
                        {
                            Console.CursorTop++;
                            Console.CursorLeft = 7;
                            tempI = ' ';
                        }

                        Console.Write(tempI);
                    }
                }
                else
                {
                    Console.WriteLine("  " + description);
                }
                Console.CursorLeft = Console.WindowWidth - 20;
                Console.CursorTop = Console.WindowHeight - 5;
                Console.ForegroundColor = bFore;
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" <Quit> ");
                Console.CursorLeft = Console.WindowWidth - 20;
                Console.CursorTop = Console.WindowHeight - 5;
                Console.ForegroundColor = bFore;
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" <Quit> ");
            keyReading:
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.ResetColor();
                    Console.Clear();
                    return;
                }
                else if (key.Key == ConsoleKey.D)
                {
                    toggleDarkMode();
                    goto redraw;
                }
                else
                {
                    goto keyReading;
                }
            }
        }

        public void toggleDarkMode()
        {
            darkMode = !darkMode;
            if (darkMode)
            {
                fore = ConsoleColor.White;
                bFore = ConsoleColor.Black;
                back = ConsoleColor.Black;
            }
            else
            {
                fore = ConsoleColor.Black;
                bFore = ConsoleColor.White;
                back = ConsoleColor.White;
            }
        }
    }
}
