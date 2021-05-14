using DataAccessLayer;
using DataBase.Model;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MovieNex
{
    class Program
    {
        #region variable for full screen console by default
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;
        #endregion

        static void Main(string[] args)
        {
            Display();
        }
        #region Display Home
        public static void Display()
        {
            while (true)
            {
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                ShowWindow(ThisConsole, MAXIMIZE);
                PersonOps Person = new PersonOps();
                Person.Start();
            }
        }
        #endregion
    }
}
