using Microsoft.AspNetCore.JsonPatch;

namespace CarefulBitesAPI.Managers {
    public enum ClientError {
        Conflict,
        Other
    }

    public class CarefulBitesManager {
        private readonly ICarefulBitesDbContext _dbContext;

        public CarefulBitesManager(ICarefulBitesDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Item? GetFoodItem(int itemId) {
            var item = _dbContext.Items.Find(itemId);

            return item;
        }

        public IEnumerable<Item> GetAllFoodItems() {
            List<Item> foodItemList = _dbContext.Items.ToList();

            return foodItemList;
        }

        public Item? PostFoodItem(Item foodItem) {
            var newItem = _dbContext.Items.Add(foodItem);

            _dbContext.SaveChanges();

            return newItem?.Entity;
        }

        public void PatchFoodItem(int itemId, JsonPatchDocument<Item> value) {
            var item = _dbContext.Items.Find(itemId);

            if (item != null)
                value.ApplyTo(item);

            _dbContext.SaveChanges();
        }

        public void DeleteFoodItem(int itemId) {
            var item = _dbContext.Items.Find(itemId);

            if (item != null) {
                _dbContext.Items.Remove(item);
                _dbContext.SaveChanges();
            }
        }

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

        public void PatchUser(int userId, JsonPatchDocument<User> value) {
            var user = _dbContext.Users.Find(userId);

            if (user != null)
                value.ApplyTo(user);

            _dbContext.SaveChanges();
        }

        public void DeleteUser(int userId) {
            var user = _dbContext.Users.Find(userId);

            if (user != null) {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

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

        public void PatchItemStorage(int itemStorageId, JsonPatchDocument<ItemStorage> value) {
            var itemStorage = _dbContext.ItemStorages.Find(itemStorageId);

            if (itemStorage != null)
                value.ApplyTo(itemStorage);

            _dbContext.SaveChanges();
        }

        public void DeleteItemStorage(int itemStorageId) {
            var itemStorage = _dbContext.Users.Find(itemStorageId);

            if (itemStorage != null) {
                _dbContext.Users.Remove(itemStorage);
                _dbContext.SaveChanges();
            }
        }
    }
}
