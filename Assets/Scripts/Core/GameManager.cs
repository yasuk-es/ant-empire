using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager: controla el flujo global del juego
// Es un Singleton: existe una sola instancia en toda la partida
// Guarda qué hormigas eligió el jugador y qué escenario está jugando
public class GameManager : MonoBehaviour
{
    // Instancia única accesible desde cualquier script
    public static GameManager Instancia { get; private set; }

    // Hormigas que el jugador seleccionó para ir al combate
    public List<Hormiga> HormigasSeleccionadas { get; private set; } = new List<Hormiga>();

    // Número del escenario actual (1, 2 o 3)
    public int EscenarioActual { get; private set; } = 1;

    // Roster completo: todas las hormigas disponibles para elegir
    public List<Hormiga> RosterCompleto { get; private set; } = new List<Hormiga>();

    // Se ejecuta antes que Start(), configura el Singleton
    void Awake()
    {
        // Si ya existe una instancia, destruye este objeto duplicado
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        // Esta es la instancia principal, no se destruye al cambiar de escena
        Instancia = this;
        DontDestroyOnLoad(gameObject);

        // Crea el roster de hormigas disponibles al iniciar el juego
        CrearRoster();
    }

    // Crea las 6 hormigas disponibles (2 de cada rol)
    void CrearRoster()
    {
        // --- TANQUES ---
        // Tanque 1: más vida y defensa
        RosterCompleto.Add(new Hormiga(
            "Guardiana",
            RolHormiga.Tanque,
            new EstadisticasHormiga(150, 20, 15, 5),
            new List<Habilidad> { new AtaqueBasico(), new GolpeAturdidor() },
            "escudo_1"
        ));

        // Tanque 2: más ataque pero menos defensa
        RosterCompleto.Add(new Hormiga(
            "Coraza",
            RolHormiga.Tanque,
            new EstadisticasHormiga(130, 25, 12, 6),
            new List<Habilidad> { new AtaqueBasico(), new GolpeAturdidor() },
            "escudo_2"
        ));

        // --- LUCHADORES ---
        // Luchador 1: equilibrado
        RosterCompleto.Add(new Hormiga(
            "Soldado",
            RolHormiga.Luchador,
            new EstadisticasHormiga(100, 35, 5, 10),
            new List<Habilidad> { new AtaqueBasico(), new GolpeDoble() },
            "espada_1"
        ));

        // Luchador 2: más velocidad, menos vida
        RosterCompleto.Add(new Hormiga(
            "Veloz",
            RolHormiga.Luchador,
            new EstadisticasHormiga(85, 30, 4, 14),
            new List<Habilidad> { new AtaqueBasico(), new GolpeDoble() },
            "espada_2"
        ));

        // --- SOPORTES ---
        // Soporte 1: más curación
        RosterCompleto.Add(new Hormiga(
            "Sanadora",
            RolHormiga.Soporte,
            new EstadisticasHormiga(90, 15, 8, 12),
            new List<Habilidad> { new AtaqueBasico(), new Curar() },
            "cruz_1"
        ));

        // Soporte 2: más velocidad para curar antes
        RosterCompleto.Add(new Hormiga(
            "Enfermera",
            RolHormiga.Soporte,
            new EstadisticasHormiga(80, 12, 6, 15),
            new List<Habilidad> { new AtaqueBasico(), new Curar() },
            "cruz_2"
        ));
    }

    // Guarda las hormigas que el jugador eligió en la pantalla de selección
    public void GuardarSeleccion(List<Hormiga> seleccionadas)
    {
        HormigasSeleccionadas = seleccionadas;
    }

    // Guarda el escenario que el jugador eligió en el mapa
    public void GuardarEscenario(int numero)
    {
        EscenarioActual = numero;
    }

    // --- NAVEGACIÓN ENTRE ESCENAS ---

    // Va al menú principal
    public void IrAMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Va al mapa de escenarios
    public void IrAMapa()
    {
        SceneManager.LoadScene("Mapa");
    }

    // Va a la pantalla de selección de hormigas
    public void IrASeleccion()
    {
        SceneManager.LoadScene("Seleccion");
    }

    // Va a la escena de combate
    public void IrACombate()
    {
        SceneManager.LoadScene("Combate");
    }
}
