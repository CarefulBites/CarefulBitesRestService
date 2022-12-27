using CarefulBitesAPI.Managers;
using CarefulBitesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarefulBitesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class MealController : Controller {
        private readonly MealsManager _manager = new();

        // GET: MealController
        [HttpGet]
        public ActionResult GetMeals([FromQuery] List<string> ingredient)
        {
            var ingredients = string.Join(",", ingredient);
            var meals = _manager.GetFood(ingredients);

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

        [HttpGet("RandomMeals")]
        public ActionResult GetRandomMeals([FromQuery] int amountOfMeals)
        {
            var meals = _manager.GetRandomMeals(amountOfMeals);

            if (meals != null && meals.Any())
                return Ok(meals);

            return NoContent();
                */

            }
        }
}
