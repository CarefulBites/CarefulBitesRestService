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
        public ActionResult GetRandomMeals([FromQuery] int count = 1) {
            
                List<string> ingredients = _manager.GetRandomIngreds(count);
                return Ok(ingredients);
                /*
                Random rand = new Random();

                // Making a loop that runs till successful data is found equal to count
                int index = 0;
                int successEntries = 0;
                List<TempMeal> result = new List<TempMeal>();
                while (index >= count) {
                    if (successEntries > count)
                        break;

                    List<TempMeal>? meals = _manager.GetFood(ingredients[index]);
                    index++;
                    if (meals != null) {
                        successEntries++;
                        int randind = rand.Next(meals.Count-1);
                        result.Add(meals[randind]);
                    }
                }

                if (successEntries >= count) {
                    return Ok(result);
                }
           
            }
            return NoContent();
                */

            }
        }
}
