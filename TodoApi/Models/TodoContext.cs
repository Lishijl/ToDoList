using Microsoft.EntityFrameworkCore;
// importa EntityFrameworkCore, packet per accedir a DbContext.
// l'import del nuget EFC.InMemory (per la certificació del dev i la generació del https)
namespace TodoApi.Models;
// importa la classe Models

// la classe TodoContext(contextdb) afegeit el DbContext
public class TodoContext : DbContext
{
    // el TodoContext reb paràmetre un tipus de Todocontext que AFEGEIX una BASE amb la variable options del paràmetre
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
    }

    // guardaDB tipus TodoItem (classe amb accessors), funcio que retorna o conte 2 mètodes accessors
    // per defecte és null.
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}