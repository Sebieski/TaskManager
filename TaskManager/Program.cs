using Microsoft.VisualBasic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using TaskManager.BusinessLogic;
using Task = TaskManager.BusinessLogic.Task;
using TaskStatus = TaskManager.BusinessLogic.TaskStatus;

namespace TaskManager
{
    public class Program
    {
        private static TaskMangerService _taskManagerService = new TaskMangerService();

        static void Main(string[] args)
        {
            Console.WriteLine("Witaj w programie do zarządzania zadaniami!");

            var commands = new Dictionary<int, string>
            {
                { 1, "Dodaj zadanie" },
                { 2, "Usuń zadanie" },
                { 3, "Pokaż szczegóły zadania" },
                { 4, "Wyświetl wszystkie zadania" },
                { 5, "Wyświetl zadania według statusu" },
                { 6, "Szukaj zadania" },
                { 7, "Zmień status zadania" },
                { 8, "Zakończ" }
            };

            bool stayInLoop = true;
            while (stayInLoop)
            {
                Console.WriteLine();
                Console.WriteLine("---- Dostępne komendy: ----");
                //wyświetlenie dostępnych komend
                foreach (var kvPair in commands)
                {
                    Console.WriteLine($"{kvPair.Key} - {kvPair.Value}");
                }

                //pobranie i wykonanie komendy
                Console.Write("\nPodaj numer komendy do wykonania i zatwierdź ENTERem: ");
                var command = Console.ReadLine().Trim();
                switch (command)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        RemoveTask();
                        break;
                    case "3":
                        ShowDetails();
                        break;
                    case "4":
                        ShowAll();
                        break;
                    case "5":
                        ShowByStatus();
                        break;
                    case "6":
                        FindTask();
                        break;
                    case "7":
                        Update();
                        break;
                    case "8":
                        Console.WriteLine("Praca programu zakończona.");
                        stayInLoop = false;
                        break;
                    default:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Podano nieznaną komendę, wprowadź cyfrę (1-8) i zatwierdź ENTERem");
                        Console.ResetColor();
                        break;
                }

            }
            
        }

        private static void Update()
        {
            Console.Clear();
            Console.Write("Podaj ID zadania do zmiany statusu: ");
            int taskId;
            if (!int.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out taskId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Podano niewłaściwy format ID zadania!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Podaj numer nowego statusu zadania: 1 - ToDo, 2 - InProgress, 3 - Done");
            int newStatus;
            if (!int.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out newStatus))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Podano zły format statusu! Operacja nieudana!");
                Console.ResetColor();
            }
            else
            {
                if (newStatus < 0 || newStatus > 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Podano nieznany format statusu! Operacja nieudana!");
                    Console.ResetColor();
                }
                else
                {
                    if(_taskManagerService.ChangeStatus(taskId,(TaskStatus)newStatus-1))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Zmiana statusu zadania o ID nr {taskId} na {(TaskStatus)newStatus-1} dokonana pomyślnie!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Nie udało się zmienić statusu zadania");
                        Console.ResetColor();
                    }
                }
            }
        }

        private static void FindTask()
        {
            Console.Clear();
            Console.Write("Podaj fragment opisu zadania do znalezienia: ");
            string descriptionToFind = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(descriptionToFind))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nie podano opisu. Nie mogę rozpocząć poszukiwania!");
                Console.ResetColor();
                return;
            }

            var allTasks = _taskManagerService.GetAll(descriptionToFind);
            if (allTasks.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"W systemie nie ma jeszcze żadnych zadań z {descriptionToFind} w opisie!");
                Console.ResetColor();
                return;
            }
            Console.WriteLine($"Aktualna liczba zadań ze statusem {descriptionToFind} w systemie: {allTasks.Length}. Oto lista: ");
            foreach (var task in allTasks)
            {
                Console.WriteLine(task);
            }
        }

        private static void ShowByStatus()
        {
            Console.Clear();
            Console.Write("Podaj numer odpowiadający statusowi do wyświetlenia: 1 - ToDo, 2 - InProgress, 3 - Done: ");
            int statusToDisplay;
            if (!int.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out statusToDisplay))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Podano zły format statusu.");
                Console.ResetColor();
                return;
            }
            if (statusToDisplay < 0 || statusToDisplay > 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Podano nieznany format statusu! Operacja nieudana!");
                Console.ResetColor();
            }

            var allTasks = _taskManagerService.GetAll((TaskStatus)statusToDisplay-1);
            if (allTasks.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"W systemie nie ma jeszcze żadnych zadań ze statusem {(TaskStatus)statusToDisplay-1}!");
                Console.ResetColor();
                return;
            }
            Console.WriteLine($"Aktualna liczba zadań ze statusem {(TaskStatus)statusToDisplay} w systemie: {allTasks.Length}. Oto lista: ");
            foreach (var task in allTasks)
            {
                Console.WriteLine(task);
            }
        }

        private static void ShowAll()
        {
            Console.Clear();
            var allTasks = _taskManagerService.GetAll();
            if (allTasks.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("W systemie nie ma jeszcze żadnych zadań!");
                Console.ResetColor();
                return;
            }
            Console.WriteLine($"Aktualna liczba zadań w systemie: {allTasks.Length}. Oto lista: ");
            foreach (var task in allTasks)
            {
                Console.WriteLine(task);
            }
        }

        private static void ShowDetails()
        {
            Console.Clear();
            Console.Write("Podaj ID zadania do wyświetlenia: ");
            int taskId;
            if (!int.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out taskId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Podano niewłaściwy format ID zadania!");
                Console.ResetColor();
                return;
            }

            if (_taskManagerService.Get(taskId) != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(_taskManagerService.Get(taskId));
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Brak zadania o podanym ID w systemie!");
                Console.ResetColor();
            }
        }

        private static void RemoveTask()
        {
            Console.Clear();
            Console.Write("Podaj ID zadania do usunięcia: ");
            int taskId;
            if (!int.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out taskId))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Podano niewłaściwy format ID zadania!");
                Console.ResetColor();
                return;
            }

            if (_taskManagerService.Remove(taskId))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Zadanie o ID: {taskId} zostało usunięte!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Brak zadania o podanym ID w systemie!");
                Console.ResetColor();
            }
        }

        private static void AddTask()
        {
            Console.Clear();
            Console.Write("Podaj opis zadania: ");
            var description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nie podano opisu! Zadanie musi mieć opis.");
                Console.ResetColor();
                return;
            }

            Console.Write("Podaj opcjonalnie datę do kiedy zadanie powinno być wykonane: ");
            DateTime? dueDate;
            if (!DateTime.TryParse(Console.ReadLine(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                dueDate = null;
            }
            else
            {
                dueDate = result;
            }

            var task = _taskManagerService.Add(description, dueDate);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Dodano zadanie {task}");
            Console.ResetColor();
        }
    }
}