﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace CarefulBitesAPI.Managers {
    public enum ClientError {
        Conflict
    }

    public class CarefulBitesManager {
        private ICarefulBitesDbContext _dbContext;

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
            foodItem.ItemId = null;

            var newItem = _dbContext.Items.Add(foodItem);

            _dbContext.SaveChanges();

            if (newItem != null)
                return newItem.Entity;

            return null;
        }

        public void PutFoodItem(int itemId, Item foodItem) {
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

        public User? GetUserByUserId(int userId) {
            var user = _dbContext.Users.Find(userId);

            return user;
        }

        public User? GetUserByUsername(string username) {
            var newUser = _dbContext.Users.ToList().Find(u => u.Username == username);

            return newUser;
        }

        public IEnumerable<User> GetAllUsers() {
            List<User> userList = _dbContext.Users.ToList();

            return userList;
        }

        public (User? user, ClientError? error) PostUser(User user) {
            user.UserId = null;

            var userWithSameName = _dbContext.Users.ToList().Exists(u => u.Username.Equals(user.Username));

            if (userWithSameName)
                return (null, ClientError.Conflict);

            var newUser = _dbContext.Users.Add(user);

            //try {
            //    _dbContext.SaveChanges();
            //} catch (DbUpdateException e) {
            //    Console.WriteLine(e);
            //    return null;
            //}

            return (newUser.Entity, null);
        }

        public void PutUser(int userId, User user) {
            var oldUser = _dbContext.Users.Find(userId);

            if (oldUser != null) {
                oldUser.Username = user.Username;
                oldUser.Password = user.Password;

                _dbContext.SaveChanges();
            }
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

        public IEnumerable<ItemStorage> GetAllItemStorages() {
            List<ItemStorage> itemStorageList = _dbContext.ItemStorages.ToList();

            return itemStorageList;
        }

        public ItemStorage? GetItemStorage(int itemStorageId) {
            var itemStorage = _dbContext.ItemStorages.Find(itemStorageId);

            return itemStorage;
        }

        public ItemStorage PostItemStorage(ItemStorage itemStorage) {
            itemStorage.ItemStorageId = null;

            var newItemStorage = _dbContext.ItemStorages.Add(itemStorage);

            _dbContext.SaveChanges();

            return newItemStorage.Entity;
        }

        public void PutItemStorage(int itemStorageId, ItemStorage itemStorage) {
            var oldItemStorage = _dbContext.ItemStorages.Find(itemStorageId);

            if (oldItemStorage != null) {
                oldItemStorage.Name = itemStorage.Name;
                oldItemStorage.UserId = itemStorage.UserId;

                _dbContext.SaveChanges();
            }
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
