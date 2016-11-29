using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Data.IO.Tests
{
    public class Message
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
    }
}
