namespace ToDoList.DTO {
    public class TodoTask {
        public int Id { get; set; }
        public string? Name { get; init; }
        public bool IsComplete { get; set; } = false;
        public override string ToString() => Name ?? string.Empty; // la flecheta diu que es un mètode que retorna aixo.

        /*public override string ToString() {
            return Text == null ? string.Empty : Text; // aquí, si es null, fica string buit, sino el text rebut
            // if (Text == null) return string.Empty;
            // return string.Empty;

            // if (Text != null) return Text;
            // return Text;

            // return Text ?? string.Empty; // Null coalesce
            // si lo de l'esquerra és null, retorno la part dreta. i sinó el propi Text.
            // podria encadenarse xx ?? yy ?? zz, si no es complex lo previ, lo seguent ?? yy, i si tampoc es compleix ?? zz


            // if(stringIsNullOrEmpty(Text)) return string.Empty;
            // return Text;
        }*/
    }
}