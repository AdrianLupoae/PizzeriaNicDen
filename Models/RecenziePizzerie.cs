namespace PizzeriaNicDen.Models
{
    public class RecenziePizzerie
    {
        public int Id { get; set; }
        public int Stele { get; set; } // 1 până la 5
        public string Text { get; set; } = string.Empty;
        public DateTime DataAdaugare { get; set; } = DateTime.Now;

        public int UtilizatorId { get; set; }
        public virtual Utilizator? Utilizator { get; set; }
    }
}