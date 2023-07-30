using Cosmos.System.FileSystem.VFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class initsys
    {
        public initsys()
        {
            var setup = false;
            Console.SetCursorPosition(0, 1);
            log.print("Kernel", "Booting up.");
            log.print("Kernel", "Setting filesystem up!");
            VFSManager.RegisterVFS(data.fs);
            data.fs.Initialize(true);
            if (!Directory.Exists("0:\\Config"))
            {
                setup = true;
            }
            try
            {
                if (!Directory.Exists("0:\\Config\\")) { Directory.CreateDirectory("0:\\Config\\"); }
                if (!File.Exists("0:\\Config\\root.ers"))
                {
                    File.Create("0:\\Config\\root.ers");
                    File.WriteAllText("0:\\Config\\root.ers", "user");
                }
                if (!File.Exists("0:\\Config\\pwd.s"))
                {
                    File.Create("0:\\Config\\pwd.s");
                    File.WriteAllText("0:\\Config\\pwd.s", "passwd");
                }
                if (!File.Exists("0:\\Config\\user.s"))
                {
                    File.Create("0:\\Config\\user.s");
                    File.WriteAllText("0:\\Config\\user.s", "user");
                }
            }
            catch (Exception)
            {
                log.sPrint("Cannot make configs, System may break.");
            }
            data.ProcMgr = new processMgr();
            data.ProcMgr.addProc("initsys");
            if (!Kernel.gui)
            {
                Thread.Sleep(200);
                if (setup)
                {
                    log.print("Kernel", "Setting system up!");
                    log.print("SetupMgr", "Starting fcg");
                    Thread.Sleep(1000);
                    fcg Setup = new fcg(":System Setup [User]\ntext: Enter your username for this FalconOS system!\ncolor:blue\n!text");
                    Setup.Run();
                    if (File.Exists(data.currentDir+"output.txt"))
                    {
                        var text = File.ReadAllText(data.currentDir + "output.txt");
                        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text))
                        {
                            text = "user";
                        }
                        File.WriteAllText("0:\\Config\\root.ers", text);
                        File.WriteAllText("0:\\Config\\user.s", text);
                        fcg Password = new fcg(":System Setup [Password]\ntext: Enter your password for this system!\ncolor:blue\n!text");
                        Password.passwd = true;
                        Password.Run();

                    } else
                    {
                        fcg failed = new fcg(":Setup Failed\ntext: Your setup failed, didn't output the username!\ncolor:red\n!dialog");
                        failed.Run();
                    }
                }
                log.print("Kernel", "Booting into console mode.");

                sysmgr.login();
            }
        }
    }
}
