using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DishChallenge
{
    public class ChallengeService
    {
        internal static async Task<int> CalcAsync()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://tech-api.homedish.co.nz/products");
            var content = await response.Content.ReadAsStringAsync();

            if (response.Content != null)
                response.Content.Dispose();

            var basket = JsonSerializer.Deserialize<Basket>(content);

            return -1;
        }
    }
}