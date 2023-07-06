using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;

namespace FalconOS
{
    public class Kernel : Sys.Kernel
    {
        public static Shell shell = new Shell(data.osname, data.ver);

        protected override void BeforeRun()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Welcome to ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("FalconOS");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("!");
            Console.SetCursorPosition(0, 1);
            log.print("Kernel", "Booting up.");
            Thread.Sleep(1000);
            log.print("Kernel", "Setting filesystem up!");
            VFSManager.RegisterVFS(data.fs);
            Thread.Sleep(200);
            log.print("Kernel", "Booting into console mode.");
            Thread.Sleep(2000);
            data.ProcMgr = new processMgr();

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\nLogging in as [root]");
            log.drawTitleBar("FalconOS: Shell");
            Console.SetCursorPosition(0, 8);
        }

        protected override void Run()
        {
            Cosmos.System.KeyEvent keyy;
            var key = Cosmos.System.KeyboardManager.TryReadKey(out keyy);
            if (keyy.Key == Sys.ConsoleKeyEx.UpArrow)
            {
                Console.WriteLine("UP");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("root@falcon:" + data.currentDir + "$ ");
            var input = Console.ReadLine();
            shell.exec(input);
        }
    }
}
