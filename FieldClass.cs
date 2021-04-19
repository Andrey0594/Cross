using System;

namespace CrossApplication
{
    class FieldClass
    {
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public char[,] Field { get; set; }
        public  char EmptyDot { get; set; }






        public FieldClass(int width, int height)
        {
            Field = new char[height, width];
            SizeX = width;
            SizeY = height;
            EmptyDot = ' ';
            ClearField();

        }

        private void ClearField()
        {
            for (int i = 0; i < SizeY; i++)
            {
                for (int j = 0; j < SizeX; j++)
                {
                    Field[i, j] = EmptyDot;
                }
            }
        }

        public void DrawField()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < SizeY; i++)
            {
                Console.Write(" _______");
            }
            Console.WriteLine();
            for (int i = 0; i < SizeX; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    for (int j = 0; j < SizeY; j++)
                    {
                        if (k == 1)
                            Console.Write($"|   {Field[j, i]}   ");
                        else if (k == 2)
                            Console.Write("|_______");
                        else
                        {
                            Console.Write("|       ");
                        }

                    }
                    Console.Write("|\n");
                }
            }
        }
    }
}
