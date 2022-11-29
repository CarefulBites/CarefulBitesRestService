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

        [HttpGet("foodItems", Name = "GetAllFoodItems")]
        public IEnumerable<Item> GetAllFoodItems() {
            return CarefulBitesManager.GetAllFoodItems();
        }

        [HttpGet("foodItems/{itemId}", Name = "GetFoodItem")]
        public Item GetFoodItem(int itemId) {
            return CarefulBitesManager.GetFoodItem(itemId);
        }

        [HttpPost("foodItems", Name = "PostFoodItem")]
        public void PostFoodItem([FromBody] Item foodItem) {
            CarefulBitesManager.PostFoodItem(foodItem);
        }

        [HttpPut("foodItems/{itemId}", Name = "PutFoodItem")]
        public void PutFoodItem(int itemId, [FromBody] Item foodItem) {
            CarefulBitesManager.PutFoodItem(itemId, foodItem);
        }

        [HttpDelete("foodItems/{itemId}", Name = "DeleteFoodItem")]
        public void DeleteFoodItem(int itemId) {
            CarefulBitesManager.DeleteFoodItem(itemId);
        }

        [HttpGet("users", Name = "GetUsers")]
        public IEnumerable<User> GetUsers() {
            return CarefulBitesManager.GetUsers();
        }

        [HttpPost("users", Name = "PostUser")]
        public void PostUser([FromBody] User user) {
            CarefulBitesManager.PostUser(user);
        }

        [HttpPut("users/{userId}", Name = "PutUser")]
        public void PutUser(int userId, [FromBody] User user) {
            CarefulBitesManager.PutUser(userId, user);
        }

        [HttpDelete("foodItems/{userId}", Name = "DeleteUser")]
        public void DeleteUser(int userId) {
            CarefulBitesManager.DeleteUser(userId);
        }

        [HttpGet("itemStorages", Name = "GetItemStorages")]
        public IEnumerable<ItemStorage> GetItemStorages() {
            return CarefulBitesManager.GetItemStorages();
        }

        [HttpPost("itemStorages", Name = "PostItemStorage")]
        public void PostItemStorages([FromBody] ItemStorage itemStorage) {
            CarefulBitesManager.PostItemStorage(itemStorage);
        }

        [HttpPut("itemStorages/{itemStorageId}", Name = "PutItemStorage")]
        public void PutItemStorage(int itemStorageId, [FromBody] ItemStorage itemStorage) {
            CarefulBitesManager.PutItemStorage(itemStorageId, itemStorage);
        }

        [HttpDelete("foodItems/{itemStorageId}", Name = "DeleteItemStorage")]
        public void DeleteItemStorage(int itemStorageId) {
            CarefulBitesManager.DeleteItemStorage(itemStorageId);
        }
    }
}