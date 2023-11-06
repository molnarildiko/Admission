using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace Admission
{
    internal class Program
    {
        enum EventType
        {
            Be,
            Ki
        }
        struct ScanEvent
        {
            public int hour;
            public int minute;
            public EventType type;
            public int id;
            public string name;
            public string gender;

            public static ScanEvent Convert(string input)
            {
                string[] data = input.Split(';');
                ScanEvent scanEvent = new ScanEvent();
                scanEvent.hour =int.Parse(data[0]);
                scanEvent.minute = int.Parse(data[1]);
                scanEvent.type = (EventType)Enum.Parse(typeof(EventType), data[2]);
                scanEvent.id = int.Parse(data[3]);
                scanEvent.name = data[4];
                scanEvent.gender = data[5];

                return scanEvent;
            }
        }

        static List<ScanEvent> ScanEvents = new List<ScanEvent>();

        static void LoadFile(string path)
        {
            StreamReader sr = new StreamReader ("gate.txt");
            while (!sr.EndOfStream)
            {
                string r = "";
                try
                {
                    r = sr.ReadLine();
                    ScanEvents.Add(ScanEvent.Convert(r));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Hibás sor \"{r}\"({e.Message})");
                }
            }   
                sr.Close();
        }

        static void Main(string[] args)
        {
            LoadFile("gate.txt");

            Console.WriteLine($"Az adott napon {ScanEvents.Count} esemény történt.");

            int income = 0;
            int outcome = 0;

            foreach(var i in ScanEvents)
            {
                switch(i.type) 
                {
                    case EventType.Be:income++;break;
                    case EventType.Ki: outcome++; break;
                }
            }

            Console.WriteLine($"\t{income} Bejövetel");
            Console.WriteLine($"\t{outcome} Kimenetel");

            if (income > outcome)
            {
                Console.WriteLine("Többen jöttek be, mint kimentek!");
            }

            if (income < outcome)
            {
                Console.WriteLine("Többen mentek ki, mint jöttek be!");
            }

            int inMale = 0;
            foreach(var i in ScanEvents)
            {
                if (i.type == EventType.Be && i.gender == "férfi")
                {
                    inMale++;
                }
            }
            Console.WriteLine($"A bejövő férfiak száma = {inMale}");

            StreamWriter sw = new StreamWriter("inFemale.txt");
            Console.WriteLine("A bejövő nők nevei:");
            sw.WriteLine("A bejövő nők nevei: ");

            foreach (var i in ScanEvents)
            {
              if (i.type == EventType.Be && i.gender == "nő")
                {
                    Console.WriteLine(i.name);
                    sw.WriteLine(i.name);
                }
            }
            try
            {
                Console.WriteLine("Add meg az érkező/távozó személy azonosító számát, akiről szeretnéd tudni, hogy kiment-e ezen a kapun!(id)");
                foreach (var i in ScanEvents)
                {
                    Console.WriteLine($"A követkető id-ből választhatsz: {i.id} : {i.name}");
                }

                Console.WriteLine("Írd ide a számot!");

                int number;
                if (int.TryParse(Console.ReadLine(), out number))
                {
                    Console.WriteLine($"A választott id: {number}");

                    foreach (var i in ScanEvents)
                    {
                        if (i.id == number)
                        {
                            Console.WriteLine($"A {i.id} azonosítójú {i.name} nevű személy \"{i.type}\" irányban haladt át a kapun.");
                        }
                        if (i.id != number)
                        {
                        }
                    }
                    Console.WriteLine("Ilyen azonosítójú személy ma nem haladt át a kapun.");
                }
                else throw new FormatException("Helytelen szám formátum");
            }
            catch (Exception e)
                {
                    Console.WriteLine($"Hiba történt: {e.Message}");
                    return;
                }

            int[] db = new int[24];
            foreach (var i in ScanEvents)
            {
                db[i.hour]++;
            }
            Console.WriteLine("Óránkénti aktivitás");
            for (int i = 6; i <= 17; i++)
            {
                Console.WriteLine($"{i}: órakor: {db[i]} kapuátlépés történt.");
            }

        }
    }
}