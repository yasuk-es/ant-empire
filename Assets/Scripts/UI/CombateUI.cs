using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controla toda la interfaz visual del combate
// Layout lateral: hormigas izquierda, enemigos derecha (estilo Darkest Dungeon)
public class CombateUI : MonoBehaviour
{
    private SistemaCombate sistemaCombate;

    // Barras de vida de las 3 hormigas (solo lectura, no interactuables)
    [SerializeField] private Slider[] barrasVidaHormigas;
    [SerializeField] private TextMeshProUGUI[] nombresHormigas;

    // Barras de vida de los 3 enemigos (solo lectura)
    [SerializeField] private Slider[] barrasVidaEnemigos;
    [SerializeField] private TextMeshProUGUI[] nombresEnemigos;

    // Flechas de seleccion de enemigo (una por enemigo, se activa la seleccionada)
    [SerializeField] private GameObject[] flechasEnemigos;

    // Panel con los 2 botones de habilidades
    [SerializeField] private GameObject panelHabilidades;
    [SerializeField] private Button[] botonesHabilidades;
    [SerializeField] private TextMeshProUGUI[] textosHabilidades;

    // Toggle para cambiar entre manual y automatico
    [SerializeField] private Toggle toggleAutomatico;
    [SerializeField] private TextMeshProUGUI textoModo;

    // Panel de resultado
    [SerializeField] private GameObject panelResultado;
    [SerializeField] private TextMeshProUGUI textoResultado;
    [SerializeField] private Button botonSiguiente;
    [SerializeField] private Button botonRepetir;
    [SerializeField] private Button botonMenu;

    // Hormiga cuyo turno esta activo
    private Hormiga hormigaActual;

    // Indice del enemigo seleccionado como objetivo
    private int indiceEnemigoSeleccionado = 0;

    void Start()
    {
        sistemaCombate = FindFirstObjectByType<SistemaCombate>();
        panelResultado.SetActive(false);
        panelHabilidades.SetActive(false);
        toggleAutomatico.onValueChanged.AddListener(OnToggleAutomatico);
        ActualizarTextoModo(false);

        // Desactiva la interaccion de todos los sliders para que no sean modificables
        BloquearSliders();
    }

    // Desactiva la interaccion de los sliders (el jugador no puede moverlos)
    void BloquearSliders()
    {
        foreach (var s in barrasVidaHormigas)
            if (s != null) s.interactable = false;
        foreach (var s in barrasVidaEnemigos)
            if (s != null) s.interactable = false;
    }

    // Refresca todas las barras de vida
    public void ActualizarUI(List<Hormiga> equipo, List<Enemigo> enemigos)
    {
        for (int i = 0; i < barrasVidaHormigas.Length; i++)
        {
            if (i >= equipo.Count) break;
            barrasVidaHormigas[i].maxValue = equipo[i].Stats.VidaMaxima;
            barrasVidaHormigas[i].value    = equipo[i].Stats.VidaActual;
            nombresHormigas[i].text        = equipo[i].Nombre;
        }
        for (int i = 0; i < barrasVidaEnemigos.Length; i++)
        {
            if (i >= enemigos.Count) break;
            barrasVidaEnemigos[i].maxValue = enemigos[i].Stats.VidaMaxima;
            barrasVidaEnemigos[i].value    = enemigos[i].Stats.VidaActual;
            nombresEnemigos[i].text        = enemigos[i].Nombre;
        }
    }

    // Muestra los botones de habilidades y selecciona el primer enemigo vivo
    public void MostrarTurnoJugador(Hormiga hormiga)
    {
        hormigaActual = hormiga;
        panelHabilidades.SetActive(true);

        // Selecciona automaticamente el primer enemigo vivo
        SeleccionarPrimerEnemigoVivo();

        for (int i = 0; i < botonesHabilidades.Length; i++)
        {
            if (i >= hormiga.Habilidades.Count) break;
            Habilidad hab = hormiga.Habilidades[i];
            bool disponible = hab.PuedeUsarse();
            textosHabilidades[i].text = disponible
                ? hab.Nombre
                : hab.Nombre + "\n(CD: " + hab.EnfriamientoActual + ")";
            botonesHabilidades[i].interactable = disponible;
        }
    }

