using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace CarefulBitesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CarefulBitesController : ControllerBase {

#if DEBUG
        private Uri baseUri = new Uri("https://localhost:7116/CarefulBites/");
#endif
#if RELEASE
        private Uri baseUri = new Uri("https://carefulbitesapi20221128134821.azurewebsites.net/CarefulBites/");
#endif

        private readonly ILogger<CarefulBitesController> _logger;
        private CarefulBitesManager _manager = new CarefulBitesManager(new CarefulBitesDbContext());

        public CarefulBitesController(ILogger<CarefulBitesController> logger) {
            _logger = logger;
        }

        [HttpGet("foodItems", Name = "GetAllFoodItems")]
        public ActionResult<IEnumerable<Item>> GetAllFoodItems() {
            var items = _manager.GetAllFoodItems();

            if (items.Any())
                return Ok(items);

            return NoContent();
        }

        [HttpGet("foodItems/{itemId}", Name = "GetFoodItem")]
        public ActionResult<Item> GetFoodItem(int itemId) {
            var item = _manager.GetFoodItem(itemId);

            if (item != null)
                return Ok(_manager.GetFoodItem(itemId));

            return NotFound();
        }

        [HttpPost("foodItems", Name = "PostFoodItem")]
        public ActionResult PostFoodItem([FromBody] Item foodItem) {
            var createdItem = _manager.PostFoodItem(foodItem);
            if (createdItem != null)
                return Created(new Uri(baseUri, $"foodItems/{createdItem.ItemId}"), createdItem);

            throw new SystemException("Posted Food Item is null.");
        }

        [HttpPut("foodItems/{itemId}", Name = "PutFoodItem")]
        public ActionResult PutFoodItem(int itemId, [FromBody] Item foodItem) {
            _manager.PutFoodItem(itemId, foodItem);
            return NoContent();
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

        [HttpGet("users", Name = "GetAllUsers")]
        public ActionResult<IEnumerable<User>> GetAllUsers() {
            var users = _manager.GetAllUsers();

            if (users.Any())
                return Ok(users);

            return NoContent();
        }

        [HttpGet("users/{userId}", Name = "GetUserByUserId")]
        public ActionResult<User> GetUserByUserId(int userId) {
            var user = _manager.GetUserByUserId(userId);

            if (user != null)
                return Ok(user);

            return NotFound();
        }

        [HttpGet("users/byUsername/{username}", Name = "GetUserByUsername")]
        public ActionResult<User> GetUserByUsername(string username) {
            var user = _manager.GetUserByUsername(username);

            if (user != null)
                return Ok(user);

            return NotFound();
        }

        [HttpPost("users", Name = "PostUser")]
        public ActionResult PostUser([FromBody] User user) {
            var createdUser = _manager.PostUser(user);

            switch (createdUser.error) {
                case null:
                    return Created(new Uri(baseUri, $"users/{user.UserId}"), createdUser);
                case ClientError.Conflict:
                    return Conflict();
                default:
                    return BadRequest();
            }
        }

        [HttpPut("users/{userId}", Name = "PutUser")]
        public ActionResult PutUser(int userId, [FromBody] User user) {
            _manager.PutUser(userId, user);
            return NoContent();
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

        [HttpGet("itemStorages", Name = "GetAllItemStorages")]
        public ActionResult<IEnumerable<ItemStorage>> GetAllItemStorages() {
            var itemStorages = _manager.GetAllItemStorages();

            if (itemStorages.Any())
                return Ok(itemStorages);

            return NoContent();
        }

        [HttpGet("itemStorages/{itemStorageId}", Name = "GetItemStorage")]
        public ActionResult<ItemStorage> GetItemStorage(int itemStorageId) {
            var itemStorage = _manager.GetItemStorage(itemStorageId);

            if (itemStorage != null)
                return Ok(itemStorage);

            return NotFound();
        }

        [HttpPost("itemStorages", Name = "PostItemStorage")]
        public ActionResult PostItemStorages([FromBody] ItemStorage itemStorage) {
            var createdItemStorage = _manager.PostItemStorage(itemStorage);
            return Created(new Uri(baseUri, $"itemStorages/{itemStorage.ItemStorageId}"), createdItemStorage);
        }

        [HttpPut("itemStorages/{itemStorageId}", Name = "PutItemStorage")]
        public ActionResult PutItemStorage(int itemStorageId, [FromBody] ItemStorage itemStorage) {
            _manager.PutItemStorage(itemStorageId, itemStorage);
            return NoContent();
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