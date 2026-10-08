using System.ComponentModel.DataAnnotations;

namespace PizzeriaNicDen.Models;

public class Anunt
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Titlul este obligatoriu")]
    [StringLength(100)]
    public string Titlu { get; set; } = "";

    [Required(ErrorMessage = "Mesajul este obligatoriu")]
    [StringLength(1000)]
    public string Mesaj { get; set; } = "";

    [Required]
    public DateTime DataInceput { get; set; }

    [Required]
    public DateTime DataSfarsit { get; set; }

    public bool Activ { get; set; } = true;

    public DateTime DataCreare { get; set; }
}