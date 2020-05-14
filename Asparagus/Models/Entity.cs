using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asparagus.Models
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public int CountOfAsparagus { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string Email { get; set; }
    }
}
