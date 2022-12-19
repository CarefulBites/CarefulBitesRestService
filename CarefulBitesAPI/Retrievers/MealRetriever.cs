using CarefulBitesAPI.Models;
using Newtonsoft.Json;

namespace CarefulBitesAPI.Retrievers {
    public class MealRetriever {
        private static readonly HttpClient Client = new();
        private const string Path = "https://www.themealdb.com/api/json/v1/1/";
        private const string PathV2 = "https://www.themealdb.com/api/json/v2/9973533/";

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
            var jsonStringTrimmed = jsonString.Replace("null", "\"\"");
            var deserializedResponse = JsonConvert.DeserializeObject<MealByIdList>(jsonStringTrimmed);

            if (deserializedResponse?.Meals != null) return deserializedResponse.Meals.FirstOrDefault();

            return null;
        }

        public static async Task<List<MealIngredient>?> GetIngredientsAsync() {
            var response = await Client.GetAsync(Path + $"list.php?i=list");
            MealIngredientList? mealIngredient = null;

            if (response.IsSuccessStatusCode) {
                mealIngredient = await response.Content.ReadFromJsonAsync<MealIngredientList>();
            }

            var mealList = new List<MealIngredient>();
            if (mealIngredient != null)
                if (mealIngredient.Ingredients != null)
                    foreach (var tm in mealIngredient.Ingredients) {
                        mealList.Add(tm);
                    }

            return mealList;
        }

        public static async Task<List<TempMeal>?> GetRandomMealsAsync(int amountOfMeals)
        {
            if (amountOfMeals > 10 || amountOfMeals < 0)
            {
                amountOfMeals = 10;
            }
            var response = await Client.GetAsync(PathV2 + "randomselection.php");
            TempMealList? tempMealList = null;

            if (!response.IsSuccessStatusCode) return null;
            tempMealList = await response.Content.ReadFromJsonAsync<TempMealList>();
            

            var mealList = new List<TempMeal>();
            if (tempMealList == null) return mealList;
            if (tempMealList.Meals == null) return mealList;
            for (var index = 0; index < amountOfMeals; index++)
            {
                var tm = tempMealList.Meals[index];
                mealList.Add(tm);
            }
            return mealList;
        }
    }
}
