using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
// importa EntityFrameworkCore, packet per accedir a DbContext.
// l'import del nuget EFC.InMemory (per la certificació del dev i la generació del https)
namespace TodoApi.Models;
// importa la classe Models

// la classe TodoContext(contextdb) afegeit el DbContext
// : hereda de un dbcontext existent de entityFramework, TodoContext, nova classe que de dbContext
// es converteix en context de Base de Dades per la app.
public class TodoContext : DbContext
{
    // el TodoContext reb paràmetre un tipus de Todocontext que AFEGEIX una BASE amb la variable options del paràmetre
    // CONSTRUCTOR PER DEFECTE de TodoContext, reb parametre options tipus DbCO<TContext> i ho passa al
    // constructor BASE de (DbContext), "base(options)" constructor base de DbContext cridat.
    // DbContextOptons<> serveix per configurar la dbcontext de manera flexible
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
    }

    // Definición de Conjuntos de Entidades (DbSet) : representa les taules de la base de dades
    // com la taula de items/elements/taskes, o la taula de llistes (que permet operacions del CRUD sobre l'entitat/taula)
    // guardaDB tipus TodoItem (classe amb accessors), funcio que retorna o conte 2 mètodes accessors
    // null (!) que assegurem que no serà null, confia en mi, no serà null
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<TodoList> TodoLists { get; set; } = null!;
}