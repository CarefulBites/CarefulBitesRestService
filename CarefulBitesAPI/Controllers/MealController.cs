using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.Mvc;

namespace CarefulBitesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MealController : Controller
    {
        private readonly MealsManager _manager = new MealsManager();
        // GET: MealController
        [HttpGet]
        public ActionResult GetMeals([FromQuery] string ingredient)
        {
            var meals = _manager.GetFood(ingredient);

            if (meals != null && meals.Any())
                return Ok(meals);

            return NoContent();
        }

        [HttpGet("foodById")]
        public ActionResult GetMealsById([FromQuery] string id)
        {
            var mealsById = _manager.GetFoodById(id);
            if (mealsById?.IdMeal != null)
            {
                return Ok(mealsById);
            }
            return NoContent();

        }
    }
}
