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
            Description = "Socks, sports, apparel",
            Image = "images/products/athletic-cotton-socks-6-pairs.jpg"
        },
        new()
        {
            Id = 2,
            Name = "Intermediate Size Basketball",
            Price = 20.95m,
            Description = "Sports, basketballs",
            Image = "images/products/intermediate-composite-basketball.jpg"
        },
        new()
        {
            Id = 3,
            Name = "Adults Plain Cotton T-Shirt - 2 Pack",
            Price = 7.99m,
            Description = "Tshirts, apparel, mens",
            Image = "images/products/adults-plain-cotton-tshirt-2-pack-teal.jpg"
        },
        new()
        {
            Id = 4,
            Name = "2 Slot Toaster - White",
            Price = 18.99m,
            Description = "Toaster, kitchen, appliances",
            Image = "images/products/2-slot-toaster-white.jpg"
        },
        new()
        {
            Id = 5,
            Name = "2 Piece White Dinner Plate Set",
            Price = 20.67m,
            Description = "Plates, kitchen, dining",
            Image = "images/products/elegant-white-dinner-plate-set.jpg"
        },
        new()
        {
            Id = 6,
            Name = "3 Piece Non-Stick, Black Cooking Pot Set",
            Price = 34.99m,
            Description = "Kitchen, cookware",
            Image = "images/products/3-piece-cooking-set.jpg"
        },
        new()
        {
            Id = 7,
            Name = "Cotton Oversized Sweater - Gray",
            Price = 24.00m,
            Description = "Sweaters, apparel",
            Image = "images/products/women-plain-cotton-oversized-sweater-gray.jpg"
        },
        new()
        {
            Id = 8,
            Name = "2 Piece Luxury Towel Set - White",
            Price = 35.99m,
            Description = "Bathroom, washroom, towels",
            Image = "images/products/luxury-towel-set.jpg"
        },
        new()
        {
            Id = 9,
            Name = "Ultra Soft Tissue 2-Ply - 8 Boxes",
            Price = 23.74m,
            Description = "Tissues, kitchen, napkins",
            Image = "images/products/facial-tissue-2-ply-8-boxes.jpg"
        },
        new()
        {
            Id = 10,
            Name = "Women's Striped Beach Dress",
            Price = 29.70m,
            Description = "Robe, swimsuit, swimming, apparel",
            Image = "images/products/women-striped-beach-dress.jpg"
        },
        new()
        {
            Id = 11,
            Name = "Women's Sandal Heels - Pink",
            Price = 53.00m,
            Description = "Garbage, bins, cans, kitchen",
            Image = "images/products/women-sandal-heels-white-pink.jpg"
        },
        new()
        {
            Id = 12,
            Name = "Round Sunglasses",
            Price = 35.60m,
            Description = "Accessories, shades",
            Image = "images/products/round-sunglasses-gold.jpg"
        },
        new()
        {
            Id = 13,
            Name = "Blackout Curtains Set - Beige",
            Price = 45.99m,
            Description = "Bedroom, curtains, home",
            Image = "images/products/blackout-curtain-set-beige.jpg"
        },
        new()
        {
            Id = 14,
            Name = "Women's Summer Jean Shorts",
            Price = 16.99m,
            Description = "Shorts, apparel, mens",
            Image = "images/products/women-summer-jean-shorts.jpg"
        },
        new()
        {
            Id = 15,
            Name = "Electric Hot Water Kettle - White",
            Price = 50.74m,
            Description = "Water kettle, appliances, kitchen",
            Image = "images/products/electric-steel-hot-water-kettle-white.jpg"
        },
        new()
        {
            Id = 16,
            Name = "Waterproof Knit Athletic Sneakers - Gray",
            Price = 53.90m,
            Description = "Shoes, running shoes, footwear",
            Image = "images/products/knit-athletic-sneakers-gray.jpg"
        },
        new()
        {
            Id = 17,
            Name = "Straw Wide Brim Sun Hat",
            Price = 22.00m,
            Description = "Hats, straw hats, summer, apparel",
            Image = "images/products/straw-sunhat.jpg"
        },
        new()
        {
            Id = 18,
            Name = "Men's Athletic Sneaker - White",
            Price = 45.90m,
            Description = "Shoes, running shoes, footwear, mens",
            Image = "images/products/men-athletic-shoes-white.jpg"
        },
        new()
        {
            Id = 19,
            Name = "Men's Wool Sweater - Black",
            Price = 33.74m,
            Description = "Sweaters, apparel",
            Image = "images/products/men-stretch-wool-sweater-black.jpg"
        },
        new()
        {
            Id = 20,
            Name = "Bathroom Bath Mat 16 x 32 Inch - Grey",
            Price = 18.50m,
            Description = "Bathmat, bathroom, home",
            Image = "images/products/bathroom-mat.jpg"
        },
        new()
        {
            Id = 21,
            Name = "Women's Ballet Flat - White",
            Price = 26.40m,
            Description = "Shoes, flats, womens, footwear",
            Image = "images/products/women-knit-ballet-flat-white.jpg"
        },
        new()
        {
            Id = 22,
            Name = "Men's Golf Polo Shirt - Gray",
            Price = 15.99m,
            Description = "Tshirts, shirts, apparel, mens",
            Image = "images/products/men-golf-polo-t-shirt-gray.jpg"
        },
        new()
        {
            Id = 23,
            Name = "Laundry Detergent Tabs, 50 Loads",
            Price = 28.99m,
            Description = "Bathroom, cleaning",
            Image = "images/products/laundry-detergent-tabs.jpg"
        },
        new()
        {
            Id = 24,
            Name = "Sterling Silver Leaf Branch Earrings",
            Price = 67.99m,
            Description = "Jewelry, accessories, womens",
            Image = "images/products/sky-leaf-branch-earrings.jpg"
        },
        new()
        {
            Id = 25,
            Name = "Duvet Cover Set, Diamond Pattern",
            Price = 43.99m,
            Description = "Bedroom, bed sheets, sheets, covers, home",
            Image = "images/products/duvet-cover-set-gray-queen.jpg"
        },
        new()
        {
            Id = 26,
            Name = "Women's Knit Winter Beanie - Blue",
            Price = 19.50m,
            Description = "Hats, winter hats, beanies, apparel, womens",
            Image = "images/products/women-knit-beanie-pom-pom-blue.jpg"
        },
        new()
        {
            Id = 27,
            Name = "Men's Chino Pants - Beige",
            Price = 22.90m,
            Description = "Pants, apparel, mens",
            Image = "images/products/men-chino-pants-beige.jpg"
        },
        new()
        {
            Id = 28,
            Name = "Men's Navigator Sunglasses",
            Price = 36.90m,
            Description = "Sunglasses, glasses, accessories, shades",
            Image = "images/products/men-navigator-sunglasses-black.jpg"
        },
        new()
        {
            Id = 29,
            Name = "Men's Brown Flat Sneakers",
            Price = 24.99m,
            Description = "Footwear, men, sneakers",
            Image = "images/products/men-brown-flat-sneakers.jpg"
        },
        new()
        {
            Id = 30,
            Name = "Non-Stick Cook Set With Lids - 4 Pieces",
            Price = 67.97m,
            Description = "Cooking set, kitchen",
            Image = "images/products/non-stick-cooking-set-4-pieces.jpg"
        },
        new()
        {
            Id = 31,
            Name = "Vanity Mirror with LED Lights - Pink",
            Price = 25.49m,
            Description = "Bathroom, washroom, mirrors, home",
            Image = "images/products/vanity-mirror-pink.jpg"
        },
        new()
        {
            Id = 32,
            Name = "Women's Relaxed Lounge Pants - Pink",
            Price = 34.00m,
            Description = "Pants, apparel, womens",
            Image = "images/products/women-relaxed-lounge-pants-pink.jpg"
        },
        new()
        {
            Id = 33,
            Name = "Crystal Zirconia Stud Earrings - Pink",
            Price = 34.67m,
            Description = "Accessories, womens",
            Image = "images/products/crystal-zirconia-stud-earrings-pink.jpg"
        },
        new()
        {
            Id = 34,
            Name = "Glass Screw Lid Containers - 3 Pieces",
            Price = 28.99m,
            Description = "Food containers, kitchen",
            Image = "images/products/glass-screw-lid-food-containers.jpg"
        },
        new()
        {
            Id = 35,
            Name = "Black and Silver Espresso Maker",
            Price = 82.50m,
            Description = "Espresso makers, kitchen, appliances",
            Image = "images/products/black-and-silver-espresso-maker.jpg"
        },
        new()
        {
            Id = 36,
            Name = "Blackout Curtains Set 42 x 84-Inch - Teal",
            Price = 30.99m,
            Description = "Bedroom, home, curtains",
            Image = "images/products/blackout-curtains-set-teal.jpg"
        },
        new()
        {
            Id = 37,
            Name = "Bath Towels 2 Pack - Gray, Rosewood",
            Price = 29.90m,
            Description = "Bathroom, home, towels",
            Image = "images/products/bath-towel-set-gray-rosewood.jpg"
        },
        new()
        {
            Id = 38,
            Name = "Athletic Skateboard Shoes - Gray",
            Price = 33.90m,
            Description = "Shoes, running shoes, footwear",
            Image = "images/products/athletic-skateboard-shoes-gray.jpg"
        },
        new()
        {
            Id = 39,
            Name = "Countertop Push Blender - Black",
            Price = 107.47m,
            Description = "Food blenders, kitchen, appliances",
            Image = "images/products/countertop-push-blender-black.jpg"
        },
        new()
        {
            Id = 40,
            Name = "Men's Fleece Hoodie - Light Teal",
            Price = 38.00m,
            Description = "Sweaters, hoodies, apparel, mens",
            Image = "images/products/men-cozy-fleece-hoodie-light-teal.jpg"
        },
        new()
        {
            Id = 41,
            Name = "Artistic Bowl Set - 6 Pieces",
            Price = 26.99m,
            Description = "Bowls, kitchen, dining",
            Image = "images/products/artistic-bowl-set-6-piece.jpg"
        },
        new()
        {
            Id = 42,
            Name = "Kitchen Paper Towels - 8 Rolls",
            Price = 12.49m,
            Description = "Paper towels, kitchen, cleaning",
            Image = "images/products/kitchen-paper-towels-8-pack.jpg"
        },
        new()
        {
            Id = 43,
            Name = "Lash Princess Volumizing Mascara",
            Price = 9.99m,
            Description = "Beauty, makeup, mascara, womens",
            Image = "images/products/essence-mascara-lash-princess.png"
        },
        new()
        {
            Id = 44,
            Name = "Eyeshadow Palette with Mirror",
            Price = 19.99m,
            Description = "Beauty, makeup, eyeshadow, womens",
            Image = "images/products/eyeshadow-palette-with-mirror.png"
        },
        new()
        {
            Id = 45,
            Name = "Red Matte Lipstick",
            Price = 12.99m,
            Description = "Beauty, makeup, lipstick, womens",
            Image = "images/products/red-lipstick.png"
        },
        new()
        {
            Id = 46,
            Name = "Wood Base Table Lamp",
            Price = 49.99m,
            Description = "Lamps, lighting, home decor, bedroom",
            Image = "images/products/table-lamp-wood-base.png"
        },
        new()
        {
            Id = 47,
            Name = "Family Tree Photo Frame",
            Price = 29.99m,
            Description = "Frames, home decor, living room",
            Image = "images/products/family-tree-photo-frame.png"
        },
        new()
        {
            Id = 48,
            Name = "Artificial Showpiece Plant in Pot",
            Price = 39.99m,
            Description = "Plants, home decor, living room",
            Image = "images/products/house-showpiece-plant.png"
        },
        new()
        {
            Id = 49,
            Name = "Carbon Steel Wok with Wooden Handle",
            Price = 29.99m,
            Description = "Woks, kitchen, cookware",
            Image = "images/products/carbon-steel-wok.png"
        },
        new()
        {
            Id = 50,
            Name = "Wooden Chopping Board",
            Price = 12.99m,
            Description = "Cutting boards, kitchen, cutlery",
            Image = "images/products/wooden-chopping-board.png"
        },
        new()
        {
            Id = 51,
            Name = "Countertop Microwave Oven - 25L",
            Price = 89.99m,
            Description = "Microwave, kitchen, appliances",
            Image = "images/products/countertop-microwave-oven.png"
        },
        new()
        {
            Id = 52,
            Name = "12-Jar Wall Mount Spice Rack",
            Price = 19.99m,
            Description = "Spices, kitchen, organizers",
            Image = "images/products/spice-rack-12-jar.png"
        },
        new()
        {
            Id = 53,
            Name = "Blue and Black Check Shirt",
            Price = 29.99m,
            Description = "Shirts, apparel, mens",
            Image = "images/products/blue-black-check-shirt.png"
        },
        new()
        {
            Id = 54,
            Name = "Men's Plaid Flannel Shirt",
            Price = 34.99m,
            Description = "Shirts, apparel, mens",
            Image = "images/products/mens-plaid-flannel-shirt.png"
        },
        new()
        {
            Id = 55,
            Name = "Nike Air Jordan Retro Sneakers - Red/Black",
            Price = 149.99m,
            Description = "Shoes, sneakers, footwear, mens",
            Image = "images/products/nike-air-jordan-sneakers.png"
        },
        new()
        {
            Id = 56,
            Name = "Puma Future Rider Trainers",
            Price = 89.99m,
            Description = "Shoes, sneakers, footwear",
            Image = "images/products/puma-future-rider-trainers.png"
        },
        new()
        {
            Id = 57,
            Name = "Brown Leather Belt Watch",
            Price = 89.99m,
            Description = "Watches, accessories, mens",
            Image = "images/products/brown-leather-belt-watch.png"
        },
        new()
        {
            Id = 58,
            Name = "Amazon Echo Plus Smart Speaker",
            Price = 99.99m,
            Description = "Electronics, speakers, smart home",
            Image = "images/products/amazon-echo-plus-speaker.png"
        },
        new()
        {
            Id = 59,
            Name = "Apple AirPods Wireless Earbuds",
            Price = 129.99m,
            Description = "Electronics, headphones, accessories",
            Image = "images/products/apple-airpods-wireless-earbuds.png"
        },
        new()
        {
            Id = 60,
            Name = "Stainless Steel Pot with Glass Lid",
            Price = 39.99m,
            Description = "Pots, kitchen, cookware",
            Image = "images/products/stainless-steel-pot-glass-lid.png"
        }
    ];
}