using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenuoliktaPaskaita.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Condition { get; set; }
        public decimal Price { get; set; }
        public int InStock { get; set; }
    }
}
