using System.Collections.Generic;
using UnityEngine;

// Habilidad exclusiva del Tanque
// Golpea al enemigo y tiene probabilidad de aturdirlo (pierde su turno)
public class GolpeAturdidor : Habilidad
{
    // Probabilidad de aturdir al enemigo (0.0 a 1.0)
    private float probabilidadAturdimiento = 0.5f; // 50% de chance

    // Constructor: enfriamiento de 3 turnos
    public GolpeAturdidor() : base("Golpe Aturdidor", 3) { }

    public override void Usar(Hormiga lanzador, Enemigo objetivoEnemigo, List<Hormiga> aliados, List<Enemigo> enemigos)
    {
        // Aplica el daño normal del tanque
        if (objetivoEnemigo != null)
            objetivoEnemigo.Stats.RecibirDaño(lanzador.Stats.Ataque);

        // Lanza un número aleatorio entre 0 y 1
        float tirada = Random.value;

        // Si la tirada es menor a la probabilidad, el enemigo queda aturdido
        if (tirada < probabilidadAturdimiento && objetivoEnemigo != null)
        {
            objetivoEnemigo.Aturdido = true;
            Debug.Log($"{objetivoEnemigo.Nombre} fue aturdido!");
        }

        // Activa el enfriamiento de la habilidad
        ActivarEnfriamiento();
    }
}
