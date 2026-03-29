// Sistema de estadísticas de las hormigas

public class AntStats
{
    // Salud de la hormiga
    public int Health { get; set; }

    // Ataque de la hormiga
    public int Attack { get; set; }

    // Defensa de la hormiga
    public int Defense { get; set; }

    // Velocidad de la hormiga
    public int Speed { get; set; }

    // Experiencia de la hormiga
    public int Experience { get; set; }

    // Nivel de la hormiga
    public int Level { get; set; }

    // Constructor
    public AntStats(int health, int attack, int defense, int speed)
    {
        Health = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Experience = 0; // La experiencia inicial es 0
        Level = 1; // La hormiga comienza en el nivel 1
    }

    // Método para aplicar daño a la hormiga
    public void TakeDamage(int damage)
    {
        int damageTaken = damage - Defense; // Aplicar defensa
        if (damageTaken > 0)
        {
            Health -= damageTaken; // Restar la salud
        }
    }

    // Método para aumentar la experiencia y nivel
    public void GainExperience(int amount)
    {
        Experience += amount;
        // Lógica para aumentar el nivel según la experiencia
        if (Experience >= 100) // Por ejemplo, cada 100 puntos de experiencia se sube de nivel
        {
            Level++;
            Experience -= 100; // Restar experiencia al nivel siguiente
        }
    }
}