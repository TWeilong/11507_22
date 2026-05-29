using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime;
using System.Text.Json;

namespace ConsoleApp51
{
    class IO1
    {
        static void Main(string[] args)
        {
            CoffeeMachine service = new CoffeeMachine();

            bool work = true;

            while (work)
            {
                Console.WriteLine();
                Console.WriteLine("1 - Показать меню");
                Console.WriteLine("2 - Остатки ингредиентов");
                Console.WriteLine("3 - Приготовить напиток");
                Console.WriteLine("4 - Отчёт за день");
                Console.WriteLine("0 - Выход");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        service.PrintMenu();
                        break;

                    case "2":
                        service.PrintIngredients();
                        break;

                    case "3":
                        Console.Write("Введите напиток: ");
                        string drinkName = Console.ReadLine();

                        service.PrepareDrink(drinkName);
                        break;

                    case "4":
                        service.CreateReport();
                        break;

                    case "0":
                        work = false;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда");
                        break;
                }
            }
        }
    }

    class CoffeeMachine
    {
        private const string ConfigPath = "config.json";
        private const string SalesPath = "sales_history.txt";

        private Settings settings;

        private readonly Dictionary<string, Dictionary<string, int>> recipes = new Dictionary<string, Dictionary<string, int>>
            {
            {
                "Эспрессо",
                new Dictionary<string, int>
                {
                    { "Вода", 50 },
                    { "Зерна", 10 }
                }
            },

            {
                "Американо",
                new Dictionary<string, int>
                {
                    { "Вода", 150 },
                    { "Зерна", 10 }
                }
            },

            {
                "Капучино",
                new Dictionary<string, int>
                {
                    { "Вода", 50 },
                    { "Молоко", 100 },
                    { "Зерна", 10 }
                }
            },

            {
                "Латте",
                new Dictionary<string, int>
                {
                    { "Вода", 50 },
                    { "Молоко", 150 },
                    { "Зерна", 10 }
                }
            }
            };

        public CoffeeMachine()
        {
            settings = LoadSettings();
        }

        private Settings LoadSettings()
        {
            if (File.Exists(ConfigPath))
            {
                string json = File.ReadAllText(ConfigPath);

                return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
            }
            Settings defaultSettings = new Settings();

            defaultSettings.DrinkPrices.Add("Эспрессо", 100);
            defaultSettings.DrinkPrices.Add("Американо", 120);
            defaultSettings.DrinkPrices.Add("Капучино", 150);
            defaultSettings.DrinkPrices.Add("Латте", 170);

            defaultSettings.Ingredients.Add("Вода", 5000);
            defaultSettings.Ingredients.Add("Молоко", 3000);
            defaultSettings.Ingredients.Add("Зерна", 2000);

            SaveSettings(defaultSettings);

            return defaultSettings;
        }

        private void SaveSettings(Settings cfg)
        {
            string json = JsonSerializer.Serialize(cfg, new JsonSerializerOptions{ WriteIndented = true});
            File.WriteAllText(ConfigPath, json);
        }

        private void SaveCurrentSettings()
        {
            SaveSettings(settings);
        }

    
    public void PrintMenu()
        {
            Console.WriteLine();

            foreach (var drink in settings.DrinkPrices)
            {
                Console.WriteLine($"{drink.Key} - {drink.Value} руб.");
            }
        }

        public void PrintIngredients()
        {
            Console.WriteLine();

            foreach (var ingredient in settings.Ingredients)
            {
                Console.WriteLine($"{ingredient.Key}: {ingredient.Value}");
            }
        }

        private bool HasEnoughIngredients(string drinkName)
        {
            foreach (var ingredient in recipes[drinkName])
            {
                int currentAmount = settings.Ingredients.GetValueOrDefault( ingredient.Key,0);

                if (currentAmount < ingredient.Value)
                {
                    Console.WriteLine( $"Недостаточно ингредиента: {ingredient.Key}");

                    return false;
                }
            }

            return true;
        }

        private void SpendIngredients(string drinkName)
        {
            foreach (var ingredient in recipes[drinkName])
            {
                settings.Ingredients[ingredient.Key] -= ingredient.Value;
            }
        }

        private void SaveSale(string drinkName)
        {
            int price = settings.DrinkPrices[drinkName];

            string record = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{drinkName};{price}";

            File.AppendAllText(SalesPath, record + Environment.NewLine);
        }

        public bool PrepareDrink(string drinkName)
        {
            if (string.IsNullOrWhiteSpace(drinkName))
            {
                Console.WriteLine("Название не введено");
                return false;
            }

            if (!settings.DrinkPrices.ContainsKey(drinkName))
            {
                Console.WriteLine("Такого напитка нет");
                return false;
            }

            if (!recipes.ContainsKey(drinkName))
            {
                Console.WriteLine("Рецепт не найден");
                return false;
            }

            if (!HasEnoughIngredients(drinkName))
            {
                return false;
            }

            SpendIngredients(drinkName);

            SaveCurrentSettings();

            SaveSale(drinkName);

            Console.WriteLine( $"Напиток \"{drinkName}\" успешно приготовлен");

            return true;
        }

        public void CreateReport()
        {
            if (!File.Exists(SalesPath))
            {
                Console.WriteLine("Продаж пока нет");
                return;
            }

            string today = DateTime.Now.ToString("yyyy-MM-dd");

            List<string> todaySales = File.ReadAllLines(SalesPath).Where(line => line.StartsWith(today)).ToList();

            int revenue = 0;

            foreach (string sale in todaySales)
            {
                string[] parts = sale.Split(';');

                if (parts.Length >= 3)
                {
                    revenue += int.Parse(parts[2]);
                }
            }

            var drinksStatistics =todaySales.Select(line => line.Split(';')[1]).GroupBy(name => name).ToDictionary(group => group.Key, group => group.Count());

            var report = new
            {
                Date = today,
                SalesCount = todaySales.Count,
                Revenue = revenue,
                Drinks = drinksStatistics
            };
            string reportFileName = $"report_{DateTime.Now:yyyy_MM_dd}.json";

            string reportJson = JsonSerializer.Serialize( report,new JsonSerializerOptions{ WriteIndented = true });

            File.WriteAllText(reportFileName, reportJson);

            Console.WriteLine();
            Console.WriteLine( $"Отчёт сохранён в файл: {reportFileName}");

            Console.WriteLine( $"Количество продаж: {todaySales.Count}");

            Console.WriteLine( $"Общая выручка: {revenue} руб.");
        }
    } 
}

class Settings
{
    public Dictionary<string, int> DrinkPrices{get;set;} = new Dictionary<string, int>();
    public Dictionary<string, int> Ingredients {get;set;} = new Dictionary<string, int>();
}


