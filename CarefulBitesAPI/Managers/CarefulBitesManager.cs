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
            if (GetFoodItems(out _, itemStorageId).ToList().Count != 0)
                return ClientError.Conflict;

            _dbContext.ItemStorages.Remove(itemStorage);
            _dbContext.SaveChanges();

            return null;
        }
        #endregion
    }
}
