﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Test_Task.Models
{
    internal class OrderProduct
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public  int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
