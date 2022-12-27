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

        public List<TempMeal>? GetRandomMeals(int amountOfMeals)
        {
            var food = MealRetriever.GetRandomMealsAsync(amountOfMeals);

            return food.Result;
        }
    }
}
