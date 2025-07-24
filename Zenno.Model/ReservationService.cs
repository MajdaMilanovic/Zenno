using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenno.Model
{
    public class ReservationServiceMapping
    {
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
