using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using CoderdojoOS2;
using Cosmos.System.FileSystem;

namespace CoderdojoOS
{
    public class Kernel : Sys.Kernel
    {
        public static string ver = "pre alpha 2.0";
        public static uint build = 15;

        public bool crash = false;

        public FormatException ex;
        public static string cmd;
        public static CosmosVFS fs = new CosmosVFS();

        public static string ls = @"0:\";

        protected override void BeforeRun()
        {
            Console.WriteLine("Coderdojo OS " + ver + " build " + build);
             fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.WriteLine("Coderdojo OS został załadowny poprawnie\n");
            var userPasswd = @"0:\userPasswd.pass";

            if (!File.Exists(userPasswd))
            {
                File.Create(userPasswd);
                File.WriteAllText(@"0:\userPasswd.pass", "toor");
            }
            User.password = File.ReadAllText(@"0:\userPasswd.pass");
        }

        protected override void Run()
        {
            //logowanie sie
            Console.Write("Login: ");
            var login = Console.ReadLine();
            Console.Write("\nHasło: ");
            var pass = Console.ReadLine();
            if (login == User.login && pass == File.ReadAllText(@"0:\userPasswd.pass"))
            {
                Console.WriteLine("\nHasło zaakceptowane\n");

                while (true)
                {
                    if (crash == true)
                    {
                        break;
                    }
                    try
                    {
                        Console.Write(login + "" + ls + " $");
                        cmd = Console.ReadLine();
                        if (cmd == "logout")
                        {
                            break;
                        }
                        else
                        {
                            Handler(cmd);
                        }
                    }
                    catch (FormatException e)
                    {
                        crash = true;
                        ex = e;
                    }

                }
                if (crash == false)
                {
                    Console.WriteLine("\nWylogowano ciebie z systemu.");
                }
                else if (crash == true)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Console.WriteLine("Awaria Systemu");
                    Console.WriteLine("==============================");
                    Console.WriteLine("Nastąpiła awaria systemu i system musi zostać zrestartowany.");
                    Console.WriteLine("\nKod błędu: " + ex);
                    Console.WriteLine("\nKliknij dowolny przycisk aby uruchomić komputer.");
                    Console.ReadKey();
                    Sys.Power.Reboot();
                }
            }
            else
            {
                Console.WriteLine("\nHasło niepoprawne\n");
            }
        }

        //###############//
        //# Shell Begin #//
        //###############//

