

using MiniOrm.Data;




#region Create Dbcontext

var connStr = "Host=localhost;Port=5432;Database=miniorm;Username=postgres;Password=postgres"; 

if (string.IsNullOrWhiteSpace(connStr))
{
    Console.WriteLine("ERROR: MINIORM_CONN environment variable is not set.");
    return;
}

var db = new AppDbConext(connStr);

Console.WriteLine("✓ DbContext created successfully (Npgsql connected)");
Console.WriteLine();

Console.WriteLine("IMPORTANT:");
Console.WriteLine("Run migrations before executing:");
Console.WriteLine("dotnet run -- migrations apply\n");


#endregion


try
{
    #region insert

    var keyboard = new Product
    {
        Name = "Keyboard",
        Price = 89.99m,
        Discount = null,
        InStock = true
    };

    int id = db.Products.Insert(keyboard);

    Console.WriteLine($"Inserted Product Id={id} Discount=null ✓\n");


    #endregion


    #region Step 5 - READ

    Console.WriteLine("========== READ ==========\n");

    var found = db.Products.FindById(id);

    if (found != null)
    {
        Console.WriteLine(
            $"Found → Name={found.Name}, Price={found.Price}, " +
            $"Discount={(found.Discount == null ? "NULL" : found.Discount.ToString())}, " +
            $"InStock={found.InStock}"
        );
    }

    Console.WriteLine();

    #endregion



    #region UPDATE

    Console.WriteLine("========== UPDATE ==========\n");

    found.Price = 79.99m;
    found.Discount = 5.00m;

    db.Products.Update(found);

    Console.WriteLine(
        $"Updated → Price={found.Price}, Discount={found.Discount} ✓"
    );

    Console.WriteLine();

    #endregion

    #region GET ALL

    Console.WriteLine("========== GET ALL ==========\n");

    var allProducts = db.Products.GetAll();

   // Console.WriteLine($"Total Products: {allProducts.Count}\n");

    foreach (var p in allProducts)
    {
        Console.WriteLine(
            $"Id={p.Id}, Name={p.Name}, Price={p.Price}, " +
            $"Discount={(p.Discount == null ? "NULL" : p.Discount.ToString())}, " +
            $"InStock={p.InStock}"
        );
    }

    Console.WriteLine();

    #endregion

    #region DELETE

    Console.WriteLine("========== DELETE ==========\n");

    db.Products.Delete(id);

    Console.WriteLine($"Deleted Id={id} ✓");
    var count = db.Products.GetAll().Count();
    string result = count.ToString();

    Console.WriteLine($"Remaining Products: {result}");

    Console.WriteLine();

    #endregion




}

catch (Exception ex)
{
    Console.WriteLine("Error Occured");
    Console.WriteLine(ex.Message);

}



