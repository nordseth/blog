//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Raven.Client.Indexes;
//using Raven.Abstractions.Indexing;
//using Blog.Model;
//using Raven.Client.Document;

//namespace Dal.Indexes
//{
//    public class UserIdentityIndex : AbstractIndexCreationTask
//    {
//        public override IndexDefinition CreateIndexDefinition()
//        {
//            return new indexdefinitionbuilder<userdata>
//            {
//                map = users => from u in users
//                               from id in u.identityids
//                               select new { id },
//            }.toindexdefinition(conventions);
//        }
//    }
//}
