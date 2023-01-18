using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booked2.Models
{
    public partial class ConferenceRoom
    {
        public ConferenceRoom()
        {
            Bookings = new HashSet<Booking>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int NrOfSeats { get; set; }
        public bool WhiteBoard { get; set; } = false;
        public bool Projector { get; set; } = false;

        public virtual ICollection<Booking> Bookings { get; set; }

        internal static void HandleConferenceRoom()
        {
            using var db = new Booked2Context();

            var loop = true;
            while (loop)
            {
                Console.Clear();
                PrintAllConferenceRooms();
                Console.WriteLine("1. Lägga till konferensrum");
                Console.WriteLine("2. Göra ändring i konferensrum");
                Console.WriteLine("3. Ta bort konferenserum");
                Console.WriteLine("0. Backa");
                var input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        AddConferenceRoom();
                        break;
                    case '2':
                        ChangeConferenceRoom();
                        break;
                    case '3':
                        RemoveConferenceRoom();
                        break;
                    case '0':
                        loop = false;
                        break;

                }
            }
        }
        private static void ChangeConferenceRoom()
        {
            using var db = new Booked2Context();
            Console.Clear();
            PrintAllConferenceRooms();
            Console.WriteLine($"Ange Id för det konferensrummet du vill göra ändringar i, eller ange [0] om du ångrat dig");
            int room = Methods.CheckForInt();
            if (room == 0) return;
            var conferenceRoom = db.ConferenceRooms.Where(x => x.Id == room).SingleOrDefault();
            if (conferenceRoom is not null)
            {
                Console.WriteLine($"Ange vad du vill ändra på");
                Console.WriteLine("1. Namn");
                Console.WriteLine("2. Antal sittplatser");
                Console.WriteLine("3. Whiteboard");
                Console.WriteLine("4. Projektor");
                Console.WriteLine("0. Om du ångrat dig");

                var answer2 = Console.ReadKey(true).KeyChar;
                switch (answer2)
                {
                    case '1':
                        Console.Write("Ange nytt namn: ");
                        conferenceRoom.Name = Console.ReadLine();
                        break;
                    case '2':
                        Console.WriteLine($"Nuvarande antal sittplatser: {conferenceRoom.NrOfSeats}");
                        Console.Write("Ange nytt antal sittplatser: ");
                        conferenceRoom.NrOfSeats = Methods.CheckForInt();
                        break;
                    case '3':
                        Console.WriteLine($"Rummet har {(conferenceRoom.WhiteBoard == false ? " inte " : "")}whiteboard");
                        Console.Write("Vill du ändra rummet angående whiteboard? ja/nej: ");
                        var whiteboard = Console.ReadLine().ToLower();
                        if (whiteboard == "ja") conferenceRoom.WhiteBoard = !conferenceRoom.WhiteBoard;
                        break;
                    case '4':
                        Console.WriteLine($"Rummet har {(conferenceRoom.Projector == false ? " inte " : "")}projektor");
                        Console.Write("Vill du ändra rummet angående projektor? ja/nej: ");
                        var projector = Console.ReadLine().ToLower();
                        if (projector == "ja") conferenceRoom.Projector = !conferenceRoom.Projector;
                        break;
                    case '0':
                        break;
                }
            }
            else
            {
                Console.WriteLine($"Det fanns inget konferensrum på valt Id. Tryck valfri tangent");
                Console.ReadKey(true);
            }
            db.SaveChanges();
        }
        private static void RemoveConferenceRoom()
        {
            using var db = new Booked2Context();

            Console.Clear();
            PrintAllConferenceRooms();
            Console.WriteLine($"Ange id på det konferensrummet du vill ta bort, eller ange [0] om du ångrat dig");
            var roomId = Methods.CheckForInt();
            if (roomId == 0) return;
            var room1 = db.ConferenceRooms.Where(x => x.Id == roomId).Include(x => x.Bookings).FirstOrDefault();
            if (room1 is not null)
            {
                Console.WriteLine($"Är du säker på att du vill radera rummet \"{room1.Name}\" med id {room1.Id}?");
                Console.WriteLine("[J] för ja");
                Console.WriteLine("[N] för nej");
                var answer = Console.ReadKey(true).KeyChar;
                switch (answer)
                {
                    case 'J':
                    case 'j':
                        if (db.Bookings.Where(x => x.ConferenceRoomId == room1.Id && x.PersonId.HasValue).SingleOrDefault() is not null)
                        {
                            Console.WriteLine($"Är du HELT SÄKER då det finns bokningar i valt konferensrum?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var input = Console.ReadKey(true).KeyChar;
                            switch (input)
                            {
                                case 'J':
                                case 'j':
                                    db.ConferenceRooms.Remove(room1);
                                    Console.WriteLine($"{room1.Name} är borttaget");
                                    break;
                                default:
                                    Console.WriteLine($"Du valde att inte ta bort konferensrummet. Tryck valfri tangent");
                                    break;
                            }
                        }
                        else
                        {
                            db.ConferenceRooms.Remove(room1);
                            Console.WriteLine($"{room1.Name} är borttaget");
                        }
                        break;
                    default:
                        Console.WriteLine($"Du valde att inte ta bort konferensrummet. Tryck valfri tangent");
                        break;
                }
            }
            else
            {
                Console.WriteLine($"Det fanns inget konferensrum på valt Id. Tryck valfri tangent");
            }
            db.SaveChanges();
            Console.ReadKey(true);

        }
        private static void AddConferenceRoom()
        {
            using var db = new Booked2Context();
            Console.Clear();
            PrintAllConferenceRooms();
            Console.WriteLine($"Ange namn för att lägga till konferansrum, eller ange [0] om du ångrat dig");
            var roomName = Console.ReadLine();
            if (roomName == "0") return;
            Console.WriteLine($"Hur många platser har \"{roomName}\"?");
            var seats = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Har \"{roomName}\" whiteboard? [j/n]");
            bool whiteBoard = false;
            var answer = Console.ReadLine();
            if (answer == "j" || answer == "J")
                whiteBoard = true;
            Console.WriteLine($"Har \"{roomName}\" projektor? [j/n]");
            bool projector = false;
            var answer2 = Console.ReadLine();
            if (answer2 == "j" || answer2 == "J")
                projector = true;

            db.ConferenceRooms.Add(new ConferenceRoom { Name = roomName, NrOfSeats = seats, WhiteBoard = whiteBoard, Projector = projector });
            db.SaveChanges();

            Console.WriteLine($"Det nya konferansrummet \"{roomName}\" med {seats} antal platser kommer synas redan nu, men kommer få bokningsbara platser" +
                $" först när du skapat ny bokningsbar vecka. Tryck på valfri tangent");
            Console.ReadKey(true);
        }
        internal static void PrintAllConferenceRooms()
        {
            using var db = new Booked2Context();
            Console.WriteLine("Alla bokningsbara konferensrum");
            Console.WriteLine("******************************");
            var allConferenceRooms = db.ConferenceRooms;
            Console.WriteLine($"[Id]\tSittplatser\tWhiteboard\tProjektor\tNamn");
            foreach (var room in allConferenceRooms)
            {
                Console.WriteLine($"[{room.Id}]\t{room.NrOfSeats}\t\t{(room.WhiteBoard == true ? "Ja" : "Nej")}" +
                    $"\t\t{(room.Projector == true ? "Ja" : "Nej")}\t\t{room.Name}");
            }
            Console.WriteLine("-----------------------------");
        }
    }
}
