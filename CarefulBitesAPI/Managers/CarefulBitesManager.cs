using Microsoft.AspNetCore.JsonPatch;

namespace CarefulBitesAPI.Managers {
    public enum ClientError {
        Conflict,
        NotFound,
        Other
    }

    public class CarefulBitesManager {
        private readonly ICarefulBitesDbContext _dbContext;

        public CarefulBitesManager(ICarefulBitesDbContext dbContext) {
            _dbContext = dbContext;
        }

        #region foodItems
        public Item? GetFoodItem(int itemId) {
            var item = _dbContext.Items.Find(itemId);

            return item;
        }

        public IEnumerable<Item> GetFoodItems(out bool foundFood, int? itemStorageId = null) {
            List<Item> foodItemList = _dbContext.Items.ToList();
            foundFood = itemStorageId != null;
            if (itemStorageId != null)
                foodItemList = foodItemList.FindAll(fI => fI.ItemStorageId.Equals(itemStorageId));

            return foodItemList;
        }

        public Item? PostFoodItem(Item foodItem) {
            var newItem = _dbContext.Items.Add(foodItem);

            _dbContext.SaveChanges();

            return newItem?.Entity;
        }

        public ClientError? PatchFoodItem(int itemId, JsonPatchDocument<Item> value) {
            var item = _dbContext.Items.Find(itemId);

            if (item != null) {
                value.ApplyTo(item);
                _dbContext.SaveChanges();

                return null;
            }

            return ClientError.NotFound;
        }

        public ClientError? DeleteFoodItem(int itemId) {
            var item = _dbContext.Items.Find(itemId);

            if (item != null) {
                _dbContext.Items.Remove(item);
                _dbContext.SaveChanges();

                return null;
            }

            return ClientError.NotFound;
        }
        #endregion

        #region users
        public User? GetUser(int userId) {
            var user = _dbContext.Users.Find(userId);

            return user;
        }

        public IEnumerable<User> GetUsers(string? username = null) {
            List<User> userList = _dbContext.Users.ToList();

            if (username != null)
                userList = userList.FindAll(u => u.Username.Equals(username));

            return userList;
        }

        public (User? user, ClientError? error) PostUser(User user) {
            var userWithSameName = _dbContext.Users.ToList().Exists(u => u.Username.Equals(user.Username));

            if (userWithSameName)
                return (null, ClientError.Conflict);

            var newUser = _dbContext.Users.Add(user);

            _dbContext.SaveChanges();

            return (newUser?.Entity, null);
        }

        public ClientError? PatchUser(int userId, JsonPatchDocument<User> value) {
            var user = _dbContext.Users.Find(userId);

            if (user == null)
                return ClientError.NotFound;

            value.ApplyTo(user);
            _dbContext.SaveChanges();

            return null;
        }

        public ClientError? DeleteUser(int userId) {
            var user = _dbContext.Users.Find(userId);

            if (user == null)
                return ClientError.NotFound;

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return null;
        }
        #endregion

        #region itemStorages
        public IEnumerable<ItemStorage> GetItemStorages(int? userId = null) {
            List<ItemStorage> itemStorageList = _dbContext.ItemStorages.ToList();

            if (userId != null)
                itemStorageList = itemStorageList.FindAll(iS => iS.UserId.Equals(userId));

            return itemStorageList;
        }

        public ItemStorage? GetItemStorage(int itemStorageId) {
            var itemStorage = _dbContext.ItemStorages.Find(itemStorageId);

            return itemStorage;
        }

        public ItemStorage? PostItemStorage(ItemStorage itemStorage) {
            var newItemStorage = _dbContext.ItemStorages.Add(itemStorage);

            _dbContext.SaveChanges();

            return newItemStorage?.Entity;
        }

        public ClientError? PatchItemStorage(int itemStorageId, JsonPatchDocument<ItemStorage> value) {
            var itemStorage = _dbContext.ItemStorages.Find(itemStorageId);

            if (itemStorage == null)
                return ClientError.NotFound;

            value.ApplyTo(itemStorage);
            _dbContext.SaveChanges();

            return null;
        }

        public ClientError? DeleteItemStorage(int itemStorageId) {
            var itemStorage = _dbContext.ItemStorages.Find(itemStorageId);

            if (itemStorage == null)
                return ClientError.NotFound;
            var itemStorageList = GetFoodItems(out _, itemStorageId).ToList();
            if (itemStorageList.Count != 0)
            {
                foreach (var item in itemStorageList)
                {
                    _dbContext.Items.Remove(item);
                }
                _dbContext.SaveChanges();
            }
            _dbContext.ItemStorages.Remove(itemStorage);
            _dbContext.SaveChanges();

            return null;
        }

        public void MoveItems(int originId, int destinationId)
        {
            var itemStorageList = GetFoodItems(out _, originId).ToList();

            if (itemStorageList.Count == 0) return;
            foreach (var item in itemStorageList)
            {
                item.ItemStorageId = destinationId;
            }
            _dbContext.SaveChanges();
        }
        #endregion

        #region categories
        public IEnumerable<Category> GetCategories() {
            List<Category> categoryList = _dbContext.Categories.ToList();

            return categoryList;
        }

        public Category? PostCategory(Category category) {
            var newCategory = _dbContext.Categories.Add(category);

            _dbContext.SaveChanges();

            return newCategory?.Entity;
        }

        public ClientError? PatchCategory(int categoryId, JsonPatchDocument<Category> value) {
            var category = _dbContext.Categories.Find(categoryId);

            if (category == null)
                return ClientError.NotFound;

            value.ApplyTo(category);
            _dbContext.SaveChanges();

            return null;
        }

        public ClientError? DeleteCategory(int categoryId) {
            var category = _dbContext.Categories.Find(categoryId);

            if (category == null)
                return ClientError.NotFound;

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            return null;
        }
        #endregion

        #region itemCategoryBindings
        public IEnumerable<ItemCategoryBinding> GetItemCategoryBindings() {
            List<ItemCategoryBinding> itemCategoryBindingList = _dbContext.ItemCategoryBindings.ToList();

            return itemCategoryBindingList;
        }

        public ItemCategoryBinding? PostItemCategoryBinding(ItemCategoryBinding itemCategoryBinding) {
            var newItemCategoryBinding = _dbContext.ItemCategoryBindings.Add(itemCategoryBinding);

            _dbContext.SaveChanges();

            return newItemCategoryBinding?.Entity;
        }

        public ClientError? PatchItemCategoryBinding(int itemCategoryBindingId, JsonPatchDocument<ItemCategoryBinding> value) {
            var itemCategoryBinding = _dbContext.ItemCategoryBindings.Find(itemCategoryBindingId);

            if (itemCategoryBinding == null)
                return ClientError.NotFound;

            value.ApplyTo(itemCategoryBinding);
            _dbContext.SaveChanges();

            return null;
        }

        public ClientError? DeleteItemCategoryBinding(int itemCategoryBindingId) {
            var itemCategoryBinding = _dbContext.ItemCategoryBindings.Find(itemCategoryBindingId);

            if (itemCategoryBinding == null)
                return ClientError.NotFound;

            _dbContext.ItemCategoryBindings.Remove(itemCategoryBinding);
            _dbContext.SaveChanges();

            return null;
        }
        #endregion

        #region Templates
        public IEnumerable<ItemTemplate> GetTemplates()
        {
            List<ItemTemplate> templateList = _dbContext.ItemTemplates.ToList();
            return templateList;
        }
        public ItemTemplate? GetTemplate(int itemId)
        {
            var item = _dbContext.ItemTemplates.Find(itemId);

            return item;
        }
        #endregion
    }
}
