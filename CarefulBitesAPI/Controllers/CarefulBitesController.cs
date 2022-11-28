using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.Mvc;

namespace CarefulBitesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarefulBitesController : ControllerBase {
        private readonly ILogger<CarefulBitesController> _logger;

        public CarefulBitesController(ILogger<CarefulBitesController> logger) {
            _logger = logger;
        }

        [HttpGet("foodItems", Name = "GetFoodItems")]
        public IEnumerable<Item> GetFoodItems() {
            return CarefulBitesManager.GetFoodItems();
        }

        [HttpPost("foodItems", Name = "PostFoodItem")]
        public void PostFoodItem(Item foodItem) {
            CarefulBitesManager.PostFoodItem(foodItem);
        }

        [HttpGet("users", Name = "GetUsers")]
        public IEnumerable<User> GetUsers() {
            return CarefulBitesManager.GetUsers();
        }

        [HttpPost("users", Name = "PostUser")]
        public void PostUser(User user) {
            CarefulBitesManager.PostUser(user);
        }

        [HttpGet("itemStorages", Name = "GetItemStorages")]
        public IEnumerable<ItemStorage> GetItemStorages() {
            return CarefulBitesManager.GetItemStorages();
        }

        [HttpPost("itemStorages", Name = "PostItemStorage")]
        public void PostItemStorages(ItemStorage itemStorage) {
            CarefulBitesManager.PostItemStorage(itemStorage);
        }
    }
}