using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controla toda la interfaz visual del combate
// Layout lateral: hormigas izquierda, enemigos derecha (estilo Darkest Dungeon)
// Mecánica de turnos por velocidad (estilo Summoners War)
public class CombateUI : MonoBehaviour
{
    // Referencia al sistema de combate
    private SistemaCombate sistemaCombate;

    // ── UNIDADES ──────────────────────────────────────────────
    // Barras de vida de las 3 hormigas (lado izquierdo)
    [SerializeField] private Slider[] barrasVidaHormigas;
    [SerializeField] private TextMeshProUGUI[] nombresHormigas;

    // Barras de vida de los 3 enemigos (lado derecho)
    [SerializeField] private Slider[] barrasVidaEnemigos;
    [SerializeField] private TextMeshProUGUI[] nombresEnemigos;

    // ── HABILIDADES ───────────────────────────────────────────
    // Panel con los 2 botones de habilidades (visible solo en turno manual)
    [SerializeField] private GameObject panelHabilidades;
    [SerializeField] private Button[] botonesHabilidades;
    [SerializeField] private TextMeshProUGUI[] textosHabilidades;

    // ── MODO AUTOMÁTICO ───────────────────────────────────────
    // Toggle para cambiar entre manual y automático en cualquier momento
    [SerializeField] private Toggle toggleAutomatico;
    [SerializeField] private TextMeshProUGUI textoModo;

    // ── RESULTADO ─────────────────────────────────────────────
    [SerializeField] private GameObject panelResultado;
    [SerializeField] private TextMeshProUGUI textoResultado;
    [SerializeField] private Button botonSiguiente;
    [SerializeField] private Button botonRepetir;
    [SerializeField] private Button botonMenu;

    // Hormiga cuyo turno está activo
    private Hormiga hormigaActual;

    void Start()
    {
        sistemaCombate = FindFirstObjectByType<SistemaCombate>();
        panelResultado.SetActive(false);
        panelHabilidades.SetActive(false);
        toggleAutomatico.onValueChanged.AddListener(OnToggleAutomatico);
        ActualizarTextoModo(false);
    }


    // Refresca todas las barras de vida (llamar después de cada acción)
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

    // Muestra los botones de habilidades cuando es turno de una hormiga (modo manual)
    public void MostrarTurnoJugador(Hormiga hormiga)
    {
        hormigaActual = hormiga;
        panelHabilidades.SetActive(true);

        for (int i = 0; i < botonesHabilidades.Length; i++)
        {
            if (i >= hormiga.Habilidades.Count) break;
            Habilidad hab = hormiga.Habilidades[i];
            bool disponible = hab.PuedeUsarse();
            // Muestra el enfriamiento restante si no está lista
            textosHabilidades[i].text = disponible
                ? hab.Nombre
                : $"{hab.Nombre}\n(CD: {hab.EnfriamientoActual})";
            botonesHabilidades[i].interactable = disponible;
        }
    }

    // Oculta los botones de habilidades
    public void OcultarHabilidades()
    {
        panelHabilidades.SetActive(false);
        hormigaActual = null;
    }

    // Botón 1: Ataque Básico
    public void OnBotonHabilidad1() { EjecutarHabilidadManual(0); }

    // Botón 2: Habilidad especial con enfriamiento
    public void OnBotonHabilidad2() { EjecutarHabilidadManual(1); }

    void EjecutarHabilidadManual(int indice)
    {
        if (hormigaActual == null) return;
        Habilidad habilidad = hormigaActual.Habilidades[indice];
        if (!habilidad.PuedeUsarse()) return;

        // Primer enemigo vivo como objetivo
        Enemigo objetivo = sistemaCombate.enemigos.Find(e => e.EstaVivo());
        if (objetivo == null) return;

        OcultarHabilidades();
        sistemaCombate.JugadorEligeHabilidad(hormigaActual, habilidad, objetivo);
    }

    // Llamado cuando el jugador mueve el toggle de modo automático
    public void OnToggleAutomatico(bool activo)
    {
        sistemaCombate.CambiarModoAutomatico(activo);
        ActualizarTextoModo(activo);

        // Si se activa el auto mientras hay botones visibles, continúa el turno solo
        if (activo && hormigaActual != null)
        {
            Hormiga hormiga = hormigaActual;
            OcultarHabilidades();
            sistemaCombate.EjecutarTurnoAutomaticoDesdeUI(hormiga);
        }
    }

    void ActualizarTextoModo(bool automatico)
    {
        textoModo.text = automatico ? "Modo: Automático" : "Modo: Manual";
    }

    // Feedback cuando un enemigo ataca (aquí se puede agregar animación después)
    public void MostrarAtaqueEnemigo(Enemigo atacante, Hormiga objetivo)
    {
        Debug.Log($"{atacante.Nombre} ataca a {objetivo.Nombre}!");
    }

    // Muestra el panel de resultado al terminar la batalla
    public void MostrarResultado(bool victoria)
    {
        OcultarHabilidades();
        panelResultado.SetActive(true);
        textoResultado.text = victoria ? "¡Victoria!" : "Derrota...";

        // "Siguiente" solo aparece en victoria y si hay escenario siguiente
        int siguiente = GameManager.Instancia.EscenarioActual + 1;
        botonSiguiente.gameObject.SetActive(victoria && siguiente <= 3);
    }

    // Botón "Siguiente escenario"
    public void OnBotonSiguiente()
    {
        GameManager.Instancia.GuardarEscenario(GameManager.Instancia.EscenarioActual + 1);
        GameManager.Instancia.IrASeleccion();
    }

    // Botón "Repetir": mismo escenario, vuelve a selección
    public void OnBotonRepetir()
    {
        GameManager.Instancia.IrASeleccion();
    }

    // Botón "Menú Principal"
    public void OnBotonMenu()
    {
        GameManager.Instancia.IrAMenuPrincipal();
    }
}
