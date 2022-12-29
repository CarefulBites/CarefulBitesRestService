using CarefulBitesAPI;
using CarefulBitesAPI.Managers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json.Serialization;

namespace CarefulBitesAPITests {
    public class UnitTests {
        private readonly CarefulBitesManager _manager = new(new FakeCarefulBitesDbContext());
        private readonly MealsManager _mealsManager = new();

        #region FoodItems
       [Fact]
        public void TestPostFoodItemAndDeleteFoodItem() {
            Assert.Empty(_manager.GetFoodItems(out _));

            var testItem = new Item {
                ItemId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            _manager.PostFoodItem(testItem);

            Assert.NotEmpty(_manager.GetFoodItems(out _));

            Assert.Equal(testItem, _manager.GetFoodItem(7));

            _manager.DeleteFoodItem(7);

            Assert.Null(_manager.GetFoodItem(7));

            Assert.Empty(_manager.GetFoodItems(out _));
        }

        [Fact]
        public void TestDeleteFoodItemNotFound() {
            Assert.Empty(_manager.GetFoodItems(out _));

            _manager.DeleteFoodItem(7);

            var error = _manager.DeleteFoodItem(7);

            Assert.Equal(ClientError.NotFound, error);
        }

        [Fact]
        public void TestPostFoodItemAndPatchFoodItem() {
            Assert.Empty(_manager.GetFoodItems(out _));

            var testItem = new Item() {
                ItemId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ItemStorageId = 3,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            _manager.PostFoodItem(testItem);
            Assert.NotEmpty(_manager.GetFoodItems(out var foundFood));

            Assert.False(foundFood);

            Assert.NotEmpty(_manager.GetFoodItems(out foundFood, 3));

            Assert.True(foundFood);

            Assert.Equal(testItem, _manager.GetFoodItem(7));

            var jsonPatch = new JsonPatchDocument<Item>(new List<Operation<Item>>() { new("replace", "/name", "", "CoolerPeanuts") }, new DefaultContractResolver());

            _manager.PatchFoodItem(7, jsonPatch);

            Assert.Equal("CoolerPeanuts", _manager.GetFoodItem(7)?.Name);
        }

        [Fact]
        public void TestPatchFoodItemNotFound() {
            Assert.Empty(_manager.GetFoodItems(out _));

            var jsonPatch = new JsonPatchDocument<Item>(new List<Operation<Item>>() { new("replace", "/name", "", "CoolerPeanuts") }, new DefaultContractResolver());

            var error = _manager.PatchFoodItem(7, jsonPatch);

            Assert.Equal(ClientError.NotFound, error);
        }

        [Fact]
        public void TestGetFoodItemsByItemStorageId() {
            Assert.Empty(_manager.GetFoodItems(out _));

            var testItem = new Item() {
                ItemId = 7,
                ItemStorageId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            var testItem2 = new Item() {
                ItemId = 8,
                ItemStorageId = 7,
                Name = "CoolCucumbers",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            var testItem3 = new Item() {
                ItemId = 9,
                ItemStorageId = 8,
                Name = "CoolTomatoes",
                Amount = 3,
                Unit = 0,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            _manager.PostFoodItem(testItem);
            _manager.PostFoodItem(testItem2);
            _manager.PostFoodItem(testItem3);

            Assert.NotEmpty(_manager.GetFoodItems(out _));

            Assert.Equal(3, _manager.GetFoodItems(out var foundFood).Count());
            Assert.False(foundFood);

            Assert.Single(_manager.GetFoodItems(out foundFood, itemStorageId: 8));
            Assert.True(foundFood);
            Assert.Equal(2, _manager.GetFoodItems(out foundFood, itemStorageId: 7).Count());

            Assert.True(foundFood);
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
        public void TestPostUserAndPatchUser() {
            Assert.Empty(_manager.GetUsers());

            var testUser = new User() {
                UserId = 7,
                Username = "Barry",
                Password = "1234",
            };

            _manager.PostUser(testUser);

            Assert.NotEmpty(_manager.GetUsers());

            Assert.Equal(testUser, _manager.GetUser(7));

            var jsonPatch = new JsonPatchDocument<User>(new List<Operation<User>>() { new("replace", "/username", "", "Larry"), new("replace", "/password", "", "12345") }, new DefaultContractResolver());

            _manager.PatchUser(7, jsonPatch);

            Assert.Equal("Larry", _manager.GetUser(7)?.Username);
            Assert.Equal("12345", _manager.GetUser(7)?.Password);
        }

        [Fact]
        public void TestPostUsersWithSameUsernameAndGetUsersByUsername() {
            Assert.Empty(_manager.GetUsers());

            var testUser = new User() {
                UserId = 7,
                Username = "Barry",
                Password = "1234",
            };

            var testUser2 = new User() {
                UserId = 8,
                Username = "Barry",
                Password = "1234",
            };

            var testUser3 = new User() {
                UserId = 9,
                Username = "Herman",
                Password = "1234",
            };

            _manager.PostUser(testUser);
            var error = _manager.PostUser(testUser2).error;

            Assert.Equal(ClientError.Conflict, error);

            _manager.PostUser(testUser3);

            Assert.NotEmpty(_manager.GetUsers());
            Assert.Equal(2, _manager.GetUsers().Count());

            Assert.Single(_manager.GetUsers(username: "Herman"));
            Assert.Single(_manager.GetUsers(username: "Barry"));
        }
        #endregion

        #region ItemStorages
        [Fact]
        public void TestPostItemStorageAndDeleteItemStorage() {
            Assert.Empty(_manager.GetItemStorages());

            var testItemStorage = new ItemStorage() {
                Name = "TheDump",
                UserId = 7,
                ItemStorageId = 7
            };

            _manager.PostItemStorage(testItemStorage);

            Assert.NotEmpty(_manager.GetItemStorages());

            Assert.Single(_manager.GetItemStorages());

            Assert.Equal(testItemStorage, _manager.GetItemStorage(7));

            _manager.DeleteItemStorage(7);

            Assert.Null(_manager.GetItemStorage(7));

            Assert.Empty(_manager.GetItemStorages());
        }

        [Fact]
        public void TestGetItemStoragesByUserId() {
            Assert.Empty(_manager.GetItemStorages());

            var testItemStorage = new ItemStorage() {
                Name = "TheDump",
                UserId = 7,
                ItemStorageId = 7
            };

            var testItemStorage2 = new ItemStorage() {
                Name = "MyTummy",
                UserId = 7,
                ItemStorageId = 7
            };

            var testItemStorage3 = new ItemStorage() {
                Name = "GarbagePail",
                UserId = 8,
                ItemStorageId = 7
            };

            _manager.PostItemStorage(testItemStorage);
            _manager.PostItemStorage(testItemStorage2);
            _manager.PostItemStorage(testItemStorage3);

            Assert.NotEmpty(_manager.GetItemStorages());
            Assert.Equal(3, _manager.GetItemStorages().Count());

            Assert.Single(_manager.GetItemStorages(userId: 8));
            Assert.Equal(2, _manager.GetItemStorages(userId: 7).Count());
        }

        [Fact]
        public void TestPostItemStorageAndPatchItemStorage() {
            Assert.Empty(_manager.GetItemStorages());

            var testItemStorage = new ItemStorage() {
                Name = "TheDump",
                UserId = 7,
                ItemStorageId = 7
            };

            _manager.PostItemStorage(testItemStorage);

            Assert.NotEmpty(_manager.GetItemStorages());

            Assert.Equal(testItemStorage, _manager.GetItemStorage(7));

            var jsonPatch = new JsonPatchDocument<ItemStorage>(new List<Operation<ItemStorage>>() { new("replace", "/name", "", "MyTummy") }, new DefaultContractResolver());

            _manager.PatchItemStorage(7, jsonPatch);

            Assert.Equal("MyTummy", _manager.GetItemStorage(7)?.Name);
        }

        [Fact]
        public void TestDeleteItemStorageNotFound() {
            Assert.Empty(_manager.GetItemStorages());

            var error = _manager.DeleteItemStorage(7);

            Assert.Equal(ClientError.NotFound, error);
        }

        [Fact]
        public void TestDeleteItemStorageConflict() {
            Assert.Empty(_manager.GetItemStorages());

            var testItemStorage = new ItemStorage() {
                Name = "TheDump",
                UserId = 7,
                ItemStorageId = 7
            };

            _manager.PostItemStorage(testItemStorage);

            var testItem = new Item() {
                ItemId = 7,
                Name = "CoolPeanuts",
                Amount = 3,
                Unit = 0,
                ItemStorageId = 7,
                CaloriesPer = 200,
                ExpirationDate = new DateTime(2022, 12, 24)
            };

            _manager.PostFoodItem(testItem);

            var error = _manager.DeleteItemStorage(7);
            var itemStorage = _manager.GetItemStorages(7).FirstOrDefault();
            Assert.Null(itemStorage);
        }
        #endregion

        [Fact]
        public void TestGetRandomMeals()
        {
            //arrange
            var randomImages = 5;

            //act
            var result = _mealsManager.GetRandomMeals(randomImages);

            //assert
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void TestGetMealsById()
        {
            //arrange
            var mealId = "53016";

            //act
            var result = _mealsManager.GetFoodById(mealId);

            //assert
            Assert.NotNull(result);
            Assert.True(result.StrMeal.Equals("Chick-Fil-A Sandwich"));
        }

        [Fact]
        public void TestGetMealsByIngredient()
        {
            //arrange
            var ingredient = "Milk";

            //act
            var result = _mealsManager.GetFood(ingredient);

            //assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}