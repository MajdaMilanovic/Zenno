﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenno.Model
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }

        public ICollection<ReservationServiceMapping> ReservationServices { get; set; } = new List<ReservationServiceMapping>();
    }
}
