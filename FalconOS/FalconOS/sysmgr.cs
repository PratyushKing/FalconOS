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
            else if (!(File.ReadAllText("0:\\Config\\user.s").Contains(usr)))
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
                        try
                        {
                            Console.SetCursorPosition(0, Console.CursorTop);
                            if (pass.Length > 0)
                            {
                                pass = pass.Remove(pass.Length - 1, 1);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write("Error: " + ex.ToString());
                            Console.ReadKey();
                        }
                        Console.Write("Password: " + new string('*', pass.Length - 1) + " ");
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("Password: " + new string('*', pass.Length - 1));
                        continue;
                    }
                    key = Console.ReadKey(true);
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                if (pass.Length > 0)
                {
                    pass = pass.Remove(pass.Length - 1, 1);
                }
                if (File.ReadAllText("0:\\Config\\pwd.s").Contains(pass))
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
            Console.CursorTop += 2;
            if (Kernel.cUser == "root")
            {
                Kernel.asRoot = true;
            }
        }
    }
}
