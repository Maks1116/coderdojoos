using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using Cosmos.System.FileSystem;

namespace CoderdojoOS2
{
   public static class User
    {
        public static string name;
        public static string lastName;
        public static string login = "root";
        public static string password = File.ReadAllText(@"0:\userPasswd.pass");   
        //public static string password = "toor";





    }
}
