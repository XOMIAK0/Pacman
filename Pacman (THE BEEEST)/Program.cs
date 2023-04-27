using System;
using System.IO; //для работы File.ReadAllLines
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; //для Task.Run
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Pacman__THE_BEEEST_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[,] map = ReadMap("map.txt");
            Console.CursorVisible = false;

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            //запускаем бесконечный цикл в ОТДЕЛЬНОМ потоке
            Task.Run(() =>
            {
                while (true)
                {
                    pressedKey = Console.ReadKey();
                }
            });
            

            int pacmanX = 1;
            int pacmanY = 1;
            int score = 0;

            while (true)
            {
                Console.Clear();
                
                HandleInput(pressedKey, ref pacmanX, ref pacmanY, map, ref score);

                Console.ForegroundColor = ConsoleColor.Blue;
                DrawMap(map);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(pacmanX, pacmanY);
                Console.Write("@");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(32, 0);
                Console.Write($"You score: {score}");

                //частота обновления консоли
                Thread.Sleep(100);
            
            }


        }


        private static char[,] ReadMap(string path) //path - путь (типо путь к карте)
                                                    //он возвращает двумерный массив, который и представляет карту
        {
            string[] file = File.ReadAllLines("map.txt");

            char[,] map = new char[GetMaxLengthOfRow(file), file.Length];

            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y] = file[y][x]; //по разному потому что ориентация в блокноте и тут противоположны
                }
            }
            return map;
        } 

        private static char[,] DrawMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x,y]);
                }
                Console.Write("\n");
            }
            return map;
        }

        private static void HandleInput(ConsoleKeyInfo pressedKey, ref int pacmanX, ref int pacmanY, char[,] map, ref int score)
        {
            int[] direction = GetDirection(pressedKey);

            int nextPacmanPositionX = pacmanX + direction[0]; 
            int nextPacmanPositionY = pacmanY + direction[1];

            char nextCell = map[nextPacmanPositionX, nextPacmanPositionY];


            if (nextCell == ' ' || nextCell == '.')
            {
                pacmanX = nextPacmanPositionX;
                pacmanY = nextPacmanPositionY;
                if (nextCell == '.')
                {
                    score++;
                    map[nextPacmanPositionX, nextPacmanPositionY] = ' ';
                }
            }



        }

        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            int[] direction = { 0, 0 };

            if (pressedKey.Key == ConsoleKey.UpArrow)
                direction[1] = -1;
            else if (pressedKey.Key == ConsoleKey.DownArrow)
                direction[1] = 1;
            else if (pressedKey.Key == ConsoleKey.LeftArrow)
                direction[0] = -1;
            else if (pressedKey.Key == ConsoleKey.RightArrow)
                direction[0] = 1;

            return direction;
        }

        private static int GetMaxLengthOfRow(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach(var line in lines)
                if (line.Length > maxLength)
                    maxLength = line.Length;

            return maxLength;

        }
    }
}
