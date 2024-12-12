using Microsoft.EntityFrameworkCore;
using ToDoListAPI.Entities;

namespace ToDoListAPI.Db;

public class ToDoListContext : DbContext
{
    public ToDoListContext(DbContextOptions<ToDoListContext> options) : base(options)
    {

    }

    public DbSet<Assignment> Assignments { get; set; }
}
