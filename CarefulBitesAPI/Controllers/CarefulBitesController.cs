using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CarefulBitesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CarefulBitesController : ControllerBase {

#if DEBUG
        private readonly Uri _baseUri = new("https://localhost:7116/CarefulBites/");
#endif
#if RELEASE
        private readonly Uri _baseUri = new("https://carefulbitesapi20221128134821.azurewebsites.net/CarefulBites/");
#endif

        private readonly CarefulBitesManager _manager = new(new CarefulBitesDbContext());

        #region foodItems
        [HttpGet("foodItems", Name = "GetFoodItems")]
        public ActionResult<IEnumerable<Item>> GetFoodItems([FromQuery] int? itemStorageId = null) {
            var items = _manager.GetFoodItems(out _, itemStorageId);

            if (items.Any())
                return Ok(items);

            return NoContent();
        }

        [HttpGet("randomFood", Name = "GetRandomFood")]
        public ActionResult<IEnumerable<Item>> GetRandomFood([FromQuery] int? number = null) {
            List<Item> allFoods = _manager.GetFoodItems(out _).ToList();
            Random rand = new();
            number ??= 1;

            List<Item> outList = new();
            for (int i = 0; i < number; i++) {
                int id = rand.Next(allFoods.Count);
                outList.Add(allFoods[id]);
                allFoods.RemoveAt(id);
            }
            return outList;
        }

        [HttpGet("foodItems/{itemId}", Name = "GetFoodItemById")]
        public ActionResult<Item> GetFoodItemById(int itemId) {
            var item = _manager.GetFoodItem(itemId);

            if (item != null)
                return Ok(_manager.GetFoodItem(itemId));

            return NotFound();
        }

        [HttpPost("foodItems", Name = "PostFoodItem")]
        public ActionResult PostFoodItem([FromBody] Item foodItem) {
            foodItem.ItemId = null;

            var createdItem = _manager.PostFoodItem(foodItem);

            if (createdItem != null)
                return Created(new Uri(_baseUri, $"foodItems/{createdItem.ItemId}"), createdItem);

            return BadRequest();
        }

        [HttpPatch("foodItems/{itemId}", Name = "PatchFoodItem")]
        public ActionResult PatchFoodItem(int itemId, [FromBody] JsonPatchDocument<Item> value) {
            var error = _manager.PatchFoodItem(itemId, value);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        [HttpDelete("foodItems/{itemId}", Name = "DeleteFoodItem")]
        public ActionResult DeleteFoodItem(int itemId) {
            var error = _manager.DeleteFoodItem(itemId);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        [HttpGet("usersFood/{userId}", Name = "GetUsersFood")]
        public ActionResult<IEnumerable<Item>> GetUsersFood(int userId, [FromQuery] int? index = null, [FromQuery] bool exact = false) {
            var user = _manager.GetUser(userId);

            if (user == null)
                return NotFound();

            var itemStorages = _manager.GetItemStorages(user.UserId);
            var enumerable = itemStorages as ItemStorage[] ?? itemStorages.ToArray();
            ItemStorage[] storList = enumerable.ToArray();
            List<Item> foods = new();
            bool foundfood;

            if (index == null) {
                foreach (var stor in enumerable) {
                    List<Item> temp = _manager.GetFoodItems(out foundfood, stor.ItemStorageId).ToList();
                    if (foundfood)
                        foods.AddRange(temp);
                }
            } else {
                List<Item> temp;
                if (exact)
                    temp = _manager.GetFoodItems(out foundfood, index).ToList();
                else
                    temp = _manager.GetFoodItems(out foundfood, storList[(int)(index % storList.GetLength(1))].ItemStorageId).ToList();

                if (foundfood)
                    foods.AddRange(temp);

            }
            if (foods.Count >= 0)
                return foods;

            return NotFound();
        }
        #endregion

        #region users
        [HttpGet("users", Name = "GetUsers")]
        public ActionResult<IEnumerable<User>> GetUsers([FromQuery] string? username = null) {
            var users = _manager.GetUsers(username);

            if (users.Any())
                return Ok(users);

            return NoContent();
        }

        [HttpGet("users/{userId}", Name = "GetUserById")]
        public ActionResult<User> GetUserById(int userId) {
            var user = _manager.GetUser(userId);

            if (user != null)
                return Ok(user);

            return NotFound();
        }

        [HttpPost("users", Name = "PostUser")]
        public ActionResult PostUser([FromBody] User user) {
            user.UserId = null;

            var createdUserTuple = _manager.PostUser(user);

            return createdUserTuple.error switch {
                null => Created(new Uri(_baseUri, $"users/{createdUserTuple.user?.UserId}"), createdUserTuple.user),
                ClientError.Conflict => Conflict(),
                _ => BadRequest()
            };
        }

        [HttpPatch("users/{userId}", Name = "PatchUser")]
        public ActionResult PatchUser(int userId, [FromBody] JsonPatchDocument<User> value) {
            var error = _manager.PatchUser(userId, value);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        [HttpDelete("users/{userId}", Name = "DeleteUser")]
        public ActionResult DeleteUser(int userId) {
            var error = _manager.DeleteUser(userId);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }
        #endregion

        #region itemStorages
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
            var error = _manager.PatchItemStorage(itemStorageId, value);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        [HttpDelete("itemStorages/{itemStorageId}", Name = "DeleteItemStorage")]
        public ActionResult DeleteItemStorage(int itemStorageId) {
            var error = _manager.DeleteItemStorage(itemStorageId);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                ClientError.Conflict => Conflict(),
                _ => BadRequest()
            };
        }
        #endregion
    }
}