using System;

namespace Ants {
    // Clase que representa una hormiga
    public class Ant {
        // Propiedades de la hormiga
        public string Name { get; set; }  // Nombre de la hormiga
        public string Type { get; set; }  // Tipo de hormiga (p.ej., obrera, reina)
        public int Level { get; set; }  // Nivel de la hormiga
        public int[] Stats { get; set; }  // Estadísticas de la hormiga (vida, daño, etc.)
        public string[] Equipment { get; set; }  // Equipamiento de la hormiga

        // Constructor de la clase Ant
        public Ant(string name, string type, int level, int[] stats, string[] equipment) {
            Name = name;
            Type = type;
            Level = level;
            Stats = stats;
            Equipment = equipment;
        }

        // Método para recibir daño
        public void TakeDamage(int damage) {
            Stats[0] -= damage;  // Suponiendo que el primer índice es 'vida'
            if (Stats[0] < 0) {
                Stats[0] = 0;  // La vida no puede ser negativa
            }
        }

        // Método para ganar experiencia
        public void GainExperience(int experience) {
            // Lógica para ganar experiencia y subir de nivel
            // Aquí se puede agregar la lógica de experiencia
            Level += experience / 100;  // Por ejemplo, sube de nivel cada 100 de experiencia
        }

        // Método para subir de nivel
        public void LevelUp() {
            Level++;  // Incrementa el nivel
            // Aquí se pueden incrementar los stats si se desea
        }
    }
}