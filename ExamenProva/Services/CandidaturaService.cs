using ExamenProva.Data;
using ExamenProva.Models;

namespace ExamenProva.Services;

public class CandidaturaService
{
    private AwardsContext ctx;

    public CandidaturaService(AwardsContext context)
    {
        ctx = context;
    }

    public List<Candidatura> GetGuanyadors(int anyMinim)
    {
        return ctx.Candidaturas
            .Where(c => c.HaGuanyat && c.Any >= anyMinim) 
            .ToList();
    }
    
    public void MostraCandidaturesRelacionades(int idCandidatura)
    {
        Candidatura? c = ctx.Candidaturas.FirstOrDefault(c => c.CandidaturaID == idCandidatura);
        if (c == null)
        {
            Console.WriteLine($"ERROR: no existeix cap candidatura amb id {idCandidatura}");
            return;
        }

        Console.WriteLine($"Candidatura: {c.NomJoc} - {c.Categoria} - {c.Representant.Nom}");

        if (c.CandidaturesRelacionades.Count == 0)
        {
            Console.WriteLine("    No té cap candidatura relacionada");
            return;
        }
        
        Console.WriteLine("Candidatures relacionades: ");
        foreach (Candidatura r in c.CandidaturesRelacionades)
        {
            Console.WriteLine($"    {r.NomJoc} - {r.Categoria} - {r.Representant.Nom}");
        }
    }

    public bool RelacionarCandidatures(int idPrincipal, int idRelacionada)
    {
        Candidatura? principal = ctx.Candidaturas.FirstOrDefault(c => c.CandidaturaID == idPrincipal);
        if (principal == null)
        {
            Console.WriteLine($"ERROR: no existeix cap candidatura amb id {idPrincipal}");
            return false;
        }
        
        Candidatura? relacionada = ctx.Candidaturas.FirstOrDefault(c => c.CandidaturaID == idRelacionada);
        if (relacionada == null)
        {
            Console.WriteLine($"ERROR: no existeix cap candidatura amb id {idRelacionada}");
            return false;
        }

        if (principal.CandidaturesRelacionades.Contains(relacionada))
        {
            Console.WriteLine($"ERROR: la candidatura {idRelacionada} ja está relacionada amb la {idPrincipal}");
            return false;
        }

        if (principal.NomJoc != relacionada.NomJoc)
        {
            Console.WriteLine($"ERROR: les candidatures no son del mateix joc ({principal.NomJoc} - {relacionada.NomJoc})");
            return false;
        }

        // no es comprova nomJoc pq ja ha pasat el if anterior
        if (principal.Categoria == relacionada.Categoria && principal.Any == relacionada.Any)
        {
            Console.WriteLine($"ERROR: les dues categories son la mateixa");
            return false;
        }

        relacionada.CandidaturaPrincipal = principal;
        principal.CandidaturesRelacionades.Add(relacionada);
        ctx.SaveChanges();
        
        Console.WriteLine($"S'ha relacionat correctament la categoría {idRelacionada} amb {idPrincipal}");
        return true;
    }

    public List<Candidatura> CandidaturesExitoses()
    {
        return ctx.Candidaturas
            .Where(c => c.HaGuanyat || c.CandidaturesRelacionades.Any(r => r.HaGuanyat))
            .ToList();
    }

    public void TopEstudis(int votsMinims)
    {
        Console.WriteLine($"Llista d'estudis amb més de {votsMinims} vots:");
        
        var estudis = ctx.Candidaturas
            .GroupBy(c => c.Representant.Estudi)
            .Select(e => new
            {
                Estudi = e.Key,
                Candidatures = e.Count(),
                Vots = e.Sum(c => c.VotsPublic)
            })
            .Where(v => v.Vots > votsMinims)
            .ToList();

        if (estudis.Count == 0)
        {
            Console.WriteLine($"    No s'ha trobat cap estudi amb més de {votsMinims} vots");
            return;
        }
        
        foreach (var e in estudis)
        {
            string num = e.Candidatures == 1 ? "candidatura" : "candidatures"; // plural o singular
            Console.WriteLine($"    {e.Estudi}: {e.Candidatures} {num}, {e.Vots} vots");
        }
    }
}