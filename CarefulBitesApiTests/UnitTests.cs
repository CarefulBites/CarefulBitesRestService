using CarefulBitesAPI;
using CarefulBitesAPI.Managers;

namespace CarefulBitesAPITests {
    public class UnitTests {
        private CarefulBitesManager _manager = new CarefulBitesManager(new FakeCarefulBitesDbContext());

        [Fact]
        public void TestPostFoodItemAndDeleteFoodItem() {
            Assert.Empty(_manager.GetAllFoodItems());

            var testItem = new Item() {
                ItemId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            _manager.PostFoodItem(testItem);

            Assert.NotEmpty(_manager.GetAllFoodItems());

            _manager.DeleteFoodItem(7);

            Assert.Empty(_manager.GetAllFoodItems());
        }

        [Fact]
        public void TestPostUserAndDeleteUser() {
            Assert.Empty(_manager.GetUsers());

            var testUser = new User() {
                UserId = 7,
                Username = "Barry",
                Password = "1234",
            };

            _manager.PostUser(testUser);

            Assert.NotEmpty(_manager.GetUsers());

            _manager.DeleteUser(7);

            Assert.Empty(_manager.GetUsers());
        }
    }
}