using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalconOS
{
    internal class executor
    {
        public string arg1 = "0";
        public string arg2 = "0";
        public string arg3 = "0";
        public string arg4 = "0";
        public string arg5 = "0";

        public void executeFE(string file)
        {
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file);
                var lineNum = 0;
                foreach (var line in lines)
                {
                    if (line.StartsWith("00")) {
                        if (line.StartsWith("00 1 "))
                        {
                            arg1 = line.Replace("00 1 ", "");
                        } else if (line.StartsWith("00 2 "))
                        {
                            arg2 = line.Replace("00 2 ", "");
                        } else if (line.StartsWith("00 3 "))
                        {
                            arg3 = line.Replace("00 3 ", "");
                        } else if (line.StartsWith("00 4 "))
                        {
                            arg4 = line.Replace("00 4 ", "");
                        } else if (line.StartsWith("00 5 "))
                        {
                            arg3 = line.Replace("00 5 ", "");
                        } else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Kernel Panic at line " + lineNum);
                        }
                    } else if (line.StartsWith("01"))
                    {
                        if (line.StartsWith("01 1"))
                        {
                            arg1 = "0";
                        } else if (line.StartsWith("01 2"))
                        {
                            arg2 = "0";
                        } else if (line.StartsWith("01 3"))
                        {
                            arg3 = "0";
                        } else if (line.StartsWith("01 4"))
                        {
                            arg4 = "0";
                        } else if (line.StartsWith("01 5"))
                        {
                            arg5 = "0";
                        } else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Kernel Panic at line " + lineNum);
                            return;
                        }
                    } else if (line.StartsWith("02"))
                    {
                        switch (line.Replace("02 ", ""))
                        {
                            case "000":
                                var newL = "";
                                if (arg2 == "1")
                                {
                                    newL = "\n";
                                }
                                log.sPrint(arg1, newL);
                                break;
                            case "001":
                                arg1 = Console.ReadLine();
                                break;
                            case "002":
                                log.sPrint(fetchnum(arg1));
                                break;
                            case "003":
                                File.WriteAllText(data.currentDir + arg1, arg2);
                                break;
                            case "004":
                                arg2 = File.ReadAllText(arg1);
                                break;
                            case "005":
                                try
                                {
                                    Directory.CreateDirectory(data.currentDir + arg1);
                                }
                                catch (Exception)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Kernel Panic at line " + lineNum);
                                    return;
                                }
                                break;
                            case "006":
                                try
                                {
                                    Directory.Delete(data.currentDir + arg1);
                                }
                                catch (Exception)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Kernel Panic at line " + lineNum);
                                    return;
                                }
                                break;
                            case "007":
                                Shell shell = new Shell("FalconOS ASSEMBLY", data.ver);
                                shell.exec(fetch(arg1), Kernel.asRoot, "yes");
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Kernel Panic at line " + lineNum);
                                return;
                        }
                    } else if (line.StartsWith("03"))
                    {
                        add(line.Split(' ')[1], line.Split(' ')[2]);
                    }
                    lineNum++;
                }
                Console.WriteLine();
            } else
            {
                log.sPrint("File does not exist.");
            }
        }
        public string fetch(string arg)
        {
            switch (arg)
            {
                case "arg1": return arg1;
                case "arg2": return arg2;
                case "arg3": return arg3;
                case "arg4": return arg4;
                case "arg5": return arg5;
                default: return "NullArg";
            }
        }

        public string fetchnum(string arg)
        {
            switch (arg)
            {
                case "1": return arg1;
                case "2": return arg2;
                case "3": return arg3;
                case "4": return arg4;
                case "5": return arg5;
                default: return "NullArg";
            }
        }

        public void add(string num1, string num2)
        {
            var first = fetchnum(num1); var second = fetchnum(num2);
            arg5 = (Convert.ToInt64(first + second)).ToString();
        }
    }
}
