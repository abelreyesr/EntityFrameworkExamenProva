using ExamenProva.Data;
using ExamenProva.Models;
using ExamenProva.Services;

namespace ExamenProva;

internal class Program
{
    static void Main(string[] args)
    {
        using (var ctx = new AwardsContext())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            CandidaturaService candidaturaService = new CandidaturaService(ctx);
            
            // crear representants
            Representant r1 = new Representant()
            {
                Nom = "Christopher Larkin",
                Carrec = "Compositor",
                Estudi = "Team Cherry"
            };
            Representant r2 = new Representant()
            {
                Nom = "Ari Gibson",
                Carrec = "Director Artístic",
                Estudi = "Team Cherry"
            };
            Representant r3 = new Representant()
            {
                Nom = "William Pellen",
                Carrec = "Productor Executiu",
                Estudi = "Team Cherry"
            };
            Representant r4 = new Representant()
            {
                Nom = "Guillaume Broche",
                Carrec = "Director",
                Estudi = "SandFall Interative"
            };
            Representant r5 = new Representant()
            {
                Nom = "Masayuki Yamada",
                Carrec = "Director",
                Estudi = "Sony Interactive"
            };
            
            // crear candidatures
            Candidatura c1 = new Candidatura()
            {
                NomJoc = "Hollow Knight: Silksong",
                Categoria = CategoriaGOTY.MillorBandaSonora,
                Any = 2025,
                HaGuanyat = true,
                VotsPublic = 15420,
                Representant = r1
            };
            Candidatura c2 = new Candidatura()
            {
                NomJoc = "Hollow Knight: Silksong",
                Categoria = CategoriaGOTY.MillorDireccioArt,
                Any = 2025,
                HaGuanyat = false,
                VotsPublic = 18930,
                Representant = r2
            };
            Candidatura c3 = new Candidatura()
            {
                NomJoc = "Hollow Knight: Silksong",
                Categoria = CategoriaGOTY.MillorJoc,
                Any = 2025,
                HaGuanyat = false,
                VotsPublic = 25600,
                Representant = r3
            };
            Candidatura c4 = new Candidatura()
            {
                NomJoc = "Expedition 33",
                Categoria = CategoriaGOTY.MillorJoc,
                Any = 2025,
                HaGuanyat = true,
                VotsPublic = 32100,
                Representant = r4
            };
            Candidatura c5 = new Candidatura()
            {
                NomJoc = "Astrobot",
                Categoria = CategoriaGOTY.MillorJoc,
                Any = 2024,
                HaGuanyat = true,
                VotsPublic = 8200 ,
                Representant = r5
            };
            
            // EX2 candidatures relacionades
            c1.CandidaturaPrincipal = c3;
            c2.CandidaturaPrincipal = c3;
            
            ctx.Candidaturas.AddRange(c1, c2, c3, c4, c5);

            ctx.SaveChanges();
            
            // EX3 get guanyadors
            Console.WriteLine("Llista de guanyadors a partir de 2025:");
            foreach (Candidatura c in candidaturaService.GetGuanyadors(2025))
            {
                Console.WriteLine($"    Candidatura {c.CandidaturaID}: {c.NomJoc}, {c.Categoria} ({c.Any})");
            }
            Console.WriteLine();
            
            // EX4 mostrar relacionades
            candidaturaService.MostraCandidaturesRelacionades(1); Console.WriteLine();
            candidaturaService.MostraCandidaturesRelacionades(3); Console.WriteLine();
            
            // EX5 RelacionarCandidatures
            c3.CandidaturesRelacionades = new List<Candidatura>();
            ctx.SaveChanges();
            
            candidaturaService.RelacionarCandidatures(c3.CandidaturaID, c1.CandidaturaID);
            candidaturaService.RelacionarCandidatures(c3.CandidaturaID, c2.CandidaturaID);
            candidaturaService.RelacionarCandidatures(c3.CandidaturaID, c2.CandidaturaID);
            candidaturaService.RelacionarCandidatures(c3.CandidaturaID, c3.CandidaturaID);
            candidaturaService.RelacionarCandidatures(c3.CandidaturaID, c5.CandidaturaID);
            Console.WriteLine();
            
            // EX6 Exitoses
            Console.WriteLine($"Llista de candidatures exitoses:");
            foreach (Candidatura c in candidaturaService.CandidaturesExitoses())
            {
                Console.WriteLine($"    Candidatura {c.CandidaturaID}: {c.NomJoc} ({c.Any}) - {c.Categoria} - {c.Representant.Nom}");
            } 
            Console.WriteLine();
            
            // EX7 Top Estudis
            candidaturaService.TopEstudis(10000); Console.WriteLine();
            candidaturaService.TopEstudis(50000); Console.WriteLine();
            candidaturaService.TopEstudis(670000);
        }
    }
}
