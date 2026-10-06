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
        },
        new()
        {
            Id = 61,
            Name = "Powder Canister",
            Price = 14.99m,
            Description = "Beauty, cosmetics, makeup, womens",
            Image = "images/products/powder-canister.webp"
        },
        new()
        {
            Id = 62,
            Name = "Red Nail Polish",
            Price = 8.99m,
            Description = "Beauty, cosmetics, makeup, womens",
            Image = "images/products/red-nail-polish.webp"
        },
        new()
        {
            Id = 63,
            Name = "Calvin Klein CK One",
            Price = 49.99m,
            Description = "Fragrances, perfume, cologne",
            Image = "images/products/calvin-klein-ck-one.webp"
        },
        new()
        {
            Id = 64,
            Name = "Chanel Coco Noir Eau De",
            Price = 129.99m,
            Description = "Fragrances, perfume, cologne",
            Image = "images/products/chanel-coco-noir-eau-de.webp"
        },
        new()
        {
            Id = 65,
            Name = "Dior J'adore",
            Price = 89.99m,
            Description = "Fragrances, perfume, cologne",
            Image = "images/products/dior-j-adore.webp"
        },
        new()
        {
            Id = 66,
            Name = "Dolce Shine Eau de",
            Price = 69.99m,
            Description = "Fragrances, perfume, cologne",
            Image = "images/products/dolce-shine-eau-de.webp"
        },
        new()
        {
            Id = 67,
            Name = "Gucci Bloom Eau de",
            Price = 79.99m,
            Description = "Fragrances, perfume, cologne",
            Image = "images/products/gucci-bloom-eau-de.webp"
        },
        new()
        {
            Id = 68,
            Name = "Annibale Colombo Bed",
            Price = 1899.99m,
            Description = "Furniture, home decor, living room",
            Image = "images/products/annibale-colombo-bed.webp"
        },
        new()
        {
            Id = 69,
            Name = "Annibale Colombo Sofa",
            Price = 2499.99m,
            Description = "Furniture, home decor, living room",
            Image = "images/products/annibale-colombo-sofa.webp"
        },
        new()
        {
            Id = 70,
            Name = "Bedside Table African Cherry",
            Price = 299.99m,
            Description = "Furniture, home decor, living room",
            Image = "images/products/bedside-table-african-cherry.webp"
        },
        new()
        {
            Id = 71,
            Name = "Knoll Saarinen Executive Conference Chair",
            Price = 499.99m,
            Description = "Furniture, home decor, living room",
            Image = "images/products/knoll-saarinen-executive-conference-chair.webp"
        },
        new()
        {
            Id = 72,
            Name = "Wooden Bathroom Sink With Mirror",
            Price = 799.99m,
            Description = "Furniture, home decor, living room",
            Image = "images/products/wooden-bathroom-sink-with-mirror.webp"
        },
        new()
        {
            Id = 73,
            Name = "Apple",
            Price = 1.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/apple.webp"
        },
        new()
        {
            Id = 74,
            Name = "Beef Steak",
            Price = 12.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/beef-steak.webp"
        },
        new()
        {
            Id = 75,
            Name = "Cat Food",
            Price = 8.99m,
            Description = "Groceries, food, pet supplies",
            Image = "images/products/cat-food.webp"
        },
        new()
        {
            Id = 76,
            Name = "Chicken Meat",
            Price = 9.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/chicken-meat.webp"
        },
        new()
        {
            Id = 77,
            Name = "Cooking Oil",
            Price = 4.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/cooking-oil.webp"
        },
        new()
        {
            Id = 78,
            Name = "Cucumber",
            Price = 1.49m,
            Description = "Groceries, food, pantry",
            Image = "images/products/cucumber.webp"
        },
        new()
        {
            Id = 79,
            Name = "Dog Food",
            Price = 10.99m,
            Description = "Groceries, food, pet supplies",
            Image = "images/products/dog-food.webp"
        },
        new()
        {
            Id = 80,
            Name = "Eggs",
            Price = 2.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/eggs.webp"
        },
        new()
        {
            Id = 81,
            Name = "Fish Steak",
            Price = 14.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/fish-steak.webp"
        },
        new()
        {
            Id = 82,
            Name = "Green Bell Pepper",
            Price = 1.29m,
            Description = "Groceries, food, pantry",
            Image = "images/products/green-bell-pepper.webp"
        },
        new()
        {
            Id = 83,
            Name = "Green Chili Pepper",
            Price = 0.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/green-chili-pepper.webp"
        },
        new()
        {
            Id = 84,
            Name = "Honey Jar",
            Price = 6.99m,
            Description = "Groceries, food, beverages",
            Image = "images/products/honey-jar.webp"
        },
        new()
        {
            Id = 85,
            Name = "Ice Cream",
            Price = 5.49m,
            Description = "Groceries, food, beverages",
            Image = "images/products/ice-cream.webp"
        },
        new()
        {
            Id = 86,
            Name = "Juice",
            Price = 3.99m,
            Description = "Groceries, food, beverages",
            Image = "images/products/juice.webp"
        },
        new()
        {
            Id = 87,
            Name = "Kiwi",
            Price = 2.49m,
            Description = "Groceries, food, pantry",
            Image = "images/products/kiwi.webp"
        },
        new()
        {
            Id = 88,
            Name = "Lemon",
            Price = 0.79m,
            Description = "Groceries, food, pantry",
            Image = "images/products/lemon.webp"
        },
        new()
        {
            Id = 89,
            Name = "Milk",
            Price = 3.49m,
            Description = "Groceries, food, beverages",
            Image = "images/products/milk.webp"
        },
        new()
        {
            Id = 90,
            Name = "Mulberry",
            Price = 4.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/mulberry.webp"
        },
        new()
        {
            Id = 91,
            Name = "Nescafe Coffee",
            Price = 7.99m,
            Description = "Groceries, food, beverages",
            Image = "images/products/nescafe-coffee.webp"
        },
        new()
        {
            Id = 92,
            Name = "Potatoes",
            Price = 2.29m,
            Description = "Groceries, food, pantry",
            Image = "images/products/potatoes.webp"
        },
        new()
        {
            Id = 93,
            Name = "Protein Powder",
            Price = 19.99m,
            Description = "Groceries, food, beverages",
            Image = "images/products/protein-powder.webp"
        },
        new()
        {
            Id = 94,
            Name = "Red Onions",
            Price = 1.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/red-onions.webp"
        },
        new()
        {
            Id = 95,
            Name = "Rice",
            Price = 5.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/rice.webp"
        },
        new()
        {
            Id = 96,
            Name = "Soft Drinks",
            Price = 1.99m,
            Description = "Groceries, food, beverages",
            Image = "images/products/soft-drinks.webp"
        },
        new()
        {
            Id = 97,
            Name = "Strawberry",
            Price = 3.99m,
            Description = "Groceries, food, pantry",
            Image = "images/products/strawberry.webp"
        },
        new()
        {
            Id = 98,
            Name = "Tissue Paper Box",
            Price = 2.49m,
            Description = "Groceries, food, pantry",
            Image = "images/products/tissue-paper-box.webp"
        },
        new()
        {
            Id = 99,
            Name = "Water",
            Price = 0.99m,
            Description = "Groceries, food, beverages",
            Image = "images/products/water.webp"
        },
        new()
        {
            Id = 100,
            Name = "Decoration Swing",
            Price = 59.99m,
            Description = "Home decor, decorations, living room",
            Image = "images/products/decoration-swing.webp"
        },
        new()
        {
            Id = 101,
            Name = "Plant Pot",
            Price = 14.99m,
            Description = "Home decor, decorations, living room",
            Image = "images/products/plant-pot.webp"
        },
        new()
        {
            Id = 102,
            Name = "Black Aluminium Cup",
            Price = 5.99m,
            Description = "Kitchen, utensils, cutlery",
            Image = "images/products/black-aluminium-cup.webp"
        },
        new()
        {
            Id = 103,
            Name = "Black Whisk",
            Price = 9.99m,
            Description = "Kitchen, utensils, cutlery",
            Image = "images/products/black-whisk.webp"
        },
        new()
        {
            Id = 104,
            Name = "Boxed Blender",
            Price = 39.99m,
            Description = "Kitchen, appliances",
            Image = "images/products/boxed-blender.webp"
        },
        new()
        {
            Id = 105,
            Name = "Electric Stove",
            Price = 49.99m,
            Description = "Kitchen, appliances",
            Image = "images/products/electric-stove.webp"
        },
        new()
        {
            Id = 106,
            Name = "Hand Blender",
            Price = 34.99m,
            Description = "Kitchen, appliances",
            Image = "images/products/hand-blender.webp"
        },
        new()
        {
            Id = 107,
            Name = "Lunch Box",
            Price = 12.99m,
            Description = "Kitchen, cookware",
            Image = "images/products/lunch-box.webp"
        },
        new()
        {
            Id = 108,
            Name = "Pan",
            Price = 24.99m,
            Description = "Kitchen, cookware",
            Image = "images/products/pan.webp"
        },
        new()
        {
            Id = 109,
            Name = "Tray",
            Price = 16.99m,
            Description = "Kitchen, cookware",
            Image = "images/products/tray.webp"
        },
        new()
        {
            Id = 110,
            Name = "Wooden Rolling Pin",
            Price = 11.99m,
            Description = "Kitchen, utensils, cutlery",
            Image = "images/products/wooden-rolling-pin.webp"
        },
        new()
        {
            Id = 111,
            Name = "Apple MacBook Pro 14 Inch Space Grey",
            Price = 1999.99m,
            Description = "Laptops, electronics, computers",
            Image = "images/products/apple-macbook-pro-14-inch-space-grey.webp"
        },
        new()
        {
            Id = 112,
            Name = "Asus Zenbook Pro Dual Screen Laptop",
            Price = 1799.99m,
            Description = "Laptops, electronics, computers",
            Image = "images/products/asus-zenbook-pro-dual-screen-laptop.webp"
        },
        new()
        {
            Id = 113,
            Name = "Huawei Matebook X Pro",
            Price = 1399.99m,
            Description = "Laptops, electronics, computers",
            Image = "images/products/huawei-matebook-x-pro.webp"
        },
        new()
        {
            Id = 114,
            Name = "Lenovo Yoga 920",
            Price = 1099.99m,
            Description = "Laptops, electronics, computers",
            Image = "images/products/lenovo-yoga-920.webp"
        },
        new()
        {
            Id = 115,
            Name = "New DELL XPS 13 9300 Laptop",
            Price = 1499.99m,
            Description = "Laptops, electronics, computers",
            Image = "images/products/new-dell-xps-13-9300-laptop.webp"
        },
        new()
        {
            Id = 116,
            Name = "Gigabyte Aorus Men Tshirt",
            Price = 24.99m,
            Description = "Shirts, apparel, mens",
            Image = "images/products/gigabyte-aorus-men-tshirt.webp"
        },
        new()
        {
            Id = 117,
            Name = "Man Short Sleeve Shirt",
            Price = 19.99m,
            Description = "Shirts, apparel, mens",
            Image = "images/products/man-short-sleeve-shirt.webp"
        },
        new()
        {
            Id = 118,
            Name = "Men Check Shirt",
            Price = 27.99m,
            Description = "Shirts, apparel, mens",
            Image = "images/products/men-check-shirt.webp"
        },
        new()
        {
            Id = 119,
            Name = "Nike Baseball Cleats",
            Price = 79.99m,
            Description = "Shoes, sneakers, footwear, mens",
            Image = "images/products/nike-baseball-cleats.webp"
        },
        new()
        {
            Id = 120,
            Name = "Sports Sneakers Off White & Red",
            Price = 119.99m,
            Description = "Shoes, sneakers, footwear, mens",
            Image = "images/products/sports-sneakers-off-white-red.webp"
        },
        new()
        {
            Id = 121,
            Name = "Sports Sneakers Off White Red",
            Price = 109.99m,
            Description = "Shoes, sneakers, footwear, mens",
            Image = "images/products/92-sports-sneakers-off-white-red.webp"
        },
        new()
        {
            Id = 122,
            Name = "Longines Master Collection",
            Price = 1499.99m,
            Description = "Watches, accessories, mens",
            Image = "images/products/longines-master-collection.webp"
        },
        new()
        {
            Id = 123,
            Name = "Rolex Cellini Date Black Dial",
            Price = 8999.99m,
            Description = "Watches, accessories, mens",
            Image = "images/products/rolex-cellini-date-black-dial.webp"
        },
        new()
        {
            Id = 124,
            Name = "Rolex Cellini Moonphase",
            Price = 12999.99m,
            Description = "Watches, accessories, mens",
            Image = "images/products/rolex-cellini-moonphase.webp"
        },
        new()
        {
            Id = 125,
            Name = "Rolex Datejust",
            Price = 10999.99m,
            Description = "Watches, accessories, mens",
            Image = "images/products/rolex-datejust.webp"
        },
        new()
        {
            Id = 126,
            Name = "Rolex Submariner Watch",
            Price = 13999.99m,
            Description = "Watches, accessories, mens",
            Image = "images/products/rolex-submariner-watch.webp"
        },
        new()
        {
            Id = 127,
            Name = "Apple AirPods Max Silver",
            Price = 549.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/apple-airpods-max-silver.webp"
        },
        new()
        {
            Id = 128,
            Name = "Apple Airpower Wireless Charger",
            Price = 79.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/apple-airpower-wireless-charger.webp"
        },
        new()
        {
            Id = 129,
            Name = "Apple HomePod Mini Cosmic Grey",
            Price = 99.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/apple-homepod-mini-cosmic-grey.webp"
        },
        new()
        {
            Id = 130,
            Name = "Apple iPhone Charger",
            Price = 19.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/apple-iphone-charger.webp"
        },
        new()
        {
            Id = 131,
            Name = "Apple MagSafe Battery Pack",
            Price = 99.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/apple-magsafe-battery-pack.webp"
        },
        new()
        {
            Id = 132,
            Name = "Apple Watch Series 4 Gold",
            Price = 349.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/apple-watch-series-4-gold.webp"
        },
        new()
        {
            Id = 133,
            Name = "Beats Flex Wireless Earphones",
            Price = 49.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/beats-flex-wireless-earphones.webp"
        },
        new()
        {
            Id = 134,
            Name = "iPhone 12 Silicone Case with MagSafe Plum",
            Price = 29.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/iphone-12-silicone-case-with-magsafe-plum.webp"
        },
        new()
        {
            Id = 135,
            Name = "Monopod",
            Price = 19.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/monopod.webp"
        },
        new()
        {
            Id = 136,
            Name = "Selfie Lamp with iPhone",
            Price = 14.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/selfie-lamp-with-iphone.webp"
        },
        new()
        {
            Id = 137,
            Name = "Selfie Stick Monopod",
            Price = 12.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/selfie-stick-monopod.webp"
        },
        new()
        {
            Id = 138,
            Name = "TV Studio Camera Pedestal",
            Price = 499.99m,
            Description = "Electronics, phone accessories, accessories",
            Image = "images/products/tv-studio-camera-pedestal.webp"
        },
        new()
        {
            Id = 139,
            Name = "Attitude Super Leaves Hand Soap",
            Price = 8.99m,
            Description = "Beauty, skincare, cosmetics, womens",
            Image = "images/products/attitude-super-leaves-hand-soap.webp"
        },
        new()
        {
            Id = 140,
            Name = "Olay Ultra Moisture Shea Butter Body Wash",
            Price = 12.99m,
            Description = "Beauty, skincare, cosmetics, womens",
            Image = "images/products/olay-ultra-moisture-shea-butter-body-wash.webp"
        },
        new()
        {
            Id = 141,
            Name = "Vaseline Men Body and Face Lotion",
            Price = 9.99m,
            Description = "Beauty, skincare, cosmetics, womens",
            Image = "images/products/vaseline-men-body-and-face-lotion.webp"
        },
        new()
        {
            Id = 142,
            Name = "iPhone 5s",
            Price = 199.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/iphone-5s.webp"
        },
        new()
        {
            Id = 143,
            Name = "iPhone 6",
            Price = 299.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/iphone-6.webp"
        },
        new()
        {
            Id = 144,
            Name = "iPhone 13 Pro",
            Price = 1099.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/iphone-13-pro.webp"
        },
        new()
        {
            Id = 145,
            Name = "iPhone X",
            Price = 899.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/iphone-x.webp"
        },
        new()
        {
            Id = 146,
            Name = "Oppo A57",
            Price = 249.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/oppo-a57.webp"
        },
        new()
        {
            Id = 147,
            Name = "Oppo F19 Pro Plus",
            Price = 399.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/oppo-f19-pro-plus.webp"
        },
        new()
        {
            Id = 148,
            Name = "Oppo K1",
            Price = 299.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/oppo-k1.webp"
        },
        new()
        {
            Id = 149,
            Name = "Realme C35",
            Price = 149.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/realme-c35.webp"
        },
        new()
        {
            Id = 150,
            Name = "Realme X",
            Price = 299.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/realme-x.webp"
        },
        new()
        {
            Id = 151,
            Name = "Realme XT",
            Price = 349.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/realme-xt.webp"
        },
        new()
        {
            Id = 152,
            Name = "Samsung Galaxy S7",
            Price = 299.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/samsung-galaxy-s7.webp"
        },
        new()
        {
            Id = 153,
            Name = "Samsung Galaxy S8",
            Price = 499.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/samsung-galaxy-s8.webp"
        },
        new()
        {
            Id = 154,
            Name = "Samsung Galaxy S10",
            Price = 699.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/samsung-galaxy-s10.webp"
        },
        new()
        {
            Id = 155,
            Name = "Vivo S1",
            Price = 249.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/vivo-s1.webp"
        },
        new()
        {
            Id = 156,
            Name = "Vivo V9",
            Price = 299.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/vivo-v9.webp"
        },
        new()
        {
            Id = 157,
            Name = "Vivo X21",
            Price = 499.99m,
            Description = "Electronics, smartphones, phones",
            Image = "images/products/vivo-x21.webp"
        },
        new()
        {
            Id = 158,
            Name = "American Football",
            Price = 19.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/american-football.webp"
        },
        new()
        {
            Id = 159,
            Name = "Baseball Ball",
            Price = 8.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/baseball-ball.webp"
        },
        new()
        {
            Id = 160,
            Name = "Baseball Glove",
            Price = 24.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/baseball-glove.webp"
        },
        new()
        {
            Id = 161,
            Name = "Basketball",
            Price = 14.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/basketball.webp"
        },
        new()
        {
            Id = 162,
            Name = "Basketball Rim",
            Price = 39.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/basketball-rim.webp"
        },
        new()
        {
            Id = 163,
            Name = "Cricket Ball",
            Price = 12.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/cricket-ball.webp"
        },
        new()
        {
            Id = 164,
            Name = "Cricket Bat",
            Price = 29.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/cricket-bat.webp"
        },
        new()
        {
            Id = 165,
            Name = "Cricket Helmet",
            Price = 44.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/cricket-helmet.webp"
        },
        new()
        {
            Id = 166,
            Name = "Cricket Wicket",
            Price = 29.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/cricket-wicket.webp"
        },
        new()
        {
            Id = 167,
            Name = "Feather Shuttlecock",
            Price = 5.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/feather-shuttlecock.webp"
        },
        new()
        {
            Id = 168,
            Name = "Football",
            Price = 17.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/football.webp"
        },
        new()
        {
            Id = 169,
            Name = "Golf Ball",
            Price = 9.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/golf-ball.webp"
        },
        new()
        {
            Id = 170,
            Name = "Iron Golf",
            Price = 49.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/iron-golf.webp"
        },
        new()
        {
            Id = 171,
            Name = "Metal Baseball Bat",
            Price = 29.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/metal-baseball-bat.webp"
        },
        new()
        {
            Id = 172,
            Name = "Tennis Ball",
            Price = 6.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/tennis-ball.webp"
        },
        new()
        {
            Id = 173,
            Name = "Tennis Racket",
            Price = 49.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/tennis-racket.webp"
        },
        new()
        {
            Id = 174,
            Name = "Volleyball",
            Price = 11.99m,
            Description = "Sports, fitness, accessories",
            Image = "images/products/volleyball.webp"
        },
        new()
        {
            Id = 175,
            Name = "Black Sun Glasses",
            Price = 29.99m,
            Description = "Sunglasses, glasses, accessories, shades",
            Image = "images/products/black-sun-glasses.webp"
        },
        new()
        {
            Id = 176,
            Name = "Classic Sun Glasses",
            Price = 24.99m,
            Description = "Sunglasses, glasses, accessories, shades",
            Image = "images/products/classic-sun-glasses.webp"
        },
        new()
        {
            Id = 177,
            Name = "Green and Black Glasses",
            Price = 34.99m,
            Description = "Sunglasses, glasses, accessories, shades",
            Image = "images/products/green-and-black-glasses.webp"
        },
        new()
        {
            Id = 178,
            Name = "Party Glasses",
            Price = 19.99m,
            Description = "Sunglasses, glasses, accessories, shades",
            Image = "images/products/party-glasses.webp"
        },
        new()
        {
            Id = 179,
            Name = "Sunglasses",
            Price = 22.99m,
            Description = "Sunglasses, glasses, accessories, shades",
            Image = "images/products/sunglasses.webp"
        },
        new()
        {
            Id = 180,
            Name = "iPad Mini 2021 Starlight",
            Price = 499.99m,
            Description = "Electronics, tablets, phones",
            Image = "images/products/ipad-mini-2021-starlight.webp"
        },
        new()
        {
            Id = 181,
            Name = "Samsung Galaxy Tab S8 Plus Grey",
            Price = 599.99m,
            Description = "Electronics, tablets, phones",
            Image = "images/products/samsung-galaxy-tab-s8-plus-grey.webp"
        },
        new()
        {
            Id = 182,
            Name = "Samsung Galaxy Tab White",
            Price = 349.99m,
            Description = "Electronics, tablets, phones",
            Image = "images/products/samsung-galaxy-tab-white.webp"
        },
        new()
        {
            Id = 183,
            Name = "Blue Frock",
            Price = 29.99m,
            Description = "Tops, apparel, womens",
            Image = "images/products/blue-frock.webp"
        },
        new()
        {
            Id = 184,
            Name = "Girl Summer Dress",
            Price = 19.99m,
            Description = "Tops, apparel, womens",
            Image = "images/products/girl-summer-dress.webp"
        },
        new()
        {
            Id = 185,
            Name = "Gray Dress",
            Price = 34.99m,
            Description = "Tops, apparel, womens",
            Image = "images/products/gray-dress.webp"
        },
        new()
        {
            Id = 186,
            Name = "Short Frock",
            Price = 24.99m,
            Description = "Tops, apparel, womens",
            Image = "images/products/short-frock.webp"
        },
        new()
        {
            Id = 187,
            Name = "Tartan Dress",
            Price = 39.99m,
            Description = "Tops, apparel, womens",
            Image = "images/products/tartan-dress.webp"
        },
        new()
        {
            Id = 188,
            Name = "Blue Women's Handbag",
            Price = 49.99m,
            Description = "Bags, handbags, accessories, womens",
            Image = "images/products/blue-women-s-handbag.webp"
        },
        new()
        {
            Id = 189,
            Name = "Heshe Women's Leather Bag",
            Price = 129.99m,
            Description = "Bags, handbags, accessories, womens",
            Image = "images/products/heshe-women-s-leather-bag.webp"
        },
        new()
        {
            Id = 190,
            Name = "Prada Women Bag",
            Price = 599.99m,
            Description = "Bags, handbags, accessories, womens",
            Image = "images/products/prada-women-bag.webp"
        },
        new()
        {
            Id = 191,
            Name = "White Faux Leather Backpack",
            Price = 39.99m,
            Description = "Bags, handbags, accessories, womens",
            Image = "images/products/white-faux-leather-backpack.webp"
        },
        new()
        {
            Id = 192,
            Name = "Women Handbag Black",
            Price = 59.99m,
            Description = "Bags, handbags, accessories, womens",
            Image = "images/products/women-handbag-black.webp"
        },
        new()
        {
            Id = 193,
            Name = "Black Women's Gown",
            Price = 129.99m,
            Description = "Dresses, apparel, womens",
            Image = "images/products/black-women-s-gown.webp"
        },
        new()
        {
            Id = 194,
            Name = "Corset Leather With Skirt",
            Price = 89.99m,
            Description = "Dresses, apparel, womens",
            Image = "images/products/corset-leather-with-skirt.webp"
        },
        new()
        {
            Id = 195,
            Name = "Corset With Black Skirt",
            Price = 79.99m,
            Description = "Dresses, apparel, womens",
            Image = "images/products/corset-with-black-skirt.webp"
        },
        new()
        {
            Id = 196,
            Name = "Dress Pea",
            Price = 49.99m,
            Description = "Dresses, apparel, womens",
            Image = "images/products/dress-pea.webp"
        },
        new()
        {
            Id = 197,
            Name = "Marni Red & Black Suit",
            Price = 179.99m,
            Description = "Dresses, apparel, womens",
            Image = "images/products/marni-red-black-suit.webp"
        },
        new()
        {
            Id = 198,
            Name = "Green Crystal Earring",
            Price = 29.99m,
            Description = "Jewellery, accessories, womens",
            Image = "images/products/green-crystal-earring.webp"
        },
        new()
        {
            Id = 199,
            Name = "Green Oval Earring",
            Price = 24.99m,
            Description = "Jewellery, accessories, womens",
            Image = "images/products/green-oval-earring.webp"
        },
        new()
        {
            Id = 200,
            Name = "Tropical Earring",
            Price = 19.99m,
            Description = "Jewellery, accessories, womens",
            Image = "images/products/tropical-earring.webp"
        },
        new()
        {
            Id = 201,
            Name = "Black & Brown Slipper",
            Price = 19.99m,
            Description = "Shoes, footwear, womens",
            Image = "images/products/black-brown-slipper.webp"
        },
        new()
        {
            Id = 202,
            Name = "Calvin Klein Heel Shoes",
            Price = 79.99m,
            Description = "Shoes, footwear, womens",
            Image = "images/products/calvin-klein-heel-shoes.webp"
        },
        new()
        {
            Id = 203,
            Name = "Golden Shoes Woman",
            Price = 49.99m,
            Description = "Shoes, footwear, womens",
            Image = "images/products/golden-shoes-woman.webp"
        },
        new()
        {
            Id = 204,
            Name = "Pampi Shoes",
            Price = 29.99m,
            Description = "Shoes, footwear, womens",
            Image = "images/products/pampi-shoes.webp"
        },
        new()
        {
            Id = 205,
            Name = "Red Shoes",
            Price = 34.99m,
            Description = "Shoes, footwear, womens",
            Image = "images/products/red-shoes.webp"
        },
        new()
        {
            Id = 206,
            Name = "IWC Ingenieur Automatic Steel",
            Price = 4999.99m,
            Description = "Watches, accessories, womens",
            Image = "images/products/iwc-ingenieur-automatic-steel.webp"
        },
        new()
        {
            Id = 207,
            Name = "Rolex Cellini Moonphase",
            Price = 15999.99m,
            Description = "Watches, accessories, womens",
            Image = "images/products/191-rolex-cellini-moonphase.webp"
        },
        new()
        {
            Id = 208,
            Name = "Rolex Datejust Women",
            Price = 10999.99m,
            Description = "Watches, accessories, womens",
            Image = "images/products/rolex-datejust-women.webp"
        },
        new()
        {
            Id = 209,
            Name = "Watch Gold for Women",
            Price = 799.99m,
            Description = "Watches, accessories, womens",
            Image = "images/products/watch-gold-for-women.webp"
        },
        new()
        {
            Id = 210,
            Name = "Women's Wrist Watch",
            Price = 129.99m,
            Description = "Watches, accessories, womens",
            Image = "images/products/women-s-wrist-watch.webp"
        }
    ];
}