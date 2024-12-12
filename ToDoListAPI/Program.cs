using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Db;
using ToDoListAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ToDoListContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<IAssignmentsRepository, ToDoListAPI.Repository.AssignmentsRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "OpenCORSPolicy",
        builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("OpenCORSPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
