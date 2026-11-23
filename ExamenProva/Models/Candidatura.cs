using System.ComponentModel.DataAnnotations;

namespace ExamenProva.Models;

public class Candidatura
{
    public int CandidaturaID { set; get; }
    [Required] [MaxLength(100)] public string NomJoc { get; set; }
    public CategoriaGOTY Categoria { get; set; }
    public int Any { get; set; }
    public bool HaGuanyat { get; set; }
    public int VotsPublic { get; set; }

    public virtual Candidatura? CandidaturaPrincipal { get; set; }
    public virtual ICollection<Candidatura> CandidaturesRelacionades { get; set; }

    [Required] public virtual Representant Representant { get; set; }
    
    public Candidatura()
    {
        CandidaturesRelacionades = new HashSet<Candidatura>();
    }
}