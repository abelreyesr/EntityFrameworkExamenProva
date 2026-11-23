using System.ComponentModel.DataAnnotations;

namespace ExamenProva.Models;

public class Representant
{
    public int RepresentantID { get; set; }
    [Required] [MaxLength(80)] public string Nom { get; set; }
    [Required] [MaxLength(60)] public string Carrec { get; set; }
    [Required] [MaxLength(80)] public string Estudi { get; set; }

    [Required] public virtual Candidatura Candidatura { get; set; }

    public Representant()
    {
        
    }
}