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
            Console.SetCursorPosition(0, 1);
            log.print("Kernel", "Booting up.");
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
            data.ProcMgr = new processMgr();
            data.ProcMgr.addProc("initsys");
            if (!Kernel.gui)
            {
                Thread.Sleep(200);
                log.print("Kernel", "Booting into console mode.");

                sysmgr.login();
                Console.SetCursorPosition(0, 11);
            }
        }
    }
}
