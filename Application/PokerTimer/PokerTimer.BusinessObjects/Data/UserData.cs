﻿using System;
using System.Collections.Generic;
using System.Linq;
using PokerTimer.BusinessObjects.DataClasses;

namespace PokerTimer.BusinessObjects.Data
{
    public class UserData
    {
        public static List<User> GetList(eConnectionString cs)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.Users.ToList();
            }
        }

        public static List<User> GetListBySearchString(eConnectionString cs, string searchString)
        {
            using (var app = new BusinessDataContext(cs))
            {
                return app.Users.Where(u =>
                    u.NickName.ToLower().Contains(searchString.ToLower())
                    ||
                    u.FirstName.ToLower().Contains(searchString.ToLower())
                    ||
                    u.LastName.ToLower().Contains(searchString.ToLower())
                    ||
                    (u.FirstName + " " + u.LastName).ToLower().StartsWith(searchString.ToLower())
                    ).ToList();
            }
        }

        public static User Create(eConnectionString cs, User entity)
        {
            entity.DateCreated = DateTime.Now;
            entity.UserId = Guid.NewGuid();
            entity.PeronalId = string.Empty;
            entity.Comment = string.Empty;
            entity.Password = string.Empty;

            using (var app = new BusinessDataContext(cs))
            {
                app.Users.InsertOnSubmit(entity);
                app.SubmitChanges();
            }

            return entity;
        }

        public static void Insert(eConnectionString cs, User entity)
        {
            using (var app = new BusinessDataContext(cs))
            {
                if (!IsExist(cs, entity.UserId))
                {
                    app.Users.InsertOnSubmit(entity);
                    app.SubmitChanges();
                }
                else
                {
                    var loadedUser = app.Users.SingleOrDefault(u => u.UserId == entity.UserId);

                    loadedUser.Comment = entity.Comment;
                    loadedUser.Email = entity.Email;
                    loadedUser.FirstName = entity.FirstName;
                    loadedUser.IsBlocked = entity.IsBlocked;
                    loadedUser.NickName = entity.NickName;

                    app.SubmitChanges();
                }
            }
        }

        public static bool IsExist(eConnectionString cs, Guid id)
        {
            using (var app = new BusinessDataContext(cs))
            {
                var user = app.Users.SingleOrDefault(u => u.UserId == id);

                return user.IsNotNull();
            }
        }
    }
}