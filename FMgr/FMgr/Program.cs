using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace FMgr
{
    class StateAPP 
        {
        public string CurrentDirectory { get; set; }        
        public byte NumberLines { get; set; }

        public StateAPP()
            {
            CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            NumberLines = 25;            
            }
        }


    class Program
        {

        public static List<string> ListFD;
        public static StateAPP AppSet;
        public static int CurrentPage;
        public static bool isWorks;

        static public void Get_Info_dirictories(string str)
        {
            DirectoryInfo di = new DirectoryInfo(str);

            DirectoryInfo[] dirAll = di.GetDirectories();
            FileInfo[] fileAll = di.GetFiles();
            ListFD.Clear();

            if(di.Parent != null) ListFD.Add("..");

            for (int i = 0; i < dirAll.Length; i++)
            {
                ListFD.Add("d;"+ "1;[" + dirAll[i].Name + "]");

                DirectoryInfo did = new DirectoryInfo(dirAll[i].ToString());
                try
                {
                    DirectoryInfo[] dirAlld = did.GetDirectories();
                    

                    for (int y = 0; y < dirAlld.Length; y++)
                    {
                        ListFD.Add("d;" + "2;[" + dirAlld[y].Parent.Name +"]\\["+dirAlld[y].Name + "]");

                    }
                }
                catch (Exception e) { }

                try
                {
                    FileInfo[] fileAlld = did.GetFiles();
                    for (int x = 0; x < fileAlld.Length; x++)
                    {
                        ListFD.Add("f;" + "2;" + "[" + did.Name +"]\\" + fileAlld[x].Name);
                    }
                }
                catch (Exception e) { }

            }

            for (int i = 0; i < fileAll.Length; i++)
            {
                ListFD.Add("f;" + "1;" +fileAll[i].Name);
            }
        }

        static void PrintStrTree(string str)
        {
            string[] arrStr = str.Split(";");

            if (arrStr[0] == "..") 
                {
                Console.WriteLine(arrStr[0]);
                return;
                };

            if (arrStr[0] == "d")
                {
                if (arrStr[1] == "1") 
                    {
                    Console.ForegroundColor = ConsoleColor.Green;
                    }                
                else
                    {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                }

            string pos = arrStr[1] == "1"? "":"    ";

            Console.WriteLine(pos + arrStr[2]);
            Console.ResetColor();
        }

        static public void Show(int linePage, int page)
        {
            int coutnPage = ListFD.Count / linePage;

            if ((coutnPage < page)|(page < 0)) 
                { 
                return; 
                };

            CurrentPage = page;

            Console.Clear();
            Console.WriteLine($"Page {page}/{coutnPage}");

            for (int i = 0; i < linePage; i++)
                {
                if((page * linePage) +i < ListFD.Count)
                    {
                        try
                            {
                            PrintStrTree(ListFD[(page * linePage) + i]);
                            }
                        catch(Exception e)
                            {
                            Console.WriteLine("");
                            };
                    }
                    else
                    {
                        Console.WriteLine("");
                    };
                }
         }

        static void DEL(string str)
        {
            bool iserr = true;

            while (iserr)
            {

                iserr = false;

                if (str.Length == 0)
                {
                    iserr = false;
                    continue;
                };

                if (str[0] == ' ')
                {
                    str = str.Remove(0, 1);
                    iserr = true;
                    continue;
                }

                if (str[str.Length - 1] == ' ')
                {
                    str = str.Remove(str.Length - 1, 1);
                    iserr = true;
                    continue;
                }
            }

            if (str.Length == 0) 
                {
                Console.WriteLine("Не указаны параметры команды.");
                Console.ReadKey();
                return;
                };

            bool itFile = false;
            bool DoDel = false;
            string S = "";

            DirectoryInfo di;
            try
            {
               di = new DirectoryInfo(str);
            }
            catch {
                Console.WriteLine("Ошибка вызова команды.");
                Console.ReadKey();
                return;
            };

            if (di.Exists) 
                {
                itFile = false;
                DoDel = true;
                S = str;
                };

            if (!DoDel)
            {
                S = GetFullNameDirectory(AppSet.CurrentDirectory);
                if (S[S.Length - 1] != '\\')
                    {
                    S = S + @"\";
                    };
                S = S + str;

                try
                {
                    di = new DirectoryInfo(S);
                }
                catch
                {
                    Console.WriteLine("Ошибка вызова команды. Любую клавишу для продолжения.");
                    Console.ReadKey();
                    return;
                };

                if (di.Exists)
                {
                    itFile = false;
                    DoDel = true;
                    S = S;
                };

            };

            FileInfo fi;
            if (!DoDel)
                {
                fi = new FileInfo(str);

                if (fi.Exists) 
                    {
                    itFile = true;
                    DoDel = true;
                    S = str;
                }

                }

            fi = new FileInfo(S);

            if (!DoDel)
            {

                if (fi.Exists)
                {
                    itFile = true;
                    DoDel = true;
                    S = str;
                }

            }

            if (DoDel)
            {
                if (itFile)
                {
                    try { 
                    File.Delete(fi.FullName);
                }
                    catch(Exception e)
                {
                    Console.WriteLine($"Ошибка выполнения File.Delete({S})");
                    Console.ReadKey();
                     return;
                };
            }
                else
                {
                    try { 
                    Directory.Delete(di.FullName, true);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Ошибка выполнения Directory.Delete({di.FullName});");
                        Console.ReadKey();
                        return;
                    };
                };
                Console.WriteLine($"Удалил {str}");
                Console.WriteLine("Любую клвишу для продолжения.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Каталог или файл {str} не существует.");
                Console.WriteLine("Любую клвишу для продолжения.");
                Console.ReadKey();

            };
        }

        static void GETINFO(string str)
        {
            bool iserr = true;

            while (iserr)
            {

                iserr = false;

                if (str.Length == 0)
                {
                    iserr = false;
                    continue;
                };

                if (str[0] == ' ')
                {
                    str = str.Remove(0, 1);
                    iserr = true;
                    continue;
                }

                if (str[str.Length - 1] == ' ')
                {
                    str = str.Remove(str.Length - 1, 1);
                    iserr = true;
                    continue;
                }
            }

            if (str.Length == 0)
            {
                Console.WriteLine("Не указаны параметры команды.");
                Console.ReadKey();
                return;
            };

            bool itFile = false;
            bool DoDel = false;
            string S = "";

            DirectoryInfo di;
            try
            {
                di = new DirectoryInfo(str);
            }
            catch
            {
                Console.WriteLine("Ошибка вызова команды.");
                Console.ReadKey();
                return;
            };

            if (di.Exists)
            {
                itFile = false;
                DoDel = true;
                S = str;
            };

            if (!DoDel)
            {
                S = GetFullNameDirectory(AppSet.CurrentDirectory);
                if (S[S.Length - 1] != '\\')
                {
                    S = S + @"\";
                };
                S = S + str;

                try
                {
                    di = new DirectoryInfo(S);
                }
                catch
                {
                    Console.WriteLine("Ошибка вызова команды. Любую клавишу для продолжения.");
                    Console.ReadKey();
                    return;
                };

                if (di.Exists)
                {
                    itFile = false;
                    DoDel = true;
                    S = S;
                };

            };

            FileInfo fi;
            if (!DoDel)
            {
                fi = new FileInfo(str);

                if (fi.Exists)
                {
                    itFile = true;
                    DoDel = true;
                    S = str;
                }

            }

            fi = new FileInfo(S);

            if (!DoDel)
            {

                if (fi.Exists)
                {
                    itFile = true;
                    DoDel = true;
                    S = str;
                }

            }

            if (DoDel)
            {
                if (itFile)
                {
                    try
                    {
                        Console.WriteLine("Инфорсвция о файле:");
                        Console.WriteLine($"Полный путь {fi.FullName}");
                        Console.WriteLine($"Размер {fi.Length}");
                        Console.WriteLine($"последние изменение {fi.LastWriteTime}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Ошибка выполнения File.Delete({S})");
                        Console.ReadKey();
                        return;
                    };
                }
                else
                {
                    try
                    {
                        Console.WriteLine("Инфорсвция о каталоге:");
                        Console.WriteLine($"Полный путь {di.FullName}");
                       Console.WriteLine($"содержит файлов {di.GetFiles().Length}");
                        Console.WriteLine($"последние изменение {di.LastWriteTime}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Ошибка выполнения Directory.Delete({di.FullName});");
                        Console.ReadKey();
                        return;
                    };
                };
                Console.WriteLine("Любую клвишу для продолжения.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Каталог или файл {str} не существует.");
                Console.WriteLine("Любую клвишу для продолжения.");
                Console.ReadKey();

            };
        }

        static void COPY(string str)
        {
            string[] arg = str.Split("|");

            if (arg.Length != 2) 
                {
                Console.WriteLine($"Не верное количество параметров. Продолжить любую клавишу.");
                Console.ReadKey();
                return;
            };

            string strFrom = arg[0];
            string S;
            string strTo = arg[1];

            bool iserr = true;

            while (iserr)
                {

                iserr = false;

                if (strTo.Length == 0)
                {
                    iserr = false;
                    continue;
                }

                if (strFrom.Length == 0)
                {
                    iserr = false;
                    continue;
                }

                if (strFrom[0] == ' ')
                {
                    strFrom = strFrom.Remove(0,1);
                    iserr = true;
                    continue;
                }

                if(strFrom[strFrom.Length-1] == ' ')
                {
                    strFrom = strFrom.Remove(strFrom.Length - 1,1);
                    iserr = true;
                    continue;
                }

                if(strTo[0] == ' ')
                {
                    strTo = strTo.Remove(0,1);
                    iserr = true;
                    continue;
                }

                if(strTo[strTo.Length - 1] == ' ')
                {
                    strTo = strTo.Remove(strTo.Length - 1,1);
                    iserr = true;
                    continue;
                }

            }

            bool itFile = false;
            bool DoCopy = false;

            DirectoryInfo di;
            try
            {
                di = new DirectoryInfo(strFrom);
            }
            catch
            {
                Console.WriteLine($"Ошибка параметра {strFrom} команды. Для продолжения любую клавишу.");
                Console.ReadKey();
                return;
            };

            if (di.Exists)
            {
                itFile = false;
                DoCopy = true;
            };

            if (!DoCopy)
            {
                 S = GetFullNameDirectory(AppSet.CurrentDirectory);
                if (S[S.Length - 1] != '\\')
                {
                    S = S + @"\";
                };
                S = S + strFrom;

                try
                {
                    di = new DirectoryInfo(S);
                }
                catch
                {
                    Console.WriteLine("Ошибка вызова команды. Любую клавишу для продолжения.");
                    Console.ReadKey();
                    return;
                };

                if (di.Exists)
                {
                    itFile = false;
                    DoCopy = true;
                    strFrom = S;
                };

            };

            FileInfo fi = new FileInfo(strFrom);
            if (!DoCopy)
            {
                if (fi.Exists)
                {
                    itFile = true;
                    DoCopy = true;               
                }

            }

            if (!DoCopy)
            {
                S = GetFullNameDirectory(AppSet.CurrentDirectory);
                if (S[S.Length - 1] != '\\')
                {
                    S = S + @"\";
                };
                S = S + strFrom;

                fi = new FileInfo(S);

                if (fi.Exists)
                {
                    itFile = true;
                    DoCopy = true;
                    strFrom = S;
                }

            }

            di = new DirectoryInfo(strTo);

            if (!di.Exists)
            {
                S = GetFullNameDirectory(AppSet.CurrentDirectory);
                if (S[S.Length - 1] != '\\')
                {
                    S = S + @"\";
                };
                S = S + strTo;

                strTo = S;

                di = new DirectoryInfo(S);
            };

         if (DoCopy)
            {
                if (itFile)
                {

                    if (!di.Exists)
                    {
                        Directory.CreateDirectory(di.FullName);
                    };

                    if (strTo[strTo.Length - 1] != '\\')
                    {
                        strTo = strTo + @"\";
                    };
                    strTo = strTo + fi.Name;
                    try
                    {
                        File.Copy(strFrom, strTo, true);
                    }
                    catch {
                        Console.WriteLine($"Ошибка выполнения");
                        Console.ReadKey();
                        return;
                    };
                }
                else
                {
                    try { 
                    CopyDir(strFrom, strTo);
                }
                    catch
                {
                    Console.WriteLine($"Ошибка выполнения");
                    Console.ReadKey();
                     return;
                };
            };
                Console.WriteLine($"Скопировл {strFrom} в {strTo}. Любую клвишу для продолжения.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"Каталог или файл {strFrom} or {strTo} не существует. Любую клвишу для продолжения.");
                Console.ReadKey();

            };
        }

        static void CopyDir(string strFrom, string strTo)
        {
            Directory.CreateDirectory(strTo);
            foreach (string s1 in Directory.GetFiles(strFrom))
            {
                string s2 = strTo + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2, true);
            }
            foreach (string s in Directory.GetDirectories(strFrom))
            {
                CopyDir(s, strTo + "\\" + Path.GetFileName(s));
            }
        }

        static public void RunComand(string str)
         {
            string comma = "";
            bool ifRun = false;
            
            for (int i = 0; i < str.Length; i++)
            {
                comma = $"{comma}{str[i].ToString()}";

                switch (comma)
                {
                    case "cd":
                        {
                            CHDIR(str.Remove(0, i+1));
                            ifRun = true;
                            break;
                        }
                    case "exit":
                        {
                            EXIT(str.Remove(0, i + 1));
                            ifRun = true;
                            break;
                        }
                    case "del":
                        {
                            DEL(str.Remove(0, i + 1));
                            ifRun = true;
                            break;
                        }
                    case "copy":
                        {
                            COPY(str.Remove(0, i + 1));
                            ifRun = true;
                            break;
                        }
                    case "getinfo":
                        {
                            GETINFO(str.Remove(0, i + 1));
                            ifRun = true;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                };
            }

            if (!ifRun) 
                { 
                Console.WriteLine($"Команда не существует. {str}. Продолжить любую клавишу.");
                Console.WriteLine();
                Console.ReadKey();
            };
         }//static public void RunComand(string str)

        static void СhangePage(int x) 
        {
            Show(AppSet.NumberLines, CurrentPage + x);
        }

        static string GetFullNameDirectory(string arg)
        {
            try
            { 
            DirectoryInfo di = new DirectoryInfo(@$"{arg}");
            if (di.Exists)
            {
                return di.FullName;
            }
            else
            {
                di = new DirectoryInfo(@$"{AppSet.CurrentDirectory}\{arg}");
                if (di.Exists)
                {
                    return di.FullName;
                };
            };
            }
            catch { return ""; }

            return "";
        }


        static void CHDIR(string arg)
        {
            string S = arg;
            bool iserr = true;

            
            while (iserr)
            {

                iserr = false;

                if (S.Length == 0)
                {
                    iserr = false;
                    continue;
                };

                if (S[0] == ' ')
                {
                    S = S.Remove(0, 1);
                    iserr = true;
                    continue;
                }

                if (S[S.Length - 1] == ' ')
                {
                    S = S.Remove(S.Length - 1, 1);
                    iserr = true;
                    continue;
                }
            }

            if (S.Length == 0) 
                {
                Console.WriteLine($"Каталог {arg} не существует. Продолжить любую клавишу.");
                Console.ReadKey();
                return;
            };

                string cd = "";
            bool doIt = false;

            if (S=="..")
                {
                DirectoryInfo di = new DirectoryInfo(AppSet.CurrentDirectory);

                if (di.Parent!=null) 
                    {
                    cd = di.Parent.FullName;
                    doIt = true;
                    };
                }
            else 
                {
                //Абсолютный путь
                S = GetFullNameDirectory(S);
                
                if (S.Length>0) 
                    {
                    cd = S;
                    doIt = true;
                    };
                };

            if (doIt) 
                {
                AppSet.CurrentDirectory = cd;
                Get_Info_dirictories(AppSet.CurrentDirectory);
                CurrentPage = 0;
                Show(AppSet.NumberLines, CurrentPage);
            }
            else
                {
                Console.WriteLine($"Каталог {arg} не существует. Продолжить любую клавишу.");
                Console.ReadKey();
                };

        }//static void CHDIR(string arg)
        static void EXIT(string arg)
        {
            
            if (arg.Length == 0)
            {
                isWorks = false;
            };

        }//static void CHDIR(string arg)

        static void Main()
        {
           Console.Title = "Привет!";
           CurrentPage = 0;

            isWorks = true;

           ListFD = new List<string>();

           FileInfo fi = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Fmgr\FmgrAppSetup.json");            

           AppSet = new StateAPP();

            List<string> ArrayCommand = new List<string>();
            int indArrayCommand = -1;

            string CommsndLine = "";
            int CursPos = 0; //Позиция курсора командной строки

            if (fi.Exists)
            {
                string Sjson = File.ReadAllText(fi.FullName);
                AppSet = JsonSerializer.Deserialize<StateAPP>(Sjson);                           
            };

            //************************************************************************

            Get_Info_dirictories(AppSet.CurrentDirectory);            

            Show(AppSet.NumberLines, CurrentPage);

            while (isWorks)
                {
                
                Console.SetCursorPosition(1, AppSet.NumberLines + 4);
                Console.WriteLine("Current directory :" + AppSet.CurrentDirectory);                
                Console.WriteLine("                                                                                                                  ");
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(CommsndLine);
                Console.SetCursorPosition(CursPos, Console.CursorTop);

                //var KeyInfo = Console.ReadKey(true);
                var KeyInfo = Console.ReadKey();

                if (KeyInfo.Key == ConsoleKey.Enter) 
                    {
                    if (CommsndLine.Length > 0)
                        {
                        ArrayCommand.Add(CommsndLine);
                        indArrayCommand = ArrayCommand.Count;

                        };

                    RunComand(CommsndLine.ToLower());
                    CommsndLine = "";
                    CursPos = 0;
                    continue;
                    };

                if ((KeyInfo.Key == ConsoleKey.PageUp)|(KeyInfo.Key == ConsoleKey.PageDown))
                    {
                    СhangePage(KeyInfo.Key == ConsoleKey.PageUp ? -1 : 1) ;
                    continue;
                    };

                if ((KeyInfo.Key == ConsoleKey.LeftArrow) | (KeyInfo.Key == ConsoleKey.RightArrow))
                {
                    //MoveCurs(CursPos, CommsndLine ,KeyInfo.Key == ConsoleKey.RightArrow ? 1 : -1);

                    CursPos += (KeyInfo.Key == ConsoleKey.RightArrow ? 1 : -1);

                    if (0 > CursPos) CursPos = 0;
                    if (CommsndLine.Length < CursPos) CursPos = CommsndLine.Length;

                    continue;
                };

                if (KeyInfo.Key == ConsoleKey.Backspace) 
                    {

                    CursPos -= 1;

                    if (CursPos >= 0)
                        {
                        CommsndLine = CommsndLine.Remove(CursPos, 1);
                        };
                    
                    if (0 > CursPos) CursPos = 0;
                    continue;
                    };

                if (KeyInfo.Key == ConsoleKey.UpArrow)
                    {
                    indArrayCommand-=1;

                    if ((indArrayCommand >= 0) & (indArrayCommand <= ArrayCommand.Count - 1))
                        {
                        CommsndLine = ArrayCommand[indArrayCommand];                        
                        }
                    else
                        {
                        indArrayCommand = ArrayCommand.Count;
                        CommsndLine = "";
                        };
                    CursPos = CommsndLine.Length;
                    continue;
                    };

                if (KeyInfo.Key == ConsoleKey.DownArrow)
                    {

                    indArrayCommand += 1;

                    if (ArrayCommand.Count-1 >= indArrayCommand)
                        {
                        CommsndLine = ArrayCommand[indArrayCommand];                        
                        }
                    else
                        {
                        indArrayCommand = ArrayCommand.Count - 1;
                        CommsndLine = "";                        
                    };
                    CursPos = CommsndLine.Length;
                    continue;
                    };

                if (KeyInfo.Key == ConsoleKey.End)
                    {
                    CursPos = CommsndLine.Length;                    
                    }

                if (KeyInfo.Key == ConsoleKey.Home)
                {
                    CursPos = 0;
                    continue;
                }


                if ((KeyInfo.Key == ConsoleKey.Escape))
                {
                    Console.Clear();
                    continue;
                };


                if (KeyInfo.KeyChar.ToString() == "\0") continue;

                 if (CommsndLine.Length < CursPos)
                    {
                    CommsndLine += KeyInfo.KeyChar.ToString();
                    }
                 else
                   {
                    CommsndLine = CommsndLine.Insert(CursPos, KeyInfo.KeyChar.ToString());
                   }
                 CursPos++; 
            }
            //************************************************************************
            //Завершаем работу
            string json = JsonSerializer.Serialize(AppSet);
            File.WriteAllText(fi.FullName, json);


        }
    }
}
