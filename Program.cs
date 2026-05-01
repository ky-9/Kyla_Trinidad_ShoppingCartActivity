using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public Product(int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        RemainingStock = stock;
    }

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock})");
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity <= RemainingStock;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Product[] products = new Product[]
        {
            new Product(1, "Spanish Latte", 99, 27),
            new Product(2, "Caramel Macchiatto", 139, 10),
            new Product(3, "Biscoff Latte", 199, 7),
            new Product(4, "Hazelnut Latte", 139, 3),
            new Product(5, "Matcha", 189, 7)
            

        };

        int[] cartIds = new int[10];
        int[] cartQty = new int[10];
        double[] cartSubtotal = new double[10];
        int cartCount = 0;

        while (true)
        {
    Console.WriteLine("\n=== MAIN MENU ===");
    Console.WriteLine("1. View Products");
    Console.WriteLine("2. Search Product");
    Console.WriteLine("3. Filter by Category");
    Console.WriteLine("4. View Cart");
    Console.WriteLine("5. Checkout");
    Console.WriteLine("6. Order History");
    Console.WriteLine("7. Exit");

    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Invalid input.");
        continue;
    }

    
    if (choice == 1)
    {
        foreach (var p in products)
            p.DisplayProduct();

        AddToCart(products, cartIds, cartQty, cartSub, ref cartCount);
    }

    
    else if (choice == 2)
    {
        Console.Write("Search: ");
        string search = (Console.ReadLine() ?? "").ToLower();

        foreach (var p in products)
            if (p.Name.ToLower().Contains(search))
                p.DisplayProduct();
    }

    
    else if (choice == 3)
    {
        Console.Write("Enter category (Drink): ");
        string cat = Console.ReadLine();

        foreach (var p in products)
            if (p.Category.Equals(cat, StringComparison.OrdinalIgnoreCase))
                p.DisplayProduct();
    }

    
    else if (choice == 4)
    {
        ManageCart(products, cartIds, cartQty, cartSub, ref cartCount);
    }

    
    else if (choice == 5)
    {
        if (cartCount == 0)
        {
            Console.WriteLine("Cart is empty.");
            continue;
        }

        double grand = 0;
        for (int i = 0; i < cartCount; i++)
            grand += cartSub[i];

        double discount = grand >= 5000 ? grand * 0.10 : 0;
        double final = grand - discount;

        Console.WriteLine($"\nFinal Total: ₱{final}");

        double payment;
        while (true)
        {
            Console.Write("Enter payment: ");
            if (!double.TryParse(Console.ReadLine(), out payment))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            if (payment < final)
                Console.WriteLine("Insufficient payment.");
            else break;
        }

        double change = payment - final;

        
        Console.WriteLine("\n=== RECEIPT ===");
        Console.WriteLine($"Receipt No: {receiptNo:D4}");
        Console.WriteLine($"Date: {DateTime.Now}");

        for (int i = 0; i < cartCount; i++)
        {
            Product p = products[cartIds[i] - 1];
            Console.WriteLine($"{p.Name} - Qty: {cartQty[i]} - ₱{cartSub[i]}");
        }

        Console.WriteLine($"\nGrand Total: ₱{grand}");
        Console.WriteLine($"Discount: ₱{discount}");
        Console.WriteLine($"Final Total: ₱{final}");
        Console.WriteLine($"Payment: ₱{payment}");
        Console.WriteLine($"Change: ₱{change}");

        orderHistory[orderCount++] = final;
        receiptNo++;


                    Console.WriteLine("\nLOW STOCK ALERT:");
            foreach (var p in products)
                if (p.RemainingStock <= 5)
                    Console.WriteLine($"{p.Name} has only {p.RemainingStock} left.");

            cartCount = 0;
        }

        
        else if (choice == 6)
        {
            Console.WriteLine("\nORDER HISTORY:");
            if (orderCount == 0)
                Console.WriteLine("No orders yet.");
            else
            {
                for (int i = 0; i < orderCount; i++)
                    Console.WriteLine($"Receipt #{i + 1:D4} - ₱{orderHistory[i]}");
            }
        }

        else if (choice == 7)
        {
            break;
        }
    }
}


static void AddToCart(Product[] products, int[] cartIds, int[] cartQty, double[] cartSub, ref int cartCount)
{
    while (true)
    {
        Console.Write("Enter product number (0 to stop): ");
        if (!int.TryParse(Console.ReadLine(), out int pNum) || pNum < 0 || pNum > products.Length)
        {
            Console.WriteLine("Invalid.");
            continue;
        }

        if (pNum == 0) break;

        Product selected = products[pNum - 1];

        Console.Write("Qty: ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
        {
            Console.WriteLine("Invalid.");
            continue;
        }

        if (!selected.HasEnoughStock(qty))
        {
            Console.WriteLine("Not enough stock.");
            continue;
        }

        double total = selected.GetItemTotal(qty);

        int found = -1;
        for (int i = 0; i < cartCount; i++)
            if (cartIds[i] == selected.Id) found = i;

        if (found != -1)
        {
            cartQty[found] += qty;
            cartSub[found] += total;
        }
        else
        {
            cartIds[cartCount] = selected.Id;
            cartQty[cartCount] = qty;
            cartSub[cartCount] = total;
            cartCount++;
        }

        selected.DeductStock(qty);
        Console.WriteLine("Added!");
    }
}
