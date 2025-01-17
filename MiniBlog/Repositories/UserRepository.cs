﻿using MiniBlog.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniBlog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> userCollection;
        public UserRepository(IMongoClient mongoClient)
        {
            var mongoDatabase = mongoClient.GetDatabase("MiniBlog");
            userCollection = mongoDatabase.GetCollection<User>(User.CollectionName);
        }

        public async Task<List<User>> GetUsers() =>
            await userCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserByName(string name)
        {
            var user = await userCollection.Find(u => u.Name == name).FirstAsync();
            if (user == null)
            {
                return null;
            }

            return user;
        }
           
        public async Task<User> Register(User user)
        {
            await userCollection.InsertOneAsync(user);
            return await userCollection.Find(u => u.Name == user.Name).FirstAsync();
        }
    }
}
