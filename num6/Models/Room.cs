using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace num6.Models
{
    public class Room
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Matrix { get; set; }
        public string SecondPlayer { get; set; }
        public bool Status { get; set; }
    }
}
