using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FindPath.PathFinder;
using System.IO;
using System.Threading;

//made by zubr

namespace FindPath
{
    class Program
    {
        static int sleepTime = 500;
        static void Main(string[] args)
        {
            int size, probability, startX, startY;
            GetDataFromUser(out size, out probability, out startX, out startY);

            Board mBoard = new Board(size, probability, startX, startY);
            List<int[]> dataToFile = new List<int[]>();
            var movement = moves.STAY;

            SetupBoard(mBoard, movement, dataToFile);

            Simulate(mBoard,movement,dataToFile);
            Console.WriteLine("Symulacja zakończona");

            SaveData(dataToFile, mBoard);
            Console.WriteLine("Dane zostały zapisane do pliku");
        }

        private static void SaveData(List<int[]> dataToFile, Board mBoard)
        {
            //save data to a file (convert coordinates to a numbered cells)
            StringBuilder sb = new StringBuilder("#Iterator cell");
            sb.Append(Environment.NewLine);
            int iterator = 0;
            foreach (int[] singleCell in dataToFile)
            {
                int cellNumber = 0;

                for (int i = 0; i < mBoard.size; i++)
                {
                    for (int j = 0; j < mBoard.size; j++)
                    {
                        if (singleCell[1] == i && singleCell[2] == j)
                        {
                            sb.Append(iterator);
                            sb.Append(" ");
                            sb.Append(cellNumber);
                            sb.Append(Environment.NewLine);
                        }
                        cellNumber++;
                    }
                }

                iterator++;
            }
            Directory.CreateDirectory(@"C:\FindPathData");
            string path = @"c:\FindPathData\plotData.txt";

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(sb.ToString());
            }
        }

        private static void Simulate(Board mBoard, moves movement, List<int[]> dataToFile)
        {
            Random random = new Random();//random for generate blockade

            while (!EndSimulation(mBoard))
            {


                bool blockadeForUp = random.Next(1, 101) <= mBoard.probability;
                bool blockadeForLeft = random.Next(1, 101) <= mBoard.probability;
                bool blockadeForDown = random.Next(1, 101) <= mBoard.probability;
                bool blockadeForRight = random.Next(1, 101) <= mBoard.probability;

                movement = moves.STAY;//0 - up, 1 - left, 2 - down, 3 - right

                //We can move up or left, and those fields are not blocked
                if (!blockadeForUp && !blockadeForLeft && mBoard.currentY > 0 && mBoard.currentX > 0)
                {
                    //if there is more way to travel up, than to the left side
                    if (mBoard.currentY > mBoard.currentX)
                    {
                        //move up
                        movement = moves.UP;
                    }
                    //otherwise
                    else
                    {
                        //move left
                        movement = moves.LEFT;
                    }
                }
                //There is probably blockade or edge of board on the left side, but we can still move up
                else if (!blockadeForUp && mBoard.currentY > 0)
                {
                    //move up
                    movement = moves.UP;
                }
                //There is probably blockade or edge of board on top, but we can still move left
                else if (!blockadeForLeft && mBoard.currentX > 0)
                {
                    //move left
                    movement = moves.LEFT;
                }
                //There are blockades on top and left (or edges of board), but we can move down or right
                else if (!blockadeForDown && mBoard.currentY < mBoard.size - 1 && !blockadeForRight && mBoard.currentX < mBoard.size - 1)
                {
                    //if there is more way to travel to the left side, than up, move down
                    if (mBoard.currentX > mBoard.currentY)
                    {
                        //move down
                        movement = moves.DOWN;
                    }
                    //otherwise
                    else
                    {
                        //move right
                        movement = moves.RIGHT;
                    }
                }
                //Path is blocked on top, left and right, but we can still move down
                else if (!blockadeForDown && mBoard.currentY < mBoard.size - 1)
                {
                    //move down
                    movement = moves.DOWN;
                }
                //Path is blocked on top, left and down, but we can still move right
                else if (!blockadeForRight && mBoard.currentX < mBoard.size - 1)
                {
                    //move right
                    movement = moves.RIGHT;
                }

                mBoard.counter++;

                mBoard.UpdateBlockades(blockadeForUp, blockadeForDown, blockadeForLeft, blockadeForRight);
                mBoard.Display();
                Thread.Sleep(sleepTime);
                mBoard.UpdateCurrent(movement);
                mBoard.Display();

                int[] currentPositionWithCounter = new int[3] { mBoard.counter, mBoard.currentX, mBoard.currentY };
                dataToFile.Add(currentPositionWithCounter);

                Thread.Sleep(sleepTime);

                //just in case
                if (mBoard.counter >= 2000000)
                {
                    break;
                }
            }
        }

        private static void SetupBoard(Board mBoard, moves movement, List<int[]> dataToFile)
        {
            mBoard.UpdateBlockades(false, false, false, false);
            mBoard.UpdateCurrent(movement);
            mBoard.Display();
            int[] currentPositionWithCounter = new int[3] { mBoard.counter, mBoard.currentX, mBoard.currentY };
            dataToFile.Add(currentPositionWithCounter);
            Thread.Sleep(sleepTime);
        }

        private static bool EndSimulation(Board mBoard)
        {
            return (mBoard.destinationX == mBoard.currentX && mBoard.destinationY == mBoard.currentY) ? true : false;
        }


        private static void GetDataFromUser(out int size,out int probability, out int startX, out int startY)
        {
            Console.WriteLine("Podaj wielkość planszy (od 1 do 100):");
            while (!int.TryParse(Console.ReadLine(), out size) || !Between(size, 1, 100))
            {
                Console.WriteLine("Podano niepoprawną wielkość. Podaj wielkość jeszcze raz:");
            }

            ClearBuffer();

            Console.WriteLine("Podaj prawdopodobieństwo blokowania drogi w % (od 0 do 99):");
            while (!int.TryParse(Console.ReadLine(), out probability) || !Between(probability, 0, 99) )
            {
                Console.WriteLine("Podano niepoprawną wartość prawdopodobieństwa. Podaj wartość jeszcze raz:");
            }

            ClearBuffer();

            Console.WriteLine("Podaj punkt startowy (nr kolumny, od 0 do wielkości tablicy - 1):");
            while (!int.TryParse(Console.ReadLine(), out startX) || !Between(startX, 0, size - 1))
            {
                Console.WriteLine("Podano niepoprawną wartość punktu startowego (nr kolumny). Podaj wartość jeszcze raz:");
            }

            ClearBuffer();

            Console.WriteLine("Podaj punkt startowy (nr wiersza, od 0 do wielkości tablicy - 1):");
            while (!int.TryParse(Console.ReadLine(), out startY) || !Between(startY, 0, size - 1))
            {
                Console.WriteLine("Podano niepoprawną wartość punktu startowego (nr wiersza). Podaj wartość jeszcze raz:");
            }

        }

        private static void ClearBuffer()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        private static bool Between(int num, int lower, int upper, bool inclusive = true)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }
    }
}
