using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//made by zubr

namespace FindPath.PathFinder
{
    enum moves { UP, LEFT, DOWN, RIGHT, STAY };

    class Board
    {
        public int size{get;set;}
        public char[,] board { get; set; }
        public int probability { get; set; }
        public int counter { get; set; }
        public int currentX { get; set; }
        public int currentY { get; set; }
        public int destinationX { get; set; }
        public int destinationY { get; set; }

        private static char finishChar = 'M';
        private static char currentChar = '1';
        private static char emptyChar = '0';
        private static char blockedChar = 'x';

        public Board(int size, int probability, int startX, int startY)
        {
            this.size = size;
            board = new char[size, size];
            this.probability = probability;
            counter = 0;
            currentX = startX;
            currentY = startY;
            destinationX = 0;
            destinationY = 0;

            board[0, 0] = finishChar;
        }

        public void UpdateBlockades(bool up, bool down, bool left, bool right)
        {
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    board[i, j] = emptyChar;
                }
            }
            board[0, 0] = finishChar;

            if (up && currentY > 0)
            {
                board[currentX, currentY - 1] = blockedChar;
            }

            if(down && currentY + 1 < size)
            {
                board[currentX, currentY + 1] = blockedChar;
            }

            if(left && currentX > 0)
            {
                board[currentX - 1, currentY] = blockedChar;
            }

            if(right && currentX + 1 < size)
            {
                board[currentX + 1, currentY] = blockedChar;
            }

            board[currentX, currentY] = currentChar;
        }

        public void UpdateCurrent(moves move)
        {
            int tempCurrentX = currentX;
            int tempCurrentY = currentY;
            switch (move)
            {
                case moves.UP:
                    currentY -= 1;
                    break;

                case moves.LEFT:
                    currentX -= 1;
                    break;

                case moves.DOWN:
                    currentY++;
                    break;

                case moves.RIGHT:
                    currentX++;
                    break;
            }

            board[tempCurrentX, tempCurrentY] = emptyChar;
            board[currentX, currentY] = currentChar;
        }

        public void Display()
        {
            Console.Clear();

            for(int j = 0; j < size; j++)
            {
                for (int i= 0; i < size; i++)
                {
                    Console.Write($" {board[i, j]} ");
                }
                Console.WriteLine();
            }

        }
    }
}
