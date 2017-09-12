using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QualityBags.Models;

namespace QualityBags.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any bags.
            if (context.Bags.Any())
            {
                return; //DB has been seeded
            }

            var categories = new Category[] {
                new Category {Name = "Handbag"},
                new Category {Name = "Backpack"},
                new Category {Name = "Tote"},
                new Category {Name = "Satchel"}
            };

            foreach (var c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            //var suppliers = new Supplier[]
            //{
            //    new Supplier {Name = "Supplier A", MobilePhoneNumber = "021-111111", EmailAddress = "111111@gmail.com" },
            //    new Supplier {Name = "Supplier B", MobilePhoneNumber = "021-222222", EmailAddress = "222222@gmail.com" },
            //    new Supplier {Name = "Supplier C", MobilePhoneNumber = "021-333333", EmailAddress = "333333@gmail.com" }
            //};

            //foreach (var s in suppliers)
            //{
            //    context.Suppliers.Add(s);
            //}
            //context.SaveChanges();

            //var bags = new Bag[]
            //{
            //    new Bag {Name = "Leather Holdall 1", Description = "Description 1", Price = Convert.ToDecimal(149.95), PathOfFile = "/Images/bag1.jpg", CategoryID = categories.Single( c => c.Name == "Handbag").CategoryID, SupplierID = suppliers.Single( s => s.Name == "Supplier A").SupplierID },
            //    new Bag {Name = "Leather Holdall 2", Description = "Description 2", Price = Convert.ToDecimal(249.95), PathOfFile = "/Images/bag2.jpg", CategoryID = categories.Single( c => c.Name == "Wallet").CategoryID, SupplierID = suppliers.Single( s => s.Name == "Supplier B").SupplierID },
            //    new Bag {Name = "Leather Holdall 3", Description = "Description 3", Price = Convert.ToDecimal(349.95), PathOfFile = "/Images/bag3.jpg", CategoryID = categories.Single( c => c.Name == "Handbag").CategoryID, SupplierID = suppliers.Single( s => s.Name == "Supplier C").SupplierID },
            //    new Bag {Name = "Leather Holdall 4", Description = "Description 4", Price = Convert.ToDecimal(449.95), PathOfFile = "/Images/bag4.jpg", CategoryID = categories.Single( c => c.Name == "Backpack").CategoryID, SupplierID = suppliers.Single( s => s.Name == "Supplier A").SupplierID },
            //    new Bag {Name = "Leather Holdall 5", Description = "Description 5", Price = Convert.ToDecimal(549.95), PathOfFile = "/Images/bag5.jpg", CategoryID = categories.Single( c => c.Name == "Wallet").CategoryID, SupplierID = suppliers.Single( s => s.Name == "Supplier B").SupplierID }
            //};

            //foreach (var b in bags)
            //{
            //    context.Bags.Add(b);
            //}
            //context.SaveChanges();


        }
    }
}
