using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Blog.Model;
using Blog.Core;

namespace Blog.Dal
{
    public class UserRepo
    {
        public UserData LoadUser(string userId)
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                return session.Load<UserData>(userId);
            }
        }

        public UserData CreateUser(string identity)
        {
            UserData user;
            using (var session = RavenDocumentStore.OpenSession())
            {
                session.Advanced.AllowNonAuthoritiveInformation = false;
                user = new UserData();
                session.Store(user);
                var id = new Identity { Id = "Identities/" + identity, UserId = user.Id };
                session.Store(id);
                user.IdentityIds.Add(id.Id);
                session.SaveChanges();
            }
            return user;
        }

        public void AddIdentityToUser(UserData user, string identity)
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                var tmpUser = session.Load<UserData>(user.Id);
                var id = new Identity { Id = "Identities/" + identity, UserId = tmpUser.Id };
                session.Store(id);
                tmpUser.IdentityIds.Add(id.Id);
                session.SaveChanges();
            }
        }

        public void DeleteUser(UserData user)
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                var tmpUser = session.Load<UserData>(user.Id);
                foreach (var i in tmpUser.IdentityIds)
                    session.Delete(session.Load<Identity>(i));
                session.Delete(tmpUser);
                session.SaveChanges();
            }
        }

        public void RemoveIdentityFromUser(UserData user, string identity)
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                var tmpUser = session.Load<UserData>(user.Id);
                var id = session.Load<Identity>("Identities/" + identity);
                tmpUser.IdentityIds.Remove(id.Id);
                session.Delete(id);
                session.SaveChanges();
            }
        }

        public UserData GetUserByIdentity(string identity)
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                var tmp = session.Load<Identity>("Identities/" + identity);
                if (tmp != null)
                    return LoadUser(tmp.UserId);
                else
                    return null;
            }
        }

        public void SaveUser(UserData user)
        {
            using (var session = RavenDocumentStore.OpenSession())
            {
                session.Store(user);
                session.SaveChanges();
            }
        }
    }
}
