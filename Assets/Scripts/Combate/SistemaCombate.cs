using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Sistema de combate por turnos estilo Darkest Dungeon
// Controla el orden de turnos, las acciones y el fin de la batalla
public class SistemaCombate : MonoBehaviour
{
    // Equipo del jugador (3 hormigas)
    public List<Hormiga> equipo = new List<Hormiga>();

    // Equipo enemigo (3 bichos del escenario)
    public List<Enemigo> enemigos = new List<Enemigo>();

    // Índice del turno actual dentro del orden de turnos
    private int indiceTurno = 0;

    // Lista ordenada por velocidad que define quién actúa primero
    private List<object> ordenTurnos = new List<object>();

    // Referencia a la UI de combate para actualizar pantalla
    private CombateUI combateUI;

    // Modo automático: si está activo, las hormigas eligen sus habilidades solas
    public bool ModoAutomatico { get; private set; } = false;

    // Se llama al iniciar la escena de combate
    void Start()
    {
        // Busca la UI en la escena
        combateUI = FindFirstObjectByType<CombateUI>();

        // Crea los equipos según el escenario seleccionado
        InicializarEquipos();

        // Calcula el orden de turnos por velocidad
        CalcularOrdenTurnos();

        // Actualiza la UI con el estado inicial
        combateUI.ActualizarUI(equipo, enemigos);

        // Inicia el primer turno
        StartCoroutine(EjecutarTurno());
    }

    // Crea las hormigas del jugador y los enemigos del escenario actual
    void InicializarEquipos()
    {
        // Obtiene las hormigas seleccionadas por el jugador desde el GameManager
        equipo = GameManager.Instancia.HormigasSeleccionadas;

        // Obtiene los enemigos del escenario actual
        int escenario = GameManager.Instancia.EscenarioActual;
        enemigos = FabricaEnemigos.CrearEnemigos(escenario);
    }

    // Ordena todas las unidades por velocidad (mayor velocidad actúa primero)
    void CalcularOrdenTurnos()
    {
        ordenTurnos.Clear();

        // Agrega hormigas y enemigos a la misma lista
        foreach (var h in equipo) ordenTurnos.Add(h);
        foreach (var e in enemigos) ordenTurnos.Add(e);

        // Ordena de mayor a menor velocidad
        ordenTurnos = ordenTurnos
            .OrderByDescending(u => u is Hormiga h ? h.Stats.Velocidad : ((Enemigo)u).Stats.Velocidad)
            .ToList();
    }

    // Corrutina que ejecuta cada turno uno por uno
    IEnumerator EjecutarTurno()
    {
        // Espera un momento antes de empezar (para que se vea la UI)
        yield return new WaitForSeconds(0.5f);

        // Verifica si la batalla ya terminó
        if (VerificarFinBatalla()) yield break;

        // Obtiene la unidad que actúa en este turno
        object unidadActual = ordenTurnos[indiceTurno];

        // Actúa según si es hormiga o enemigo
        if (unidadActual is Hormiga hormiga)
        {
            // Si la hormiga está viva, es el turno del jugador
            if (hormiga.EstaViva())
            {
                // Reduce el enfriamiento de todas sus habilidades
                foreach (var hab in hormiga.Habilidades)
                    hab.ReducirEnfriamiento();

                if (ModoAutomatico)
                {
                    // En modo automático la hormiga elige sola su habilidad
                    yield return new WaitForSeconds(0.8f);
                    EjecutarTurnoAutomatico(hormiga);
                }
                else
                {
                    // En modo manual espera a que el jugador elija desde la UI
                    combateUI.MostrarTurnoJugador(hormiga);
                    yield break; // El turno continúa cuando el jugador elige
                }
            }
        }
        else if (unidadActual is Enemigo enemigo)
        {
            // Si el enemigo está vivo, actúa automáticamente
            if (enemigo.EstaVivo())
            {
                // Si está aturdido, pierde el turno
                if (enemigo.Aturdido)
                {
                    Debug.Log($"{enemigo.Nombre} está aturdido y pierde su turno.");
                    enemigo.Aturdido = false; // se recupera del aturdimiento
                }
                else
                {
                    // El enemigo ataca a una hormiga viva aleatoria
                    Hormiga objetivo = ObtenerHormigaVivaAleatoria();
                    if (objetivo != null)
                    {
                        enemigo.Atacar(objetivo);
                        combateUI.MostrarAtaqueEnemigo(enemigo, objetivo);
                    }
                }

                // Espera para que se vea la animación
                yield return new WaitForSeconds(1f);
            }

            // Pasa al siguiente turno automáticamente
            PasarSiguienteTurno();
        }
    }

    // Llamado desde la UI cuando el jugador elige una habilidad
    public void JugadorEligeHabilidad(Hormiga lanzador, Habilidad habilidad, Enemigo objetivo)
    {
        // Ejecuta la habilidad sobre el objetivo
        habilidad.Usar(lanzador, null, equipo, enemigos);

        // Actualiza la UI
        combateUI.ActualizarUI(equipo, enemigos);

        // Pasa al siguiente turno
        PasarSiguienteTurno();
    }

    // Avanza al siguiente turno en el orden
    public void PasarSiguienteTurno()
    {
        // Avanza el índice, volviendo al inicio si llegó al final
        indiceTurno = (indiceTurno + 1) % ordenTurnos.Count;

        // Inicia el siguiente turno
        StartCoroutine(EjecutarTurno());
    }

    // Llamado desde la UI cuando el toggle se activa en mitad de un turno manual
    public void EjecutarTurnoAutomaticoDesdeUI(Hormiga hormiga)
    {
        EjecutarTurnoAutomatico(hormiga);
    }

    // Lógica automática: elige la mejor habilidad disponible según el rol
    void EjecutarTurnoAutomatico(Hormiga hormiga)
    {
        Habilidad elegida;

        // Intenta usar la habilidad especial (índice 1) si está disponible
        // Si no, usa el ataque básico (índice 0)
        if (hormiga.Habilidades[1].PuedeUsarse())
            elegida = hormiga.Habilidades[1];
        else
            elegida = hormiga.Habilidades[0];

        // Busca el primer enemigo vivo como objetivo
        Enemigo objetivo = enemigos.Find(e => e.EstaVivo());
        if (objetivo == null) return;

        // Ejecuta la habilidad
        elegida.Usar(hormiga, null, equipo, enemigos);
        combateUI.ActualizarUI(equipo, enemigos);

        // Verifica si la batalla terminó antes de pasar el turno
        if (!VerificarFinBatalla())
            PasarSiguienteTurno();
    }

    // Activa o desactiva el modo automático (llamado desde el toggle de la UI)
    public void CambiarModoAutomatico(bool activo)
    {
        ModoAutomatico = activo;
    }

    // Devuelve una hormiga viva al azar para que el enemigo ataque
    Hormiga ObtenerHormigaVivaAleatoria()
    {
        // Filtra solo las hormigas vivas
        var vivas = equipo.Where(h => h.EstaViva()).ToList();
        if (vivas.Count == 0) return null;

        // Elige una al azar
        return vivas[Random.Range(0, vivas.Count)];
    }

    // Verifica si la batalla terminó (todos muertos de un lado)
    bool VerificarFinBatalla()
    {
        // Si todas las hormigas murieron, el jugador perdió
        if (equipo.All(h => !h.EstaViva()))
        {
            combateUI.MostrarResultado(false);
            return true;
        }

        // Si todos los enemigos murieron, el jugador ganó
        if (enemigos.All(e => !e.EstaVivo()))
        {
            combateUI.MostrarResultado(true);
            return true;
        }

        return false;
    }
}
