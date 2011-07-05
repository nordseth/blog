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
        public UserRepo()
        {
        }

        public UserRepo(IDocumentSession session)
        {
            Session = session;
        }

        public IDocumentSession Session { get; set; }

        public UserData LoadUser(string userId)
        {
            return Session.Load<UserData>(userId);
        }

        public UserData CreateUser(string identity)
        {
            var user = new UserData();
            var id = new Identity { Id = "Identities/" + identity };
            user.IdentityIds.Add("Identities/" + identity);
            Session.Store(user);
            id.UserId = user.Id;
            Session.Store(id);
            return user;
        }

        public void AddIdentityToUser(UserData user, string identity)
        {
            user.IdentityIds.Add("Identities/" + identity);
            Session.Store(new Identity { Id = "Identities/" + identity, UserId = user.Id });
        }

        public void DeleteUser(UserData user)
        {
            foreach (var i in user.IdentityIds)
                Session.Delete(Session.Load<Identity>( i));
            Session.Delete(user);
        }

        public void RemoveIdentityFromUser(UserData user, string identity)
        {
            //var userTmp = LoadUser(user.Id);
            //userTmp.IdentityIds.Remove(identity);
            user.IdentityIds.Remove("Identities/" + identity);
            Session.Delete(Session.Load<Identity>("Identities/" + identity));
        }

        public UserData GetUserByIdentity(string identity)
        {
            var tmp = Session.Load<Identity>("Identities/" + identity);
            if (tmp != null)
                return LoadUser(tmp.UserId);
            else
                return null;
        }
    }
}
