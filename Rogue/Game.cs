using System;
using System.Numerics;
using ZeroElectric.Vinculum; 

namespace Rogue
{
    public class Game
    {
        public static readonly int tileSize = 16; 

        private PlayerCharacter player;
        private GameMap? level01;

        public void Run()
        {
            Init();
            GameLoop();
        }

        
        private void Init()
        { 
            player = CreateCharacter();

           
            MapReader reader = new MapReader();
            try
            {
                level01 = reader.LoadJSON("mapfile.json");
                if (level01 == null)
                {
                    throw new Exception("Karttaa ei voitu ladata.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Virhe ladattaessa karttaa: {ex.Message}");
                Environment.Exit(1);
            }

            
            Raylib.InitWindow(480, 270, "Rogue");
            Raylib.SetTargetFPS(60);

            player.position = new Vector2(1, 1);
        }

        
        private void GameLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                UpdateGame();
                DrawGame();
            }

            Raylib.CloseWindow();
        }

        
        private string AskName()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Valitse nimi: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            while (true)
            {
                string? playerName = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Red;
                if (IsValidName(playerName))
                {
                    return playerName!;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nNimessä on vääränlaisia merkkejä. \nKirjoita uudelleen:");
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
            }
        }

        
        private Race AskRace()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nValitse rotu:");
            Console.WriteLine(" 1 - Hevonen \n 2 - Possu \n 3 - Kana");
            Console.ForegroundColor = ConsoleColor.Blue;
            while (true)
            {
                string? input = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Green;
                switch (input)
                {
                    case "1":
                        return Race.Hevonen;
                    case "2":
                        return Race.Possu;
                    case "3":
                        return Race.Kana;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("\nVirheellinen valinta. Valitse uudelleen:");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                }
            }
        }

        
        private Class AskClass()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nValitse tyyppi:");
            Console.WriteLine(" 1 - Puukko \n 2 - Pistooli");
            Console.ForegroundColor = ConsoleColor.Blue;
            while (true)
            {
                string? input = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                switch (input)
                {
                    case "1":
                        return Class.Puukko;
                    case "2":
                        return Class.Pistooli;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("\nVirheellinen valinta. Valitse uudelleen:");
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
            }
        }

        
        private PlayerCharacter CreateCharacter()
        {
            string name = AskName();
            Race race = AskRace();
            Class type = AskClass();
            PlayerCharacter player = new PlayerCharacter('@', Raylib.GREEN)
            {
                name = name,
                race = race,
                type = type
            };

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nPelaajan tiedot:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Pelaajan nimi: {player.name}\nPelaajan tyyppi: {player.type}\nPelaajan rotu: {player.race}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nJatka painamalla mitä tahansa näppäintä:");
            Console.ReadKey();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            return player;
        }

        
        private void DrawGame()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.RAYWHITE);

            if (level01 != null)
            {
                for (int y = 0; y < level01.mapTiles.Length / level01.mapWidth; y++)
                {
                    for (int x = 0; x < level01.mapWidth; x++)
                    {
                        int tileId = level01.mapTiles[y * level01.mapWidth + x];
                        Color tileColor = tileId == 1 ? Raylib.GRAY : Raylib.DARKGRAY;
                        Raylib.DrawRectangle(x * tileSize, y * tileSize, tileSize, tileSize, tileColor);
                        if (tileId == 1)
                        {
                            Raylib.DrawText(".", x * tileSize + 4, y * tileSize, tileSize, Raylib.BLACK);
                        }
                        else if (tileId == 2)
                        {
                            Raylib.DrawText("#", x * tileSize + 4, y * tileSize, tileSize, Raylib.BLACK);
                        }
                    }
                }
            }

            player.Draw(level01);
            Raylib.EndDrawing();
        }

        
        private void UpdateGame()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                player.Move(0, -1, level01!);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                player.Move(0, 1, level01!);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT))
            {
                player.Move(-1, 0, level01!);
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT))
            {
                player.Move(1, 0, level01!);
            }
        }

        
        private bool IsValidName(string? name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            foreach (char c in name)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }
    }
}