using CarefulBitesAPI.Managers;
using CarefulBitesAPI.Models;
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
        public ActionResult PostFoodItem([FromBody] Tuple<Item, int[]?> tuple)
        {
            tuple.Item1.ItemId = null;

            var createdItem = _manager.PostFoodItem(tuple.Item1);

            if (createdItem != null)
            {
                if (tuple.Item2 != null)
                {
                    foreach (var categoryId in tuple.Item2)
                    {
                        var newItemCategoryBinding = new ItemCategoryBinding
                        {
                            CategoryId = categoryId,
                            ItemId = (int)createdItem.ItemId
                        };
                        _manager.PostItemCategoryBinding(newItemCategoryBinding);
                    }
                }

                return Created(new Uri(_baseUri, $"foodItems/{createdItem.ItemId}"), createdItem);
            }

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

            List<ItemCategoryBinding> bindings = _manager.GetItemCategoryBindings().Where(b=> b.ItemId == itemId).ToList();
            if (bindings.Count > 0)
            {
                foreach (var binding in bindings)
                {
                    _manager.DeleteItemCategoryBinding((int)binding.ItemCategoryBindingId);
                }
            }
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
        public ActionResult DeleteItemStorage(int itemStorageId,[FromQuery] int destinationId = -1) {
            if (destinationId != -1)
            {
                _manager.MoveItems(itemStorageId, destinationId);
            }
            var error = _manager.DeleteItemStorage(itemStorageId);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                ClientError.Conflict => Conflict(),
                _ => BadRequest()
            };
        }
        #endregion

        #region categories
        [HttpGet("categories", Name = "GetCategories")]
        public ActionResult GetCategories() {
            var categories = _manager.GetCategories();

            if (categories.Any()) {
                return Ok(categories);
            }

            return NoContent();
        }

        [HttpPost("categories", Name = "PostCategory")]
        public ActionResult PostCategory([FromBody] Category category) {
            category.CategoryId = null;

            var createdCategory = _manager.PostCategory(category);
            return Created(new Uri(_baseUri, $"categories/{category.CategoryId}"), createdCategory);
        }

        [HttpPatch("categories/{categoryId}", Name = "PatchCategory")]
        public ActionResult PatchCategory(int categoryId, [FromBody] JsonPatchDocument<Category> value) {
            var error = _manager.PatchCategory(categoryId, value);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        [HttpDelete("categories/{categoryId}", Name = "DeleteCategory")]
        public ActionResult DeleteCategory(int categoryId) {
            var error = _manager.DeleteCategory(categoryId);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }
        #endregion

        #region itemCategoryBindings
        [HttpGet("itemCategoryBindings", Name = "GetItemCategoryBindings")]
        public ActionResult GetItemCategoryBindings() {
            var itemCategoryBindings = _manager.GetItemCategoryBindings();

            if (itemCategoryBindings.Any()) {
                return Ok(itemCategoryBindings);
            }

            return NoContent();
        }

        [HttpPost("itemCategoryBindings", Name = "PostItemCategoryBinding")]
        public ActionResult PostItemCategoryBinding([FromBody] ItemCategoryBinding itemCategoryBinding) {
            itemCategoryBinding.ItemCategoryBindingId = null;

            var createdItemCategoryBinding = _manager.PostItemCategoryBinding(itemCategoryBinding);
            return Created(new Uri(_baseUri, $"itemCategoryBindings/{itemCategoryBinding.ItemCategoryBindingId}"), createdItemCategoryBinding);
        }

        [HttpPatch("itemCategoryBindings/{itemCategoryBindingId}", Name = "PatchItemCategoryBinding")]
        public ActionResult PatchItemCategoryBinding(int itemCategoryBindingId, [FromBody] JsonPatchDocument<ItemCategoryBinding> value) {
            var error = _manager.PatchItemCategoryBinding(itemCategoryBindingId, value);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }

        [HttpDelete("itemCategoryBindings/{itemCategoryBindingId}", Name = "DeleteItemCategoryBinding")]
        public ActionResult DeleteItemCategoryBinding(int itemCategoryBindingId) {
            var error = _manager.DeleteItemCategoryBinding(itemCategoryBindingId);

            return error switch {
                null => NoContent(),
                ClientError.NotFound => NotFound(),
                _ => BadRequest()
            };
        }
        #endregion

        #region Templates
        [HttpGet("itemTemplates", Name = "GetItemTemplates")]
        public ActionResult<IEnumerable<ItemTemplate>> GetItemTemplates()
        {
            var templates = _manager.GetTemplates();

            if (templates.Any())
                return Ok(templates);

            return NoContent();
        }

        [HttpGet("itemTemplates/{itemTemplateId}", Name = "GetItemTemplate")]
        public ActionResult<IEnumerable<ItemTemplate>> GetItemTemplate(int itemTemplateId)
        {
            var template = _manager.GetTemplate(itemTemplateId);

            if (template != null)
                return Ok(template);

            return NoContent();
        }
        #endregion
    }
}