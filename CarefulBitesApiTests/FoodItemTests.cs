using CarefulBitesAPI;
using CarefulBitesAPI.Managers;

namespace CarefulBitesApiTests {
    public class FoodItemTests {
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
    }
}