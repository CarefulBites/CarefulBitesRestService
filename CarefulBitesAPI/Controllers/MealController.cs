using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.Mvc;
using CarefulBitesAPI.Models;

namespace CarefulBitesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class MealController : Controller {
        private readonly MealsManager _manager = new MealsManager();

        // GET: MealController
        [HttpGet]
        public ActionResult GetMeals([FromQuery] string ingredient) {
            var meals = _manager.GetFood(ingredient);

            if (meals != null && meals.Any())
                return Ok(meals);

            return NoContent();
        }

        [HttpGet("foodById")]
        public ActionResult GetMealsById([FromQuery] string id) {
            var mealsById = _manager.GetFoodById(id);
            if (mealsById?.IdMeal != null) {
                return Ok(mealsById);
            }
            return NoContent();
        }

        [HttpGet("RandomMeals")]  // Pretty sure it has a bug or 2 but might work
        public ActionResult GetRandomMeals([FromQuery] int? count = null) {
            if (count < 1)
                count = 1;

            if (count != null) {
                List<string> ingredients = _manager.GetRandomIngreds(count.Value + 1);
                Random rand = new Random();

                // Making a loop that runs till successful data is found equal to count
                int test = 0;
                int successEntries = 0;
                List<TempMeal> result = new List<TempMeal>();
                while (test >= count) {
                    if (successEntries > count)
                        break;

                    List<TempMeal>? meals = _manager.GetFood(ingredients[test]);
                    test++;
                    if (meals != null) {
                        successEntries++;
                        int index = rand.Next(meals.Count);
                        result.Add(meals[index]);
                    }
                }

                if (successEntries >= count) {
                    return Ok(result);
                }
            }

            return NoContent();
        }
    }
}
