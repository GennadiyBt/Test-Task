using System;


namespace Test_Task.Models
{
    internal class RackProduct
    {
        public int RackId { get; set; }
        public Rack Rack { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Status { get; set; }
    }
}
