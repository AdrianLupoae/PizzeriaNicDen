namespace PizzeriaNicDen.Models
{
    public class Utilizator
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nume { get; set; } = string.Empty;
        public string Rol { get; set; } = "Client"; // Poate fi "Client" sau "Admin"
        public int NumarLogari { get; set; }
        
        public virtual ICollection<AprecierePizza> Aprecieri { get; set; } = new List<AprecierePizza>();
    }
}