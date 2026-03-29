using System.Collections.Generic;

// Representa una hormiga dentro del juego
public class Ant
{
    // Nombre de la hormiga
    public string Name { get; set; }

    // Tipo de hormiga (obrera, soldado, reina, etc.)
    public string Type { get; set; }

    // Rol en combate (Tank, Fighter, Support)
    public AntRole Role { get; set; }

    // Estadísticas de la hormiga
    public AntStats Stats { get; set; }

    // Lista de habilidades que posee la hormiga
    public List<Skill> Skills { get; set; }

    // Constructor
    public Ant(string name, string type, AntRole role, AntStats stats, List<Skill> skills)
    {
        Name = name;
        Type = type;
        Role = role;
        Stats = stats;
        Skills = skills;
    }

    // Método para recibir daño (delegado a AntStats)
    public void TakeDamage(int damage)
    {
        Stats.TakeDamage(damage);
    }
}

// Define los roles disponibles en el juego
public enum AntRole
{
    Tank,
    Fighter,
    Support
}