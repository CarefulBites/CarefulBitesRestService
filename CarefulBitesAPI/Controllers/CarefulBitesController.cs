using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace CarefulBitesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CarefulBitesController : ControllerBase {

#if DEBUG
        private readonly Uri _baseUri = new Uri("https://localhost:7116/CarefulBites/");
#endif
#if RELEASE
        private readonly Uri _baseUri = new Uri("https://carefulbitesapi20221128134821.azurewebsites.net/CarefulBites/");
#endif

        private readonly CarefulBitesManager _manager = new CarefulBitesManager(new CarefulBitesDbContext());

        [HttpGet("foodItems", Name = "GetAllFoodItems")]
        public ActionResult<IEnumerable<Item>> GetAllFoodItems() {
            var items = _manager.GetAllFoodItems();

            if (items.Any())
                return Ok(items);

            return NoContent();
        }

        [HttpGet("foodItems/{itemId}", Name = "GetFoodItemById")]
        public ActionResult<Item> GetFoodItem(int itemId) {
            var item = _manager.GetFoodItem(itemId);

            if (item != null)
                return Ok(_manager.GetFoodItem(itemId));

            return NotFound();
        }

        [HttpPost("foodItems", Name = "PostFoodItem")]
        public ActionResult PostFoodItem([FromBody] Item foodItem) {
            foodItem.ItemId = null;

            var createdItem = _manager.PostFoodItem(foodItem);

            return Created(new Uri(_baseUri, $"foodItems/{createdItem.ItemId}"), createdItem);
        }

        [HttpPatch("foodItems/{itemId}", Name = "PatchFoodItem")]
        public ActionResult PatchFoodItem(int itemId, [FromBody] JsonPatchDocument<Item> value) {
            _manager.PatchFoodItem(itemId, value);
            return NoContent();
        }

        [HttpDelete("foodItems/{itemId}", Name = "DeleteFoodItem")]
        public ActionResult DeleteFoodItem(int itemId) {
            _manager.DeleteFoodItem(itemId);
            return NoContent();
        }

        [HttpGet("users", Name = "GetUsers")]
        public ActionResult<IEnumerable<User>> GetUsers([FromQuery] string? username = null) {
            var users = _manager.GetUsers(username);

            if (users.Any())
                return Ok(users);

            return NoContent();
        }

        [HttpGet("users/{userId}", Name = "GetUserById")]
        public ActionResult<User> GetUser(int userId) {
            var user = _manager.GetUser(userId);

            if (user != null)
                return Ok(user);

            return NotFound();
        }

        [HttpPost("users", Name = "PostUser")]
        public ActionResult PostUser([FromBody] User user) {
            user.UserId = null;

            var createdUser = _manager.PostUser(user);

            switch (createdUser.error) {
                case null:
                    return Created(new Uri(_baseUri, $"users/{user.UserId}"), createdUser);
                case ClientError.Conflict:
                    return Conflict();
                default:
                    return BadRequest();
            }
        }

        [HttpPatch("users/{userId}", Name = "PatchUser")]
        public ActionResult PatchUser(int userId, [FromBody] JsonPatchDocument<User> value) {
            _manager.PatchUser(userId, value);
            return NoContent();
        }

        [HttpDelete("users/{userId}", Name = "DeleteUser")]
        public ActionResult DeleteUser(int userId) {
            _manager.DeleteUser(userId);
            return NoContent();
        }

        [HttpGet("itemStorages", Name = "GetItemStorages")]
        public ActionResult<IEnumerable<ItemStorage>> GetItemStorages([FromQuery] int? userId = null) {
            var itemStorages = _manager.GetItemStorages(userId);

            if (itemStorages.Any())
                return Ok(itemStorages);

            return NoContent();
        }

        [HttpGet("itemStorages/{itemStorageId}", Name = "GetItemStorageById")]
        public ActionResult<ItemStorage> GetItemStorage(int itemStorageId) {
            var itemStorage = _manager.GetItemStorage(itemStorageId);

            if (itemStorage != null)
                return Ok(itemStorage);

            return NotFound();
        }

        [HttpPost("itemStorages", Name = "PostItemStorage")]
        public ActionResult PostItemStorages([FromBody] ItemStorage itemStorage) {
            itemStorage.ItemStorageId = null;

            var createdItemStorage = _manager.PostItemStorage(itemStorage);
            return Created(new Uri(_baseUri, $"itemStorages/{itemStorage.ItemStorageId}"), createdItemStorage);
        }

        [HttpPatch("itemStorages/{itemStorageId}", Name = "PatchItemStorage")]
        public ActionResult PatchItemStorage(int itemStorageId, [FromBody] JsonPatchDocument<ItemStorage> value) {
            _manager.PatchItemStorage(itemStorageId, value);
            return NoContent();
        }

        [HttpDelete("itemStorages/{itemStorageId}", Name = "DeleteItemStorage")]
        public ActionResult DeleteItemStorage(int itemStorageId) {
            _manager.DeleteItemStorage(itemStorageId);
            return NoContent();
        }
    }
}