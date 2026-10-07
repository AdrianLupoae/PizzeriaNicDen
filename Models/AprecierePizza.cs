namespace PizzeriaNicDen.Models
{
    public class AprecierePizza
    {
        public int Id { get; set; }
        public int UtilizatorId { get; set; }
        public virtual Utilizator? Utilizator { get; set; }
        
        public int PizzaId { get; set; }
        public virtual Pizza? Pizza { get; set; }
    }
}