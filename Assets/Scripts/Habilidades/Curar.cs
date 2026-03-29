using System.Collections.Generic;
using UnityEngine;

// Habilidad exclusiva del Soporte
// Cura al aliado con menos vida del equipo
public class Curar : Habilidad
{
    // Cantidad fija de vida que restaura
    private int cantidadCura = 30;

    // Constructor: enfriamiento de 3 turnos
    public Curar() : base("Curar", 3) { }

    public override void Usar(Hormiga lanzador, Hormiga objetivo, List<Hormiga> aliados, List<Enemigo> enemigos)
    {
        // Busca al aliado vivo con menos vida para curarlo
        Hormiga masHerida = null;
        int menorVida = int.MaxValue;

        foreach (var aliado in aliados)
        {
            // Solo considera aliados vivos
            if (aliado.EstaViva() && aliado.Stats.VidaActual < menorVida)
            {
                menorVida = aliado.Stats.VidaActual;
                masHerida = aliado;
            }
        }

        // Si encontró un aliado herido, lo cura
        if (masHerida != null)
        {
            masHerida.Stats.Curar(cantidadCura);
            Debug.Log($"{lanzador.Nombre} cura a {masHerida.Nombre} por {cantidadCura} de vida!");
        }

        // Activa el enfriamiento
        ActivarEnfriamiento();
    }
}
