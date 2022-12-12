using CarefulBitesAPI.Models;
using CarefulBitesAPI.Retrievers;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarefulBitesAPI.Managers
{
    public class MealsManager
    {
        public List<TempMeal>? GetFood(string ingredient)
        {
            var food = MealRetriever.GetMealsByIngredientsAsync(ingredient);

            return food.Result;
        }

        public MealById? GetFoodById(string id)
        {
            var foodById = MealRetriever.GetMealsByIdAsync(id);

            return foodById.Result;
        }
        public List<MealIngredient>? GetIngredients()
        {
            var ingredientById = MealRetriever.GetIngredientsAsync();
            return ingredientById.Result;
        }
        public List<string> GetRandomIngreds(int count)
        {
            List<string> ingrilist = new List<string>();

            var ingredientById = MealRetriever.GetIngredientsAsync();
            List<MealIngredient>? ingByid = ingredientById.Result;

            if (ingByid == null) return ingrilist;
      

            Random rand = new Random();
       
            for (int i = 0; i < count; i++)
            {
                int index = (int)rand.Next(ingByid.Count);
                string? str = ingByid[index].strIngredient;
                if (str != null)
                    ingrilist.Add(str);

                ingByid.RemoveAt(index);
            }

            return ingrilist;
        }
    }
}
