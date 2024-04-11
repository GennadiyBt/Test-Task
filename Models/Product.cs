using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test_Task.Models
{
    internal class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        public ICollection<RackProduct> RackProducts { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
