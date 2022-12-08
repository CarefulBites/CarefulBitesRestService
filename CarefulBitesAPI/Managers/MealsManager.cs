using CarefulBitesAPI.Models;
using CarefulBitesAPI.Retrievers;
using Microsoft.EntityFrameworkCore;

namespace CarefulBitesAPI.Managers
{
    public class MealsManager
    {
        public List<TempMeal> GetFood(string ingredient)
        {
            var food = MealRetriever.GetMealsByIngredientsAsync(ingredient);

            return food.Result;
        }

        public MealById? GetFoodById(string id)
        {
            var foodById = MealRetriever.GetMealsByIdAsync(id);

            return foodById.Result;
        }
    }
}
