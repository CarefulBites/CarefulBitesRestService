using CarefulBitesAPI.Models;
using CarefulBitesAPI.Retrievers;

namespace CarefulBitesAPI.Managers {
    public class MealsManager {
        public List<TempMeal>? GetFood(string ingredient) {
            var food = MealRetriever.GetMealsByIngredientsAsync(ingredient);

            return food.Result;
        }

        public MealById? GetFoodById(string id) {
            var foodById = MealRetriever.GetMealsByIdAsync(id);

            return foodById.Result;
        }

        public List<MealIngredient>? GetIngredients() {
            var ingredientById = MealRetriever.GetIngredientsAsync();
            return ingredientById.Result;
        }

        public List<string> GetRandomIngredients(int count) {
            List<string> ingredientList = new List<string>();

            var ingredientById = MealRetriever.GetIngredientsAsync();
            List<MealIngredient>? ingById = ingredientById.Result;

            if (ingById == null) return ingredientList;

            Random rand = new Random();

            for (int i = 0; i < count; i++) {
                int index = rand.Next(ingById.Count);
                string? str = ingById[index].strIngredient;
                if (str != null)
                    ingredientList.Add(str);

                ingById.RemoveAt(index);
            }

            return ingredientList;
        }
    }
}
