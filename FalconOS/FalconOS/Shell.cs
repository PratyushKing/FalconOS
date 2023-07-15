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

        public void exec(string cmd, bool root = false)
        {
            data.ProcMgr.addProc(cmd);
            if (cmd == "starty")
            {
                log.print("Yindow Manager", "Starting window manager");
                Thread.Sleep(1500);
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
                log.print("Basic System Info:");
                log.print("Disk Space: " + data.fs.GetTotalFreeSpace("0:\\") + "/" + data.fs.GetTotalSize("0:\\"));
                log.print("Virtual Machine? [VirtualBox/VMWare/QEMU]: " + Sys.VMTools.IsVirtualBox + "/" + Sys.VMTools.IsVMWare + "/" + Sys.VMTools.IsQEMU);
                log.print("Disks Count: " + data.fs.Disks.Count.ToString());
            }
            else if (cmd.StartsWith("sysctl proclist"))
            {
                log.print("Processes(" + data.ProcMgr.processes.Count + "):");
                foreach (var process in data.ProcMgr.processes)
                {
                    log.print(process);
                }
            }
            else if (cmd.StartsWith("sysctl console-cursor"))
            {
                Console.Write("Console-Cursor: ");
                if (cmd.StartsWith("sysctl console-cursor false"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    log.print("OFF");
                    Console.CursorVisible = false;
                    Console.ResetColor();
                }
                else if (cmd.StartsWith("sysctl console-cursor true"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    log.print("ON");
                    Console.CursorVisible = true;
                    Console.ResetColor();
                }
            }
            else if (cmd.StartsWith("sysctl"))
            {
                log.print("Invalid Args");
            }
            else if (cmd.StartsWith("falnfo"))
            {
                log.print("Falnfo 1.0");
                if (cmd.StartsWith("falnfo -a"))
                {
                    log.print("FalconOS v1.0.1\nBase version v1.0.0\nFalVM 0.5");
                }
                else if (cmd.StartsWith("falnfo -f"))
                {
                    log.print("FalconOS v1.0.1");
                }
                else if (cmd.StartsWith("falnfo -b"))
                {
                    log.print("Base version v1.0.1");
                }
                else if (cmd.StartsWith("falnfo -vm"))
                {
                    log.print("FalVM 0.5");
                }
                else
                {
                    log.print("Falnfo: Unrecognized Command");
                }
            }
            else if (cmd.StartsWith("touch"))
            {
                try
                {
                    File.Create(cmd.Replace("touch ", data.currentDir));
                }
                catch (Exception)
                {
                    log.print("Can't make a file.");
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
                    log.print("Can't remove the file.");
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
                        log.print("ERROR: Can't make directory, (does it already exist?)");
                    }
                }
                catch (Exception)
                {
                    log.print("ERROR: Can't make directory, (does it already exist?)");
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
                    log.print("Can't remove the directory.");
                }
            }
            else if (cmd.StartsWith("cd"))
            {
                if (cmd.Contains("Config") && data.currentDir == "0:\\" && Kernel.cUser.ToLower() != "root")
                {
                    log.programPrint("cd", "Permission denied");
                }
                if (cmd.StartsWith("cd ..."))
                {
                    data.currentDir = "0:\\";
                    return;
                }
                if (Directory.Exists(cmd.Replace("cd ", data.currentDir + "\\")))
                {
                    data.currentDir += cmd.Replace("cd ", "") + "\\";
                    data.lastDir = cmd.Replace("cd ", "") + "\\";
                }
                else
                {
                    log.print("Invalid Directory!");
                }
            }
            else if (cmd.StartsWith("uname"))
            {
                log.print("uname v1.0 utility");
                if (cmd.StartsWith("uname "))
                {
                    if (cmd.StartsWith("uname -a"))
                    {
                        log.programPrint("uname", data.osname + " " + data.vername + " " + data.ver);
                    }
                    else if (cmd.StartsWith("uname -s"))
                    {
                        log.programPrint("uname", "Falcon Stable");
                    }
                    else if (cmd.StartsWith("uname -n"))
                    {
                        log.programPrint("uname", "falcon");
                    }
                    else if (cmd.StartsWith("uname -r"))
                    {
                        log.programPrint("uname", "1.0.1-normal");
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
                        log.programPrint("uname", "Falcon");
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
                    Console.Write("\"" + dirs + "\" ");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                foreach (var files in Directory.GetFiles(data.currentDir))
                {
                    Console.Write("\"" + files + "\" ");
                }
                Console.Write("\n");
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
                Console.Clear();
                sysmgr.login();
            }
            else if (cmd == "") { }
            else if (cmd.StartsWith("fash "))
            {
                log.print("Starting fash..");
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
                    log.print("Error: Either file doesn't exist [Use: fash <file.ash>] or doesn't end in .ash");
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
                    log.print(cmd.Replace("echo \"", "").TrimEnd('\"'));
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
                        log.print("Invalid time [Use: sleep <milliseconds>]");
                    }
                }
            }
            else if (cmd.StartsWith("cat"))
            {
                if (cmd.StartsWith("cat "))
                {
                    if (File.Exists(cmd.Replace("cat ", data.currentDir)))
                    {
                        log.print(File.ReadAllText(cmd.Replace("cat ", data.currentDir)));
                    }
                    else
                    {
                        log.print("ERROR: Invalid File. [Use: cat <file>]");
                    }
                }
            }
            else if (cmd.StartsWith("ver"))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(this.os + " ");
                Console.ResetColor();
                log.print(this.ver);
            }
            else if (cmd.StartsWith("falcp"))
            {
                if (cmd.StartsWith("falcp "))
                {
                    if (cmd.StartsWith("falcp --help") || cmd.StartsWith("falcp -h"))
                    {
                        log.print("FalCompile [falcp] v0.5");
                        log.print("FalCompile is a FalVM compiler to compile .fal files to .fex [Falcon Executable]");
                        log.print("Uses:\n   falcp <file> (to get a <file>.fex file that is compiled)\n   falcp --help/-h (to get this prompt)");
                    }
                    else if (cmd.EndsWith(".fa"))
                    {
                        string[] program;
                        if (File.Exists(cmd.Replace("falcp ", "")))
                        {
                            program = File.ReadAllLines(cmd.Replace("falcp ", data.currentDir));
                        }
                        else
                        {
                            log.programPrint("falcp", "ERROR: File doesn't exist.");
                        }
                    }
                }
                else
                {
                    log.print("FalCompile [falcp] v0.5");
                    log.print("FalCompile is a FalVM compiler to compile .fal files to .fex [Falcon Executable]");
                    log.print("Uses:\n   falcp <file>.fa (to get a <file>.fex file that is compiled)\n   falcp --help/-h (to get this prompt)");
                }
            }
            else if (cmd.StartsWith("pwd"))
            {
                log.print(data.currentDir);
            }
            else if (cmd.StartsWith("sudo "))
            {
                sudo su = new sudo(Kernel.cUser);
                su.execAsRoot(cmd.Replace("sudo ", ""));
            }
            else if (cmd.StartsWith("usrname"))
            {
                log.print(Kernel.cUser);
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
                Console.Write("\n 88888888b          dP                                .88888.  .d88888b  \r\n 88                 88                               d8'   `8b 88.    \"' \r\na88aaaa    .d8888b. 88 .d8888b. .d8888b. 88d888b.    88     88 `Y88888b. \r\n 88        88'  `88 88 88'  `\"\" 88'  `88 88'  `88    88     88       `8b \r\n 88        88.  .88 88 88.  ... 88.  .88 88    88    Y8.   .8P d8'   .8P \r\n dP        `88888P8 dP `88888P' `88888P' dP    dP     `8888P'   Y88888P  \r\n                                                                         \r\n                                                                             \n");
                log.print("FalconOS " + ver + ": Is VM?: " + infochecks.isVM());
            }
            else if (cmd.StartsWith("fetch"))
            {
                if (cmd.StartsWith("fetch "))
                {
                    var fWhat = cmd.Replace("fetch ", "");
                    if (fWhat == "disks")
                    {
                        foreach (var disk in VFSManager.GetDisks())
                        {
                            log.print(disk.ToString());
                        }
                    }
                    else
                    {
                        log.print("fetch v1.0\nFetches different system stuff");
                    }
                }
            }
            else if (cmd.StartsWith("logout"))
            {
                sysmgr.login(true);
            }
            else if (cmd.StartsWith("./"))
            {

                if (cmd.EndsWith(".fe"))
                {
                    executor.executeFE(cmd.Replace("./", ""));
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
                        log.print("Error: File doesn't exist or syntax error.");
                    }
                }
            }
            else if (cmd.StartsWith("as "))
            {
                    if (cmd.Replace("as ", "") == "--help")
                    {
                        log.print("as v1.0\nBasic assembly-ish language.");
                    }
                    else
                    {
                        if (File.Exists(cmd.Replace("as ", data.currentDir)) && cmd.Replace("as ", data.currentDir).EndsWith(".asm"))
                        {
                            var output = cmd.Replace("as ", "").Replace(".asm", ".fe");
                            assemble.handleCode(File.ReadAllText(cmd.Replace("as ", "")));
                        }
                        else
                        {
                            log.print("Invalid Extension");
                        }
                    }
            }else if (cmd.StartsWith("as"))
            {
                log.print("as v1.0\nBasic assembly-ish language.\nUsage: as <file>");
            } else if (cmd.StartsWith("as-fe "))
            {
                if (cmd.Replace("as-fe ", "") == "--help")
                {
                    log.print("as-fe v1.0\nBasic assembly-ish language but compiled.");
                }
                else
                {
                    if (File.Exists(cmd.Replace("as-fe ", data.currentDir)) && cmd.Replace("as-fe ", data.currentDir).EndsWith(".asm"))
                    {
                        var output = cmd.Replace("as-fe ", data.currentDir).Replace(".asm", ".fe");
                        var text = assemble.compile(File.ReadAllText(cmd.Replace("as-fe ", "")), output);
                        File.WriteAllText(output, text);
                    }
                    else
                    {
                        log.print("Invalid Extension");
                    }
                }
            } else if (cmd.StartsWith("as-fe"))
            {
                Console.WriteLine("as-fe v1.0\nBasic assembly-ish compiled language.\nUsage: as-fe <file>");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                log.print("Unrecognized Command!");
            }
            Console.ResetColor();
            data.lastCMD = cmd;
            data.ProcMgr.removeProc(cmd);
            return; 
        }
    }
}
