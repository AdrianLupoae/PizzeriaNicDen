using Microsoft.EntityFrameworkCore;
using PizzeriaNicDen.Models;

namespace PizzeriaNicDen.Data
{
    public class PizzeriaContext : DbContext
    {
        public PizzeriaContext(DbContextOptions<PizzeriaContext> options) : base(options)
        {
        }

        public DbSet<Pizza> Pizze { get; set; }
        public DbSet<PizzaFamily> PizzeFamily { get; set; }
        public DbSet<Utilizator> Utilizatori { get; set; }
        public DbSet<AprecierePizza> AprecieriPizze { get; set; }
        public DbSet<RecenziePizzerie> Recenzii { get; set; }
        public DbSet<Anunt> Anunturi { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Configurarea relației (deja existentă)
            modelBuilder.Entity<Pizza>()
                .HasOne(p => p.VariantaFamily)
                .WithOne(pf => pf.Pizza)
                .HasForeignKey<PizzaFamily>(pf => pf.PizzaId);

            // 2. Inserarea datelor pentru Pizza (tabelul principal - 30cm)
            modelBuilder.Entity<Pizza>().HasData(
                new Pizza 
                { 
                    Id = 1, 
                    Nume = "Margherita", 
                    Ingrediente = "Sos de roșii, mozzarella fior di latte, busuioc proaspăt, ulei de măsline", 
                    CaleImagine = "/img/margherita.jpg", 
                    Pret30cm = 28.00m, 
                    ValoriNutritionale30cm = "250 kcal / 100g" 
                },
                new Pizza 
                { 
                    Id = 2, 
                    Nume = "Diavola", 
                    Ingrediente = "Sos de roșii, mozzarella, salam picant, ardei iute jalapeño", 
                    CaleImagine = "/img/diavola.jpg", 
                    Pret30cm = 34.00m, 
                    ValoriNutritionale30cm = "310 kcal / 100g" 
                },
                new Pizza 
                { 
                    Id = 3, 
                    Nume = "Specialitatea Nic&Den", 
                    Ingrediente = "Sos de roșii, mozzarella, șuncă, ciuperci, măsline, bacon, ardei gras", 
                    CaleImagine = "/img/nicden.jpg", 
                    Pret30cm = 38.00m, 
                    ValoriNutritionale30cm = "290 kcal / 100g" 
                }
            );

            // 3. Inserarea datelor pentru PizzaFamily (varianta de 50cm)
            // Observă că inserăm doar pentru PizzaId = 1 (Margherita) și 3 (Nic&Den). Diavola nu va avea varianta de 50cm.
            modelBuilder.Entity<PizzaFamily>().HasData(
                new PizzaFamily 
                { 
                    Id = 1, 
                    PizzaId = 1, // Se leagă de Margherita
                    Pret50cm = 50.00m, 
                    ValoriNutritionale50cm = "250 kcal / 100g" 
                },
                new PizzaFamily 
                { 
                    Id = 2, 
                    PizzaId = 3, // Se leagă de Specialitatea Nic&Den
                    Pret50cm = 65.00m, 
                    ValoriNutritionale50cm = "290 kcal / 100g" 
                }
            );
        }
    }
}