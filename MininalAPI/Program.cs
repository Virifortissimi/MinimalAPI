using MininalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Get to Try out the routing
app.MapGet("/", () => "Welcome to minimal APIs");

//Get all Todo Items
app.MapGet("/api/TodoItems", async (DataContext context) => await context.TodoItems.ToListAsync());

//Get Todo Items by id
app.MapGet("/api/TodoItems/{id}", async (DataContext context, int id) => 
    await context.TodoItems.FindAsync(id) is TodoItem todoItem ? Results.Ok(todoItem) : Results.NotFound("Todo item not found ./"));

//Create Todo Items 
app.MapPost("/api/TodoItems", async (DataContext context, TodoItem todoItem) =>
{
    context.TodoItems.Add(todoItem);
    await context.SaveChangesAsync();
    return Results.Created($"/api/TodoItems/{todoItem.Id}",todoItem);
});

//Updating Todo Items
app.MapPut("/api/TodoItems/{id}", async (DataContext context, TodoItem todoItem, int id) =>
{
    var todoItemFromDb = await context.TodoItems.FindAsync(id);
    
    if (todoItemFromDb != null)
    {
        todoItemFromDb.Title = todoItem.Title;
        todoItemFromDb.DueAt = todoItem.DueAt;
        todoItemFromDb.IsDone = todoItem.IsDone;

        await context.SaveChangesAsync();
        return Results.Ok(todoItem);
    }
    return Results.NotFound("TodoItem not found");
});


//Deleting Todo Items
app.MapDelete("/api/TodoItems/{id}", async (DataContext context, int id) =>
{
    var todoItemFromDb = await context.TodoItems.FindAsync(id);

    if (todoItemFromDb != null)
    {
        context.Remove(todoItemFromDb);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
    return Results.NotFound("TodoItem not found");
});

app.Run();
