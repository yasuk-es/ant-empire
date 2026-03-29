using System.Collections.Generic;

// Habilidad de ataque básico: disponible para todos los roles
// No tiene enfriamiento, siempre se puede usar
public class AtaqueBasico : Habilidad
{
    // Constructor: sin enfriamiento (0)
    public AtaqueBasico() : base("Ataque Básico", 0) { }

    // Aplica el daño del lanzador al objetivo enemigo
    public override void Usar(Hormiga lanzador, Hormiga objetivo, List<Hormiga> aliados, List<Enemigo> enemigos)
    {
        // El daño es el ataque del lanzador
        objetivo.RecibirDaño(lanzador.Stats.Ataque);
    }
}
