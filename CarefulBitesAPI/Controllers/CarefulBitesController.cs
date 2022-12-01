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

        public CarefulBitesController(ILogger<CarefulBitesController> logger) {
            _logger = logger;
        }

        [HttpGet("foodItems", Name = "GetAllFoodItems")]
        public ActionResult<IEnumerable<Item>> GetAllFoodItems() {
            var items = CarefulBitesManager.GetAllFoodItems();

            if (items.Any())
                return Ok(items);

            return NoContent();
        }

        [HttpGet("foodItems/{itemId}", Name = "GetFoodItem")]
        public ActionResult<Item> GetFoodItem(int itemId) {
            var item = CarefulBitesManager.GetFoodItem(itemId);

            if (item != null)
                return Ok(CarefulBitesManager.GetFoodItem(itemId));

            return NotFound();
        }

        [HttpPost("foodItems", Name = "PostFoodItem")]
        public ActionResult PostFoodItem([FromBody] Item foodItem) {
            var createdItem = CarefulBitesManager.PostFoodItem(foodItem);
            return Created(new Uri(baseUri, $"foodItems/{createdItem.ItemId}"), createdItem);
        }

        [HttpPut("foodItems/{itemId}", Name = "PutFoodItem")]
        public ActionResult PutFoodItem(int itemId, [FromBody] Item foodItem) {
            CarefulBitesManager.PutFoodItem(itemId, foodItem);
            return NoContent();
        }

        [HttpPatch("foodItems/{itemId}", Name = "PatchFoodItem")]
        public ActionResult PatchFoodItem(int itemId, [FromBody] JsonPatchDocument<Item> value) {
            CarefulBitesManager.PatchFoodItem(itemId, value);
            return NoContent();
        }

        [HttpDelete("foodItems/{itemId}", Name = "DeleteFoodItem")]
        public ActionResult DeleteFoodItem(int itemId) {
            CarefulBitesManager.DeleteFoodItem(itemId);
            return NoContent();
        }

        [HttpGet("users", Name = "GetUsers")]
        public ActionResult<IEnumerable<User>> GetUsers() {
            return Ok(CarefulBitesManager.GetUsers());
        }

        [HttpPost("users", Name = "PostUser")]
        public ActionResult PostUser([FromBody] User user) {
            var createdUser = CarefulBitesManager.PostUser(user);
            return Created(new Uri(baseUri, $"users/{user.UserId}"), createdUser);
        }

        [HttpPut("users/{userId}", Name = "PutUser")]
        public ActionResult PutUser(int userId, [FromBody] User user) {
            CarefulBitesManager.PutUser(userId, user);
            return NoContent();
        }

        [HttpPatch("users/{userId}", Name = "PatchUser")]
        public ActionResult PatchUser(int userId, [FromBody] JsonPatchDocument<User> value) {
            CarefulBitesManager.PatchUser(userId, value);
            return NoContent();
        }

        [HttpDelete("users/{userId}", Name = "DeleteUser")]
        public ActionResult DeleteUser(int userId) {
            CarefulBitesManager.DeleteUser(userId);
            return NoContent();
        }

        [HttpGet("itemStorages", Name = "GetItemStorages")]
        public ActionResult<IEnumerable<ItemStorage>> GetItemStorages() {
            return Ok(CarefulBitesManager.GetItemStorages());
        }

        [HttpPost("itemStorages", Name = "PostItemStorage")]
        public ActionResult PostItemStorages([FromBody] ItemStorage itemStorage) {
            var createdItemStorage = CarefulBitesManager.PostItemStorage(itemStorage);
            return Created(new Uri(baseUri, $"itemStorages/{itemStorage.ItemStorageId}"), createdItemStorage);
        }

        [HttpPut("itemStorages/{itemStorageId}", Name = "PutItemStorage")]
        public ActionResult PutItemStorage(int itemStorageId, [FromBody] ItemStorage itemStorage) {
            CarefulBitesManager.PutItemStorage(itemStorageId, itemStorage);
            return NoContent();
        }

        [HttpPatch("itemStorages/{itemStorageId}", Name = "PatchItemStorage")]
        public ActionResult PatchItemStorage(int itemStorageId, [FromBody] JsonPatchDocument<ItemStorage> value) {
            CarefulBitesManager.PatchItemStorage(itemStorageId, value);
            return NoContent();
        }

        [HttpDelete("itemStorages/{itemStorageId}", Name = "DeleteItemStorage")]
        public ActionResult DeleteItemStorage(int itemStorageId) {
            CarefulBitesManager.DeleteItemStorage(itemStorageId);
            return NoContent();
        }
    }
}