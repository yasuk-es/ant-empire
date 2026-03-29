using System.Collections.Generic;
using UnityEngine;

// Habilidad exclusiva del Luchador
// Golpea una vez y tiene una pequeña probabilidad de golpear una segunda vez
public class GolpeDoble : Habilidad
{
    // Probabilidad de que ocurra el segundo golpe (10%)
    private float probabilidadSegundoGolpe = 0.1f;

    // Constructor: enfriamiento de 2 turnos
    public GolpeDoble() : base("Golpe Doble", 2) { }

    public override void Usar(Hormiga lanzador, Hormiga objetivo, List<Hormiga> aliados, List<Enemigo> enemigos)
    {
        // Primer golpe: siempre ocurre
        objetivo.RecibirDaño(lanzador.Stats.Ataque);
        Debug.Log($"{lanzador.Nombre} golpea a {objetivo.Nombre}!");

        // Lanza un número aleatorio para ver si hay segundo golpe
        float tirada = Random.value;

        // Si la tirada es menor a la probabilidad, golpea una segunda vez
        if (tirada < probabilidadSegundoGolpe)
        {
            objetivo.RecibirDaño(lanzador.Stats.Ataque);
            Debug.Log($"{lanzador.Nombre} golpea de nuevo! (golpe doble)");
        }

        // Activa el enfriamiento
        ActivarEnfriamiento();
    }
}
