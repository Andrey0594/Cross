using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossApplication
{
    class GameClass
    {
        public FieldClass Field { get; set; }
        public int WinSeries { get; set; }
        public char PlayerDot { get; set; }
        public char AIDot { get; set; }

        private List<(int x, int y)> _emptyPoints { get; set; }



        public GameClass(FieldClass field, int winSeries, char playerDot, char aiDot)
        {
            Field = field;
            WinSeries = winSeries;
            PlayerDot = playerDot;
            AIDot = aiDot;
            _emptyPoints = new List<(int x, int y)>();
            for (int i = 0; i < field.SizeX; i++)
            {
                for (int j = 0; j < field.SizeY; j++)
                {
                    _emptyPoints.Add((x: i, y: j));
                }
            }
            Game();
        }


        private bool IsEmpty(int x, int y)
        {
            if (Field.Field[x, y] == ' ')
                return true;
            return false;
        }


        private (int X, int Y) GetIndexesFromCoordinates(int currentX, int currentY)
        {
            var result = (x: (currentX - 4) / 8, y: (currentY - 2) / 3);
            return result;
        }





        private bool IsHorisontalWin((int x, int y) point, char dot)
        {
            int count = 0;

            for (int i = 0; i < Field.SizeX; i++)
            {
                if (Field.Field[i, point.y] == dot)
                    count++;
                else
                    count = 0;
                if (count == WinSeries)
                    return true;
            }
            return false;
        }
        private bool IsVerticalWin((int x, int y) point, char dot)
        {
            int count = 0;
            for (int i = 0; i < Field.SizeY; i++)
            {
                if (Field.Field[point.x, i] == dot)
                    count++;
                else
                    count = 0;
                if (count == WinSeries)
                    return true;
            }
            return false;

        }
        private bool IsDownDiagonalWin((int x, int y) point, char dot)
        {
            int count = 0;
            int delta = point.y - point.x;
            for (int i = 0; i < Field.SizeX - delta; i++)
            {
                if (Field.Field[i, i + delta] == dot)
                    count++;
                else
                {
                    count = 0;
                }

                if (count == WinSeries)
                    return true;
            }
            return false;
        }
        private bool IsUpDiagonalWin((int x, int y) point, char dot)
        {
            int count = 0;
            int delta = point.x - point.y;
            for (int i = delta; i < Field.SizeX; i++)
            {
                if (Field.Field[i, i - delta] == dot)
                    count++;
                else
                {
                    count = 0;
                }

                if (count == WinSeries)
                    return true;
            }
            return false;
        }
        private bool IsDownReverseDiagonalWin((int x, int y) point, char dot)
        {
            int count = 0;
            int sum = point.x + point.y;
            for (int i = Field.SizeX - 1; i >= sum - (Field.SizeX - 1); i--)
            {
                if (Field.Field[i, sum - i] == dot)
                    count++;
                else
                {
                    count = 0;
                }

                if (count == WinSeries)
                    return true;
            }
            return false;
        }
        private bool IsUpReverseDiagonalWin((int x, int y) point, char dot)
        {
            int count = 0;
            int sum = point.x + point.y;
            for (int i = sum; i >= 0; i--)
            {
                if (Field.Field[i, sum - i] == dot)
                    count++;
                else
                {
                    count = 0;
                }

                if (count == WinSeries)
                    return true;
            }
            return false;
        }
        private bool IsWin((int x, int y) point, char dot)
        {
            bool isWin = IsHorisontalWin(point, dot) || IsVerticalWin(point, dot);
            if (point.x <= point.y)
                isWin = isWin || IsDownDiagonalWin(point, dot);
            else
                isWin = isWin || IsUpDiagonalWin(point, dot);
            if (point.x + point.y >= Field.SizeY)
                isWin = isWin || IsDownReverseDiagonalWin(point, dot);
            else
                isWin = isWin || IsUpReverseDiagonalWin(point, dot);
            return isWin;
        }
        private (int x, int y) PersonMove(char dot)
        {
            int currentX = 4;
            int currentY = 2;
            Field.DrawField();
            ConsoleKey key;
            Console.SetCursorPosition(currentX, currentY);
            (int X, int Y) result;
            do
            {
                result = GetIndexesFromCoordinates(currentX, currentY);
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.LeftArrow && currentX > 4)
                    currentX -= 8;
                else if (key == ConsoleKey.RightArrow && currentX < (Field.SizeX * 8) - 4)
                    currentX += 8;
                else if (key == ConsoleKey.UpArrow && currentY > 2)
                    currentY -= 3;
                else if (key == ConsoleKey.DownArrow && currentY < (Field.SizeY * 3) - 2)
                    currentY += 3;
                else if (key == ConsoleKey.Enter && IsEmpty(result.X, result.Y))
                {
                    Field.Field[result.X, result.Y] = dot;
                    _emptyPoints.Remove((result.X, result.Y));
                    Console.Clear();
                    Field.DrawField();
                    Console.SetCursorPosition(currentX, currentY);
                    break;
                }
                Console.Clear();
                Field.DrawField();
                Console.SetCursorPosition(currentX, currentY);
            } while (true);

            return result;



        }



        private (int x, int y, bool win) IsNextHorisontalAiStepWin((int x, int y) point)
        {
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            for (int i = 0; i < Field.SizeX; i++)
            {
                if (Field.Field[i, point.y] == AIDot)
                    count++;
                else if (Field.Field[i, point.y] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, point.y);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count == WinSeries)
                    return (result.x, result.y, true);
            }
            return (result.x, result.y, false);
        }
        private (int x, int y, bool win) IsNextVerticalAiStepWin((int x, int y) point)
        {
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            for (int i = 0; i < Field.SizeY; i++)
            {
                if (Field.Field[point.x, i] == AIDot)
                    count++;
                else if (Field.Field[point.x, i] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (point.x, i);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count == WinSeries)
                    return (result.x, result.y, true);
            }
            return (result.x, result.y, false);
        }
        private (int x, int y, bool win) IsNextDownDiagonalAiStepWin((int x, int y) point)
        {
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int delta = point.y - point.x;
            for (int i = 0; i < Field.SizeX - delta; i++)
            {
                if (Field.Field[i, i + delta] == AIDot)
                    count++;
                else if (Field.Field[i, i + delta] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, i + delta);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count == WinSeries)
                    return (result.x, result.y, true);
            }
            return (result.x, result.y, false);
        }
        private (int x, int y, bool win) IsNextUpDiagonalAiStepWin((int x, int y) point)
        {
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int delta = point.x - point.y;
            for (int i = delta; i < Field.SizeX; i++)
            {
                if (Field.Field[i, i - delta] == AIDot)
                    count++;
                else if (Field.Field[i, i - delta] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, i - delta);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count == WinSeries)
                    return (result.x, result.y, true);
            }
            return (result.x, result.y, false);
        }
        private (int x, int y, bool win) IsNextDownReverseDiagonalAiStepWin((int x, int y) point)
        {
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int sum = point.x + point.y;
            for (int i = Field.SizeX - 1; i >= sum - (Field.SizeX - 1); i--)
            {
                if (Field.Field[i, sum - i] == AIDot)
                    count++;
                else if (Field.Field[i, sum - i] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, sum - i);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count == WinSeries)
                    return (result.x, result.y, true);
            }
            return (result.x, result.y, false);
        }
        private (int x, int y, bool win) IsNextUpReverseDiagonalAiStepWin((int x, int y) point)
        {
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int sum = point.x + point.y;
            for (int i = sum; i >= 0; i--)
            {
                if (Field.Field[i, sum - i] == AIDot)
                    count++;
                else if (Field.Field[i, sum - i] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, sum - i);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count == WinSeries)
                    return (result.x, result.y, true);
            }
            return (result.x, result.y, false);
        }
        private (int x, int y, bool win) IsNextAiStepWin()
        {
            for (int i = 0; i < _emptyPoints.Count; i++)
            {
                (int x, int y, bool win) result = IsNextHorisontalAiStepWin(_emptyPoints[i]);
                if (result.win)
                    return result;
                result = IsNextVerticalAiStepWin(_emptyPoints[i]);
                if (result.win)
                    return result;
                if (_emptyPoints[i].x <= _emptyPoints[i].y)
                {
                    result = IsNextDownDiagonalAiStepWin(_emptyPoints[i]);
                    if (result.win)
                        return result;
                }
                else
                {
                    result = IsNextUpDiagonalAiStepWin(_emptyPoints[i]);
                    if (result.win)
                        return result;
                }
                if (_emptyPoints[i].x + _emptyPoints[i].y >= Field.SizeY)
                {
                    result = IsNextDownReverseDiagonalAiStepWin(_emptyPoints[i]);
                    if (result.win)
                        return result;
                }
                else
                {
                    result = IsNextUpReverseDiagonalAiStepWin(_emptyPoints[i]);
                    if (result.win)
                        return result;
                }

            }




            return (0, 0, false);




        }


        private List<(int x, int y, int count)> IsNextHorisontalPlayerStepWin((int x, int y) point)
        {
            List<(int x, int y, int count)> points = new List<(int x, int y, int count)>();
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            for (int i = 0; i < Field.SizeX; i++)
            {
                if (Field.Field[i, point.y] == PlayerDot)
                    count++;
                else if (Field.Field[i, point.y] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, point.y);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count >= WinSeries - 1)
                    points.Add((result.x, result.y, count));
            }

            return points;
        }
        private List<(int x, int y, int count)> IsNextVerticalPlayerStepWin((int x, int y) point)
        {
            List<(int x, int y, int count)> points = new List<(int x, int y, int count)>();
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            for (int i = 0; i < Field.SizeY; i++)
            {
                if (Field.Field[point.x, i] == PlayerDot)
                    count++;
                else if (Field.Field[point.x, i] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (point.x, i);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count >= WinSeries - 1)
                    points.Add((result.x, result.y, count));
            }

            return points;
        }
        private List<(int x, int y, int count)> IsNextDownDiagonalPlayerStepWin((int x, int y) point)
        {
            List<(int x, int y, int count)> points = new List<(int x, int y, int count)>();
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int delta = point.y - point.x;
            for (int i = 0; i < Field.SizeX - delta; i++)
            {
                if (Field.Field[i, i + delta] == PlayerDot)
                    count++;
                else if (Field.Field[i, i + delta] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, i + delta);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }
                if (count >= WinSeries - 1)
                    points.Add((result.x, result.y, count));
            }
            return points;
        }
        private List<(int x, int y, int count)> IsNextUpDiagonalPlayerStepWin((int x, int y) point)
        {
            List<(int x, int y, int count)> points = new List<(int x, int y, int count)>();
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int delta = point.x - point.y;
            for (int i = delta; i < Field.SizeX; i++)
            {
                if (Field.Field[i, i - delta] == PlayerDot)
                    count++;
                else if (Field.Field[i, i - delta] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, i - delta);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count >= WinSeries - 1)
                    points.Add((result.x, result.y, count));
            }

            return points;
        }
        private List<(int x, int y, int count)> IsNextDownReverseDiagonalPlayerStepWin((int x, int y) point)
        {
            List<(int x, int y, int count)> points = new List<(int x, int y, int count)>();
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int sum = point.x + point.y;
            for (int i = Field.SizeX - 1; i >= sum - (Field.SizeX - 1); i--)
            {
                if (Field.Field[i, sum - i] == PlayerDot)
                    count++;
                else if (Field.Field[i, sum - i] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, sum - i);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count >= WinSeries - 1)
                    points.Add((result.x, result.y, count));
            }

            return points;
        }
        private List<(int x, int y, int count)> IsNextUpReverseDiagonalPlayerStepWin((int x, int y) point)
        {
            List<(int x, int y, int count)> points = new List<(int x, int y, int count)>();
            (int x, int y) result = (0, 0);
            int count = 0;
            bool oneEmpty = false;
            int sum = point.x + point.y;
            for (int i = sum; i >= 0; i--)
            {
                if (Field.Field[i, sum - i] == PlayerDot)
                    count++;
                else if (Field.Field[i, sum - i] == Field.EmptyDot && !oneEmpty)
                {
                    oneEmpty = true;
                    result = (i, sum - i);
                    count++;
                }
                else
                {
                    oneEmpty = false;
                    count = 0;
                }

                if (count >= WinSeries - 1)
                    points.Add((result.x, result.y, count));
            }

            return points;
        }
        private (int x, int y, bool win) IsNextPlayerStepWin()
        {
            List<(int x, int y, int count)> points = new List<(int x, int y, int count)>();
            for (int i = 0; i < _emptyPoints.Count; i++)
            {
                points.AddRange(IsNextHorisontalPlayerStepWin(_emptyPoints[i]));
                points.AddRange(IsNextVerticalPlayerStepWin(_emptyPoints[i]));
                if (_emptyPoints[i].x <= _emptyPoints[i].y)
                    points.AddRange(IsNextDownDiagonalPlayerStepWin(_emptyPoints[i]));
                else
                    points.AddRange(IsNextUpDiagonalPlayerStepWin(_emptyPoints[i]));

                if (_emptyPoints[i].x + _emptyPoints[i].y >= Field.SizeY)
                    points.AddRange(IsNextDownReverseDiagonalPlayerStepWin(_emptyPoints[i]));
                else
                    points.AddRange(IsNextUpReverseDiagonalPlayerStepWin(_emptyPoints[i]));


            }
            if (points.Count > 0)
            {
                int max = points.Max(t => t.count);
                points = points.Where(t => t.count == max).ToList();

                Random rnd = new Random();
                var result = points[rnd.Next(0, points.Count)];
                return (result.x, result.y, true);
            }
            return (0, 0, false);




        }

        private (int x, int y) AiMove(char dot)
        {

            var result = IsNextAiStepWin();
            if (result.win)
                return (result.x, result.y);
            result = IsNextPlayerStepWin();
            if (result.win)
                return (result.x, result.y);

            Random rnd = new Random();
            return _emptyPoints[rnd.Next(0, _emptyPoints.Count)];






        }
        private bool FirstStep(ref string currentPlayer)
        {
            if (PlayerDot == 'X')
            {
                currentPlayer = "Человек";
                var point = PersonMove(PlayerDot);
                if (IsWin(point, PlayerDot))
                {
                    return true;
                }
            }
            else
            {
                currentPlayer = "Компьютер";
                var point = AiMove(PlayerDot);
                Field.Field[point.x, point.y] = AIDot;
                _emptyPoints.Remove((point.x, point.y));
                Console.Clear();
                Field.DrawField();
                if (IsWin(point, AIDot))
                {
                    return true;
                }
            }

            return false;
        }

        private bool SecondStep(ref string currentPlayer)
        {
            if (PlayerDot == 'O')
            {
                currentPlayer = "Человек";
                var point = PersonMove(PlayerDot);
                if (IsWin(point, PlayerDot))
                {
                    return true;
                }
            }
            else
            {
                currentPlayer = "Компьютер";
                var point = AiMove(PlayerDot);
                Field.Field[point.x, point.y] = AIDot;
                _emptyPoints.Remove((point.x, point.y));
                Console.Clear();
                Field.DrawField();
                if (IsWin(point, AIDot))
                {
                    return true;
                }
            }

            return false;
        }




        public void Game()
        {
            string currentPlayer = "";
            bool winFlag = false;
            do
            {
                if (_emptyPoints.Count == 0)
                    break;
                winFlag = FirstStep(ref currentPlayer);
                if (winFlag)
                    break;
                winFlag = false;
                winFlag = SecondStep(ref currentPlayer);
                if (winFlag)
                    break;
                winFlag = false;

            } while (true);
            
            Console.SetWindowPosition(0,(Field.SizeY*8)+12);
            if (winFlag)
                Console.WriteLine($"Выиграл {currentPlayer}");
            else
                Console.WriteLine("Ничья");
            Console.SetWindowPosition(0, 0);
            Console.ReadLine();

        }
    }
}
