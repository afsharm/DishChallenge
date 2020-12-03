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

    public class ProductInventory
    {
        public string Name { set; get; }
        public int Current { set; get; }
        public int Price { get; set; }
    }

    public class Order
    {
        public int TotalQuantity { set; get; }
        public int TotalPrice { set; get; }
        public int MinQuantity { set; get; }
    }
}