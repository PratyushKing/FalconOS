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
            try
            {
                VFSManager.RegisterVFS(data.fs);
            }
            catch (Exception)
            {
                log.print("Kernel", "Filesystem initialization failed!");
                Console.ReadKey();
                Cosmos.System.Power.Shutdown();
            }
            if (!Directory.Exists(data.baseDir + "Config"))
            {
                setup = true;
            }
                if (!Directory.Exists(data.baseDir + "Config\\")) { Directory.CreateDirectory(data.baseDir + "Config\\"); }
                if (!File.Exists(data.baseDir + "Config\\root.conf"))
                {
                    File.WriteAllText(data.baseDir + "Config\\root.conf", "user");
                }
                if (!File.Exists(data.baseDir + "Config\\passwd.conf"))
                {
                    File.WriteAllText(data.baseDir + "Config\\passwd.conf", "passwd");
                }
                if (!File.Exists(data.baseDir + "Config\\user.conf"))
                {
                    File.WriteAllText(data.baseDir + "Config\\user.conf", "user");
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

                    //Setup
                    fcg Setup = new fcg(":System Setup [User]\ntext: Enter your username for this FalconOS system!\ncolor: blue\n!text");
                    Setup.Run();
                    if (File.Exists(data.currentDir + "output.txt"))
                    {
                        var text = File.ReadAllText(data.currentDir + "output.txt");
                        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrEmpty(text))
                        {
                            text = "user";
                        }
                        File.WriteAllText(data.baseDir + "Config\\root.conf", text);
                        File.WriteAllText(data.baseDir + "Config\\user.conf", text);
                        fcg Password = new fcg(":System Setup [Password]\ntext: Enter your password for " + text + "!\ncolor: blue\n!text");
                        Password.passwd = true;
                        Password.Run();
                        var texta = File.ReadAllText(data.currentDir + "output.txt");
                        if (string.IsNullOrEmpty(texta) || string.IsNullOrWhiteSpace(texta))
                        {
                            texta = "passwd";
                            fcg failed = new fcg(":Setup Warning\ntext: Your setup failed, didn't output password! Using default password 'passwd'\ncolor: green\n!dialog");
                            failed.Run();
                        }
                        File.WriteAllText(data.baseDir + "Config\\passwd.conf", texta);
                        fcg askRoot = new fcg(":System Setup [Root?]\ntext: Would you like to be root user?\ncolor: blue\n!yesno");
                        askRoot.Run();
                        if (askRoot.output)
                        {
                            File.WriteAllText(data.baseDir + "Config\\root.conf", text);
                        }

                        File.Delete(data.baseDir + "output.txt");
                    } else
                    {
                        fcg failed = new fcg(":Setup Failed\ntext: Your setup failed, didn't output the username!\ncolor:red\n!dialog");
                        failed.Run();
                    }
                }
                fcg success = new fcg(":Setup Success\ntext: Your setup has worked! Your username and password has been set!");
                log.print("Kernel", "Booting into console mode.");

                sysmgr.login();
            }
        }
    }
}
