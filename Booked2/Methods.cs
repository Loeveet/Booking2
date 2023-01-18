using Booked2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Booked2
{
    internal class Methods
    {
        public static void BookingSystem()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

                Console.WriteLine("1. Admin");
                Console.WriteLine("2. Boka rum");
                Console.WriteLine("3. Queries");
                Console.WriteLine("0. Lämna bokningssystemet");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        Admin();
                        break;
                    case '2':
                        Person.BookRoom();
                        break;
                    case '3':
                        Queries.HandleQueries();
                        break;
                    case '0':
                        loop = false;
                        break;
                }
            }
        }
        private static void Admin()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

                Console.WriteLine("1. Hantera veckor");
                Console.WriteLine("2. Hantera konferansrum");
                Console.WriteLine("3. Hantera bokningar");
                Console.WriteLine("0. Backa menyn");

                var input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        Booking.HandleWeek();  // klart
                        break;
                    case '2':
                        ConferenceRoom.HandleConferenceRoom();  // klart
                        break;
                    case '3':
                        Booking.HandleBooking();  // klart
                        break;
                    case '0':
                        loop = false;
                        break;
                    default:
                        break;
                }
            }

        }
        internal static int PrintAllPlaces()
        {
            using var db = new Booked2Context();

            int week = 0;
            var loop = true;
            while (loop)
            {
                Console.Clear();
                PrintAvailableWeek();
                Console.WriteLine("Välj vecka ");
                week = CheckForInt();
                if (week >= db.Bookings.Min(x => x.WeekNumber) && week <= db.Bookings.Max(x => x.WeekNumber))
                {
                    var conferenceRooms = from r in db.ConferenceRooms
                                          join b in db.Bookings on r.Id equals b.ConferenceRoomId
                                          where b.WeekNumber == week
                                          select r;

                    var availableRooms = conferenceRooms.Distinct();


                    foreach (var r in availableRooms)
                    {
                        Console.Write($"\t{r.Name} {(r.Name.Length > 6 ? "\t" : "\t\t")}");
                    }
                    Console.WriteLine();
                    foreach (var x in db.Days.Include(x => x.Bookings))
                    {
                        Console.WriteLine();
                        Console.Write(x.Name);
                        foreach (var y in x.Bookings.Where(x => x.WeekNumber == week))
                        {
                            using var dbb = new Booked2Context();
                            var name = (from n in dbb.Persons
                                        where n.Id == y.PersonId
                                        select n.Name).SingleOrDefault();

                            Console.Write($"\t{(y.PersonId is not null == true ? name : "------")} {(y.PersonId is not null && name.Length > 6 ? "\t" : "\t\t")}");
                        }
                        Console.WriteLine();
                    }
                    loop = false;
                }
                else
                {
                    Console.WriteLine("Valt veckonummer är inte valbart. Tryck på valfri tangent");
                    Console.ReadKey(true);
                }
            }
            return week;
        }
        internal static void PrintAvailableWeek()
        {
            using var db = new Booked2Context();

            var weeks = db.Bookings
                .GroupBy(x => x.WeekNumber)
                .Select(x => x.Key);
            Console.WriteLine("Bokningsbara veckor");
            Console.WriteLine("**********************");
            foreach (var week in weeks)
            {
                Console.WriteLine($"Vecka [{week}]");
            }
            Console.WriteLine("----------------------");
        }

        internal static int CheckForInt()
        {
            int id;
            var input = Console.ReadLine();
            while (!int.TryParse(input, out id))
            {
                Console.WriteLine("Felaktig inmatning, försök igen");
                input = Console.ReadLine();
            }
            return id;
        }

    }
}
