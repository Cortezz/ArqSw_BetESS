using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    public class Menu
    {
        private List<string> lines;
        int option;


        /// <summary>
        /// This is the only constructor of this class. It receives an array of strings and transforms it into a List.
        /// </summary>
        /// <param name="l"></param>
        public Menu (string[] l)
        {
            lines = new List<string>();
            foreach (string s in l)
                lines.Add(s);
            this.option = 0;
        }

        /*Getters and Setters*/
        public int getOption () { return this.option; }



        /// <summary>
        /// This method will print this instance's menu.
        /// </summary>
        public void ShowMenu()
        {
            Console.WriteLine("##############################");
            int i = 1;
            foreach (string s in lines)
            {
                Console.WriteLine(i + " - " + s);
                i++;
            }
            Console.WriteLine("0 - Leave");
            Console.Write("---------\nOption: ");
            
        }

        /// <summary>
        /// Method which shows the menu (by calling @ShowMenu) and parses the user's option.
        /// It checks if the option is between the possible range.
        /// </summary>
        public void ExecuteMenu()
        {
            string line;
            do
            {
                ShowMenu();
                line = Console.ReadLine();
                Int32.TryParse(line, out this.option);
                Console.WriteLine("##############################");
            }
            while (this.option == -1);
        
            
        }
    }
}
