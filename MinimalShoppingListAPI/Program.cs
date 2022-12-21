using Microsoft.EntityFrameworkCore;
using MinimalShoppingListAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseInMemoryDatabase("ShopListAPI"));

var app = builder.Build();
// CRUD >> detail for all
app.MapGet("/shoppinglist", async (ApiDbContext db) => await db.Groceries.ToListAsync());
//CRUD >> detail for one item
app.MapGet("/shoppinglist/{id}", async (int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);
    return grocery != null ? Results.Ok(grocery) : Results.NotFound();
});
// CRUD >> Create
app.MapPost("/shoppinglist", async (Grocery grocery, ApiDbContext db) =>
{
    db.Groceries.Add(grocery);
    await db.SaveChangesAsync();
    return Results.Created($"/shoppinglist/{grocery.Id}", grocery);
});

app.MapDelete("/shoppinglist/{id}", async(int id, ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);
    if (grocery != null)
    {
        db.Groceries.Remove(grocery);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});
// CRUD Update Single item in DB
app.MapPut("/shoppinglist/{id}", async (int id,Grocery groceryUp ,ApiDbContext db) =>
{
    var grocery = await db.Groceries.FindAsync(id);
    if (grocery != null)
    {
        grocery.Name = groceryUp.Name;
        grocery.Purchased= groceryUp.Purchased;

        await db.SaveChangesAsync();
        return Results.Ok(grocery);
    }
    return Results.NotFound();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();