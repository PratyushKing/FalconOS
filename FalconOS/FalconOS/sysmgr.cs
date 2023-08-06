using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FalconOS
{
    internal static class sysmgr
    {
        public static void login(bool clear = false)
        {
            askuser:
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            log.sPrint("Login to FalconOS!\nUsername: ", "");
            var usr = Console.ReadLine();
            if (usr == "root")
            {
                Kernel.cUser = "root";
            }
            else if (!(File.ReadAllText("0:\\Config\\user.conf").Contains(usr)))
            {
                Console.WriteLine("Invalid User!");
                goto askuser;
            }
            if (!(usr == "root"))
            {
                passask:
                log.sPrint("Password: ", "");
                var pass = "";
                var key = new ConsoleKeyInfo();
                
                
                while (!(key.Key == ConsoleKey.Enter))
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        if (pass.Length > 0)
                        {
                            pass = pass.Remove(pass.Length - 1, 1);
                        }
                    }
                    key = Console.ReadKey(true);
                    pass += key.KeyChar;
                }
                if (pass.Length > 0)
                {
                    pass = pass.Remove(pass.Length - 1, 1);
                }
                if (File.ReadAllText("0:\\Config\\passwd.conf").Contains(pass))
                {
                    Kernel.cUser = usr;
                }
                else
                {
                    if (!(usr == "root"))
                    {
                        Console.WriteLine("Invalid Password!");
                        pass = "";
                        goto passask;
                    }
                }
            }
            log.sPrint("\nLogging in as " + Kernel.cUser);
            if (clear) { Console.Clear(); }
            log.drawTitleBar("FalconOS: Shell");
            Console.CursorTop += 7;
            if (Kernel.cUser == "root")
            {
                Kernel.asRoot = true;
            }
        }
    }
}
