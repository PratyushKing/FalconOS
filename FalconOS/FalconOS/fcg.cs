using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Schema;

namespace FalconOS
{
    public class fcg
    {
        private string config = "";
        public bool passwd = false;
        private ConsoleColor fore = ConsoleColor.Black;
        private ConsoleColor back = ConsoleColor.White;

        public fcg(string file)
        {
            this.config = file;
        }

        public void Run()
        {
        rerun:
            bool firstTime = true;
            var title = "Title";
            var description = "";
            var color = ConsoleColor.Cyan;
            string nextPage = null;
            bool dialog = false;
            bool text = false;

            var file = config.Split('\n');
            foreach (var line in file)
            {
                if (line.StartsWith(":"))
                {
                    title = line.TrimStart(':');
                }
                else if (line.StartsWith("text: "))
                {
                    description += line.Replace("text: ", "");
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
                    nextPage = line.Replace("goto ", data.currentDir);
                } else if (line.StartsWith("!text"))
                {
                    text = true;
                }
            }

            var select = "ok";

            if (dialog)
            {
            redraw:
                Console.BackgroundColor = color;
                Console.ForegroundColor = fore;
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2), Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2));
                Console.BackgroundColor = back;
                for (var i = 0; i < 14; i++)
                {
                    Console.CursorTop++;
                    Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2);
                    Console.Write(new string(' ', Console.WindowWidth / 2));
                }
                Console.CursorTop = Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2);
                Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2) + 1;
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
                    Console.WriteLine(" " + newTitle + " \n");
                }
                else
                {
                    Console.WriteLine(" " + title + " \n");
                }
                Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2);
                if (description.Length > 32)
                {
                    var newDescription = "";
                    for (var i = 0; i < description.Length; i++)
                    {
                        if ((i % 32) == 1)
                        {
                            newDescription += "\n";
                        }
                        newDescription += description[i];
                    }
                    Console.WriteLine("  " + newDescription);
                }
                else
                {
                    Console.WriteLine("  " + description);
                }
                
                if (select == "ok")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop += 10;
                    Console.ForegroundColor = fore;
                    Console.BackgroundColor = back;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" <OK> ");
                }
                else if (select == "cancel")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop += 10;
                    Console.ForegroundColor = fore;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = back;
                    Console.WriteLine(" <OK> ");
                }
            redrawbtns:
                if (select == "ok")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop -= 1;
                    Console.ForegroundColor = fore;
                    Console.BackgroundColor = back;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(" <OK> ");
                }
                else if (select == "cancel")
                {
                    Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + Console.WindowWidth / 2 - (" <Cancel>  <OK>    ").Length;
                    Console.CursorTop -= 1;
                    Console.ForegroundColor = fore;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.Write(" <Cancel> ");
                    Console.BackgroundColor = back;
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
                    } else
                    {
                        goto redraw;
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (select == "cancel")
                    {
                        select = "ok";
                        goto redrawbtns;
                    } else
                    {
                        goto redraw;
                    }
                }
                else
                {
                    goto keyReading;
                }
                Console.Clear();
            } else if (text)
            {
                Console.BackgroundColor = color;
                Console.ForegroundColor = fore;
                Console.Clear();
                Console.SetCursorPosition(Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2), Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2));
                Console.BackgroundColor = back;
                for (var i = 0; i < 14; i++)
                {
                    Console.CursorTop++;
                    Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2);
                    Console.Write(new string(' ', Console.WindowWidth / 2));
                }
                Console.CursorTop = Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2);
                Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2) + 1;
                Console.ForegroundColor = fore;
                Console.WriteLine(" " + title + " \n");
                Console.CursorLeft = Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2);
                if (description.Length > 32)
                {
                    var first = false;
                    var second = false;
                    Console.Write("  ");
                    for (var i = 0; i < description.Length; i++)
                    {
                        if ((i % 32) == 1 && first && second)
                        {
                            Console.Write("\n");
                            Console.CursorLeft = (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + 2;
                        }
                        Console.Write(description[i]);
                        if (first)
                        {
                            second = true;
                        }
                        first = true;
                    }
                }
                else
                {
                    Console.WriteLine("  " + description);
                }
                Console.SetCursorPosition((Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + 2, Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2) + 13);
                Console.BackgroundColor = color;
                Console.Write(new string('_', (Console.WindowWidth / 2) - 3));
                Console.SetCursorPosition((Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + 2, Console.WindowHeight / 2 - (Console.WindowHeight / 2 / 2) + 13);
                var key = Console.ReadKey(true);
                var input = "";
                while (!(key.Key == ConsoleKey.Enter))
                {
                    if (key.Key == ConsoleKey.Backspace && input.Length > 0 && Console.CursorLeft > (Console.WindowWidth / 2 - (Console.WindowWidth / 2 / 2)) + 2)
                    {
                        Console.CursorLeft--;
                        Console.Write("_");
                        Console.CursorLeft--;
                        if (input.Length > 0)
                        {
                            input = input.Remove(input.Length - 1, 1);
                        }
                    }
                    else if ((Char.IsLetterOrDigit(key.KeyChar) || Char.IsSymbol(key.KeyChar) || Char.IsPunctuation(key.KeyChar) || key.Key == ConsoleKey.Spacebar) && input.Length < ((Console.WindowWidth / 2) - 3) - 1)
                    {
                        if (!passwd)
                        {
                            Console.Write(key.KeyChar);
                            
                        } else
                        {
                            Console.Write('*');
                        }
                        input += key.KeyChar;
                    }
                    key = Console.ReadKey(true);
                }
                if (nextPage == null)
                {
                    Console.ResetColor();
                    Console.Clear();
                    File.WriteAllText(data.currentDir + "output.txt", input);
                    return;
                }
                else if (File.Exists(nextPage))
                {
                    File.WriteAllText(data.currentDir + "output.txt", input);
                    this.config = File.ReadAllText(nextPage);
                    Run();
                }
                Console.Clear();
            }
        }
    }
}
