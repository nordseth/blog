using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blog.Core;
using Blog.Model;
using Blog.Dal;
using Raven.Client.Linq;

namespace Test
{
    [TestClass]
    public class RavenUserData
    {
        public string Id = new Encryption().Random(10).ToHex();

        [TestMethod]
        public void UserData_DeleteAll()
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                session.Advanced.UseOptimisticConcurrency = true;
                RavenQueryStatistics stats;
                var identities = session.Query<Identity>().Customize(x => x.WaitForNonStaleResults()).Statistics(out stats);
                Console.WriteLine(identities.Count());
                Console.WriteLine(stats);
                foreach (var id in identities)
                {
                    Console.WriteLine(id.Id);
                    session.Delete(id);
                }
                var users = session.Query<UserData>().Customize(x => x.WaitForNonStaleResults()).Statistics(out stats);
                Console.WriteLine(users.Count());
                Console.WriteLine(stats);
                foreach (var u in users)
                {
                    Console.WriteLine(u.Id);
                    session.Delete(u);
                }
                session.SaveChanges();
            }
        }

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
            int preUsers = 0;
            int preIds = 0;
            using (var session = RavenDocumentStore.OpenSession())
            {
                session.Advanced.AllowNonAuthoritiveInformation = false;
                var identities = session.Query<Identity>();
                preIds = identities.Count();
                var users = session.Query<UserData>();
                preUsers = users.Count();
            }

            UserRepo repo = new UserRepo();
            string savedId;

            var id = repo.GetUserByIdentity(Id);
            Assert.IsNull(id);

            var user1 = repo.CreateUser(Id);
            savedId = user1.Id;
            user1.Claims.Add(new SimpleClaim { Subject = Id, ClaimType = "test", Issuer = "noone", Value = "dummy" });
            repo.SaveUser(user1);

            var user2 = repo.GetUserByIdentity(Id);
            Assert.IsNotNull(user2);
            Assert.AreEqual(savedId, user2.Id);
            Assert.IsTrue(user2.IdentityIds.First().Contains(Id));
            Assert.IsTrue(user2.Claims[0].Issuer == "noone");

            var user3 = repo.GetUserByIdentity(Id);
            Assert.IsNotNull(user3);
            Assert.AreEqual(savedId, user3.Id);
            repo.DeleteUser(user3);

            var user4 = repo.GetUserByIdentity(Id);
            Assert.IsNull(user4);

            using (var session = RavenDocumentStore.OpenSession())
            {
                session.Advanced.AllowNonAuthoritiveInformation = false;
                var identities = session.Query<Identity>();
                Assert.AreEqual(preIds, identities.Count());
                var users = session.Query<UserData>();
                Assert.AreEqual(preUsers, users.Count());
            }

        }
    }
}