    // Selecciona el primer enemigo vivo y muestra su flecha
    void SeleccionarPrimerEnemigoVivo()
    {
        var enemigos = sistemaCombate.enemigos;
        for (int i = 0; i < enemigos.Count; i++)
        {
            if (enemigos[i].EstaVivo())
            {
                SeleccionarEnemigo(i);
                return;
            }
        }
    }

    // Activa la flecha del enemigo seleccionado y desactiva las demas
    void SeleccionarEnemigo(int indice)
    {
        indiceEnemigoSeleccionado = indice;
        if (flechasEnemigos == null) return;
        for (int i = 0; i < flechasEnemigos.Length; i++)
        {
            if (flechasEnemigos[i] != null)
                flechasEnemigos[i].SetActive(i == indice);
        }
    }

    // Llamado cuando el jugador toca el boton de un enemigo para seleccionarlo
    public void OnSeleccionarEnemigo(int indice)
    {
        var enemigos = sistemaCombate.enemigos;
        if (indice < enemigos.Count && enemigos[indice].EstaVivo())
            SeleccionarEnemigo(indice);
    }

    // Oculta los botones de habilidades y las flechas
    public void OcultarHabilidades()
    {
        panelHabilidades.SetActive(false);
        hormigaActual = null;
        // Oculta todas las flechas
        if (flechasEnemigos != null)
            foreach (var f in flechasEnemigos)
                if (f != null) f.SetActive(false);
    }

    public void OnBotonHabilidad1() { EjecutarHabilidadManual(0); }
    public void OnBotonHabilidad2() { EjecutarHabilidadManual(1); }

    void EjecutarHabilidadManual(int indice)
    {
        if (hormigaActual == null) return;
        Habilidad habilidad = hormigaActual.Habilidades[indice];
        if (!habilidad.PuedeUsarse()) return;

        // Usa el enemigo seleccionado por el jugador con la flecha
        var enemigos = sistemaCombate.enemigos;
        Enemigo objetivo = null;
        if (indiceEnemigoSeleccionado < enemigos.Count && enemigos[indiceEnemigoSeleccionado].EstaVivo())
            objetivo = enemigos[indiceEnemigoSeleccionado];
        else
            objetivo = enemigos.Find(e => e.EstaVivo()); // fallback al primero vivo

        if (objetivo == null) return;
        OcultarHabilidades();
        sistemaCombate.JugadorEligeHabilidad(hormigaActual, habilidad, objetivo);
    }

    // Llamado cuando el jugador mueve el toggle de modo automatico
    public void OnToggleAutomatico(bool activo)
    {
        sistemaCombate.CambiarModoAutomatico(activo);
        ActualizarTextoModo(activo);
        if (activo && hormigaActual != null)
        {
            Hormiga h = hormigaActual;
            OcultarHabilidades();
            sistemaCombate.EjecutarTurnoAutomaticoDesdeUI(h);
        }
    }

    void ActualizarTextoModo(bool automatico)
    {
        textoModo.text = automatico ? "Modo: Automatico" : "Modo: Manual";
    }

    public void MostrarAtaqueEnemigo(Enemigo atacante, Hormiga objetivo)
    {
        Debug.Log(atacante.Nombre + " ataca a " + objetivo.Nombre + "!");
    }

    // Muestra el panel de resultado al terminar la batalla
    public void MostrarResultado(bool victoria)
    {
        OcultarHabilidades();
        panelResultado.SetActive(true);
        textoResultado.text = victoria ? "Victoria!" : "Derrota...";
        int siguiente = GameManager.Instancia.EscenarioActual + 1;
        botonSiguiente.gameObject.SetActive(victoria && siguiente <= 3);
    }

    public void OnBotonSiguiente()
    {
        GameManager.Instancia.GuardarEscenario(GameManager.Instancia.EscenarioActual + 1);
        GameManager.Instancia.IrASeleccion();
    }

    public void OnBotonRepetir() { GameManager.Instancia.IrASeleccion(); }
    public void OnBotonMenu() { GameManager.Instancia.IrAMenuPrincipal(); }
}