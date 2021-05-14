using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DataAccessLayer
{
    class Menu
    {
        private int SelectedIndex;
        private readonly string[] Options;
        private readonly string Prompt = @"
███╗   ███╗ ██████╗ ██╗   ██╗██╗███████╗███╗   ██╗███████╗██╗  ██╗
████╗ ████║██╔═══██╗██║   ██║██║██╔════╝████╗  ██║██╔════╝╚██╗██╔╝
██╔████╔██║██║   ██║██║   ██║██║█████╗  ██╔██╗ ██║█████╗   ╚███╔╝   
██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██╔══╝  ██║╚██╗██║██╔══╝   ██╔██╗   
██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║███████╗██║ ╚████║███████╗██╔╝ ██╗
╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝";


        public Menu(string[] Option)
        {
            Options = Option;
            SelectedIndex = 0;
        }
        private void DisplayOptions()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Welcome to MovieNex. What Would you like to perform?\n");
            ResetColor();
            for (int i = 0; i < Options.Length; i++)
            {
                string CurrentOptions = Options[i];
                string Prefix, Suffix;
                if (i == SelectedIndex)
                {
                    Prefix = " ";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                    Suffix = " ";
                }
                else
                {
                    Prefix = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                    Suffix = " ";
                }
                WriteLine($"{Prefix} << {CurrentOptions} >> {Suffix}");
            }
            ResetColor();
        }
        public int Run()
        {
            ConsoleKey KeyPressed;
            do
            {
                Clear();
                DisplayOptions();
                ConsoleKeyInfo KeyInfo = ReadKey(true);
                KeyPressed = KeyInfo.Key;

                if (KeyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex == -1)
                    {
                        SelectedIndex = Options.Length - 1;
                    }
                }
                else if (KeyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }
            } while (KeyPressed != ConsoleKey.Enter);
            return SelectedIndex;
        }
    }
}
