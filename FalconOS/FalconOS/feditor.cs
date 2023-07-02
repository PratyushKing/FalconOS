using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    public static class feditor
    {
        public static bool repalcevars = false;
        public static bool IDE = false;
        public static int xint;
        public static bool wrap = true;
        static string tosav;
        static string cursor = " ";
        public static string ver = "Feditor v0.7";

        static ConsoleColor BarFg = ConsoleColor.Black;
        static ConsoleColor BarBg = ConsoleColor.DarkCyan;
        static ConsoleColor EnterBg = ConsoleColor.Black;
        static ConsoleColor EnterFg = ConsoleColor.White;
        static ConsoleColor EditorBg = ConsoleColor.Black;
        static ConsoleColor EditorFg = ConsoleColor.White;
        static ConsoleColor MenuBg = ConsoleColor.DarkCyan;
        static ConsoleColor MenuFg = ConsoleColor.Black;
        public static string Run(string[] argv)
        {
            try
            {
                Editor(argv);
                Console.CursorVisible = true;
                Console.CursorSize = 3;
            }
            catch (Exception e)
            {
                Console.WriteLine(" ~ ERROR ~\n" + e.Message);
            }
            return null;
        }


        public static string Editor(string[] argv)
        {
            Console.CursorVisible = false;
            string PATH = "";
            Console.Clear();

            if (argv[1] == "--help") { Console.WriteLine("Feditor is a part of the basic FalconOS utilities used to edit files."); }
            else if (argv[1] == "--ide") { IDE = true; }
            else
            {
                if (!(argv[1].StartsWith("0:\\")))
                {
                    argv[1] = data.currentDir + argv[1];
                }
                PATH = argv[1];
            }

            if (!File.Exists(@PATH))
            {
                File.Create(PATH);
            }

            Console.BackgroundColor = EditorBg;
            string arrow = "";
            string old = File.ReadAllText(@PATH);
            tosav = File.ReadAllText(@PATH);
            for (; ; )
            {
                Console.BackgroundColor = EditorBg;
                Console.ForegroundColor = EditorFg;
                Console.Clear();
                Console.Write("\n\n");
                if (tosav.Split("\n").Length > Console.WindowHeight - 3)
                {
                    for (int i = tosav.Split('\n').Length - Console.WindowHeight - 3; i < tosav.Split('\n').Length; i++)
                    {
                        Console.Write(tosav.Split('\n')[i]);
                        if (i != tosav.Split('\n').Length - 1 || !tosav.EndsWith('\n'))
                        {
                            Console.Write('\n');
                        }
                    }
                }
                else
                {
                    Console.Write(tosav);
                }
                xint = Console.CursorLeft;
                int yint = Console.CursorTop;
                for (int kk = 0; kk < arrow.Length; kk++)
                {
                    if (Console.CursorTop == Console.WindowHeight - 1)
                    {
                        break;
                    }
                    Console.Write(arrow[kk]);
                }
                Console.SetCursorPosition(xint, yint);
                Console.BackgroundColor = EditorFg;
                Console.ForegroundColor = EditorBg;
                if (arrow != "" && arrow[0] != '\n')
                {
                    Console.Write(arrow[0]);
                }
                else
                {
                    Console.Write(cursor);
                }
                xint = Console.CursorLeft;
                yint = Console.CursorTop;
                Console.SetCursorPosition(0, 0);
                Drw(@PATH, tosav, arrow, old);
                Console.SetCursorPosition(xint, yint);
                Console.BackgroundColor = EditorBg;
                Console.ForegroundColor = EditorFg;

                ConsoleKeyInfo input = new ConsoleKeyInfo();
                input = Console.ReadKey(true);
                if (repalcevars == true)
                {
                    if (tosav.Contains("%DATE%"))
                    {
                        var d = DateTime.Now.ToShortDateString();
                        tosav = tosav.Replace("%DATE%", d.ToString());
                    }
                    else if (tosav.Contains("%TIME%"))
                    {
                        DateTime dt = DateTime.Now;
                        var t = dt.ToShortTimeString();
                        tosav = tosav.Replace("%TIME%", t.ToString());
                    }
                    else if (tosav.Contains("%NOW%"))
                    {
                        DateTime dt = DateTime.Now;

                        tosav = tosav.Replace("%NOW%", dt.ToString());
                    }
                }
                if (input.Key == ConsoleKey.Enter)
                {
                    tosav += "\n";

                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.S) != 0)
                    {
                        File.Delete(@PATH);
                        File.AppendAllText(@PATH, tosav + arrow);
                        old = tosav + arrow;
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.O) != 0)
                    {
                        string open = Open();
                        if (open != null && File.Exists(open))
                        {
                            tosav = File.ReadAllText(open);
                            arrow = "";
                            PATH = open;
                        }
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.N) != 0)
                    {
                        Console.BackgroundColor = EnterBg;
                        Console.ForegroundColor = EnterFg;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.WriteLine("[New file name]");
                        Console.SetCursorPosition(20, 14);
                        string newflnam = FeditorTerminal();
                        if (newflnam != null)
                        {
                            tosav = File.ReadAllText(newflnam);
                            arrow = "";
                            PATH = newflnam;
                        }
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.Q) != 0)
                    {
                        Console.BackgroundColor = EnterBg;
                        Console.ForegroundColor = EnterFg;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.WriteLine("[Save as...]");
                        Console.SetCursorPosition(20, 14);
                        string savas = FeditorTerminal();
                        if (savas != null)
                        {
                            File.AppendAllText(@savas, old);
                            PATH = savas;
                        }
                    }

                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.H) != 0)
                    {
                        Replace(tosav, old, arrow, PATH);
                    }

                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.F) != 0)
                    {
                        Find(tosav, old, arrow, PATH);
                    }

                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.X) != 0)
                    {
                        Exit(tosav, old, arrow, PATH);
                        Console.CursorVisible = true; return null;
                    }
                }
                else if ((input.Modifiers & ConsoleModifiers.Control) != 0)
                {
                    if ((input.Key & ConsoleKey.C) != 0)
                    {
                        Console.CursorVisible = true;
                        Console.CursorSize = 3;
                        return "-1";
                    }
                }
                else if (input.Key == ConsoleKey.F1)
                {
                    Console.BackgroundColor = MenuBg;
                    Console.ForegroundColor = MenuFg;
                    Console.SetCursorPosition(0, 2);
                    Console.Write("| File           \n");
                    Console.Write("| S  [SAVE]     S\n");
                    Console.Write("| O  [OPEN]     O\n");
                    Console.Write("| N  [NEW]      N\n");
                    Console.Write("| Q  [SAVE AS]  Q\n");
                    Console.Write("| X  [EXIT]     X\n");
                    Console.ForegroundColor = EditorFg;

                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.S)
                    {
                        File.Delete(@PATH);
                        File.AppendAllText(@PATH, tosav + arrow);
                        old = tosav + arrow;
                    }
                    else if (input.Key == ConsoleKey.O)
                    {
                        string open = Open();
                        if (open != null && File.Exists(open))
                        {
                            tosav = File.ReadAllText(open);
                            arrow = "";
                            PATH = open;
                        }

                    }
                    else if (input.Key == ConsoleKey.N)
                    {
                        Console.BackgroundColor = EnterBg;
                        Console.ForegroundColor = EnterFg;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.WriteLine("[New file name]");
                        Console.SetCursorPosition(20, 14);
                        string newflnam = FeditorTerminal();
                        if (newflnam != null)
                        {
                            tosav = File.ReadAllText(newflnam);
                            arrow = "";
                            PATH = newflnam;
                        }
                    }
                    else if (input.Key == ConsoleKey.Q)
                    {
                        Console.BackgroundColor = EnterBg;
                        Console.ForegroundColor = EnterFg;
                        Console.Clear();
                        Console.SetCursorPosition(20, 13);
                        Console.WriteLine("[Save as...]");
                        Console.SetCursorPosition(20, 14);
                        string savas = FeditorTerminal();
                        if (savas != null)
                        {
                            File.AppendAllText(@savas, old);
                            PATH = savas;
                        }
                    }

                    else if (input.Key == ConsoleKey.X)
                    {
                        Exit(tosav, old, arrow, PATH);
                        Console.CursorVisible = true; return null;
                    }
                    else
                    {
                        Console.BackgroundColor = EditorBg;
                        DrawBar(@PATH, tosav, arrow, old);
                    }


                }
                else if (input.Key == ConsoleKey.F2)

                {
                    Console.BackgroundColor = MenuBg;
                    Console.ForegroundColor = MenuFg;
                    Console.SetCursorPosition(8, 2);
                    Console.Write("| Edit           \n");
                    Console.SetCursorPosition(8, 3);
                    Console.Write("| H  [REPLACE] H\n");
                    Console.SetCursorPosition(8, 4);
                    Console.Write("| K  [FIND]    K\n");
                    Console.ForegroundColor = EditorFg;
                    input = Console.ReadKey();

                    if (input.Key == ConsoleKey.H)
                    {
                        Replace(tosav, old, arrow, PATH);
                    }
                    else if (input.Key == ConsoleKey.K)
                    {
                        Find(tosav, old, arrow, PATH);

                    }
                    else
                    {
                        Console.BackgroundColor = EditorBg;
                        DrawBar(@PATH, tosav, arrow, old);
                    }
                }
                else if (input.Key == ConsoleKey.F3)
                {
                    Console.BackgroundColor = MenuBg;
                    Console.ForegroundColor = MenuFg;
                    Console.SetCursorPosition(19, 2);
                    Console.Write("| Other          \n");
                    Console.SetCursorPosition(19, 4);
                    Console.Write("| A   [ABOUT]   \n");
                    Console.SetCursorPosition(19, 5);
                    Console.Write("| P   [PATH]    \n");
                    Console.ForegroundColor = EditorFg;
                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.A)
                    {
                        Console.BackgroundColor = EditorBg;
                        Console.ForegroundColor = EditorFg;
                        Console.Clear();
                        Console.WriteLine(
                            "    Feditor\n" +
                            "    Version" + ver + "\n"
                            );
                        Console.ReadKey();
                    }
                    else if (input.Key == ConsoleKey.P)
                    {
                        Console.BackgroundColor = EnterBg;
                        Console.ForegroundColor = EnterFg;
                        Console.Clear();
                        Console.SetCursorPosition(20, 14);
                        Console.WriteLine("[New path...]");
                        string np = FeditorTerminal();
                        if (np != null)
                        {
                            File.AppendAllText(np, old);
                            PATH = np;
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = EditorBg;
                        DrawBar(@PATH, tosav, arrow, old);
                    }
                }
                else if (input.Key == ConsoleKey.F4)
                {
                    Console.BackgroundColor = MenuBg;
                    Console.ForegroundColor = MenuFg;

                    Console.SetCursorPosition(29, 2);
                    Console.Write("| Insert          \n");
                    Console.SetCursorPosition(29, 3);
                    Console.Write("| N  [NOW]       \n");
                    Console.SetCursorPosition(29, 4);
                    Console.Write("| T  [TIME]      \n");
                    Console.SetCursorPosition(29, 5);
                    Console.Write("| D  [DATE]      \n");
                    Console.SetCursorPosition(29, 6);
                    Console.Write("| F  [FILE]      \n");
                    Console.SetCursorPosition(29, 7);
                    Console.Write("| A  [ASCII]     \n");
                    Console.ForegroundColor = EditorFg;
                    input = Console.ReadKey();
                    if (input.Key == ConsoleKey.N)
                    {
                        DateTime dt = DateTime.Now;

                        tosav += dt.ToString();
                    }
                    else if (input.Key == ConsoleKey.T)
                    {
                        DateTime dt = DateTime.Now;
                        var t = dt.ToShortTimeString();
                        tosav += t.ToString();
                    }
                    else if (input.Key == ConsoleKey.D)
                    {
                        var dt = DateTime.Now;
                        var d = dt.ToShortDateString();
                        tosav += d.ToString();
                    }
                    else if (input.Key == ConsoleKey.F)
                    {

                        string open = Open();
                        if (open != null)
                        {
                            if (File.Exists(open))
                            {
                                string toconcat = File.ReadAllText(open);
                                tosav += toconcat;
                            }
                            else
                            {
                                DrawBar(@PATH, tosav, arrow, old);
                                Console.SetCursorPosition(20, 13);
                                Console.ForegroundColor = EnterFg;
                                Console.Write("CANNOT FIND FILE!"); Console.SetCursorPosition(27, 13);
                                Console.ForegroundColor = EditorFg;
                                Console.ReadKey();
                                DrawBar(@PATH, tosav, arrow, old);
                            }
                        }
                    }
                    else if (input.Key == ConsoleKey.A)
                    {
                        Console.BackgroundColor = EditorBg;
                        DrawBar(@PATH, tosav, arrow, old);
                        Console.BackgroundColor = EditorBg;
                        Console.Write(@tosav);
                        Console.Write(@arrow);
                        Console.ForegroundColor = EnterBg;
                        Console.BackgroundColor = EnterFg;
                        Console.SetCursorPosition(20, 13);
                        Console.Write("CHAR [                              ]"); Console.SetCursorPosition(26, 13);
                        string chr = FeditorTerminal();
                        if (!Int32.TryParse(chr, out int ascii))
                        {
                            Console.Beep(300, 300);
                        }
                        else
                        {
                            tosav += (char)ascii;
                        }
                        DrawBar(@PATH, tosav, arrow, old);
                    }
                    else
                    {
                        Console.BackgroundColor = EditorBg;
                        Console.Clear();
                        DrawBar(@PATH, tosav, arrow, old);
                    }
                }
                else if (input.Key == ConsoleKey.Home)
                {
                    Console.ResetColor();
                    Console.Clear();
                    Console.CursorVisible = true; return null;
                }

                else if (input.Key == ConsoleKey.Escape)
                {

                    Console.CursorLeft--;
                }

                else if (input.Key == ConsoleKey.Tab)
                {
                    tosav += "\t";

                }
                else if (input.Key == ConsoleKey.Spacebar)
                {
                    tosav += " ";
                }
                else if (input.KeyChar == '{')
                {

                    tosav += "{";
                    if (IDE == true) tosav += "\n"; arrow = "\n}" + arrow;

                }
                else if (input.KeyChar == '(')
                {


                    tosav += "(";
                    if (IDE == true) arrow = ")" + arrow;

                }
                else if (input.KeyChar == '<')
                {

                    tosav += "<";
                    if (IDE == true) arrow = ">" + arrow;

                }
                else if (input.KeyChar == '"')
                {


                    tosav += "\"";
                    if (IDE == true) arrow = "\"" + arrow;

                }
                else if (input.KeyChar == '\'')
                {

                    tosav += "'";
                    if (IDE == true) arrow = "'" + arrow;

                }
                else if (input.KeyChar == '[')
                {

                    tosav += "[";
                    if (IDE == true) arrow = "]" + arrow;

                }
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (tosav.Length != 0)
                    {
                        tosav = tosav.Remove(tosav.Length - 1, 1);
                    }
                }
                else if (input.Key == ConsoleKey.Delete)
                {
                    if (arrow.Length != 0)
                    {
                        arrow = arrow.Remove(0, 1);
                    }
                }
                else if (input.Key == ConsoleKey.Insert)
                {
                    Console.SetCursorPosition(0, 24);
                    Console.Write("[Document locked]");
                    ConsoleKey lockv;
                    for (; ; )
                    {
                        lockv = Console.ReadKey().Key;
                        if (lockv == ConsoleKey.Insert) break;
                        else Console.CursorLeft--;
                    }
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {

                    if (tosav.Length > 0)
                    {
                        arrow = tosav[tosav.Length - 1] + arrow;
                        tosav = tosav.Remove(tosav.Length - 1, 1);
                    }
                    else if (tosav.EndsWith("\n"))
                    {
                        arrow = "\n" + arrow;
                        tosav = tosav.Remove(tosav.Length - 1, 1);
                    }
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length > 0)
                {
                    tosav += arrow[0];
                    arrow = arrow.Remove(0, 1);
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length == 0)
                {
                    Console.CursorLeft--;
                }

                else if (input.Key == ConsoleKey.UpArrow)
                {
                    if (tosav.Length != 0 && Console.CursorTop != 2)
                    {
                        if (tosav.Contains("\n"))
                        {
                            for (; ; )
                            {
                                arrow = tosav[tosav.Length - 1] + arrow;
                                tosav = tosav.Remove(tosav.Length - 1, 1);
                                if (arrow.StartsWith("\n")) break;
                            }
                        }
                    }
                }
                else if (input.Key == ConsoleKey.DownArrow)
                {
                    if (arrow.Contains("\n"))
                    {
                        for (; ; )
                        {
                            tosav += arrow[0];
                            arrow = arrow.Remove(0, 1);
                            if (tosav.EndsWith("\n")) break;
                        }

                    }
                }
                else if (input.Key == ConsoleKey.PageUp)
                {
                    if (tosav.Length != 0)
                    {
                        arrow = tosav + arrow;
                        tosav = "";
                    }
                }

                else if (input.Key == ConsoleKey.PageDown)
                {
                    if (arrow.Length != 0)
                    {
                        tosav += arrow;
                        arrow = "";
                    }
                }

                else if (input.Key == ConsoleKey.Escape && Console.CursorLeft != 0)
                {
                    Console.CursorLeft--;
                }
                else if (input.Key == ConsoleKey.F12)
                {
                    File.Delete(@PATH);
                    File.AppendAllText(@PATH, tosav + arrow);
                    old = tosav + arrow;
                }
                else
                {
                    tosav += input.KeyChar.ToString();
                    if (wrap == true)
                    {
                        if (Console.CursorLeft == 79 && tosav.Contains(" "))
                        {
                            int LastIndex = 0;
                            string lio = tosav;
                            for (int i = 0; i < lio.Length; i++) if (lio[i] == ' ') LastIndex = i;
                            tosav = tosav.Remove(LastIndex, " ".Length).Insert(LastIndex, "\n");
                        }
                    }
                }

            }
        }
        public static void DrawBar(string PATH, string tosav, string arrow, string old)
        {
            Console.Clear();
            Console.ForegroundColor = EditorFg;
            Console.BackgroundColor = EditorBg;
            Console.Clear();
            Drw(PATH, tosav, arrow, old);
        }

        public static void Drw(string PATH, string tosav, string arrow, string old)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = BarFg;
            Console.BackgroundColor = BarBg;
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < 80; i++) Console.Write(" ");
            Console.SetCursorPosition(0, 0);
            Console.Write($"FalconOS: {ver}: " + @PATH);
            if (old != tosav + arrow) Console.WriteLine("*");
            Console.Write("\n");
            Console.SetCursorPosition(0, 1);
            for (int i = 0; i < 80; i++) Console.Write(" ");
            Console.SetCursorPosition(0, 1);
            Console.Write("F1 FILE | F2 EDIT | F3 OTHER | F4 INSERT | F5 CONFIG");
            Console.SetCursorPosition(0, 2);
            Console.ForegroundColor = EditorFg;
        }

        static void Replace(string tosav, string old, string arrow, string PATH)
        {
            Console.BackgroundColor = EditorBg;
            DrawBar(@PATH, tosav, arrow, old);
            Console.BackgroundColor = EditorBg;
            Console.Write(@tosav);
            Console.Write(@arrow);
            Console.ForegroundColor = EnterFg;
            Console.BackgroundColor = EnterBg;
            Console.SetCursorPosition(20, 13);
            Console.Write("Old [                          ]"); Console.SetCursorPosition(20, 14);
            Console.Write("New [                          ]");
            Console.SetCursorPosition(25, 13);
            string oldrep = FeditorTerminal();
            if (oldrep != null)
            {
                Console.SetCursorPosition(25, 14);
                string newrep = FeditorTerminal();
                if (newrep != null)
                {
                    if (tosav.Contains(oldrep))
                    {
                        tosav = tosav.Replace(oldrep, newrep);
                    }
                    if (arrow.Contains(oldrep))
                    {
                        arrow = arrow.Replace(oldrep, newrep);
                    }
                    else
                    {
                        DrawBar(@PATH, tosav, arrow, old);
                        Console.SetCursorPosition(25, 8);
                        Console.WriteLine("CANNOT FIND WORD TO REPLACE!");
                        Console.ReadKey();
                    }
                }

            }
            DrawBar(@PATH, tosav, arrow, old);
        }

        static string Exit(string tosav, string old, string arrow, string PATH)
        {
            if (tosav != old)
            {
                Console.BackgroundColor = EnterBg;
                Console.ForegroundColor = EnterFg;
                Console.Clear();
                bool shouldSave;
                Console.SetCursorPosition(20, 13);
                Console.WriteLine("[Do you want to save changes? Y/N ]");
                Console.SetCursorPosition(20, 14);
                string answer = Console.ReadKey().KeyChar.ToString();
                if (answer.ToLower() == "y") shouldSave = true;
                else shouldSave = false;
                if (shouldSave == false)
                {
                    File.Delete(@PATH);
                    File.AppendAllText(@PATH, old);
                    Console.ResetColor();
                    Console.Clear();
                    Console.CursorVisible = true; return null;
                }
                else if (shouldSave == true)
                {
                    File.Delete(@PATH);
                    File.AppendAllText(@PATH, tosav + arrow);
                    Console.ResetColor();
                    Console.Clear();
                    Console.CursorVisible = true; return null;
                }
            }
            else
            {
                Console.ResetColor();
                Console.Clear();
                Console.CursorVisible = true; return null;
            }
            Console.CursorVisible = true; return null;
        }


        static void Find(string tosav, string old, string arrow, string PATH)
        {
            Console.BackgroundColor = EditorBg;
            DrawBar(@PATH, tosav, arrow, old);
            Console.BackgroundColor = EditorBg;
            Console.Write(@tosav);
            Console.Write(@arrow);
            Console.ForegroundColor = EnterFg;
            Console.BackgroundColor = EnterBg;
            Console.SetCursorPosition(20, 13);
            Console.Write("FIND [                              ]"); Console.SetCursorPosition(26, 13);
            string find = FeditorTerminal();
            if (find != null)
            {
                if (tosav.Contains(find) || arrow.Contains(find))
                {
                    string tofind = "";
                    if (tosav.Contains(find)) for (int i = 0; i < find.Length; i++) tofind += tosav.Replace(find, "!");
                    if (arrow.Contains(find)) for (int i = 0; i < find.Length; i++) tofind += arrow.Replace(find, "!");
                    Console.BackgroundColor = EditorBg;
                    DrawBar(@PATH, tofind, arrow, old);
                    Console.BackgroundColor = EditorBg;
                    Console.ForegroundColor = EditorFg;
                    Console.Write(tofind);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.ReadKey();

                    DrawBar(@PATH, tofind, arrow, old);

                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                DrawBar(@PATH, tosav, arrow, old);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write(@tosav);
                Console.Write(@arrow);
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(20, 13);
                Console.Write($"Text does not contain '{find}'!"); Console.SetCursorPosition(27, 13);
                Console.ReadKey();
                DrawBar(@PATH, tosav, arrow, old);
            }
        }

        static string Open()
        {
            Console.BackgroundColor = EnterBg;
            Console.ForegroundColor = EnterFg;
            Console.Clear();
            Console.SetCursorPosition(20, 0);
            Console.Write("Files:");
            string[] Fils = Directory.GetFiles(Directory.GetCurrentDirectory());
            int numoffiles = Fils.Length;
            for (int i = 0; i < numoffiles; i++)
            {
                Console.SetCursorPosition(20, i + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(Fils[i]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("]\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("[File to open]");
            Console.SetCursorPosition(0, 1);
            string open = FeditorTerminal();
            return open;
        }

        static public string FeditorTerminal()
        {
            string toreturn = "";
            for (; ; )
            {
                string arrow = "";
                ConsoleKeyInfo input = Console.ReadKey();
                if (input.Key == ConsoleKey.Enter)
                {
                    return toreturn + arrow;
                }
                else if (input.Key == ConsoleKey.Backspace)
                {
                    if (toreturn.Length != 0)
                    {

                        Console.CursorLeft--;
                        toreturn = toreturn.Remove(toreturn.Length - 1, 1);
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                    else
                    {
                        Console.CursorLeft++;
                    }
                }
                else if (input.Key == ConsoleKey.LeftArrow)
                {

                    if (toreturn.Length > 0)
                    {
                        arrow = toreturn[toreturn.Length - 1] + arrow;
                        toreturn = toreturn.Remove(toreturn.Length - 1, 1);
                    }
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length > 0)
                {
                    toreturn += arrow[0];
                    arrow = arrow.Remove(0, 1);
                }
                else if (input.Key == ConsoleKey.RightArrow && arrow.Length == 0)
                {
                    Console.CursorLeft--;
                }
                else if (input.Key == ConsoleKey.Escape)
                {
                    return null;
                }
                else
                {
                    toreturn += input.KeyChar.ToString();
                }
            }
        }
    }
}