using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Sys = Cosmos.System;
using System.Threading.Tasks;
using System.Data;
using System.Threading;
using System.IO;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using Cosmos.System.Network;
using Cosmos.HAL;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.Core;
using System.ComponentModel.Design;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using FalconOS;
using System.Drawing;
using System.Numerics;

namespace FalconOS
{
    public class Shell
    {
        public string os; public string ver;
        public static string buffer = "";
        
        public Shell(string os, string ver)
        {
            this.os = os;
            this.ver = ver;
        }

        public string exec(string cmd, bool root = false, string kidshell = "no")
        {
            data.ProcMgr.addProc(cmd);
            if (cmd == "starty")
            {
                log.print("Yindow Manager", "Starting window manager");
                Thread.Sleep(1500);
                Kernel.gui = true;
            }
            else if (cmd == "sysctl gui=true")
            {
                Kernel.gui = true;
            }
            else if (cmd == "sysctl gui=false")
            {
                Kernel.gui = false;
            }
            else if (cmd == "sysctl reboot")
            {
                log.print("System", "Rebooting.");
                Thread.Sleep(1000);
                Sys.Power.Reboot();
            }
            else if (cmd == "sysctl shutdown")
            {
                log.print("System", "Shutting down.");
                Thread.Sleep(1000);
                Sys.Power.Shutdown();
            }
            else if (cmd == "sysctl info")
            {
                log.sPrint("Basic System Info:");
                log.sPrint("Disk Space: " + data.fs.GetTotalFreeSpace("0:\\") + "/" + data.fs.GetTotalSize("0:\\"));
                log.sPrint("Virtual Machine? [VirtualBox/VMWare/QEMU]: " + Sys.VMTools.IsVirtualBox + "/" + Sys.VMTools.IsVMWare + "/" + Sys.VMTools.IsQEMU);
                log.sPrint("Disks Count: " + data.fs.Disks.Count.ToString());
            }
            else if (cmd.StartsWith("sysctl proclist"))
            {
                log.sPrint("Processes(" + data.ProcMgr.processes.Count + "):");
                foreach (var process in data.ProcMgr.processes)
                {
                    log.sPrint(process);
                }
            }
            else if (cmd.StartsWith("sysctl console-cursor"))
            {
                Console.CursorVisible = true;
            }
            else if (cmd.StartsWith("sysctl"))
            {
                Console.WriteLine(cmd);
                log.sPrint("Invalid Args");
            }
            else if (cmd.StartsWith("falnfo"))
            {
                log.sPrint("Falnfo 1.0");
                if (cmd.StartsWith("falnfo -a"))
                {
                    log.sPrint("FalconOS v1.0.1\nBase version v1.0.0\nFalVM 0.5");
                }
                else if (cmd.StartsWith("falnfo -f"))
                {
                    log.sPrint("FalconOS v1.0.1");
                }
                else if (cmd.StartsWith("falnfo -b"))
                {
                    log.sPrint("Base version v1.0.1");
                }
                else if (cmd.StartsWith("falnfo -vm"))
                {
                    log.sPrint("FalVM 0.5");
                }
                else
                {
                    log.sPrint("Falnfo: Unrecognized Command");
                }
            }
            else if (cmd.StartsWith("touch "))
            {
                try
                {
                    File.Create(cmd.Replace("touch ", data.currentDir));
                }
                catch (Exception)
                {
                    log.sPrint("Can't make a file.");
                }
            }
            else if (cmd.StartsWith("rm") && cmd.StartsWith("rm "))
            {
                try
                {
                    File.Delete(cmd.Replace("rm ", data.currentDir));
                }
                catch (Exception)
                {
                    log.print("rm", "Can't remove the file. Permission denied");
                }
            }
            else if (cmd.StartsWith("mkdir") && cmd.StartsWith("mkdir "))
            {
                try
                {
                    if (!Directory.Exists(cmd.Replace("mkdir ", data.currentDir)))
                    {
                        Directory.CreateDirectory(cmd.Replace("mkdir ", data.currentDir));
                    }
                    else
                    {
                        log.sPrint("ERROR: Can't make directory, (does it already exist?)");
                    }
                }
                catch (Exception)
                {
                    log.sPrint("ERROR: Can't make directory, (does it already exist?)");
                }
            }
            else if (cmd.StartsWith("rmdir") && cmd.StartsWith("rmdir "))
            {
                try
                {
                    Directory.Delete(cmd.Replace("rmdir ", data.currentDir), true);
                }
                catch (Exception)
                {
                    log.sPrint("Can't remove the directory.");
                }
            }
            else if (cmd.StartsWith("cd"))
            {
                if (cmd.StartsWith("cd ..."))
                {
                    data.currentDir = "0:\\";
                    return buffer;
                }
                if (data.protectedPaths.Contains(cmd.Replace("cd ", data.currentDir)) && ((Kernel.cUser.ToLower() != "root") || !root))
                {
                    log.programPrint("cd", "Permission denied");
                    return buffer;
                }
                if (Directory.Exists(cmd.Replace("cd ", data.currentDir + "\\")))
                {
                    data.currentDir += cmd.Replace("cd ", "") + "\\";
                    data.lastDir = cmd.Replace("cd ", "") + "\\";
                }
                else
                {
                    log.sPrint("Invalid Directory!");
                }
            }
            else if (cmd.StartsWith("uname"))
            {
                log.sPrint("uname v1.0 utility");
                if (cmd.StartsWith("uname "))
                {
                    if (cmd.StartsWith("uname -a"))
                    {
                        log.programPrint("uname", data.osname + " " + data.vername + " " + data.ver);
                    }
                    else if (cmd.StartsWith("uname -s"))
                    {
                        log.programPrint("uname", data.osname + " " + data.vername);
                    }
                    else if (cmd.StartsWith("uname -n"))
                    {
                        log.programPrint("uname", "falcon");
                    }
                    else if (cmd.StartsWith("uname -r"))
                    {
                        log.programPrint("uname", data.ver + "-" + data.vername.ToLower());
                    }
                    else if (cmd.StartsWith("uname -v"))
                    {
                        log.programPrint("uname", "Not available.");
                    }
                    else if (cmd.StartsWith("uname -m") || cmd.StartsWith("uname -p") || cmd.StartsWith("uname -i"))
                    {
                        log.programPrint("uname", "x86");
                    }
                    else if (cmd.StartsWith("uname -o"))
                    {
                        log.programPrint("uname", "FalconOS");
                    }
                    else
                    {
                        log.programPrint("uname", "Unrecognized Command!");
                    }
                }
            }
            else if (cmd.StartsWith("ls"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var dirs in Directory.GetDirectories(data.currentDir))
                {
                    if (data.protectedPaths.Contains(data.currentDir + dirs))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    if (dirs.Contains(" "))
                    {
                        log.sPrint("\"" + dirs + "\" ", "");
                    }
                    else
                    {
                        log.sPrint(dirs + " ", "");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var files in Directory.GetFiles(data.currentDir))
                {
                    if (files.Contains(" "))
                    {
                        log.sPrint("\"" + files + "\" ", "");
                    }
                    else
                    {
                        log.sPrint(files + " ", "");
                    }
                }
                log.sPrint("\n", "");
            }
            else if (cmd.StartsWith("fedit "))
            {
                feditor.Run(cmd.Split(' '));
            }
            else if (cmd.StartsWith("clear"))
            {
                Console.Clear();
                log.drawTitleBar("FalconOS: Shell");
            }
            else if (cmd.StartsWith("exit"))
            {
                if (kidshell == "no")
                {
                    Console.Clear();
                    sysmgr.login();
                }
                else
                {
                    log.print("Fash", "Exiting kid instance");
                    return "exited";
                }
            }
            else if (cmd == "") { }
            else if (cmd.StartsWith("fash "))
            {
                log.sPrint("Starting fash..");
                if (File.Exists(cmd.Replace("fash ", data.currentDir)) && cmd.Replace("fash ", data.currentDir).EndsWith(".ash"))
                {
                    string[] lines = File.ReadAllLines(cmd.Replace("fash ", data.currentDir));

                    foreach (string line in lines)
                    {
                        if (!line.StartsWith("#"))
                        {
                            exec(line);
                        }
                    }
                }
                else
                {
                    log.sPrint("Error: Either file doesn't exist [Use: fash <file.ash>] or doesn't end in .ash");
                }
            }
            else if (cmd.StartsWith("fash"))
            {
                Shell Kidshell = new Shell(data.osname, data.ver);
                log.print("Fash", "Entering new instance.");
                var close = true;
                while (close)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    log.sPrint(Kernel.cUser + "@falcon:" + data.currentDir + " # ", "");
                    var input = Console.ReadLine();
                    if (input.Contains("&&"))
                    {
                        var inp = input.Split("&&");
                        foreach (var command in inp)
                        {
                            var output = Kidshell.exec(command, Kernel.asRoot, "yes");
                            if (output == "exited")
                            {
                                close = !close;
                            }
                        }
                    }
                    else
                    {
                        var output = Kidshell.exec(input, Kernel.asRoot, "yes");
                        if (output == "exited")
                        {
                            close = !close;
                        }
                    }
                }
            }
            else if (cmd.StartsWith("echo"))
            {
                if (cmd.StartsWith("echo --no-new-line \""))
                {
                    cmd = cmd.Replace("echo --no-new-line ", "echo ");
                }
                if (cmd.StartsWith("echo \"") && cmd.EndsWith("\""))
                {
                    log.sPrint(cmd.Replace("echo \"", "").TrimEnd('\"'));
                }
            }
            else if (cmd.StartsWith("sleep"))
            {
                if (cmd.StartsWith("sleep "))
                {
                    try
                    {
                        Thread.Sleep(Convert.ToInt32(cmd.Replace("sleep ", "")));
                    }
                    catch (Exception)
                    {
                        log.sPrint("Invalid time [Use: sleep <milliseconds>]");
                    }
                }
            }
            else if (cmd.StartsWith("cat"))
            {
                if (cmd.StartsWith("cat "))
                {
                    if (File.Exists(cmd.Replace("cat ", data.currentDir)))
                    {
                        log.sPrint(File.ReadAllText(cmd.Replace("cat ", data.currentDir)));
                    }
                    else
                    {
                        log.sPrint("ERROR: Invalid File. [Use: cat <file>]");
                    }
                }
            }
            else if (cmd.StartsWith("ver"))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                log.sPrint(this.os + " ", "");
                Console.ResetColor();
                log.sPrint(this.ver);
            }
            else if (cmd.StartsWith("falcp"))
            {
                if (cmd.StartsWith("falcp "))
                {
                    if (cmd.StartsWith("falcp --help") || cmd.StartsWith("falcp -h"))
                    {
                        log.sPrint("FalCompile [falcp] v0.5");
                        log.sPrint("FalCompile is a FalVM compiler to compile .fal files to .fex [Falcon Executable]");
                        log.sPrint("Uses:\n   falcp <file> (to get a <file>.fex file that is compiled)\n   falcp --help/-h (to get this prompt)");
                    }
                    else if (cmd.EndsWith(".fa"))
                    {
                        string[] program;
                        if (File.Exists(cmd.Replace("falcp ", data.currentDir)))
                        {
                            program = File.ReadAllLines(cmd.Replace("falcp ", data.currentDir));
                            var binary = "";
                            foreach (var line in program)
                            {
                                binary += falcmp.compile(line);
                            }
                            File.WriteAllText(cmd.Replace("falcp ", data.currentDir).Replace(".fa", ".fe"), binary);
                        }
                        else
                        {
                            log.programPrint("falcp", "ERROR: File doesn't exist.");
                        }
                    }
                }
                else
                {
                    log.sPrint("FalCompile [falcp] v0.5");
                    log.sPrint("FalCompile is a FalVM compiler to compile .fal files to .fex [Falcon Executable]");
                    log.sPrint("Uses:\n   falcp <file>.fa (to get a <file>.fex file that is compiled)\n   falcp --help/-h (to get this prompt)");
                }
            }
            else if (cmd.StartsWith("pwd"))
            {
                log.sPrint(data.currentDir);
            }
            else if (cmd.StartsWith("sudo "))
            {
                sudo su = new sudo(Kernel.cUser);
                var currentX = Console.CursorLeft; var currentY = Console.CursorTop;
                log.drawTitleBar("FalconOS: Shell: Running root.", ConsoleColor.DarkRed, ConsoleColor.White);
                Console.SetCursorPosition(currentX, currentY);
                su.execAsRoot(cmd.Replace("sudo ", ""));
                log.drawTitleBar("FalconOS: Shell");
                currentX = Console.CursorLeft; currentY = Console.CursorTop;
                Console.SetCursorPosition(currentX, currentY);
            }
            else if (cmd.StartsWith("usrname"))
            {
                log.sPrint(Kernel.cUser);
            }
            else if (cmd.StartsWith("fav"))
            {
                if (cmd.StartsWith("fav "))
                {
                    if (File.Exists(cmd.Replace("fav ", data.currentDir)))
                    {
                        FAV.StartFAV(cmd.Replace("fav ", data.currentDir));
                    }
                    else
                    {
                        File.WriteAllText(cmd.Replace("fav ", data.currentDir), null);
                        FAV.StartFAV(cmd.Replace("fav ", data.currentDir));
                    }
                }
                else
                {
                    FAV.StartFAV(null);
                }
            }
            else if (cmd.StartsWith("screenfetch") || cmd.StartsWith("feofetch"))
            {
                log.sPrint("                                             \r\n               */(%%&&&&&&&&%#(*                \r\n             *(&&&&&&&&&&&&&&&%#/**             \r\n            *%&&&&&&&&&&&&&&&&&&&&&&(*          \r\n        */#&&&&&&&&&&&&&&&&&&&&&&&&&%/*         \r\n    * *#&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&#*      \r\n     *%&&%%##%&&&&&&&&&&&&&&&&&&&&&&&&&#%%%/ *  \r\n     *&#*/(##%&&&&&&&&&&&&&&&&&&&&&&&&&&%*      \r\n     *(*       *%&&&&&&&&&&&&&&&&&&&&&&&&&%/    \r\n                /&&&&&&&&&&&&&&&&&&&&&&( * *  * \r\n                /&&&&&&&&&&&&&&&&&&%**/#*    *  \r\n                /&&&&&&&&&&&&&&&&(#%*           \r\n                *&&&&&&&&&&&&&&&&#              \r\n                 #&&&(/&&&&&&&%(%&/  *          \r\n                 *%&(  /&#**#&/   *             \r\n                   /    **   **                ");
                log.sPrint("FalconOS " + ver + ": Is VM?: " + infochecks.isVM());
            }
            else if (cmd.StartsWith("fetch"))
            {
                if (cmd.StartsWith("fetch "))
                {
                    var fWhat = cmd.Replace("fetch ", "");
                    if (fWhat == "disks")
                    {
                        foreach (var disk in VFSManager.GetLogicalDrives())
                        {
                            log.sPrint(disk);
                        }
                    }
                    else
                    {
                        log.sPrint("fetch v1.0\nFetches different system stuff");
                    }
                }
            }
            else if (cmd.StartsWith("logout"))
            {
                sysmgr.login(true);
            }
            else if (cmd.StartsWith("./"))
            {

                if (cmd.EndsWith(".fe") || !(cmd.EndsWith(".ash")))
                {
                    executor runner = new();
                    if (cmd.Split(' ').Length > 1)
                    {
                        if (cmd.Split(' ').Length == 2)
                        {
                            runner.arg1 = cmd.Split(' ')[1];
                        }
                        if (cmd.Split(' ').Length == 3)
                        {
                            runner.arg1 = cmd.Split(' ')[1];
                            runner.arg2 = cmd.Split(' ')[2];
                        }
                    }
                    runner.executeFE(cmd.Split(' ')[0].Replace("./", data.currentDir));
                }
                else if (cmd.EndsWith(".ash"))
                {
                    if (File.Exists(cmd.Replace("fash ", data.currentDir)) && cmd.Replace("fash ", data.currentDir).EndsWith(".ash"))
                    {
                        string[] lines = File.ReadAllLines(cmd.Replace("fash ", data.currentDir));

                        foreach (string line in lines)
                        {
                            if (!line.StartsWith("#"))
                            {
                                exec(line);
                            }
                        }
                    }
                    else
                    {
                        log.sPrint("Error: File doesn't exist or syntax error.");
                    }
                } else
                {
                    executor runner = new();
                    if (cmd.Split(' ').Length > 1)
                    {
                        if (cmd.Split(' ').Length == 2)
                        {
                            runner.arg1 = cmd.Split(' ')[1];
                        }
                        if (cmd.Split(' ').Length == 3)
                        {
                            runner.arg1 = cmd.Split(' ')[1];
                            runner.arg2 = cmd.Split(' ')[2];
                        }
                    }
                    runner.executeFE(cmd.Split(' ')[0].Replace("./", data.currentDir) + ".fe");
                }
            }
            else if (cmd.StartsWith("asm "))
            {
                if (cmd.Replace("asm ", "") == "--help")
                {
                    log.sPrint("asm v1.0\nBasic assembly language.");
                }
                else
                {
                    if (File.Exists(cmd.Replace("asm ", data.currentDir)) && cmd.Replace("asm ", data.currentDir).EndsWith(".asm"))
                    {
                        var output = cmd.Replace("asm ", data.currentDir).Replace(".asm", ".fe");
                        var text = assemble.compile(File.ReadAllText(cmd.Replace("asm ", data.currentDir)), output);
                        if (text != "ERROR!")
                        {
                            File.WriteAllText(output, text);
                        }
                    }
                    else
                    {
                        log.sPrint("Invalid Extension");
                    }
                }
            }
            else if (cmd.StartsWith("asm"))
            {
                log.sPrint("asm: ", "");
                Console.ForegroundColor = ConsoleColor.Red;
                log.sPrint("fatal error: ", "");
                Console.ForegroundColor = ConsoleColor.White;
                log.sPrint("no input files\ncompilation terminated.");
            }
            else if (cmd.StartsWith("calc "))
            {
                calc calculate = new calc();
                log.sPrint(calculate.Evaluate(cmd.Replace("calc ", "")).ToString());
            }
            else if (cmd.StartsWith("falsay"))
            {
                if (cmd == "falsay")
                {
                    log.sPrint("falsay is the debian equivalent of cowsay. (we got falcons instead)");
                }
                else if (cmd.StartsWith("falsay "))
                {
                    var falconsScript = cmd.Replace("falsay ", "");
                    if ((Console.WindowWidth - ("< " + falconsScript + " >").Length) > 0)
                    {
                        Console.Write("  ");
                        for (var i = 0; i < falconsScript.Length; i++)
                        {
                            Console.Write("_");
                        }
                        Console.WriteLine();
                        Console.WriteLine("< " + falconsScript + " >");
                        Console.Write("  ");
                        for (var i = 0; i < falconsScript.Length; i++)
                        {
                            Console.Write("-", "");
                        }


                        Console.WriteLine("\n    \\\n     \\\n      \\\n       \\\n                                             \r\n               */(%%&&&&&&&&%#(*                \r\n             *(&&&&&&&&&&&&&&&%#/**             \r\n            *%&&&&&&&&&&&&&&&&&&&&&&(*          \r\n        */#&&&&&&&&&&&&&&&&&&&&&&&&&%/*         \r\n    * *#&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&#*      \r\n     *%&&%%##%&&&&&&&&&&&&&&&&&&&&&&&&&#%%%/ *  \r\n     *&#*/(##%&&&&&&&&&&&&&&&&&&&&&&&&&&%*      \r\n     *(*       *%&&&&&&&&&&&&&&&&&&&&&&&&&%/    \r\n                /&&&&&&&&&&&&&&&&&&&&&&( * *  * \r\n                /&&&&&&&&&&&&&&&&&&%**/#*    *  \r\n                /&&&&&&&&&&&&&&&&(#%*           \r\n                *&&&&&&&&&&&&&&&&#              \r\n                 #&&&(/&&&&&&&%(%&/  *          \r\n                 *%&(  /&#**#&/   *             \r\n                   /    **   **                ");
                    }
                    else
                    {
                        Console.Write("Falcon was tired. Too big text!");
                    }
                }
                else
                {
                    log.sPrint("Usage: falsay <text>");
                }
            }
            else if (cmd.StartsWith("printlogo"))
            {
                log.sPrint("                                             \r\n               */(%%&&&&&&&&%#(*                \r\n             *(&&&&&&&&&&&&&&&%#/**             \r\n            *%&&&&&&&&&&&&&&&&&&&&&&(*          \r\n        */#&&&&&&&&&&&&&&&&&&&&&&&&&%/*         \r\n    * *#&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&#*      \r\n     *%&&%%##%&&&&&&&&&&&&&&&&&&&&&&&&&#%%%/ *  \r\n     *&#*/(##%&&&&&&&&&&&&&&&&&&&&&&&&&&%*      \r\n     *(*       *%&&&&&&&&&&&&&&&&&&&&&&&&&%/    \r\n                /&&&&&&&&&&&&&&&&&&&&&&( * *  * \r\n                /&&&&&&&&&&&&&&&&&&%**/#*    *  \r\n                /&&&&&&&&&&&&&&&&(#%*           \r\n                *&&&&&&&&&&&&&&&&#              \r\n                 #&&&(/&&&&&&&%(%&/  *          \r\n                 *%&(  /&#**#&/   *             \r\n                   /    **   **                ");
            }
            else if (cmd.StartsWith("bmgr"))
            {
                if (cmd.StartsWith("bmgr "))
                {
                    if (cmd.StartsWith("bmgr clear"))
                    {
                        if (!root)
                        {
                            if (Kernel.cUser == "root")
                            {
                                buffer = "";
                            }
                            else
                            {
                                log.sPrint("You must use sudo with this command!");
                            }
                        }
                        else
                        {
                            buffer = "";
                        }
                    }
                    else if (cmd.StartsWith("bmgr cas"))
                    {
                        if (cmd.StartsWith("bmgr cas "))
                        {
                            if (!root)
                            {
                                if (Kernel.cUser == "root")
                                {
                                    try
                                    {
                                        File.WriteAllText(cmd.Replace("bmgr cas ", data.currentDir), buffer);
                                        buffer = "";
                                    }
                                    catch (Exception)
                                    {
                                        log.programPrint("bmgr", "Error occurred (maybe file was invalid)");
                                    }
                                }
                                else
                                {
                                    log.sPrint("You must use sudo with this command!");
                                }
                            }
                            else
                            {
                                try
                                {
                                    File.WriteAllText(cmd.Replace("bmgr cas ", data.currentDir), buffer);
                                    buffer = "";
                                }
                                catch (Exception)
                                {
                                    log.programPrint("bmgr", "Error occurred (maybe file was invalid)");
                                }
                            }
                        }
                        else
                        {
                            log.programPrint("bmgr", "no file given for cas, usage: bmgr cas <file>");
                        }
                    }
                }
                else
                {
                    log.programPrint("bmgr", "A command line, shell \"buffer\" utility.\nArgs:\n   clear: clears buffer\n   cas <file>: clears buffer and set its contents to a file.");
                }
            } else if (cmd.StartsWith("changedrive "))
            {
                cmd = cmd.Replace("changedrive ", "");
                foreach (var disk in VFSManager.GetLogicalDrives())
                {
                    if (disk.Contains(cmd))
                    {
                        data.currentDir = cmd + "\\";
                    }
                }
            }
            else if (cmd.StartsWith("fcg "))
            {
                if (File.Exists(cmd.Replace("fcg ", data.currentDir)))
                {
                    fcg newFcg = new fcg(File.ReadAllText(cmd.Replace("fcg ", data.currentDir)));
                    newFcg.Run();
                }
            } else if (cmd.StartsWith("export: currentdir "))
            {
                data.currentDir = cmd.Replace("export: currentdir ", "");
            } else if (cmd.StartsWith("man") && !(cmd.StartsWith("man ")))
            {
                if (cmd == "man" || cmd == "man --help" || cmd == "man -h" || cmd == "man help")
                {
                    Console.WriteLine("man v1.0.1\nFor FalconOS, powered by fcg\nusage: man <command>");
                }
            }
            else if (cmd.StartsWith("man "))
            {
                if (File.Exists(cmd.Replace("man ", data.currentDir)))
                {
                    man manPg = new man(File.ReadAllText(cmd.Replace("man ", data.currentDir)));
                    manPg.Run();
                }
            } else if (cmd.StartsWith("mem ref"))
            {
                Cosmos.Core.Memory.Heap.Collect();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Memory Refreshed!");
            }
            else
            {
                if (File.Exists(data.currentDir + cmd + ".fa"))
                {
                    executor runner = new();
                    if (cmd.Split(' ').Length > 1)
                    {
                        if (cmd.Split(' ').Length == 2)
                        {
                            runner.arg1 = cmd.Split(' ')[1];
                        }
                        if (cmd.Split(' ').Length == 3)
                        {
                            runner.arg1 = cmd.Split(' ')[1];
                            runner.arg2 = cmd.Split(' ')[2];
                        }
                    }
                    runner.executeFE(data.currentDir + cmd.Split(' ')[0] + ".fa");
                }
                if (!String.IsNullOrWhiteSpace(cmd) || !String.IsNullOrEmpty(cmd) !File.Exists(cmd.Split(' ')[0] + ".fa"))
                {
                    log.sPrint("-fash: " + cmd + ": command not found");
                }
            }
            Console.ResetColor();
            data.lastCMD = cmd;
            data.ProcMgr.removeProc(cmd);
            var brBuffer = buffer;
            buffer = "";
            return brBuffer;
        }
    }
}
