using System;
using System.Numerics;
using ZeroElectric.Vinculum; // Raylibin nimiavaruus

namespace Rogue
{
    public enum Race
    {
        Hevonen,
        Possu,
        Kana
    }

    public enum Class
    {
        Puukko,
        Pistooli
    }

    internal class PlayerCharacter
    {
        public string name;
        public Race race;
        public Class type;

        public Vector2 position;
        private char image;
        private Color color; // Raylibin Color-tyyppi

        public PlayerCharacter(char image, Color color)
        {
            this.image = image;
            this.color = color;
        }

        public void Move(int x_move, int y_move, GameMap map)
        {
            int newX = (int)position.X + x_move;
            int newY = (int)position.Y + y_move;

            // Tarkistetaan kartta
            if (newX < 0 || newY < 0 || newX >= map.mapWidth || newY >= map.mapTiles.Length / map.mapWidth)
                return;

            int tileId = map.mapTiles[newY * map.mapWidth + newX];
            if (tileId == 2) // Seinää ei voida ylittää
                return;

            position.X = Math.Clamp(position.X + x_move, 0, map.mapWidth - 1);
            position.Y = Math.Clamp(position.Y + y_move, 0, (map.mapTiles.Length / map.mapWidth) - 1);
        }

        public void Draw(GameMap map)
        {
            int drawPixelX = (int)(position.X * Game.tileSize);
            int drawPixelY = (int)(position.Y * Game.tileSize);

            // Piirretään pelaajan neliö
            Raylib.DrawRectangle(drawPixelX, drawPixelY, Game.tileSize, Game.tileSize, color);

            // Piirretään @-merkki
            Raylib.DrawText("@", drawPixelX + 4, drawPixelY, Game.tileSize, Raylib.WHITE);
        }
    }
}