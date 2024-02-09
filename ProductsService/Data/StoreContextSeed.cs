
using Microsoft.EntityFrameworkCore;
using ProductsService.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductsService.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brands = new List<ProductBrand>
        {
            new ProductBrand { Name = "Gloves" },
            new ProductBrand { Name = "Boots" },
            new ProductBrand { Name = "Boards" },
            new ProductBrand { Name = "Hats" },
            new ProductBrand { Name = "watches" },
            new ProductBrand { Name = "wrist watches" },
            new ProductBrand { Name = "Computers" }
        };

                context.ProductBrands.AddRange(brands);
            }

            if (!context.ProductTypes.Any())
            {
                var types = new List<ProductType>
        {
            new ProductType { Name = "Angular" },
            new ProductType { Name = "NetCore" },
            new ProductType { Name = "VS Code" },
            new ProductType { Name = "React" },
            new ProductType { Name = "Typescript" },
            new ProductType { Name = "Redis" },
            new ProductType { Name = "sonata" },
            new ProductType { Name = "sonata(titan)" },
            new ProductType { Name = "new" },
            new ProductType { Name = "Rolex" },
            new ProductType { Name = "lenovo" }
        };

                context.ProductTypes.AddRange(types);
            }
            if (!context.Products.Any())
            {
                var products = new List<Product>
        {
          
                new Product
    {
        Name = "Green Angular Board 3000",
        Description = "Nunc viverra imperdiet enim. Fusce est. Vivamus a tellus.",
        Price = 150,
        PictureUrl = "images/products/sb-ang2.png",
        ProductTypeId = 1,
        ProductBrandId = 1,
        AvailableQuantity = 100,
        Discount = 5,
        Specification = "This is specification"
    },
    new Product
    {
        Name = "Core Purple Boots",
        Description = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Proin pharetra nonummy pede. Mauris et orci.",
        Price = 199,
        PictureUrl = "images/products/boot-core1.png",
        ProductTypeId = 2,
        ProductBrandId = 2,
        AvailableQuantity = 100,
        Discount = 5,
        Specification = "This is specification"
    },
    new Product
    {
        Name = "Core Red Boots",
        Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Maecenas porttitor congue massa. Fusce posuere, magna sed pulvinar ultricies, purus lectus malesuada libero, sit amet commodo magna eros quis urna.",
        Price = 189,
        PictureUrl = "images/products/boot-core2.png",
        ProductTypeId = 2,
        ProductBrandId = 2,
        AvailableQuantity = 100,
        Discount = 5,
        Specification = "This is specification"
    },
   
    new Product
    {
        Name = "lenovo laptop",
        Description = "this is a brand new lenovo laptop ",
        Price = 1123,
        PictureUrl = "images/products/fz6h27ovdihcie0zykpx2bbly2q2p6851556.jpeg",
        ProductTypeId = 1,
        ProductBrandId = 1,
        AvailableQuantity = 1232,
        Discount = 5,
        Specification = ""
    }
};

           

                context.Products.AddRange(products);
            }
            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
          


        }
    }
}
