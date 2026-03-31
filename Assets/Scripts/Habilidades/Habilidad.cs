using System.Collections.Generic;

// Clase base abstracta para todas las habilidades del juego
// Toda habilidad hereda de esta clase y debe implementar "Usar"
public abstract class Habilidad
{
    // Nombre de la habilidad que se muestra en pantalla
    public string Nombre { get; protected set; }

    // Turnos que hay que esperar para volver a usarla (0 = sin enfriamiento)
    public int Enfriamiento { get; protected set; }

    // Turnos restantes hasta poder usarla de nuevo
    public int EnfriamientoActual { get; protected set; }

    // Constructor base
    public Habilidad(string nombre, int enfriamiento)
    {
        Nombre = nombre;
        Enfriamiento = enfriamiento;
        EnfriamientoActual = 0; // al inicio está lista para usar
    }

    // Devuelve true si la habilidad está disponible para usar
    public bool PuedeUsarse()
    {
        return EnfriamientoActual == 0;
    }

    // Reduce el enfriamiento en 1 al pasar el turno
    public void ReducirEnfriamiento()
    {
        if (EnfriamientoActual > 0)
            EnfriamientoActual--;
    }

    // Activa el enfriamiento después de usar la habilidad
    protected void ActivarEnfriamiento()
    {
        EnfriamientoActual = Enfriamiento;
    }

    // Método que cada habilidad implementa con su lógica propia
    // objetivoEnemigo: el enemigo al que se ataca (null si la habilidad es de soporte)
    public abstract void Usar(Hormiga lanzador, Enemigo objetivoEnemigo, List<Hormiga> aliados, List<Enemigo> enemigos);
}
