using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booked2.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public int WeekNumber { get; set; }
        public int ConferenceRoomId { get; set; }
        public int DayId { get; set; }
        public int? PersonId { get; set; } = null;

        public virtual Day Day { get; set; }
        public virtual ConferenceRoom ConferenceRoom { get; set; }
        public virtual Person Person { get; set; }
        internal static void HandleWeek()
        {
            using var db = new Booked2Context();

            var loop = true;
            while (loop)
            {
                Console.Clear();
                Methods.PrintAvailableWeek();
                Console.WriteLine("1. Lägga till vecka");
                Console.WriteLine("2. Ta bort vecka");
                Console.WriteLine("0. Backa");
                var choise = Console.ReadKey(true).KeyChar;
                switch (choise)
                {
                    case '1':
                        CreateNewWeek();
                        break;
                    case '2':
                        RemoveWeek();
                        break;
                    case '0':
                        loop = false;
                        break;

                }
            }
        }
        private static void RemoveWeek()
        {
            using var db = new Booked2Context();

            var loop = true;
            while (loop)
            {
                Console.Clear();
                Methods.PrintAvailableWeek();

                Console.WriteLine($"Ange veckonummer för den veckan du vill ta bort, eller ange [0] om du ångrat dig");
                var id = Methods.CheckForInt();

                if (id == 0)
                {
                    loop = false;
                    break;
                }
                var week = db.Bookings.Where(x => x.WeekNumber == id).ToList();
                if (week is not null)
                {

                    Console.WriteLine($"Är du HELT säker på att du vill radera vecka {id} då även redan bokade dagar försvinner.");
                    Console.WriteLine("[J] för ja");
                    Console.WriteLine("[N] för nej");
                    var answer = Console.ReadKey(true).KeyChar;
                    switch (answer)
                    {
                        case 'J':
                        case 'j':
                            Console.WriteLine("VARNING! Är du verkligen HELT säker?");
                            Console.WriteLine("[J] för ja");
                            Console.WriteLine("[N] för nej");
                            var remove = Console.ReadKey(true).KeyChar;
                            switch (remove)
                            {
                                case 'J':
                                case 'j':
                                    foreach (var w in week)
                                    {
                                        db.Bookings.Remove(w);
                                    }
                                    db.SaveChanges();
                                    Console.WriteLine($"Vecka {id} är borttagen från bokningssystemet. Tryck på valfri tangent");
                                    loop = false;
                                    break;
                                default:
                                    Console.WriteLine($"Du valde att inte ta bort veckan. Tryck på valfri tangent");
                                    loop = false;
                                    break;
                            }
                            break;
                        case 'N':
                        case 'n':
                            Console.WriteLine($"Du valde att inte ta bort veckan. Tryck på valfri tangent");
                            loop = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Veckonumret du valt finns ej. Tryck valfri tangent");
                }
                Console.ReadKey(true);
            }
        }
        private static void CreateNewWeek()
        {
            using var db = new Booked2Context();

            Console.WriteLine($"Är du säker på att du vill lägga till ny bokningsbar vecka?");
            Console.WriteLine("[J] för ja");
            Console.WriteLine("[N] för nej");
            var answer = Console.ReadKey(true).KeyChar;
            switch (answer)
            {
                case 'J':
                case 'j':
                    int lastWeek = 0;
                    int newWeek;
                    if (db.Bookings.Any()) lastWeek = db.Bookings.Max(x => x.WeekNumber);
                    if (lastWeek == 52) newWeek = 1;
                    else newWeek = lastWeek + 1;
                    var rooms = db.ConferenceRooms;
                    foreach (var room in rooms)
                    {
                        var dbb = new Booked2Context();
                        var days = dbb.Days;
                        foreach (var day in days)
                        {
                            db.Bookings.Add(new Booking { ConferenceRoomId = room.Id, DayId = day.Id, WeekNumber = newWeek });
                        }
                    }
                    Console.WriteLine($"Ny bokningsbar vecka med veckonummer {newWeek} är skapat. Tryck på valfri tangent");
                    break;
                case 'N':
                case 'n':
                    Console.WriteLine($"Du valde att inte lägga till ny vecka. Tryck på valfri tangent");
                    break;
            }
            db.SaveChanges();
            Console.ReadKey();
        }
        internal static void HandleBooking()
        {
            using var db = new Booked2Context();

            var loop = true;
            while (loop)
            {
                Console.Clear();
                PrintAllBookings();
                Console.WriteLine("1. Ändra en bokning");
                Console.WriteLine("2. Ta bort bokning");
                Console.WriteLine("0. Backa");
                var choise = Console.ReadKey(true).KeyChar;
                switch (choise)
                {
                    case '1':
                        ChangeBookedPerson();
                        break;
                    case '2':
                        RemoveBooking();
                        break;
                    case '0':
                        loop = false;
                        break;

                }
            }
        }
        private static void ChangeBookedPerson()
        {
            using var db = new Booked2Context();

            Console.Clear();
            PrintAllBookings();
            Console.WriteLine($"Ange bokningsId för den bokningen du ändra namnet på, eller ange [0] om du ångrat dig");
            var id = Methods.CheckForInt();
            if (id == 0) return;
            var bookedPersonId = db.Bookings
                .Where(x => x.Id == id && x.PersonId != null)
                .Select(x => x.PersonId)
                .SingleOrDefault();

            //var p = db.Persons
            //    .Include(x => x.Bookings)
            //    .Where(x => x.Id == bookedPersonId)
            //    .SingleOrDefault();

            var x = db.Bookings
                .Include(x => x.Person)
                .Where(x => x.Id == id)
                .SingleOrDefault();

            if (bookedPersonId is not null)
            {
                Console.WriteLine($"Är du helt säker på att du vill byta signatur på bokningen?");
                Console.WriteLine("[J] för ja");
                Console.WriteLine("[N] för nej");
                var answer = Console.ReadKey(true).KeyChar;
                switch (answer)
                {
                    case 'J':
                    case 'j':
                        Console.WriteLine("Ange nytt namn för bokningen");
                        Person person = new Person { Name = Console.ReadLine()};
                        db.Add(person);
                        db.SaveChanges();
                        var person1 = db.Persons
                            .Where(x => x.Name == person.Name)
                            .SingleOrDefault();
                        x.PersonId = person1.Id;
                        //p.Name = Console.ReadLine();
                        Console.WriteLine("Signaturen är ändrat. Tryck på valfri tangent");
                        break;
                    default:
                        Console.WriteLine("Du valde att inte ändra namnet. Tryck på valfri tangent");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Du valde ett obokat Id. Tryck på valfri tangent");
            }

            db.SaveChanges();
            Console.ReadKey(true);
        }
        private static void RemoveBooking()
        {
            using var db = new Booked2Context();

            Console.Clear();
            PrintAllBookings();
            Console.WriteLine($"Ange bokningsId för den bokningen du vill ta bort, eller ange [0] om du ångrat dig");
            var id = Methods.CheckForInt();
            if (id == 0) return;
            var bookedPersonId = db.Bookings
                .Where(x => x.Id == id && x.PersonId != null)
                .Select(x => x.PersonId)
                .SingleOrDefault();

            if (bookedPersonId is not null)
            {
                var b = db.Persons
                    .Where(x => x.Id == bookedPersonId)
                    .SingleOrDefault();

                Console.WriteLine($"Är du helt säker på att du vill radera {b.Name}'s bokning?");
                Console.WriteLine("[J] för ja");
                Console.WriteLine("[N] för nej");
                var answer = Console.ReadKey(true).KeyChar;
                switch (answer)
                {
                    case 'J':
                    case 'j':
                        var removeBooking = db.Bookings.Where(x => x.PersonId == bookedPersonId && x.Id == id).SingleOrDefault();
                        removeBooking.PersonId = null;
                        db.SaveChanges();
                        Console.WriteLine("Bokningen är borttagen. Tryck på valfri tangent");
                        break;
                    default:
                        Console.WriteLine("Du valde att inte ta bort bokningen. Tryck på valfri tangent");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Du valde ett obokat Id. Tryck på valfri tangent");
            }

            Console.ReadKey(true);

        }
        internal static void PrintAllBookings()
        {
            using var db = new Booked2Context();

            var allBookings = db.Bookings.ToList();
            var allDays = db.Days.ToList();
            var allRooms = db.ConferenceRooms.ToList();
            var allPersons = db.Persons.ToList();

            var result = from b in allBookings
                         where b.PersonId != null
                         orderby b.Id
                         select new
                         {
                             WeekNumber = b.WeekNumber,
                             ConfRoomName = b.ConferenceRoom.Name,
                             PersonName = b.Person.Name,
                             BookingId = b.Id,
                             DayName = b.Day.Name,
                             DayId = b.DayId,
                             ConfRoomId = b.ConferenceRoomId
                         };

            var bookings = result
                .OrderBy(x => x.DayId)
                .GroupBy(x => x.WeekNumber);


            Console.WriteLine("Aktuella bokningar");
            Console.WriteLine("**********************");
            foreach (var group in bookings.OrderBy(x => x.Key))
            {
                Console.WriteLine("Vecka " + group.Key);
                foreach (var x in group.GroupBy(x => x.DayName))
                {
                    Console.WriteLine($"\t{x.Key}");
                    foreach (var y in x)

                        Console.WriteLine($"\t\t{y.PersonName} har bokat {y.ConfRoomName}. BokningsId [{y.BookingId}]");
                }
            }
            Console.WriteLine("----------------------");
        }
    }
}
