using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DishChallenge
{
    public class ChallengeService
    {
        internal static async Task<int> CalcAsync()
        {
            var content = await ReadDataAsync();

            if (content == null)
                return -3;

            var basket = DeserializeData(content);

            if (basket?.products?.Count <= 0 || basket?.specials?.Count <= 0)
                return -2;

            //Assuming 'products' is showing available products, their remaining quantity and theri price, an inventory is formed here.
            var inventory = new List<ProductInventory>();
            basket.products.ForEach(x => inventory.Add(new ProductInventory { Name = x.name, Current = x.quantity, Price = x.price }));

            //Any record in the `specials` is considered as a partial order.
            //remaining quantities from inventory are considered to see if they can reach min. quantity or not
            var candidOrders = CalcCandidOrders(inventory, basket);

            //now that every possible order is calculated, find the minimum one
            var finals = candidOrders.OrderBy(x => x.TotalPrice).ToList();

            if (finals.Count <= 0)
                return -1;
            else
                return finals[0].TotalPrice;
        }

        private static List<Order> CalcCandidOrders(List<ProductInventory> inventory, Basket basket)
        {
            var orders = new List<Order>();
            var candidOrders = new List<Order>();

            foreach (var offer in basket.specials)
            {
                //Deep copy
                //Each order is processed based on the initial inventory
                var tempInventory = new List<ProductInventory>();
                inventory.ForEach(x => tempInventory.Add(new ProductInventory { Current = x.Current, Price = x.Price, Name = x.Name }));

                var order = CalcOrder(offer.products, basket.products, tempInventory);

                if (order == null)
                    continue;

                order.MinQuantity = offer.total;

                if (order.TotalQuantity >= order.MinQuantity)
                {
                    candidOrders.Add(order);
                    continue;
                }

                var gap = order.MinQuantity - order.TotalQuantity;

                while (gap > 0)
                {
                    var sortedByPrice = tempInventory.Where(x => x.Current > 0).OrderBy(x => x.Price).ToList();

                    if (sortedByPrice.Count <= 0)
                        break;

                    var candid = sortedByPrice[0];
                    var newOrderQuantity = 0;

                    if (gap > candid.Current)
                    {
                        newOrderQuantity = candid.Current;
                        gap -= newOrderQuantity;
                    }
                    else
                    {
                        newOrderQuantity = candid.Current - gap;
                        gap = 0;
                    }

                    candid.Current -= newOrderQuantity;
                    order.TotalPrice += newOrderQuantity * candid.Price;
                }

                if (gap <= 0)
                    candidOrders.Add(order);
            }

            return candidOrders;
        }

        private static Basket DeserializeData(string content)
        {
            try
            {
                var basket = JsonSerializer.Deserialize<Basket>(content);
                return basket;
            }
            catch (Exception exception)
            {
                throw new Exception("Error Deserializing data", exception);
            }
        }

        private async static Task<string> ReadDataAsync()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("https://tech-api.homedish.co.nz/products");
                var content = await response.Content.ReadAsStringAsync();

                if (response.Content != null)
                    response.Content.Dispose();

                return content;
            }
            catch (Exception exception)
            {
                throw new Exception("Error reading data from input", exception);
            }
        }

        internal static Order CalcOrder(List<ProductOffer> offerProducts, List<Product> basketProducts, List<ProductInventory> inventory)
        {
            var totalPrice = 0;
            var totalQuantity = 0;

            foreach (var offerProduct in offerProducts)
            {
                var originalProduct = basketProducts.SingleOrDefault(x => x.name == offerProduct.name);

                if (originalProduct == null)
                    return null;

                var originalProductInventory = inventory.Single(x => x.Name == originalProduct.name);

                if (originalProductInventory.Current < offerProduct.quantity)
                    return null;

                originalProductInventory.Current -= offerProduct.quantity;
                totalPrice += offerProduct.quantity * originalProduct.price;
                totalQuantity += offerProduct.quantity;
            }

            return new Order { TotalQuantity = totalQuantity, TotalPrice = totalPrice };
        }
    }
}