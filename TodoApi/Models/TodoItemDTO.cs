namespace TodoApi.Models;
// carpeta Models s'usa per convenció, pero la classe Model pot accedir a tots els llocs del projecte 

// nou model TodoItem DTO
public class TodoItemDTO
{
    // PK in database
    public long Id { get; set; }
    // pot ser null "?"
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

}