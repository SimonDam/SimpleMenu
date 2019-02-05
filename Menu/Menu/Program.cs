using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu mainMenu = new Menu("Main Menu");
            mainMenu.Add(new Test("testmenu"));
            mainMenu.Start();
        }
    }
}
