namespace PizzeriaNicDen.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Nume { get; set; } =string.Empty;
        public string Ingrediente { get; set; }=string.Empty;
        public string CaleImagine { get; set; }=string.Empty;
        
        // Date pentru varianta standard de 30cm
        public decimal Pret30cm { get; set; }
        public string ValoriNutritionale30cm { get; set; }=string.Empty;

        // Proprietate de navigare către varianta Family (poate fi null dacă nu există)
        public virtual PizzaFamily? VariantaFamily { get; set; }
    }
}