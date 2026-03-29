using System.Collections.Generic;

// Fábrica que crea los enemigos según el escenario seleccionado
// Escenario 1: 3 Slimes | Escenario 2: 3 Larvas | Escenario 3: 3 Arañas
public static class FabricaEnemigos
{
    // Devuelve la lista de 3 enemigos según el número de escenario
    public static List<Enemigo> CrearEnemigos(int escenario)
    {
        switch (escenario)
        {
            case 1: return CrearSlimes();
            case 2: return CrearLarvas();
            case 3: return CrearArañas();
            default: return CrearSlimes(); // por defecto usa slimes
        }
    }

    // Escenario 1: 3 Slimes (enemigos fáciles, poca vida y ataque)
    static List<Enemigo> CrearSlimes()
    {
        return new List<Enemigo>
        {
            new Enemigo("Slime 1", new EstadisticasHormiga(40, 8, 2, 4), "slime"),
            new Enemigo("Slime 2", new EstadisticasHormiga(40, 8, 2, 4), "slime"),
            new Enemigo("Slime 3", new EstadisticasHormiga(40, 8, 2, 4), "slime")
        };
    }

    // Escenario 2: 3 Larvas (enemigos medios, más vida y ataque que slimes)
    static List<Enemigo> CrearLarvas()
    {
        return new List<Enemigo>
        {
            new Enemigo("Larva 1", new EstadisticasHormiga(60, 12, 4, 6), "larva"),
            new Enemigo("Larva 2", new EstadisticasHormiga(60, 12, 4, 6), "larva"),
            new Enemigo("Larva 3", new EstadisticasHormiga(60, 12, 4, 6), "larva")
        };
    }

    // Escenario 3: 3 Arañas (enemigos difíciles, más velocidad y daño)
    static List<Enemigo> CrearArañas()
    {
        return new List<Enemigo>
        {
            new Enemigo("Araña 1", new EstadisticasHormiga(80, 18, 6, 10), "araña"),
            new Enemigo("Araña 2", new EstadisticasHormiga(80, 18, 6, 10), "araña"),
            new Enemigo("Araña 3", new EstadisticasHormiga(80, 18, 6, 10), "araña")
        };
    }
}
