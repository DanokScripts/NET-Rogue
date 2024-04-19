using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    public enum Race
    {
        Zombi,
        Ihminen,
        Koira
    }
    public enum Class
    {
        Meele,
        Range
    }
    internal class PlayerCharacter
    {
       
        public string name;
        public Race race;
        public Class type;

        public Vector2 position;
        private char image;
        private ConsoleColor color;

        public PlayerCharacter(char image, ConsoleColor color)
        {
            this.image = image;
            this.color = color;
        }

        public void Move(int x_move, int y_move, Map map)
        {
            int tildeId = map.mapTiles[((int)position.Y + y_move) * map.mapWidth + (int)position.X + x_move];
            if (tildeId == 2)
                return;
            position.X = Math.Clamp(position.X + x_move, 0, Console.WindowWidth - 1);
            position.Y = Math.Clamp(position.Y + y_move, 0, Console.WindowHeight - 1);
        }

        public void Draw(Map map)
        {

            Console.ForegroundColor = ConsoleColor.Gray;
            for (int y = 0; y < map.mapTiles.Length / map.mapWidth; y++)
            {
                for (int x = 0; x < map.mapWidth; x++)
                {
                    int tileId = map.mapTiles[y * map.mapWidth + x];
                    Console.SetCursorPosition(x, y);
                    Vector2 position = new Vector2(x, y);
                    switch (tileId)
                    {
                        case 1: //floor
                            Console.Write(".");
                            break;
                        case 2: //wall
                            Console.Write("#");
                            break;
                        default:
                            Console.Write(" ");
                            break;
                    }
                }
            }
            Console.CursorVisible = false;
            Console.ForegroundColor = color;
            Console.SetCursorPosition((int)position.X, (int)position.Y);
            Console.Write(image);
        }
    }
}
