using Booked2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace Booked2
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Methods.BookingSystem();


        }
    }
}
