using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booked2.Models
{
    public partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<Booking> Bookings { get; set; }
        public virtual Booking Booking { get; set; }

        internal static void BookRoom()
        {
            using var db = new Booked2Context();

            var loop = true;
            int chosenRoom = 0;
            int selectedBookingId = 0;
            int week = 0;
            while (loop)
            {
                var loop1 = true;
                while (loop1)
                {
                    Console.Clear();
                    week = Methods.PrintAllPlaces();

                    Console.WriteLine("\n======================================\n");

                    var allRooms = from r in db.ConferenceRooms
                                   join w in db.Bookings on r.Id equals w.ConferenceRoomId
                                   select r;

                    Console.WriteLine($"[Id]\tSittplatser\tWhiteboard\tProjektor\tNamn");

                    foreach (var room in allRooms.Distinct())
                    {
                        Console.WriteLine($"[{room.Id}]\t{room.NrOfSeats}\t\t{(room.WhiteBoard == true ? "Ja" : "Nej")}" +
                        $"\t\t{(room.Projector == true ? "Ja" : "Nej")}\t\t{room.Name}");
                    }
                    Console.WriteLine("\nAnge rumsId för det rum du vill boka, eller välj [0] om du ångrat dig");
                    chosenRoom = Methods.CheckForInt();
                    if (chosenRoom != 0)
                    {
                        if ((db.ConferenceRooms
                            .Where(x => x.Id == chosenRoom)
                            .SingleOrDefault()) != null)
                        {
                            loop1 = false;
                        }
                        else
                        {
                            Console.WriteLine("Valt rumsId existerar ej. Tryck på valfri tangent");
                            Console.ReadKey(true);
                        }
                    }

                }

                Console.WriteLine("\n======================================\n");
                var loop2 = true;
                while (loop2)
                {
                    Console.Clear();
                    foreach (var x in db.ConferenceRooms.Include(x => x.Bookings).Where(x => x.Id == chosenRoom))
                    {
                        Console.WriteLine($"Rum {x.Name} vecka {week} ");
                        foreach (var y in x.Bookings.Where(x => x.WeekNumber == week))
                        {
                            using var dbb = new Booked2Context();
                            var day = dbb.Days
                                .Where(x => x.Id == y.DayId)
                                .Select(x => x.Name)
                                .SingleOrDefault();
                            Console.WriteLine("\t" + (y.PersonId is null == true ? (day + " - BokningsId [" + y.Id + "]") : (day + " - Bokat" + "")));
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine("\nAnge bokningsId för det rum du vill boka, eller välj [0] om du vill börja om bokningen ");
                    selectedBookingId = Methods.CheckForInt();
                    if (selectedBookingId != 0)
                    {
                        var booking = db.Bookings
                            .Where(x => x.Id == selectedBookingId && x.PersonId == null && x.WeekNumber == week && x.ConferenceRoomId == chosenRoom)
                            .SingleOrDefault();

                        if (booking is not null)
                        {
                            loop = false;
                            loop2 = false;
                        }
                        else
                        {
                            Console.WriteLine($"Valt rum är antingen bokat eller tillhör ej valt rum på vald vecka. Tryck på valfri tangent");
                            Console.ReadKey(true);
                        }

                    }
                    else
                    {
                        loop2 = false;
                    }
                }
            }
            Console.WriteLine("\n======================================\n");
            Console.WriteLine("Ange signatur att boka rum med. Max 14 tecken.");
            var name = CheckNameForMaxLength();
            db.Persons.Add(new Person { Name = name });
            db.SaveChanges();

            var bookingRoom = db.Bookings
                .Include(x => x.ConferenceRoom)
                .Include(x => x.Day)
                .Where(x => x.Id == selectedBookingId)
                .SingleOrDefault();

            var bookingPersonId = db.Persons
                .Where(x => x.Name == name)
                .FirstOrDefault();

            bookingRoom.PersonId = bookingPersonId.Id;
            Console.WriteLine($"Signaturen {bookingPersonId.Name} är bokad i {bookingRoom.ConferenceRoom.Name} på {bookingRoom.Day.Name.ToString().ToLower()}en vecka {bookingRoom.WeekNumber}");
            db.SaveChanges();
            Console.ReadKey(true);

        }
        private static string CheckNameForMaxLength()
        {
            var input = Console.ReadLine();
            while (input.Length > 14)
            {
                Console.WriteLine("Din valda signatur är för lång. Välj en kortare. ");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
