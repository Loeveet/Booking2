using Booked2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booked2
{
    internal class Queries
    {
        internal static void HandleQueries()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

                Console.WriteLine("1. Totalt antal aktiva bokningar");
                Console.WriteLine("2. Mest bokade konferensrummet");
                Console.WriteLine("3. Procent bokat per vecka");
                Console.WriteLine("4. Procent bokat över alla bokningsbara veckor");
                Console.WriteLine("0. Backa");

                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        TotalBookings();
                        break;
                    case '2':
                        MostPopularRoom();
                        break;
                    case '3':
                        TotalBookingsPercentage();
                        break;
                    case '4':
                        TotalBookingsPerWeekPercentage();
                        // Bokat per vecka. Antal bokningar / antalet möjliga bokningar grupperat på vecka
                        break;
                    case '0':
                        loop = false;
                        break;
                }
            }
        }
        private static void TotalBookingsPerWeekPercentage()
        {
            using var db = new Booked2Context();
            Console.Clear();
            var week = db.Bookings
                .ToList()
                .GroupBy(x => x.WeekNumber);

            foreach (var z in week)
            {
                Console.WriteLine($"Vecka {z.Key} är {((Convert.ToDouble(z.Where(x => x.PersonId != null).Count()) / z.Count() * 100)).ToString("0.##")} procent bokat");


            }
            Console.ReadKey();
        }

        private static void TotalBookingsPercentage()
        {
            using var db = new Booked2Context();
            Console.Clear();
            double percentBooked = (Convert.ToDouble(db.Bookings.Where(x => x.PersonId != null).Count()) / Convert.ToDouble(db.Bookings.Count())) * 100;
            Console.WriteLine($"Just nu är {percentBooked.ToString("0.##")} procent av de bokningsbara objekten bokade");
            Console.ReadKey(true);
        }
        private static void TotalBookings()
        {
            using var db = new Booked2Context();
            Console.Clear();
            var totalBookings = db.Bookings
                .Where(x => x.PersonId != null)
                .Count();
            Console.WriteLine($"Det finns för tillfället {totalBookings} aktiva bokningar");
            Console.ReadKey(true);
        }

        private static void MostPopularRoom()
        {
            using var db = new Booked2Context();
            Console.Clear();
            var result = db.Bookings
                .Where(x => x.PersonId != null)
                .Include(x => x.ConferenceRoom)
                .ToList();

            var group = result
                .GroupBy(x => x.ConferenceRoom)
                .OrderByDescending(x => x.Key.Bookings.Count);
            foreach (var room in group)
            {

                Console.WriteLine($"{room.Key.Name} har {(room.Key.Bookings.Count == 1 ? room.Key.Bookings.Count + " bokning" : room.Key.Bookings.Count + " stycken bokningar")}");

            }
            Console.ReadKey(true);

        }
    }
}
