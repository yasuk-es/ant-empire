using System.Collections.Generic;

// Habilidad de ataque básico: disponible para todos los roles
// No tiene enfriamiento, siempre se puede usar
public class AtaqueBasico : Habilidad
{
    // Constructor: sin enfriamiento (0)
    public AtaqueBasico() : base("Ataque Básico", 0) { }

    // Aplica el daño del lanzador al objetivo enemigo
    public override void Usar(Hormiga lanzador, Enemigo objetivoEnemigo, List<Hormiga> aliados, List<Enemigo> enemigos)
    {
        // Aplica el daño directamente al enemigo objetivo
        if (objetivoEnemigo != null)
            objetivoEnemigo.Stats.RecibirDaño(lanzador.Stats.Ataque);
    }
}
