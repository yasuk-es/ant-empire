using System.Collections.Generic;

// Clase principal que representa a una hormiga del jugador
// Contiene su nombre, rol, estadísticas y habilidades
public class Hormiga
{
    // Nombre visible de la hormiga (ej: "Guardiana", "Soldado")
    public string Nombre { get; set; }

    // Rol en combate: Tanque, Luchador o Soporte
    public RolHormiga Rol { get; set; }

    // Estadísticas de combate (vida, ataque, defensa, velocidad)
    public EstadisticasHormiga Stats { get; set; }

    // Lista de habilidades disponibles (siempre 2: básica + especial)
    public List<Habilidad> Habilidades { get; set; }

    // Nombre del sprite que usará el diseñador (ej: "escudo", "espada", "cruz")
    public string NombreSprite { get; set; }

    // Constructor: crea una hormiga con todos sus datos
    public Hormiga(string nombre, RolHormiga rol, EstadisticasHormiga stats, List<Habilidad> habilidades, string nombreSprite)
    {
        Nombre = nombre;
        Rol = rol;
        Stats = stats;
        Habilidades = habilidades;
        NombreSprite = nombreSprite;
    }

    // Delega el daño a las estadísticas
    public void RecibirDaño(int daño)
    {
        Stats.RecibirDaño(daño);
    }

    // Devuelve true si la hormiga sigue viva
    public bool EstaViva()
    {
        return Stats.EstaViva();
    }
}
