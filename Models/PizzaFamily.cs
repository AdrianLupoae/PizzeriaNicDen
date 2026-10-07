namespace PizzeriaNicDen.Models
{
    public class PizzaFamily
    {
        public int Id { get; set; }
        public decimal Pret50cm { get; set; }
        public string ValoriNutritionale50cm { get; set; } =string.Empty;

        // Foreign Key (Cheia externă) care face legătura cu tabelul Pizza
        public int PizzaId { get; set; }
        
        // Proprietate de navigare înapoi către Pizza părinte
        public virtual Pizza? Pizza { get; set; } = null;
    }
}