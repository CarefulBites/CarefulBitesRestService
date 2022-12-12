using CarefulBitesAPI.Models;
using Newtonsoft.Json;

namespace CarefulBitesAPI.Retrievers {
    public class MealRetriever {
        private static readonly HttpClient Client = new();
        private const string Path = "https://www.themealdb.com/api/json/v1/1/";

        public static async Task<List<TempMeal>?> GetMealsByIngredientsAsync(string ingredient) {
            var response = await Client.GetAsync(Path + $"filter.php?i={ingredient}");
            TempMealList? tempMealList = null;

            if (response.IsSuccessStatusCode) {
                tempMealList = await response.Content.ReadFromJsonAsync<TempMealList>();
            }

            var mealList = new List<TempMeal>();
            if (tempMealList != null)
                if (tempMealList.Meals != null)
                    foreach (var tm in tempMealList.Meals) {
                        mealList.Add(tm);
                    }

            return mealList;
        }

        public static async Task<MealById?> GetMealsByIdAsync(string id) {
            var response = await Client.GetAsync(Path + $"lookup.php?i={id}");

            if (!response.IsSuccessStatusCode) return null;

            var jsonString = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<MealByIdList>(jsonString);

            if (deserializedResponse?.Meals != null) return deserializedResponse.Meals.FirstOrDefault();

            return null;
        }
    }
}
