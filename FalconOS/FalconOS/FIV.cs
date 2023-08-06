using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace FalconOS
{
    public class FAV
    {
        public static string cursor = "_";

        public static void printFAVStartScreen(string fileN)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~                      FAV: "); Console.ForegroundColor = ConsoleColor.White; log.sPrint(" FAlcon Vi");
            Console.ForegroundColor = ConsoleColor.Cyan;
            log.sPrint("~");
            log.sPrint("~                                  version 0.2");
            log.sPrint("~                               by "); Console.ForegroundColor = ConsoleColor.Yellow; log.sPrint(" Code Devel0per");
            Console.ForegroundColor = ConsoleColor.Cyan;
            log.sPrint("~                               Modified from MIV");
            log.sPrint("~                      FAV is a very obvious clone of MIV.");
            log.sPrint("~");
            log.sPrint("~                     type :help<Enter>          for information");
            log.sPrint("~                     type :q<Enter>             to exit");
            log.sPrint("~                     type :wq<Enter>            save to file and exit");
            log.sPrint("~                     press i                    to write");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            log.sPrint("~");
            Console.ForegroundColor = ConsoleColor.White;
            log.drawBar("FalconOS: FAV: Editing " + fileN, ConsoleColor.DarkCyan, ConsoleColor.Black, false);
        }

        public static String stringCopy(String value)
        {
            String newString = String.Empty;

            for (int i = 0; i < value.Length - 1; i++)
            {
                newString += value[i];
            }

            return newString;
        }

        public static void printFAVScreen(char[] chars, int pos, String infoBar, Boolean editMode, string fileNa)
        {
            int countNewLine = 0;
            int countChars = 0;
            Console.Clear();

            for (int i = 0; i < pos; i++)
            {
                if (chars[i] == '\n')
                {
                    log.sPrint("");
                    countNewLine++;
                    countChars = 0;
                }
                else
                {
                    Console.Write(chars[i]);
                    countChars++;
                    if (countChars % 80 == 79)
                    {
                        countNewLine++;
                    }
                }
            }

            Console.Write(cursor);

            for (int i = 0; i < 22 - countNewLine; i++)
            {
                log.sPrint("");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("~");
                Console.ForegroundColor = ConsoleColor.White;
            }

            log.sPrint();
            log.drawBar("FalconOS: FAV: Editing " + fileNa, ConsoleColor.DarkCyan, ConsoleColor.Black, false);

            //PRINT INSTRUCTION
            log.sPrint();
            for (int i = 0; i < 73; i++)
            {
                if (i < infoBar.Length)
                {
                    Console.Write(infoBar[i]);
                }
                else
                {
                    Console.Write(" ");
                }
            }

            if (editMode)
            {
                Console.Write(countNewLine + 1 + "," + countChars);
            }

        }

        public static String fav(String start, String filename, string baseVal)
        {
            Boolean editMode = false;
            int pos = 0;
            char[] chars = new char[2000];
            String infoBar = baseVal;

            if (start == null)
            {
                printFAVStartScreen(filename);
            }
            else
            {
                pos = start.Length;

                for (int i = 0; i < start.Length; i++)
                {
                    chars[i] = start[i];
                }
                printFAVScreen(chars, pos, infoBar, editMode, filename);
            }

            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);

                if (isForbiddenKey(keyInfo.Key)) continue;

                else if (!editMode && keyInfo.KeyChar == ':')
                {
                    infoBar = ":";
                    printFAVScreen(chars, pos, infoBar, editMode, filename);
                    do
                    {
                        keyInfo = Console.ReadKey(true);
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            if (infoBar == ":wq")
                            {
                                String returnString = String.Empty;
                                for (int i = 0; i < pos; i++)
                                {
                                    returnString += chars[i];
                                }
                                return returnString;
                            }
                            else if (infoBar == ":q")
                            {
                                return null;
                            }
                            else if (infoBar == ":help")
                            {
                                printFAVStartScreen(filename);
                                break;
                            } else if (infoBar == ":cursor /")
                            {
                                cursor = "/";
                                break;
                            } else if (infoBar == ":cursor _")
                            {
                                cursor = "_";
                                break;
                            } else if (infoBar == ":cursor |")
                            {
                                cursor = "|";
                                break;
                            } else if (infoBar == ":w")
                            {
                                String contents = String.Empty;
                                for (int i = 0; i < pos; i++)
                                {
                                    contents += chars[i];
                                }
                                File.WriteAllText(filename, contents);
                                infoBar = "Saved " + filename + ", Length: " + start.Length;
                                Thread.Sleep(1500);
                                break;
                            } else if (infoBar == ":dg")
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                Console.Clear();
                                printFAVScreen(chars, pos, infoBar, editMode, filename);
                                break;
                            } else if (infoBar == ":bl")
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.Clear();  
                                printFAVScreen(chars, pos, infoBar, editMode, filename);
                                break;
                            }
                            else
                            {
                                infoBar = "ERROR: No such command";
                                printFAVScreen(chars, pos, infoBar, editMode, filename);
                                break;
                            }
                        }
                        else if (keyInfo.Key == ConsoleKey.Backspace)
                        {
                            infoBar = stringCopy(infoBar);
                            printFAVScreen(chars, pos, infoBar, editMode, filename);
                        }
                        else if (keyInfo.KeyChar == 'q')
                        {
                            infoBar += "q";
                        }
                        else if (keyInfo.KeyChar == ':')
                        {
                            infoBar += ":";
                        }
                        else if (keyInfo.KeyChar == 'w')
                        {
                            infoBar += "w";
                        }
                        else if (keyInfo.KeyChar == 'h')
                        {
                            infoBar += "h";
                        }
                        else if (keyInfo.KeyChar == 'e')
                        {
                            infoBar += "e";
                        }
                        else if (keyInfo.KeyChar == 'l')
                        {
                            infoBar += "l";
                        }
                        else if (keyInfo.KeyChar == 'p')
                        {
                            infoBar += "p";
                        } else if (infochecks.isLetter(keyInfo.KeyChar))
                        {
                            infoBar += keyInfo.KeyChar;
                        }
                        else
                        {
                            continue;
                        }
                        printFAVScreen(chars, pos, infoBar, editMode, filename);



                    } while (keyInfo.Key != ConsoleKey.Escape);
                }

                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    editMode = false;
                    infoBar = String.Empty;
                    printFAVScreen(chars, pos, infoBar, editMode, filename);
                    continue;
                }

                else if (keyInfo.Key == ConsoleKey.I && !editMode)
                {
                    editMode = true;
                    infoBar = "-- INSERT --";
                    printFAVScreen(chars, pos, infoBar, editMode, filename);
                    continue;
                }

                else if (keyInfo.Key == ConsoleKey.Enter && editMode && pos >= 0)
                {
                    chars[pos++] = '\n';
                    printFAVScreen(chars, pos, infoBar, editMode, filename);
                    continue;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && editMode && pos >= 0)
                {
                    if (pos > 0) pos--;

                    chars[pos] = '\0';

                    printFAVScreen(chars, pos, infoBar, editMode, filename);
                    continue;
                }

                if (editMode && pos >= 0)
                {
                    chars[pos++] = keyInfo.KeyChar;
                    printFAVScreen(chars, pos, infoBar, editMode, filename);
                }

            } while (true);
        }

        public static bool isForbiddenKey(ConsoleKey key)
        {
            ConsoleKey[] forbiddenKeys = { ConsoleKey.Print, ConsoleKey.PrintScreen, ConsoleKey.Pause, ConsoleKey.Home, ConsoleKey.PageUp, ConsoleKey.PageDown, ConsoleKey.End, ConsoleKey.NumPad0, ConsoleKey.NumPad1, ConsoleKey.NumPad2, ConsoleKey.NumPad3, ConsoleKey.NumPad4, ConsoleKey.NumPad5, ConsoleKey.NumPad6, ConsoleKey.NumPad7, ConsoleKey.NumPad8, ConsoleKey.NumPad9, ConsoleKey.Insert, ConsoleKey.F1, ConsoleKey.F2, ConsoleKey.F3, ConsoleKey.F4, ConsoleKey.F5, ConsoleKey.F6, ConsoleKey.F7, ConsoleKey.F8, ConsoleKey.F9, ConsoleKey.F10, ConsoleKey.F11, ConsoleKey.F12, ConsoleKey.Add, ConsoleKey.Divide, ConsoleKey.Multiply, ConsoleKey.Subtract, ConsoleKey.LeftWindows, ConsoleKey.RightWindows };
            for (int i = 0; i < forbiddenKeys.Length; i++)
            {
                if (key == forbiddenKeys[i]) return true;
            }
            return false;
        }

        public static void StartFAV(string file)
        {
            String text = String.Empty;
            if (file == null)
            {
                File.WriteAllText(data.currentDir + "favText.txt", "");
                fav(null, "favText.txt", "File not valid, will save as favText.txt");
                return;
            }
            text = fav(File.ReadAllText(file), file, "Opened " + file);
            Console.ResetColor();
            Console.Clear();

            if (text != null)
            {
                File.WriteAllText(file, text);
            }

            log.drawTitleBar("FalconOS: Shell");
            Console.SetCursorPosition(0, 3);
        }
    }
}