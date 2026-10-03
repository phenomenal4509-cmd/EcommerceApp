using MyApp.Api.Models;

namespace MyApp.Api.Data;

public static class SeedData
{
    public static List<DeliveryOption> DeliveryOptions =>
    [
        new() { Id = 1, DeliveryDays = 7, Price = 0m, Name = "Standard" },
        new() { Id = 2, DeliveryDays = 3, Price = 4.99m, Name = "Express" },
        new() { Id = 3, DeliveryDays = 1, Price = 9.99m, Name = "Overnight" }
    ];

    public static List<Product> Products =>
    [
        new()
        {
            Id = 1,
            Name = "Black and Gray Athletic Cotton Socks - 6 Pairs",
            Price = 10.90m,
            Description = "Socks, sports, apparel"
        },
        new()
        {
            Id = 2,
            Name = "Intermediate Size Basketball",
            Price = 20.95m,
            Description = "Sports, basketballs"
        },
        new()
        {
            Id = 3,
            Name = "Adults Plain Cotton T-Shirt - 2 Pack",
            Price = 7.99m,
            Description = "Tshirts, apparel, mens"
        },
        new()
        {
            Id = 4,
            Name = "2 Slot Toaster - White",
            Price = 18.99m,
            Description = "Toaster, kitchen, appliances"
        },
        new()
        {
            Id = 5,
            Name = "2 Piece White Dinner Plate Set",
            Price = 20.67m,
            Description = "Plates, kitchen, dining"
        },
        new()
        {
            Id = 6,
            Name = "3 Piece Non-Stick, Black Cooking Pot Set",
            Price = 34.99m,
            Description = "Kitchen, cookware"
        },
        new()
        {
            Id = 7,
            Name = "Cotton Oversized Sweater - Gray",
            Price = 24.00m,
            Description = "Sweaters, apparel"
        },
        new()
        {
            Id = 8,
            Name = "2 Piece Luxury Towel Set - White",
            Price = 35.99m,
            Description = "Bathroom, washroom, towels"
        },
        new()
        {
            Id = 9,
            Name = "Ultra Soft Tissue 2-Ply - 8 Boxes",
            Price = 23.74m,
            Description = "Tissues, kitchen, napkins"
        },
        new()
        {
            Id = 10,
            Name = "Women's Striped Beach Dress",
            Price = 29.70m,
            Description = "Robe, swimsuit, swimming, apparel"
        },
        new()
        {
            Id = 11,
            Name = "Women's Sandal Heels - Pink",
            Price = 53.00m,
            Description = "Garbage, bins, cans, kitchen"
        },
        new()
        {
            Id = 12,
            Name = "Round Sunglasses",
            Price = 35.60m,
            Description = "Accessories, shades"
        },
        new()
        {
            Id = 13,
            Name = "Blackout Curtains Set - Beige",
            Price = 45.99m,
            Description = "Bedroom, curtains, home"
        },
        new()
        {
            Id = 14,
            Name = "Women's Summer Jean Shorts",
            Price = 16.99m,
            Description = "Shorts, apparel, mens"
        },
        new()
        {
            Id = 15,
            Name = "Electric Hot Water Kettle - White",
            Price = 50.74m,
            Description = "Water kettle, appliances, kitchen"
        },
        new()
        {
            Id = 16,
            Name = "Waterproof Knit Athletic Sneakers - Gray",
            Price = 53.90m,
            Description = "Shoes, running shoes, footwear"
        },
        new()
        {
            Id = 17,
            Name = "Straw Wide Brim Sun Hat",
            Price = 22.00m,
            Description = "Hats, straw hats, summer, apparel"
        },
        new()
        {
            Id = 18,
            Name = "Men's Athletic Sneaker - White",
            Price = 45.90m,
            Description = "Shoes, running shoes, footwear, mens"
        },
        new()
        {
            Id = 19,
            Name = "Men's Wool Sweater - Black",
            Price = 33.74m,
            Description = "Sweaters, apparel"
        },
        new()
        {
            Id = 20,
            Name = "Bathroom Bath Mat 16 x 32 Inch - Grey",
            Price = 18.50m,
            Description = "Bathmat, bathroom, home"
        },
        new()
        {
            Id = 21,
            Name = "Women's Ballet Flat - White",
            Price = 26.40m,
            Description = "Shoes, flats, womens, footwear"
        },
        new()
        {
            Id = 22,
            Name = "Men's Golf Polo Shirt - Gray",
            Price = 15.99m,
            Description = "Tshirts, shirts, apparel, mens"
        },
        new()
        {
            Id = 23,
            Name = "Laundry Detergent Tabs, 50 Loads",
            Price = 28.99m,
            Description = "Bathroom, cleaning"
        },
        new()
        {
            Id = 24,
            Name = "Sterling Silver Leaf Branch Earrings",
            Price = 67.99m,
            Description = "Jewelry, accessories, womens"
        },
        new()
        {
            Id = 25,
            Name = "Duvet Cover Set, Diamond Pattern",
            Price = 43.99m,
            Description = "Bedroom, bed sheets, sheets, covers, home"
        },
        new()
        {
            Id = 26,
            Name = "Women's Knit Winter Beanie - Blue",
            Price = 19.50m,
            Description = "Hats, winter hats, beanies, apparel, womens"
        },
        new()
        {
            Id = 27,
            Name = "Men's Chino Pants - Beige",
            Price = 22.90m,
            Description = "Pants, apparel, mens"
        },
        new()
        {
            Id = 28,
            Name = "Men's Navigator Sunglasses",
            Price = 36.90m,
            Description = "Sunglasses, glasses, accessories, shades"
        },
        new()
        {
            Id = 29,
            Name = "Men's Brown Flat Sneakers",
            Price = 24.99m,
            Description = "Footwear, men, sneakers"
        },
        new()
        {
            Id = 30,
            Name = "Non-Stick Cook Set With Lids - 4 Pieces",
            Price = 67.97m,
            Description = "Cooking set, kitchen"
        },
        new()
        {
            Id = 31,
            Name = "Vanity Mirror with LED Lights - Pink",
            Price = 25.49m,
            Description = "Bathroom, washroom, mirrors, home"
        },
        new()
        {
            Id = 32,
            Name = "Women's Relaxed Lounge Pants - Pink",
            Price = 34.00m,
            Description = "Pants, apparel, womens"
        },
        new()
        {
            Id = 33,
            Name = "Crystal Zirconia Stud Earrings - Pink",
            Price = 34.67m,
            Description = "Accessories, womens"
        },
        new()
        {
            Id = 34,
            Name = "Glass Screw Lid Containers - 3 Pieces",
            Price = 28.99m,
            Description = "Food containers, kitchen"
        },
        new()
        {
            Id = 35,
            Name = "Black and Silver Espresso Maker",
            Price = 82.50m,
            Description = "Espresso makers, kitchen, appliances"
        },
        new()
        {
            Id = 36,
            Name = "Blackout Curtains Set 42 x 84-Inch - Teal",
            Price = 30.99m,
            Description = "Bedroom, home, curtains"
        },
        new()
        {
            Id = 37,
            Name = "Bath Towels 2 Pack - Gray, Rosewood",
            Price = 29.90m,
            Description = "Bathroom, home, towels"
        },
        new()
        {
            Id = 38,
            Name = "Athletic Skateboard Shoes - Gray",
            Price = 33.90m,
            Description = "Shoes, running shoes, footwear"
        },
        new()
        {
            Id = 39,
            Name = "Countertop Push Blender - Black",
            Price = 107.47m,
            Description = "Food blenders, kitchen, appliances"
        },
        new()
        {
            Id = 40,
            Name = "Men's Fleece Hoodie - Light Teal",
            Price = 38.00m,
            Description = "Sweaters, hoodies, apparel, mens"
        }
    ];
}