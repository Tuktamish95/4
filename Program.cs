using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        // Задача 1: Вставка элементов списка L за первым вхождением элемента E
        List<int> list = new List<int>();
        Console.WriteLine("Задача 1: Введите элементы списка через пробел:");
        string input = Console.ReadLine();
        var elements = input.Split(' ');
        foreach (var element in elements)
        {
            if (int.TryParse(element, out int number))
            {
                list.Add(number);
            }
            else
            {
                Console.WriteLine($"'{element}' не является числом, оно будет проигнорировано.");
            }
        }

        Console.WriteLine("Введите элемент E для вставки:");
        input = Console.ReadLine();
        if (int.TryParse(input, out int e1))
        {
            InsertAfterElement(list, e1);
        }
        else
        {
            Console.WriteLine($"'{input}' не является числом.");
        }
        Console.WriteLine("Результат после вставки:");
        Console.WriteLine(string.Join(", ", list));

        // Задача 2: Вставка элемента E в начало и конец списка
        Console.WriteLine("Задача 2: Введите элемент E для вставки в начало и конец списка:");
        input = Console.ReadLine();
        if (int.TryParse(input, out int newElement))
        {
            AddToBeginAndEnd(list, newElement);
            Console.WriteLine("Результат после вставки в начало и конец:");
            Console.WriteLine(string.Join(", ", list));
        }
        else
        {
            Console.WriteLine($"'{input}' не является числом.");
        }

        // Задача 3: Определение прочитанных книг
        DetermineReadBooks();

        // Задача 4: Поиск символов
        FindUniqueCharacters();

        // Задача 5: Школьная олимпиада с XML-сериализацией
        DetermineWinnersWithXml();
    }

    // Задача 1: Вставка элементов списка L за первым вхождением элемента E
    public static void InsertAfterElement(List<int> list, int e)
    {
        int index = list.IndexOf(e);
        if (index != -1)
        {
            var newList = new List<int>();
            newList.AddRange(list.GetRange(0, index + 1));
            newList.AddRange(list);
            list.Clear();
            list.AddRange(newList);
        }
        else
        {
            Console.WriteLine($"Элемент {e} не найден в списке.");
        }
    }

    // Задача 2: Вставка элемента в начало и конец списка
    public static void AddToBeginAndEnd(List<int> list, int e)
    {
        list.Insert(0, e);
        list.Add(e);
    }

    // Задача 3: Определение прочитанных книг
    public static void DetermineReadBooks()
    {
        Console.WriteLine("\nЗадача 3: Определение прочитанных книг.");
        Console.WriteLine("Введите количество доступных книг:");
        if (!int.TryParse(Console.ReadLine(), out int numberOfBooks) || numberOfBooks <= 0)
        {
            Console.WriteLine("Некорректное количество книг.");
            return;
        }

        // Вводим список доступных книг
        var availableBooks = new HashSet<string>();
        Console.WriteLine($"Введите названия {numberOfBooks} доступных книг через запятую:");
        var booksInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(booksInput))
        {
            availableBooks.UnionWith(booksInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(b => b.Trim()));
        }

        Console.WriteLine("Введите количество читателей:");
        if (!int.TryParse(Console.ReadLine(), out int numberOfReaders) || numberOfReaders <= 0)
        {
            Console.WriteLine("Некорректное количество читателей.");
            return;
        }

        var readersBooks = new Dictionary<string, HashSet<string>>();
        var allBooksRead = new HashSet<string>(); // Книги, которые были прочитаны хотя бы одним читателем

        // Считываем книги, прочитанные каждым читателем
        for (int i = 1; i <= numberOfReaders; i++)
        {
            Console.WriteLine($"Введите названия книг, прочитанных читателем {i}, через запятую:");
            string books = Console.ReadLine();
            if (!string.IsNullOrEmpty(books))
            {
                var bookList = new HashSet<string>(books.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(b => b.Trim()));

                // Проверка на наличие книг в списке доступных
                var validBooks = bookList.Intersect(availableBooks).ToHashSet();
                if (validBooks.Any())
                {
                    readersBooks[$"Читатель {i}"] = validBooks;
                    allBooksRead.UnionWith(validBooks); // Добавляем прочитанные книги в общий список
                }
                else
                {
                    Console.WriteLine($"Читатель {i} не прочитал никаких доступных книг.");
                }
            }
        }

        var allRead = new List<string>();
        var someRead = new List<string>();
        var noneRead = new HashSet<string>(availableBooks); // Начиная с доступных книг

        foreach (var book in availableBooks)
        {
            int count = readersBooks.Count(r => r.Value.Contains(book));
            if (count == numberOfReaders)
            {
                allRead.Add(book);
            }
            else if (count > 0)
            {
                someRead.Add(book);
            }
        }

        // Книги, которые никто не прочитал
        noneRead.ExceptWith(allBooksRead);
        Console.WriteLine("\nКниги, прочитанные всеми:");
        Console.WriteLine(allRead.Count > 0 ? string.Join(", ", allRead) : "Нет таких книг.");
        Console.WriteLine("\nКниги, прочитанные некоторыми:");
        Console.WriteLine(someRead.Count > 0 ? string.Join(", ", someRead) : "Нет таких книг.");
        Console.WriteLine("\nКниги, которые никто не прочитал:");
        Console.WriteLine(noneRead.Count > 0 ? string.Join(", ", noneRead) : "Нет таких книг.");
    }

    // Задача 4: Поиск уникальных символов
    public static void FindUniqueCharacters()
    {
        Console.WriteLine("\nЗадача 4: Поиск уникальных символов в тексте.");
        Console.WriteLine("Введите текст на русском языке:");
        string text = Console.ReadLine();
        var words = text.Split(new[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length < 2)
        {
            Console.WriteLine("Введите как минимум два слова.");
            return;
        }

        var firstWord = words[0];
        var firstWordChars = new HashSet<char>(firstWord);
        var commonChars = new HashSet<char>(words[1]);

        for (int i = 2; i < words.Length; i++)
        {
            commonChars.IntersectWith(words[i]);
        }
        commonChars.ExceptWith(firstWordChars);

        Console.WriteLine("Символы, которых нет в первом слове, но есть в каждом из остальных:");
        Console.WriteLine(commonChars.Count > 0 ? string.Join(", ", commonChars) : "Нет таких символов.");
    }

    // Задача 5: Школьная олимпиада с XML-сериализацией
    public static void DetermineWinnersWithXml()
    {
        Console.WriteLine("\nЗадача 5: Определение призеров олимпиады с XML-сериализацией.");
        Console.WriteLine("Введите количество участников олимпиады:");
        if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
        {
            Console.WriteLine("Некорректное количество участников.");
            return;
        }

        var participants = new List<Participant>();
        for (int i = 0; i < n; i++)
        {
            var input = Console.ReadLine();
            var data = input.Split(' ');

            if (data.Length != 4 || !int.TryParse(data[2], out int grade) || !int.TryParse(data[3], out int points) ||
                grade < 7 || grade > 11 || points < 0 || points > 70)
            {
                Console.WriteLine("Некорректный ввод данных.");
                return;
            }
            participants.Add(new Participant(data[0], data[1], grade, points));
        }

        // Сериализация участников в XML
        SerializeParticipants(participants);

        // Определение призеров
        var sortedParticipants = participants.OrderByDescending(p => p.Points).ToList();
        int countToSelect = (int)Math.Ceiling(0.25 * sortedParticipants.Count);
        var winners = new List<Participant>();
        int minScoreOfWinners = sortedParticipants[countToSelect - 1].Points;

        for (int i = 0; i < countToSelect; i++)
        {
            winners.Add(sortedParticipants[i]);
        }

        if (countToSelect < sortedParticipants.Count)
        {
            int lastScore = sortedParticipants[countToSelect].Points;
            if (minScoreOfWinners == lastScore)
            {
                if (minScoreOfWinners > 35)
                {
                    winners.AddRange(sortedParticipants.Skip(countToSelect).Where(p => p.Points == minScoreOfWinners));
                }
            }
        }

        var prizeCountByGrade = new int[5]; // Для классов 7-11
        foreach (var winner in winners)
        {
            prizeCountByGrade[winner.Grade - 7]++;
        }

        Console.WriteLine($"\nМинимальный балл призера: {minScoreOfWinners}");
        Console.WriteLine("Количество призеров по классам (7-11): " + string.Join(" ", prizeCountByGrade));
    }

    // Сериализация списка участников
    public static void SerializeParticipants(List<Participant> participants)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Participant>));
        using (StreamWriter writer = new StreamWriter("participants.xml"))
        {
            serializer.Serialize(writer, participants);
        }
        Console.WriteLine("Данные участников сохранены в файл participants.xml.");
    }
}

public class Participant
{
    [XmlElement("LastName")]
    public string LastName { get; set; }

    [XmlElement("FirstName")]
    public string FirstName { get; set; }

    [XmlElement("Grade")]
    public int Grade { get; set; }

    [XmlElement("Points")]
    public int Points { get; set; }

    public Participant() { }

    public Participant(string lastName, string firstName, int grade, int points)
    {
        LastName = lastName;
        FirstName = firstName;
        Grade = grade;
        Points = points;
    }
}
