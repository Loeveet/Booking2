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
                Console.WriteLine("2. Mest bokningar");
                Console.WriteLine("3. Procent bokat per vecka");
                Console.WriteLine("4. Procent bokat över alla bokningsbara veckor");
                Console.WriteLine("0. Backa");

                var input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        TotalBookings();
                        break;
                    case '2':
                        MostPopular();
                        break;
                    case '3':
                        TotalBookingsPerWeekPercentage();
                        break;
                    case '4':
                        TotalBookingsPercentage();
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
            Console.WriteLine("Andel bokade rum i procent per vecka");
            Console.WriteLine("------------------------------------");
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
            Console.WriteLine("Andel bokade rum i procent");
            Console.WriteLine("--------------------------");
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
            Console.WriteLine("Antal bokningar");
            Console.WriteLine("---------------");
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
                .ToList()
                .GroupBy(x => x.ConferenceRoom)
                .OrderByDescending(x => x.Key.Bookings.Count);
            Console.WriteLine("Populäraste rum att boka");
            Console.WriteLine("------------------------");
            foreach (var room in result)
            {

                Console.WriteLine($"{room.Key.Name} har {(room.Key.Bookings.Count == 1 ? room.Key.Bookings.Count + " bokning" : room.Key.Bookings.Count + " stycken bokningar")}");

            }
            Console.ReadKey(true);

        }
        private static void MostPopularDay()
        {
            using var db = new Booked2Context();
            Console.Clear();
            var result = db.Bookings
                .Where(x => x.PersonId != null)
                .Include(x => x.Day)
                .ToList()
                .GroupBy(x => x.Day)
                .OrderByDescending(x => x.Key.Bookings.Count);
            Console.WriteLine("Populäraste dag att boka");
            Console.WriteLine("------------------------");
            foreach (var room in result)
            {

                Console.WriteLine($"{room.Key.Name} har {(room.Key.Bookings.Count == 1 ? room.Key.Bookings.Count + " bokning" : room.Key.Bookings.Count + " stycken bokningar")}");

            }
            Console.ReadKey(true);

        }
        private static void MostPopularRoomDay()
        {
            Console.Clear();
            var xxx = RoomDay.MostPopularRoomDay();
            Console.WriteLine("Populäraste dag per rum");
            Console.WriteLine("-----------------------");
            foreach(var x in xxx) 
            {
                Console.WriteLine($"{x.NrOfBookings} - {x.Room}\t{x.Day}");
            }

            Console.ReadKey(true);
        }
        private static void MostPopular()
        {
            var loop = true;
            while (loop)
            {
                Console.Clear();

                Console.WriteLine("1. Mest bokade konferensrum");
                Console.WriteLine("2. Mest bokade dag");
                Console.WriteLine("3. Mest bokade konferenserum/dag");
                Console.WriteLine("0. Backa");


                var input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case '1':
                        MostPopularRoom();
                        break;
                    case '2':
                        MostPopularDay();
                        break;
                    case '3':
                        MostPopularRoomDay();
                        break;
                    case '0':
                        loop = false;
                        break;
                }
            }
        }

    }
}
