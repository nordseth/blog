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

        public UserData GetUserByIdentity(string identity)
        {
            return Session.Query<UserData>().FirstOrDefault(u => u.Ids.Any(i => i == identity));
            //return Session.Advanced.LuceneQuery<UserData>("UserIdentityIndex").WaitForNonStaleResultsAsOfNow().Where(identity).FirstOrDefault();
        }

        public UserData GetUserById(string id)
        {
            return Session.Load<UserData>(id);
        }

        public void AddUser(UserData user)
        {
            Session.Store(user);
        }

        public void DeleteUser(UserData user)
        {
            Session.Delete(user);
        }
    }
}
