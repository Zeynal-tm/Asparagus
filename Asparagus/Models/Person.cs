using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asparagus.Models
{
    public class Person : Entity
    {
        public string Name { get; set; }
        List<Entity> entities = new List<Entity>();
         
    }
}
