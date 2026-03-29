using System;

// Clase que contiene todas las estadísticas de una hormiga
// Aquí se guarda la vida, ataque, defensa y velocidad
public class EstadisticasHormiga
{
    // Vida máxima de la hormiga
    public int VidaMaxima { get; set; }

    // Vida actual (va bajando cuando recibe daño)
    public int VidaActual { get; set; }

    // Cuánto daño hace al atacar
    public int Ataque { get; set; }

    // Cuánto daño absorbe al recibir un golpe
    public int Defensa { get; set; }

    // Velocidad: determina quién actúa primero en el turno
    public int Velocidad { get; set; }

    // Constructor: se llama al crear una hormiga nueva
    public EstadisticasHormiga(int vida, int ataque, int defensa, int velocidad)
    {
        VidaMaxima = vida;
        VidaActual = vida; // al inicio, la vida actual es igual a la máxima
        Ataque = ataque;
        Defensa = defensa;
        Velocidad = velocidad;
    }

    // Aplica daño a la hormiga, restando la defensa
    public void RecibirDaño(int daño)
    {
        // El daño final no puede ser menor a 0
        int dañoFinal = Math.Max(daño - Defensa, 0);
        VidaActual -= dañoFinal;

        // La vida no puede bajar de 0
        if (VidaActual < 0) VidaActual = 0;
    }

    // Cura a la hormiga sin superar la vida máxima
    public void Curar(int cantidad)
    {
        VidaActual += cantidad;
        if (VidaActual > VidaMaxima) VidaActual = VidaMaxima;
    }

    // Devuelve true si la hormiga sigue viva
    public bool EstaViva()
    {
        return VidaActual > 0;
    }
}
