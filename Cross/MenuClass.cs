using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross
{
    class MenuClass
    {
        public void SelectMenu()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            int currentX = width / 2 - 5;
            int currentY = height / 2 - 1;


            int start = currentY;
            int end = currentY + 3;

            byte result = 0;


            Console.SetCursorPosition(currentX, currentY);
            ConsoleKey key;
            do
            {
                key = Console.ReadKey().Key;

                if (key == ConsoleKey.DownArrow)
                {
                    if (currentY == end)
                        currentY = start;
                    else
                    {
                        currentY++;
                    }
                    
                    
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    if (currentY == start)
                        currentY = end;
                    else
                    {
                        currentY--;
                    }
                    
                }
                else
                {
                    result =(byte) (currentY % start);
                    break;
                }
                DrawMenu();
                Console.SetCursorPosition(currentX,currentY);
            } while (key != ConsoleKey.Enter);

            if (result == 0)
            {
                GameClass game=new GameClass(new FieldClass(3,3),3, 'X', 'O');
            }
            else if (result==1)
            {
                GameClass game = new GameClass(new FieldClass(5, 5),5, 'X', 'O');
            }
            else if(result==2)
            {
                GameClass game = SetSettingsMenu();
            }
            else
            {
                return;
            }
            
        }


        public void DrawMenu()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            int currentX = width / 2 - 5;
            int currentY = height / 2 - 3;
            Console.SetCursorPosition(currentX, currentY);
            Console.WriteLine("Игра Крестики-Нолики");
            currentY+=2;
            Console.SetCursorPosition(currentX, currentY);
            Console.WriteLine("3*3");
            currentY++;
            Console.SetCursorPosition(currentX, currentY);
            Console.WriteLine("5*5");
            currentY++;
            Console.SetCursorPosition(currentX, currentY);
            Console.WriteLine("Свои настройки");
            currentY++;
            Console.SetCursorPosition(currentX, currentY);
            Console.WriteLine("Выход");

        }

        private GameClass SetSettingsMenu()
        {
            Console.Clear();
            Console.SetCursorPosition(0,0);
            int size = 0;
            int winSeries = 0;
            char playerDot = ' ';
            char aiDot = ' ';
            do
            {
                Console.Write("Введите размер поля (от 3 до 10): ");
                int.TryParse(Console.ReadLine(), out size);
            } while (size<3 && size>10);
            do
            {
                Console.Write($"Введите победную серию (от 3 до {size}): ");
                int.TryParse(Console.ReadLine(), out winSeries);
            } while (winSeries < 3 && winSeries > size);
            do
            {
                Console.Write($"Введите чем вы будете играть ('X' или 'O'): ");
                playerDot = Console.ReadLine().ToUpper()[0];
            } while (playerDot!='X' && playerDot!='O');
            aiDot = playerDot == 'X' ? 'O' : 'X';
            GameClass game=new GameClass(new FieldClass(size,size),winSeries, playerDot, aiDot);
            return game;







        }





    }
}
