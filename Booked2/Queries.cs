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
                        Queries.TotalBookings();
                        break;
                    case '2':
                        Queries.MostPopularRoom();
                        break;
                    case '3':
                        // Bokat per vecka. Antal bokningar / antalet möjliga bokningar grupperat på vecka
                        break;
                    case '4':
                        // Bokat över alla bokningsbara veckor antal bokningar / antalet möjliga bokningar totalt
                        break;
                    case '0':
                        loop = false;
                        break;
                }
            }
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

            var p = result
                .GroupBy(x => x.ConferenceRoom)
                .OrderByDescending(x => x.Key.Bookings.Count)
                .FirstOrDefault();

            Console.WriteLine($"Det rummet med flest bokningar är {p.Key.Name} med {p.Key.Bookings.Count} stycken bokningar");
            Console.ReadKey(true);

        }
    }
}
