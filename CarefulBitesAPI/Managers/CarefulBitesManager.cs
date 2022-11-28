using Microsoft.EntityFrameworkCore;

namespace CarefulBitesAPI.Managers {
    public static class CarefulBitesManager
    {
        private static CarefulBitesDbContext _dbContext = new CarefulBitesDbContext();

        public static IEnumerable<Item> GetFoodItems() {
            List<Item> foodItemList = _dbContext.Items.ToList();
            
            return foodItemList;
        }

        public static void PostFoodItem(Item foodItem) {
            _dbContext.Items.Add(foodItem);

            _dbContext.SaveChanges();
        }

        public static IEnumerable<User> GetUsers() {
            List<User> userList = _dbContext.Users.ToList();

            return userList;
        }

        public static void PostUser(User user) {
            _dbContext.Users.Add(user);

            _dbContext.SaveChanges();
        }

        public static IEnumerable<ItemStorage> GetItemStorages() {
            List<ItemStorage> itemStorageList = _dbContext.ItemStorages.ToList();

            return itemStorageList;
        }

        public static void PostItemStorage(ItemStorage itemStorage) {
            _dbContext.ItemStorages.Add(itemStorage);

            _dbContext.SaveChanges();
        }
    }
}
