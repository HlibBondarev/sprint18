using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAuthenticationAuthorization.Models;

namespace TaskAuthenticationAuthorization
{
    public class SampleData
    {
        public static void Initialize(ShoppingContext context)
        {
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }
            context.Products.AddRange(
                    new Product
                    {
                        Name = "Butter",
                        Price = 30.0
                    },
                    new Product
                    {
                        Name = "Banana",
                        Price = 20.50
                    },
                    new Product
                    {
                        Name = "Cola",
                        Price = 9.30
                    }
                );
            context.SaveChanges();

            context.SuperMarkets.AddRange(
                    new SuperMarket
                    {
                        Name = "Wellmart",
                        Address = "Lviv",
                    },
                    new SuperMarket
                    {
                        Name = "Billa",
                        Address = "Odessa",
                    }
                );
            context.SaveChanges();

            context.Roles.AddRange(
                    new Role
                    {
                        Name = "admin"
                    },
                    new Role
                    {
                        Name = "buyer"
                    }
                );
            context.SaveChanges();

            context.Users.AddRange(
                    new User
                    {
                        Email = "mail_1@gmail.com",
                        Password = "1",
                        TypeOfBuyer = BuyerType.Golden,
                        RoleId = 1
                    },
                    new User
                    {
                        Email = "mail_2@gmail.com",
                        Password = "2",
                        TypeOfBuyer = BuyerType.Golden,
                        RoleId = 2
                    },
                    new User
                    {
                        Email = "mail_3@gmail.com",
                        Password = "3",
                        TypeOfBuyer = BuyerType.None,
                        RoleId = 2
                    }
                );
            context.SaveChanges();

            context.Customers.AddRange(
                    new Customer
                    {
                        UserId = 1,
                        FirstName = "Ostap",
                        LastName = "Bender",
                        Address = "Rio de Zhmerinka",
                        Discount = Discount.O,
                    },
                    new Customer
                    {
                        UserId = 2,
                        FirstName = "Shura",
                        LastName = "Balaganov",
                        Address = "Odessa",
                        Discount = Discount.R,
                    },
                    new Customer
                    {
                        UserId = 3,
                        FirstName = "Mikhail",
                        LastName = "Panikovsky",
                        Address = "Kyiv",
                        Discount = Discount.V,
                    }
                );
            context.SaveChanges();

            context.Orders.AddRange(
                    new Order
                    {
                        CustomerId = 1,
                        SuperMarketId = 1,
                        OrderDate = DateTime.Now,
                    },
                    new Order
                    {
                        CustomerId = 1,
                        SuperMarketId = 1,
                        OrderDate = DateTime.Now,
                    }
                );
            context.SaveChanges();

            context.OrderDetails.AddRange(
                    new OrderDetail
                    {
                        OrderId = 1,
                        ProductId = 1,
                        Quantity = 2
                    },
                    new OrderDetail
                    {
                        OrderId = 2,
                        ProductId = 2,
                        Quantity = 1
                    }
                );
            context.SaveChanges();
        }
    }
}
