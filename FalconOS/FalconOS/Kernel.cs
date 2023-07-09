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
                Console.WriteLine("Cannot make configs, System may break.");
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
            var input = Console.ReadLine();
            shell.exec(input, asRoot);
        }
    }
}
