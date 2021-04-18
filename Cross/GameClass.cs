using System;


namespace Cross
{
    class GameClass
    {
        public FieldClass Field { get; set; }
        public int WinSeries { get; set; }
        public char PlayerDot { get; set; }
        public char AIDot { get; set; }



        public GameClass(FieldClass field, int winSeries, char playerDot, char aiDot)
        {
            Field = field;
            WinSeries = winSeries;
            PlayerDot = playerDot;
            AIDot = aiDot;
            Game();
        }


        private bool IsEmpty(int x, int y)
        {
            if (Field.Field[y, x] == ' ')
                return true;
            return false;
        }


        private  (int X, int Y ) GetIndexesFromCoordinates(int currentX, int currentY)
        {
            var result = (x: (currentX - 4)/ 8, y: (currentY-2)/3);
            return result;
        }





        private void PersonMove(char dot)
        {
            int currentX=4;
            int currentY=2;
            Field.DrawField();
            ConsoleKey key;
            Console.SetCursorPosition(currentX,currentY);
            do
            {
                var result = GetIndexesFromCoordinates(currentX, currentY);
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.LeftArrow && currentX > 4)
                    currentX -= 8;
                else if (key == ConsoleKey.RightArrow && currentX < (Field.SizeX * 8) - 4)
                    currentX += 8;
                else if (key == ConsoleKey.UpArrow && currentY > 2)
                    currentY -= 3;
                else if (key == ConsoleKey.DownArrow && currentY < (Field.SizeY * 3) - 2)
                    currentY += 3;
                else if (key == ConsoleKey.Enter && IsEmpty(result.Y, result.X))
                {
                    Field.Field[result.X, result.Y] = dot;
                }
                Console.Clear();
                Field.DrawField();
                Console.SetCursorPosition(currentX,currentY);
            } while (key!=ConsoleKey.Enter);


        }


        private bool IsHorisontalWin(char dot)
        {
            for (int i = 0; i < Field.SizeX; i++)
            {
                int count = 0;
                for (int j = 0; j < Field.SizeY; j++)
                {
                    if (Field.Field[j, i] == dot)
                        count++;
                    else count = 0;
                    if (count == WinSeries)
                        return true;
                }
                
            }
            return false;
        }
        private bool IsVerticalWin(char dot)
        {
            for (int i = 0; i < Field.SizeX; i++)
            {
                int count = 0;
                for (int j = 0; j < Field.SizeY; j++)
                {
                    if (Field.Field[i, j] == dot)
                        count++;
                    else count = 0;
                    if (count == WinSeries)
                        return true;
                }
                
            }
            return false;
        }
        private bool IsMaxDiagonalWin(char dot)
        {
            int count = 0;
            for (int i = 0; i < Field.SizeX; i++)
            {
                
                if (Field.Field[i, i] == dot)
                    count++;
                else
                    count = 0;
                if (count == WinSeries)
                    return true;
            }

            return false;
        }
        private bool IsUpDiagonalWin(char dot)
        {
            for (int i = 1; i < Field.SizeX; i++)
            {
                int count = 0;
                for (int j = i; j < Field.SizeX; j++)
                {
                    if (Field.Field[j, j-i] == dot)
                        count++;
                    else
                    {
                        count = 0;
                    }
                    if (count == WinSeries)
                        return true;
                }
            }
            return false;
        }
        private bool IsDownDiagonalWin(char dot)
        {
            for (int i = 1; i < Field.SizeX; i++)
            {
                int count = 0;
                for (int j = 0; j < Field.SizeX-i; j++)
                {
                    if (Field.Field[j, j + i] == dot)
                        count++;
                    else
                    {
                        count = 0;
                    }
                    if (count == WinSeries)
                        return true;
                }
            }
            return false;
        }
        private bool IsWin(char dot)
        {
            return IsHorisontalWin(dot)||IsVerticalWin(dot)|| IsMaxDiagonalWin(dot)||IsUpDiagonalWin(dot)||IsDownDiagonalWin(dot);
        }




        public void Game()
        {
            string currentPlayer;
            bool winFlag;
            do
            {
                if (PlayerDot == 'X')
                {
                    currentPlayer = "Человек";
                    PersonMove(PlayerDot);
                    if (IsWin(PlayerDot))
                    {
                        winFlag = true;
                        break;
                    }
                }
                else
                    currentPlayer = "Компьютер";








            } while (true);
            Console.Clear();

            if (winFlag)
                Console.WriteLine($"Выиграл {currentPlayer}");
            else
                Console.WriteLine("Ничья");
            Console.ReadLine();
            
        }







    }
}