        public string[] command = new string[Int32.MaxValue];
        public void Handler(string rawCommand)
        {


            command = rawCommand.Split(" ");

            string commandName = command[0].ToLower();

            if (commandName == "wait")
            {
                Wait(command[1]);
            }
            else if (commandName == "help")
            {
                Help();
            }
            else if (commandName == "echo")
            {
                Echo(command);
            }
            else if (commandName == "reboot")
            {
                Sys.Power.Reboot();
            }
            else if (commandName == "passwd")
            {
                try
                {
                    string passwd;
                    Console.Write("actual password: ");
                    passwd = Console.ReadLine();
                    if (passwd == File.ReadAllText(@"0:\userPasswd.pass"))
                    {
                        NewPassword();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (commandName == "shutdown")
            {
                Sys.Power.Shutdown();
            }
            else if (commandName == "clear")
            {
                Console.Clear();
            }
            else if (commandName == "crash")
            {
                Crash(command[1]);
            }
            else if (commandName == "bgcolor")
            {
                try
                {
                    Bgcolor(command[1]);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                }
            }
            else if (commandName == "textcolor")
            {

                Textcolor(command[1]);
            }
            else if (commandName == "pause")
            {
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            else if (commandName == "mkdir" || commandName == "makedir" || commandName == "createf")
            {
                MkDir(command[1]);
            }
            else if (commandName == "pauze")
            {
                Console.WriteLine("Pres any ki tu kontiniju");
                Console.ReadKey();
            }
            else if (commandName == "version")
            {
                Console.WriteLine("Coderdojo OS " + Kernel.ver + " build " + Kernel.build);
            }
            else if (commandName == "ls")
            {
                try
                {
                    string Adr = ls;
                    Console.WriteLine(Directory.GetFiles(Adr));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (cmd == "user info")
            {
                try
                {
                    UserInfo();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                Console.WriteLine(commandName + ": Nie znaleziono polecenia");
            }
        }
        //#############################//
        //          metody             //
        //#############################//
        public void MkDir(string Folder)
        {
            Directory.CreateDirectory(Folder);
        }
        public void Crash(string code)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("Awaria Systemu");
            Console.WriteLine("==============================");
            Console.WriteLine("Nastąpiła awaria systemu i system musi zostać zrestartowany.");
            Console.WriteLine("\nKod błędu: " + code);
            Console.WriteLine("\nKliknij dowolny przycisk aby uruchomić komputer.");
            Console.ReadKey();
            Sys.Power.Reboot();
        }

        public void Wait(string wait1)
        {
            try
            {
                int wait = Int32.Parse(wait1);
                Thread.Sleep(wait);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Błąd: " + ex + "\n");
            }
        }
        //zmienia kolor tła
        public void Bgcolor(string color1)
        {


            string color = color1.ToLower();
            switch (color)
            {
                case "blue":
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Clear();

                        break;
                    }
                case "red":
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Clear();
                        break;
                    }
                case "green":
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    }
                case "white":
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        break;
                    }
                case "black":
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                    }
                case "help":
                    {

                        break;
                    }
                default:
                    {
                        Console.WriteLine("Nie znaleziono koloru.");
                        Console.WriteLine("Oto lista kolorów:");
                        Console.WriteLine("blue");
                        Console.WriteLine("red");
                        Console.WriteLine("green");
                        Console.WriteLine("white");
                        Console.WriteLine("black");
                        break;
                    }


            }
        }

        public void Textcolor(string color1)
        {
            string color = color1.ToLower();



            switch (color)
            {
                case "red":
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    }
                case "white":
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                case "blue":
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    }
                case "green":
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Nie znaleziono koloru");
                        Console.WriteLine("Oto lista kolorów:");
                        Console.WriteLine("blue");
                        Console.WriteLine("red");
                        Console.WriteLine("green");
                        Console.WriteLine("white");

                        break;
                    }
            }





        }
        public void Echo(string[] echoCommand)
        {
            Console.WriteLine("Echo: Co chcesz żebym powtórzył? ");
            var czyt = Console.ReadLine();
            Console.WriteLine(czyt);

        }

        public void Help()
        {
            Console.WriteLine("\nPomoc Systemu Coderdojo OS");
            Console.WriteLine("==========================");
            Console.WriteLine("reboot - uruchamia ponownie komputer");
            Console.WriteLine("echo [tekst] - Wyświetla na ekranie podany tekst.");
            Console.WriteLine("Help - wyświetla wszystkie komendy shella");
            Console.WriteLine("wait [czas] - czeka określoną ilosć sekund");
            Console.WriteLine("bgcolor [kolor] - zmienia kolor tła konsoli");
            Console.WriteLine("textcolor [kolor] - zmienia kolor tekstu konsoli");
            Console.WriteLine("crash [kod błędu] - wyświetla informacje o błędzie systemu i wymaga ponownego uruchomienia");
            Console.WriteLine("shutdown - wyłącza komputer");
        }
        public void CreateFile(string Name, string Partition)
        {

        }
        public void UserInfo()
        {
            Console.WriteLine("login - " + User.name);
            Console.WriteLine("last name - " + User.lastName);
            Console.WriteLine("login - " + User.login);
        }
        public void NewPassword()
        {
            while (true)
            {
                Console.Write("Nowe haslo: ");
                var checkPasswd = Console.ReadLine();
                Console.Write("\npowtorz nowe haslo: ");
                var passwd = Console.ReadLine();
                Console.WriteLine();
                if (checkPasswd == passwd)
                {
                    File.Delete(@"0:\userPasswd.pass");
                    File.Create(@"0:\userPasswd.pass");
                    File.WriteAllText(@"0:\userPasswd.pass", passwd);
                    break;
                }
                else
                {
                    Console.WriteLine("hasla sie nie zgadzaja");
                }
            }


        }
        public void CreateFile(string file)
        {
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            else
            {
                Console.WriteLine("Plik już istnieje. Żadna edycja nie została wprowadona.");
            }
        }
        //################//
        //# shell begone #//
        //################//
    }
}