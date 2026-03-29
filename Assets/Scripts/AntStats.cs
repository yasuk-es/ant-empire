using System;

// Clase que maneja todas las estadísticas de la hormiga
public class AntStats
{
    // Vida máxima
    public int MaxHealth { get; set; }

    // Vida actual
    public int CurrentHealth { get; set; }

    // Ataque
    public int Attack { get; set; }

    // Defensa
    public int Defense { get; set; }

    // Velocidad (define el orden de turnos)
    public int Speed { get; set; }

    // Experiencia acumulada
    public int Experience { get; set; }

    // Nivel actual
    public int Level { get; set; }

    // Constructor
    public AntStats(int health, int attack, int defense, int speed)
    {
        MaxHealth = health;
        CurrentHealth = health; // Al iniciar, la vida actual es igual a la máxima
        Attack = attack;
        Defense = defense;
        Speed = speed;
        Experience = 0;
        Level = 1;
    }

    // Aplica daño considerando la defensa
    public void TakeDamage(int damage)
    {
        // Calcula el daño final
        int finalDamage = Math.Max(damage - Defense, 0);

        // Reduce la vida actual
        CurrentHealth -= finalDamage;

        // Evita valores negativos
        if (CurrentHealth < 0)
            CurrentHealth = 0;
    }

    // Verifica si la hormiga sigue viva
    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    // Añade experiencia y gestiona subida de nivel
    public void GainExperience(int amount)
    {
        Experience += amount;

        // Cada 100 de experiencia sube de nivel
        if (Experience >= 100)
        {
            Level++;
            Experience -= 100;
        }
    }
}