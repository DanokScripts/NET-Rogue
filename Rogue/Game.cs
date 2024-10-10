using System;
using System.Numerics;
using ZeroElectric.Vinculum; // Raylibin nimiavaruus

namespace Rogue
{
    public class Game
    {
        public static readonly int tileSize = 16; // Staattinen muuttuja

        private PlayerCharacter player;
        private GameMap? level01;

        public void Run()
        {
            Init();
            GameLoop();
        }

        // Alustaa pelin
        private void Init()
        {
            // Pelaajan luominen
            player = CreateCharacter();

            // Karttatiedoston lataaminen
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

            // Raylib-ikkunan alustaminen
            Raylib.InitWindow(480, 270, "Rogue");
            Raylib.SetTargetFPS(60);

            player.position = new Vector2(1, 1);
        }

        // Pelisilmukka
        private void GameLoop()
        {
            while (!Raylib.WindowShouldClose())
            {
                UpdateGame();
                DrawGame();
            }

            Raylib.CloseWindow();
        }

        // Kysyy pelaajan nimen
        private string AskName()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Valitse nimi: ");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                string? playerName = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                if (IsValidName(playerName))
                {
                    return playerName!;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNimessä on vääränlaisia merkkejä. \nKirjoita uudelleen:");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        // Kysyy pelaajan rodun
        private Race AskRace()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nValitse rotu:");
            Console.WriteLine(" 1 - Hevonen \n 2 - Possu \n 3 - Kana");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                string? input = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                switch (input)
                {
                    case "1":
                        return Race.Hevonen;
                    case "2":
                        return Race.Possu;
                    case "3":
                        return Race.Kana;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nVirheellinen valinta. Valitse uudelleen:");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        // Kysyy pelaajan luokan
        private Class AskClass()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nValitse tyyppi:");
            Console.WriteLine(" 1 - Puukko \n 2 - Pistooli");
            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                string? input = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Blue;
                switch (input)
                {
                    case "1":
                        return Class.Puukko;
                    case "2":
                        return Class.Pistooli;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nVirheellinen valinta. Valitse uudelleen:");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            }
        }

        // Luo pelaajan karakterin
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

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nPelaajan tiedot:");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Pelaajan nimi: {player.name}\nPelaajan tyyppi: {player.type}\nPelaajan rotu: {player.race}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nJatka painamalla mitä tahansa näppäintä:");
            Console.ReadKey();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            return player;
        }

        // Piirtää pelin
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

        // Päivittää pelin tilaa
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

        // Tarkistaa nimen kelpoisuuden
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