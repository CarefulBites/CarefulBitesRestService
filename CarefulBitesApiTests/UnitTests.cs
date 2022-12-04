using CarefulBitesAPI;
using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;

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

            Assert.Equal(testItem, _manager.GetFoodItem(7));

            _manager.DeleteFoodItem(7);

            Assert.Null(_manager.GetFoodItem(7));

            Assert.Empty(_manager.GetAllFoodItems());
        }

        [Fact]
        public void TestPostFoodItemAndPatchFoodItem() {
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

            Assert.Equal(testItem, _manager.GetFoodItem(7));

            var jsonPatch = new JsonPatchDocument<Item>(new List<Operation<Item>>() { new Operation<Item>("replace", "/name", "", "CoolerPeanuts") }, new DefaultContractResolver());

            _manager.PatchFoodItem(7, jsonPatch);

            Assert.Equal("CoolerPeanuts", _manager.GetFoodItem(7)?.Name);
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

            Assert.Equal(testUser, _manager.GetUser(7));

            _manager.DeleteUser(7);

            Assert.Null(_manager.GetUser(7));

            Assert.Empty(_manager.GetUsers());
        }
    }
}