using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;

namespace FalconOS
{
    public class Kernel : Sys.Kernel
    {
        public static Shell shell = new Shell(data.osname, data.ver);
        public static string cUser;
        public static bool asRoot = false;
        public static string lastCommand = "";

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Welcome to ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("FalconOS");
            Console.ForegroundColor = ConsoleColor.White;
            log.sPrint("!");
            Console.SetCursorPosition(0, 1);
            log.print("Kernel", "Booting up.");
            Thread.Sleep(1000);
            log.print("Kernel", "Setting filesystem up!");
            VFSManager.RegisterVFS(data.fs);
            data.fs.Initialize(true);
            try
            {
                if (!Directory.Exists("0:\\Config\\")) { Directory.CreateDirectory("0:\\Config\\"); }
                if (!File.Exists("0:\\Config\\root.ers"))
                {
                    File.Create("0:\\Config\\root.ers");
                    File.WriteAllText("0:\\Config\\root.ers", "admin ");
                }
                if (!File.Exists("0:\\Config\\pwd.s"))
                {
                    File.Create("0:\\Config\\pwd.s");
                    File.WriteAllText("0:\\Config\\pwd.s", "passwd");
                }
                if (!File.Exists("0:\\Config\\user.s"))
                {
                    File.Create("0:\\Config\\user.s");
                    File.WriteAllText("0:\\Config\\user.s", "user admin");
                }
            }
            catch (Exception)
            {
                log.sPrint("Cannot make configs, System may break.");
            }
            Thread.Sleep(200);
            log.print("Kernel", "Booting into console mode.");
            Thread.Sleep(2000);
            data.ProcMgr = new processMgr();

            sysmgr.login();
            Console.SetCursorPosition(0, 11);
        }

        protected override void Run()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(cUser + "@falcon:" + data.currentDir + " # ");
            var input = "";
            var key = new ConsoleKeyInfo();
            while (!(key.Key == ConsoleKey.Enter))
            {
                Console.SetCursorPosition((cUser + "@falcon:" + data.currentDir + " # " + input).Length, Console.CursorTop);
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(cUser + "@falcon:" + data.currentDir + " # " + lastCommand);
                    input = lastCommand;
                } else if (key.Key == ConsoleKey.Backspace)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    if (input.Length > 0)
                    {
                        input = input.Remove(input.Length - 1, 1);
                    }
                    Console.Write(cUser + "@falcon:" + data.currentDir + " # " + input + " ");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(cUser + "@falcon:" + data.currentDir + " # " + input);
                } else if (key.Key == ConsoleKey.Spacebar)
                {
                    input += " ";
                } else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (Console.CursorLeft > (cUser+"@falcon:"+data.currentDir+" # ").Length) {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                } else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (Console.CursorLeft < (cUser + "@falcon:" + data.currentDir + " # " + input).Length)
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                } else if (key.Key == ConsoleKey.Tab)
                {
                    if (input.StartsWith("cle"))
                    {
                        input = "clear";
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(cUser + "@falcon:" + data.currentDir + " # " + input);
                    }
                }
                else
                {
                    input += key.KeyChar;
                }
            }
            Console.WriteLine();
            if (input.Length >  0)
            {
                input = input.Remove(input.Length - 1, 1);
            }
            if (input.Contains("&&"))
            {
                var inp = input.Split("&&");
                foreach (var cmd in inp)
                {
                    shell.exec(cmd, asRoot);
                }
            } else
            {
                shell.exec(input, asRoot);
            }
            lastCommand = input;
        }
    }
}
