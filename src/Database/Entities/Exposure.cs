using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Entities
{
    public class Exposure
    {
        public int Id { get; set; }

        public DateTime ValueDate { get; set; }

        public decimal Volume { get; set; }
    }
}
