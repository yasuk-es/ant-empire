// Clase que representa a un enemigo en combate
// Los enemigos son bichos distintos a las hormigas (slimes, larvas, arañas, etc.)
public class Enemigo
{
    // Nombre del enemigo (ej: "Slime", "Larva", "Araña")
    public string Nombre { get; set; }

    // Estadísticas de combate (vida, ataque, defensa, velocidad)
    public EstadisticasHormiga Stats { get; set; }

    // Nombre del sprite que usará el diseñador (ej: "slime", "larva", "araña")
    public string NombreSprite { get; set; }

    // Si está aturdido, pierde su turno (lo activa el GolpeAturdidor del Tanque)
    public bool Aturdido { get; set; }

    // Constructor: crea un enemigo con sus datos
    public Enemigo(string nombre, EstadisticasHormiga stats, string nombreSprite)
    {
        Nombre = nombre;
        Stats = stats;
        NombreSprite = nombreSprite;
    }

    // El enemigo ataca a una hormiga objetivo
    // Por ahora el enemigo solo hace ataque básico
    public void Atacar(Hormiga objetivo)
    {
        objetivo.RecibirDaño(Stats.Ataque);
    }

    // Devuelve true si el enemigo sigue vivo
    public bool EstaVivo()
    {
        return Stats.EstaViva();
    }
}
