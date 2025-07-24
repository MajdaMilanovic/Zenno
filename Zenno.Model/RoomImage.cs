using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenno.Model
{
    public class RoomImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!; // Navigation property to Room
    }
}
