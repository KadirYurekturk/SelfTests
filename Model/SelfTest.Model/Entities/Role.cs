using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfTest.Model.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
