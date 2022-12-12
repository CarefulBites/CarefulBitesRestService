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
        [HttpGet("randomMeal", Name = "GetUserById")]
        public ActionResult<User> GetFoodItems([FromQuery] int? number = null) {
            List<Item> meals = _manager.GetFood(ingredient);
            Random rand = new Random();
            if (number == null)
                number = 1;

            List<Item> outList = new List<Item>()
            for (int i = 0; i < number; i++)
			{
                int id = (int)rand.Next(meals.count);
                outList.Add(allfoods[id])
                meals.removeAt(id);
			}
            return outList;
        }
    }
}
