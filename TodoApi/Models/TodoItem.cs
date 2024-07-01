namespace TodoApi.Models;
// carpeta Models s'usa per convenció, pero la classe Model pot accedir a tots els llocs del projecte 
public class TodoItem
{
    // PK in database
    public long Id { get; set; }
    // pot ser null "?"
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    // nou camp Secret, no pel client sinó per l'adm que pot optar mostrar-lo
    // verifica que pot publicar i obtenir el camp Secret
    public string? Secret { get; set; }

}