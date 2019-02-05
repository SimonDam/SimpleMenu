using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleMenu
{
    public class Menu : IMenuable
    {
        #region BACKEND VARIABLES
        private int _index;
        #endregion

        #region PRIVATE VARIABLES
        private int index
        {
            get { return _index; }
            set
            {
                if (value >= 0 && value < menuList.Count)
                {
                    _index = value;
                }
            }
        }

        private int currentX;
        private int currentY;

        private int menuOffsetY;

        private IMenuable currentSelection
        {
            get
            {
                try
                {
                    return menuList.ToArray()[index];
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
                
            }
        }

        private List<IMenuable> menuList;
        #endregion

        #region PUBLIC VARIABLES
        public string Title { get; set; }
        #endregion

        #region CONSTRUCTORS
        public Menu(string title)
        {
            Title = title;
            menuList = new List<IMenuable>();
            
            _index = 0;
        }
        #endregion
        
        #region METHODS

        #region PUBLIC
        /// <summary>
        /// Adds individual items to the menu.
        /// </summary>
        /// <param name="menuAddition"></param>
        public void Add(params IMenuable[] menuAddition)
        {
            this.Add(menuAddition.ToList());
        }

        /// <summary>
        /// Adds an IEnumerable to the menu.
        /// </summary>
        /// <param name="menuAddition"></param>
        public void Add(IEnumerable<IMenuable> menuAddition)
        {
            foreach (IMenuable item in menuAddition)
            {
                menuList.Add(item);
            }
        }

        public void Start()
        {
            //CurrentX doesn't change, but is probably needed if horizontal movement is needed.
            currentX = 0;
            currentY = 0;
            bool running = true;
            
            //Input for update strings.
            Tuple<int, string> oldString = null;
            Tuple<int, string> newString = null;

            string errorString = null;

            //Initial creation of menu.
            constructMenu();

            //Main menu loop
            while (running)
            {
                constructMenuHeader(errorString);
                updateMenu(oldString, newString);

                errorString = null;

                Console.SetWindowPosition(currentX, currentY);

                if(menuList.Count > 1)
                {
                    oldString = new Tuple<int, string>(index + menuOffsetY, $"{index}. " + menuList.ToArray()[index].Title);
                }

                string input = Console.ReadKey(true).Key.ToString();

                switch (input)
                {
                    case "UpArrow":
                        if ((index == currentY + menuOffsetY) && currentY > 0)
                        {
                            currentY--;
                        }

                        index--;

                        if (menuList.Count > 1)
                        {
                            newString = new Tuple<int, string>(index + menuOffsetY, $"{index}. " + menuList.ToArray()[index].Title);
                        }
                        break;
                    case "DownArrow":
                        if (index - currentY >= Console.WindowHeight - 4 && currentY <= menuList.Count - Console.WindowHeight+1)//TODO find a way to get the "-4" dynamically.
                        {
                            currentY++;
                        }

                        index++;

                        if (menuList.Count > 1)
                        {
                            newString = new Tuple<int, string>(index + menuOffsetY, $"{index}. " + menuList.ToArray()[index].Title);
                        }
                        break;
                    case "RightArrow":
                    case "Enter":
                        Console.Clear();
                        if(currentSelection == null)
                        {
                            errorString = $"no submenu was found.";
                        }
                        else
                        {
                            currentSelection.Start();
                        }
                        constructMenu();
                        break;
                    case "LeftArrow":
                    case "Escape":
                        Console.Clear();
                        running = false;
                        break;
                    default:
                        errorString = $"{input} is an invalid input.";
                        break;
                }
            }
        }
        #endregion

        #region PRIVATE
        private void constructMenuHeader(string errorString)
        {
            //Write title and index
            Console.SetCursorPosition(0, 0);
            string titlePart = $"{Title} [{index}]";
            Console.Write($"{titlePart}");

            int seperatorStringLength = titlePart.Length;

            //Write error string
            if (errorString != null)
            {
                errorString = $" - {errorString}";
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{errorString}");
                Console.ResetColor();
                seperatorStringLength += errorString.Length;
            }

            //Remove potential previous error string.
            Console.WriteLine(new string(' ', getRemainderBuffer(seperatorStringLength)));

            //Write seperator string
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('#', seperatorStringLength));

            //Remove potential previous seperator string.
            Console.WriteLine(new string(' ', getRemainderBuffer(seperatorStringLength)));
            Console.ResetColor();

            menuOffsetY = Console.CursorTop;
        }

        private void constructMenu()
        {
            Console.Clear();
            constructMenuHeader(null);
            for (int i = 0; i < menuList.Count; i++)
            {
                if (i == index)
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                }
                Console.WriteLine($"{i}. {menuList.ToArray()[i].Title}");
                Console.ResetColor();
            }
        }

        private void updateMenu(Tuple<int, string> oldString, Tuple<int, string> newString)
        {
            if(oldString != null && newString != null)
            {
                Console.SetCursorPosition(0, oldString.Item1);
                Console.Write(oldString.Item2);

                Console.SetCursorPosition(0, newString.Item1);
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.Write(newString.Item2);
                Console.ResetColor();
            }
        }

        private int getRemainderBuffer(int length)
        {
            if(Console.BufferWidth - length - 1 < 0)
            {
                return 0;
            }
            return Console.BufferWidth - length - 1;
        }
        #endregion

        #endregion
    }
}
