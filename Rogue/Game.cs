using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class Game
    {
        public string LineReader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string input = Console.ReadLine();
            return input;
        }
        public void Run()
        {

            PlayerCharacter player = new PlayerCharacter('@', ConsoleColor.Green);
            Console.WriteLine("Valitse nimi: ");
            while (true)
            {
                bool okAnswer = true;
                player.name = LineReader();
                for (int i = 0; i < player.name.Length; i++)
                {
                    if (char.IsLetter(player.name[i]) == false)
                    {
                        okAnswer = false;
                        break;
                    }
                }
                if (!okAnswer)
                {
                    Console.WriteLine("Nimessä on sopimattomia merkkejä. Kirjoita uudestaan:");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Valitse rotu:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" 1 = Zombi \n 2 = Ihminen \n 3 = Koira");
            switch (LineReader())
            {
                case "1":
                    player.race = Race.Zombi;
                    break;
                case "2":
                    player.race = Race.Ihminen;
                    break;
                case "3":
                    player.race = Race.Koira;
                    break;
                default:
                    player.race = Race.Ihminen;
                    break;
            }

            Console.WriteLine("Valitse tyyppi:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" 1 = Meele \n 2 = Range");
            switch (LineReader())
            {
                case "1":
                    player.type = Class.Meele;
                    break;
                case "2":
                    player.type = Class.Range;
                    break;
                default:
                    player.type = Class.Meele;
                    break;
            }
            MapReader reader = new MapReader();
            Map level01 = reader.LoadJSON("mapfile.json");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Pelajaan tiedot: \n Nimi on {player.name} \n Tyyppi on {player.type} \n Rotu on {player.race}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Press any button to continiue:");
            Console.ReadKey();

            player.position = new Vector2(1, 1);

            Console.Clear();
            player.Draw(level01);

            bool game_running = true;
            while (game_running)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        player.Move(0, -1, level01);
                        break;
                    case ConsoleKey.DownArrow:
                        player.Move(0, 1, level01);
                        break;
                    case ConsoleKey.LeftArrow:
                        player.Move(-1, 0, level01);
                        break;
                    case ConsoleKey.RightArrow:
                        player.Move(1, 0, level01);
                        break;
                    case ConsoleKey.Escape:
                        game_running = false;
                        break;
                    default:
                        break;
                };
                Console.Clear();
                player.Draw(level01);
            }
        }
    }
}
