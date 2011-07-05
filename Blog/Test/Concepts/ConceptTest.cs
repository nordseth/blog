using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Concepts
{
    [TestClass]
    public class ConceptTest
    {
        [TestMethod]
        public void CreateProducts()
        {
            using (var sess = Blog.Dal.RavenDocumentStore.OpenSession())
            {
                sess.Store(new Product
                {
                    Id = 1,
                    Name = "Boks",
                    Price = 250.2,
                    PriceType = PriceType.once
                });
                sess.Store(new Product
                {
                    Id = 2,
                    Name = "Frakt",
                    Price = 49,
                    PriceType = PriceType.once
                });
                sess.Store(new Product
                {
                    Id = 3,
                    Name = "Software leie",
                    Price = 25,
                    PriceType = PriceType.periodical
                });
                sess.Store(new Product
                {
                    Id = 4,
                    Name = "Premium Boks",
                    Price = 300,
                    PriceType = PriceType.once
                });
                sess.Store(new Product
                {
                    Id = 5,
                    Name = "ekstra forsikring",
                    Price = 10,
                    PriceType = PriceType.periodical
                });

                sess.SaveChanges();
            }
        }

        [TestMethod]
        public void CreateCustomer()
        {
            using (var sess = Blog.Dal.RavenDocumentStore.OpenSession())
            {
                var products = sess.Query<Product>().ToList();

                sess.Store(new Customer
                {
                    Address = new Address
                    {
                        City = "Old York",
                        Entrance = null,
                        HouseNo = "3",
                        StreetName = "Broadway",
                        ZipCode = "12342",
                    },
                    OwnedProducts = new List<Contract>
                    {
                        new Contract {
                             Count = 1,
                             Product = products.Single(p=> p.Id == 1),
                        },
                    },
                    Person = new Person
                    {
                        Name = "Mr Black",
                        Phone = "9999",
                        Email = "b@b.com",
                    }
                });

                sess.SaveChanges();
            }
        }

        [TestMethod]
        public void CreateOrder()
        {
            using (var sess = Blog.Dal.RavenDocumentStore.OpenSession())
            {
                var products = sess.Query<Product>().ToList();
                var customer = sess.Load<Customer>(1);
                var order = new Order();
                order.Customer = customer;
                order.ShippingAddress = new Address
                {
                    StreetName = "test street",
                    City = "Boring town",
                    HouseNo = "1000",
                    ZipCode = "99999",
                };
                order.Items = new List<Contract>();

                order.Items.Add(new Contract
                {
                    Product = products.Single(p => p.Id == 2),
                    Count = 1,
                });
                order.Items.Add(new Contract
                {
                    Product = products.Single(p => p.Id == 3),
                    Count = 1,
                });
                order.Items.Add(new Contract
                {
                    Product = products.Single(p => p.Id == 4),
                    Count = 1,
                    Price = 250,
                });


                sess.Store(order);
                sess.SaveChanges();
            }
        }

        [TestMethod]
        public void RetriveOrder()
        {
            using (var sess = Blog.Dal.RavenDocumentStore.OpenSession())
            {
                var order = sess.Load<Order>(1025);
            }
        }
    }
}
