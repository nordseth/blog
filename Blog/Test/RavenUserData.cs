using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blog.Core;
using Blog.Model;
using Blog.Dal;

namespace Test
{
    [TestClass]
    public class RavenUserData
    {
        public string Id = new Encryption().Random(10).ToHex();

        //[TestMethod]
        //public void UserData_Create_Retrive_Delete_Verify()
        //{
        //    UserRepo repo = new UserRepo();

        //    string savedId;
        //    using (repo.Session = RavenDocumentStore.OpenSession())
        //    {
        //        var user = repo.GetUserByIdentity(Id);
        //        Assert.IsNull(user);
        //    }

        //    using (repo.Session = RavenDocumentStore.OpenSession())
        //    {
        //        var newUser = new UserData();
        //        newUser.Ids.Add(Id);
        //        newUser.Claims.Add(new SimpleClaim { ClaimType = "test", Issuer = "none", Value = "dummy" });
        //        repo.AddUser(newUser);

        //        repo.Session.SaveChanges();
        //        savedId = newUser.Id;
        //    }

        //    using (repo.Session = RavenDocumentStore.OpenSession())
        //    {
        //        var user = repo.GetUserByIdentity(Id);
        //        Assert.IsNotNull(user);
        //        Assert.AreEqual(savedId, user.Id);
        //    }

        //    using (repo.Session = RavenDocumentStore.OpenSession())
        //    {
        //        var user = repo.GetUserByIdentity(Id);
        //        Assert.IsNotNull(user);
        //        Assert.AreEqual(savedId, user.Id);
        //        repo.DeleteUser(user);
        //        repo.Session.SaveChanges();
        //    }

        //    using (repo.Session = RavenDocumentStore.OpenSession())
        //    {
        //        var user = repo.GetUserByIdentity(Id);
        //        Assert.IsNull(user);
        //    }
        //}

        [TestMethod]
        public void UserData_withreference_Create_Retrive_Delete_Verify()
        {
            UserRepo repo = new UserRepo();
            string savedId;

            using (repo.Session = RavenDocumentStore.OpenSession())
            {
                var id = repo.GetUserByIdentity(Id);
                Assert.IsNull(id);
            }

            using (repo.Session = RavenDocumentStore.OpenSession())
            {
                var user = repo.CreateUser(Id);
                user.Claims.Add(new SimpleClaim { Subject = Id, ClaimType = "test", Issuer = "noone", Value = "dummy" });
                savedId = user.Id;
                repo.Session.SaveChanges();
            }

            using (repo.Session = RavenDocumentStore.OpenSession())
            {
                var user = repo.GetUserByIdentity(Id);
                Assert.IsNotNull(user);
                Assert.AreEqual(savedId, user.Id);
            }

            using (repo.Session = RavenDocumentStore.OpenSession())
            {
                var user = repo.GetUserByIdentity(Id);
                Assert.IsNotNull(user);
                Assert.AreEqual(savedId, user.Id);
                repo.DeleteUser(user);
                repo.Session.SaveChanges();
            }

            using (repo.Session = RavenDocumentStore.OpenSession())
            {
                var user = repo.GetUserByIdentity(Id);
                Assert.IsNull(user);
            }
        }
    }
}
