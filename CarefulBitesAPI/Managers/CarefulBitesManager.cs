using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarefulBitesAPI.Managers {
    public static class CarefulBitesManager {
        private static CarefulBitesDbContext _dbContext = new CarefulBitesDbContext();

        public static Item GetFoodItem(int itemId) {
            var item = _dbContext.Items.Find(itemId);

            return item;
        }

        public static IEnumerable<Item> GetAllFoodItems() {
            List<Item> foodItemList = _dbContext.Items.ToList();

            return foodItemList;
        }

        public static void PostFoodItem(Item foodItem) {
            foodItem.ItemId = null;

            _dbContext.Items.Add(foodItem);

            _dbContext.SaveChanges();
        }

        public static void PutFoodItem(int itemId, Item foodItem) {
            var oldItem = _dbContext.Items.Find(itemId);

            if (oldItem != null) {
                oldItem.Amount = foodItem.Amount;
                oldItem.CaloriesPer = foodItem.CaloriesPer;
                oldItem.DaysAfterOpen = foodItem.DaysAfterOpen;
                oldItem.ExpirationDate = foodItem.ExpirationDate;
                oldItem.Name = foodItem.Name;
                oldItem.ItemStorageId = foodItem.ItemStorageId;
                oldItem.OpenDate = foodItem.OpenDate;
                oldItem.Unit = foodItem.Unit;

                _dbContext.SaveChanges();
            }
        }

        public static void PatchFoodItem(int itemId, JsonPatchDocument<Item> value) {
            var item = _dbContext.Items.Find(itemId);

            value.ApplyTo(item);
        }

        public static void DeleteFoodItem(int itemId) {
            var item = _dbContext.Items.Find(itemId);

            if (item != null) {
                _dbContext.Items.Remove(item);
                _dbContext.SaveChanges();
            }
        }

        public static IEnumerable<User> GetUsers() {
            List<User> userList = _dbContext.Users.ToList();

            return userList;
        }

        public static void PostUser(User user) {
            _dbContext.Users.Add(user);

            _dbContext.SaveChanges();
        }

        public static void PutUser(int userId, User user) {
            var oldUser = _dbContext.Users.Find(userId);

            if (oldUser != null) {
                oldUser.Username = user.Username;
                oldUser.Password = user.Password;

                _dbContext.SaveChanges();
            }
        }

        public static void DeleteUser(int userId) {
            var user = _dbContext.Users.Find(userId);

            if (user != null) {
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public static IEnumerable<ItemStorage> GetItemStorages() {
            List<ItemStorage> itemStorageList = _dbContext.ItemStorages.ToList();

            return itemStorageList;
        }

        public static void PostItemStorage(ItemStorage itemStorage) {
            _dbContext.ItemStorages.Add(itemStorage);

            _dbContext.SaveChanges();
        }

        public static void PutItemStorage(int itemStorageId, ItemStorage itemStorage) {
            var oldItemStorage = _dbContext.ItemStorages.Find(itemStorageId);

            if (oldItemStorage != null) {
                oldItemStorage.Name = itemStorage.Name;
                oldItemStorage.User = itemStorage.User;
                oldItemStorage.UserId = itemStorage.UserId;

                _dbContext.SaveChanges();
            }
        }

        public static void DeleteItemStorage(int itemStorageId) {
            var itemStorage = _dbContext.Users.Find(itemStorageId);

            if (itemStorage != null) {
                _dbContext.Users.Remove(itemStorage);
                _dbContext.SaveChanges();
            }
        }
    }
}
