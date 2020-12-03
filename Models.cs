using System.Collections.Generic;

namespace DishChallenge
{
    public class Basket
    {
        public List<Product> products { get; set; }
        public List<Offer> specials { set; get; }
    }

    public class Product
    {
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }

    public class ProductOffer
    {
        public string name { get; set; }
        public int quantity { get; set; }
    }

    public class Offer
    {
        public List<ProductOffer> products { get; set; }
        public int total { get; set; }
    }
}