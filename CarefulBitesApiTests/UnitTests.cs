using CarefulBitesAPI;
using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;

namespace CarefulBitesAPITests {
    public class UnitTests {
        private CarefulBitesManager _manager = new CarefulBitesManager(new FakeCarefulBitesDbContext());
        #region Fooditems
        [Fact]
        public void TestPostFoodItemAndDeleteFoodItem() {
            Assert.Empty(_manager.GetFoodItems());

            var testItem = new Item() {
                ItemId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            _manager.PostFoodItem(testItem);

            Assert.NotEmpty(_manager.GetFoodItems());

            Assert.Equal(testItem, _manager.GetFoodItem(7));

            _manager.DeleteFoodItem(7);

            Assert.Null(_manager.GetFoodItem(7));

            Assert.Empty(_manager.GetFoodItems());
        }

        [Fact]
        public void TestPostFoodItemAndPatchFoodItem() {
            Assert.Empty(_manager.GetFoodItems());

            var testItem = new Item() {
                ItemId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            _manager.PostFoodItem(testItem);

            Assert.NotEmpty(_manager.GetFoodItems());

            Assert.Equal(testItem, _manager.GetFoodItem(7));

            var jsonPatch = new JsonPatchDocument<Item>(new List<Operation<Item>>() { new Operation<Item>("replace", "/name", "", "CoolerPeanuts") }, new DefaultContractResolver());

            _manager.PatchFoodItem(7, jsonPatch);

            Assert.Equal("CoolerPeanuts", _manager.GetFoodItem(7)?.Name);
        }
        #endregion

        #region Users
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

        [Fact]
        public void TestPostUserAndPatchUser()
        {
            Assert.Empty(_manager.GetUsers());

            var testUser = new User()
            {
                UserId = 7,
                Username = "Barry",
                Password = "1234",
            };

            _manager.PostUser(testUser);

            Assert.NotEmpty(_manager.GetUsers());

            Assert.Equal(testUser, _manager.GetUser(7));

            var jsonPatch = new JsonPatchDocument<User>(new List<Operation<User>>() { new Operation<User>("replace", "/Username", "", "Larry"), new Operation<User>("replace", "/Password", "", "12345") }, new DefaultContractResolver());

            _manager.PatchUser(7, jsonPatch);

            Assert.Equal("Larry", _manager.GetUser(7)?.Username);
            Assert.Equal("12345", _manager.GetUser(7)?.Password);
        }
        #endregion

        #region ItemStorage
        [Fact]
        public void TestPostItemStorageAndDeleteItemStorage()
        {
            Assert.Empty(_manager.GetItemStorages());

            var testItemStorage = new ItemStorage()
            {
                ItemStorageId = 7,
                Name = "Test Køleskab"
            };

            _manager.PostItemStorage(testItemStorage);

            Assert.NotEmpty(_manager.GetItemStorages());

            Assert.Equal(testItemStorage, _manager.GetItemStorage(7));

            var testItem = new Item()
            {
                ItemId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24),
                ItemStorageId = 7
            };

            _manager.PostFoodItem(testItem);


            _manager.DeleteItemStorage(7);

            Assert.Null(_manager.GetItemStorage(7));

            Assert.Empty(_manager.GetItemStorages());

            _manager.GetFoodItem(7);
        }
        #endregion

    }
}