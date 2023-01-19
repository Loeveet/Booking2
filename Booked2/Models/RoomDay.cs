using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Booked2.Models
{
    internal class RoomDay
    {
        public string Room { get; set; }
        public string Day { get; set; }
        public int NrOfBookings { get; set; }

        public static List<RoomDay> MostPopularRoomDay()
        {
            var connectionString = "data source=tcp:dbrobindemo.database.windows.net,1433;Initial Catalog=dbDemo;Persist Security Info=False;User ID=robinadmin;Password=Sverige123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var sql = @"select c.Name as 'Room'
		                    ,d.Name as 'Day'
		                    ,Count(c.Name) as 'NrOfBookings'
                            from bookings b
                            join ConferenceRooms c on b.ConferenceRoomId = c.Id
                            join days d on b.DayId = d.Id
                            where b.PersonId is not null
                            group by c.Name, d.Name
                            order by Count(c.Name) desc";
            var orderHistories = new List<RoomDay>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                orderHistories = connection.Query<RoomDay>(sql).ToList();
                connection.Close();
            }
            return orderHistories;
        }
    }
}
