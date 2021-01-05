using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class SnakeLogic
    {
        private int appleX;
        private int appleY;

        private Random myLocalRandom = new Random();

        public void createApple(int SIZE, int DOT_SIZE)  // Генерация координат яблочка
        {
            appleX = myLocalRandom.Next(1, (SIZE - DOT_SIZE) / DOT_SIZE) * DOT_SIZE;
            appleY = myLocalRandom.Next(1, (SIZE - DOT_SIZE) / DOT_SIZE) * DOT_SIZE;
        }

        public int getAppleX => appleX;
        public int getAppleY => appleY;
        public int setAppleX(int x) => appleX = x;
        public int setAppleY(int y) => appleY = y;
    }
}
