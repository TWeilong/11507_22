using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;




namespace ConsoleApp2
{
    public abstract class Hero
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }

        public Hero(string name)
        {
            Name = name;
            Level = 1;
        }
    }

    public class StrengthHero : Hero
    {
        public StrengthHero(string name) : base(name)
        {
            Strength = 76;
            Agility = 27;
            Intelligence = 19;

            Health = Strength * 10;
            Mana = Intelligence * 10;
        }
    }

    public class IntelligenceHero : Hero
    {
        public IntelligenceHero(string name) : base(name)
        {
            Strength = 33;
            Agility = 27;
            Intelligence = 48;

            Health = Strength * 10;
            Mana = Intelligence * 10;
        }
    }

    public class AgilityHero : Hero
    {
        public AgilityHero(string name) : base(name)
        {
            Strength = 54;
            Agility = 79;
            Intelligence = 32;

            Health = Strength * 10;
            Mana = Intelligence * 10;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Hero> heroes = new List<Hero>()
            {
                new StrengthHero("Destroyer"),
                new IntelligenceHero("Herta"),
                new AgilityHero("Joker")
            };

            string path = "heroes.txt";

            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (Hero hero in heroes)
                {
                    writer.WriteLine($"Имя: {hero.Name}");
                    writer.WriteLine($"Уровень: {hero.Level}");
                    writer.WriteLine($"Здоровье: {hero.Health}");
                    writer.WriteLine($"Мана: {hero.Mana}");
                    writer.WriteLine();
                }
            }

            Console.WriteLine("Файл создан!");

            Console.WriteLine("\nСодержимое файла:\n");

            using (StreamReader reader = new StreamReader(path))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}

