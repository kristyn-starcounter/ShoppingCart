using Starcounter;
using System.Collections.Generic;

namespace ShoppingCart
{
    [Database]
    public class Customer
    {
        public string Key => this.GetObjectID();
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}