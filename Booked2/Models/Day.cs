using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booked2.Models
{
    public partial class Day
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
