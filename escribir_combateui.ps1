$ruta = "Assets\Scripts\UI\CombateUI.cs"
$codigo = @"
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controla toda la interfaz visual del combate
public class CombateUI : MonoBehaviour
{
    private SistemaCombate sistemaCombate;

    [SerializeField] private Slider[] barrasVidaHormigas;
    [SerializeField] private TextMeshProUGUI[] nombresHormigas;
    [SerializeField] private Slider[] barrasVidaEnemigos;
    [SerializeField] private TextMeshProUGUI[] nombresEnemigos;
    [SerializeField] private GameObject[] flechasEnemigos;
    [SerializeField] private GameObject panelHabilidades;
    [SerializeField] private Button[] botonesHabilidades;
    [SerializeField] private TextMeshProUGUI[] textosHabilidades;
    [SerializeField] private Toggle toggleAutomatico;
    [SerializeField] private TextMeshProUGUI textoModo;
    [SerializeField] private GameObject panelResultado;
    [SerializeField] private TextMeshProUGUI textoResultado;
    [SerializeField] private Button botonSiguiente;
    [SerializeField] private Button botonRepetir;
    [SerializeField] private Button botonMenu;
    // Boton para abandonar el combate y volver al mapa
    [SerializeField] private Button botonSalirCombate;

    private Hormiga hormigaActual;
    private int indiceEnemigoSeleccionado = 0;

    void Start()
    {
        sistemaCombate = FindFirstObjectByType<SistemaCombate>();
        panelResultado.SetActive(false);
        panelHabilidades.SetActive(false);

        // El toggle empieza en false (modo manual)
        toggleAutomatico.isOn = false;
        toggleAutomatico.onValueChanged.AddListener(OnToggleAutomatico);
        ActualizarTextoModo(false);

        BloquearSliders();
        OcultarFlechas();
    }

    // Desactiva la interaccion de los sliders
    void BloquearSliders()
    {
        foreach (var s in barrasVidaHormigas) if (s != null) s.interactable = false;
        foreach (var s in barrasVidaEnemigos) if (s != null) s.interactable = false;
    }

    void OcultarFlechas()
    {
        if (flechasEnemigos == null) return;
        foreach (var f in flechasEnemigos) if (f != null) f.SetActive(false);
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

    // Muestra los botones de habilidades cuando es turno del jugador
    public void MostrarTurnoJugador(Hormiga hormiga)
    {
        hormigaActual = hormiga;
        panelHabilidades.SetActive(true);
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

    void SeleccionarPrimerEnemigoVivo()
    {
        var enemigos = sistemaCombate.enemigos;
        for (int i = 0; i < enemigos.Count; i++)
        {
            if (enemigos[i].EstaVivo()) { SeleccionarEnemigo(i); return; }
        }
    }

    void SeleccionarEnemigo(int indice)
    {
        indiceEnemigoSeleccionado = indice;
        if (flechasEnemigos == null) return;
        for (int i = 0; i < flechasEnemigos.Length; i++)
            if (flechasEnemigos[i] != null)
                flechasEnemigos[i].SetActive(i == indice);
    }

    // Llamado por los botones invisibles sobre cada enemigo
    public void OnSeleccionarEnemigo(int indice)
    {
        var enemigos = sistemaCombate.enemigos;
        if (indice < enemigos.Count && enemigos[indice].EstaVivo())
            SeleccionarEnemigo(indice);
    }

    public void OcultarHabilidades()
    {
        panelHabilidades.SetActive(false);
        hormigaActual = null;
        OcultarFlechas();
    }

    public void OnBotonHabilidad1() { EjecutarHabilidadManual(0); }
    public void OnBotonHabilidad2() { EjecutarHabilidadManual(1); }

    void EjecutarHabilidadManual(int indice)
    {
        if (hormigaActual == null) return;
        Habilidad habilidad = hormigaActual.Habilidades[indice];
        if (!habilidad.PuedeUsarse()) return;

        // Para habilidades de ataque usa el enemigo seleccionado
        // Para Curar pasa null como enemigo (la habilidad ignora el objetivo enemigo)
        Enemigo objetivo = null;
        if (!(habilidad is Curar))
        {
            var enemigos = sistemaCombate.enemigos;
            if (indiceEnemigoSeleccionado < enemigos.Count && enemigos[indiceEnemigoSeleccionado].EstaVivo())
                objetivo = enemigos[indiceEnemigoSeleccionado];
            else
                objetivo = enemigos.Find(e => e.EstaVivo());
            if (objetivo == null) return;
        }

        OcultarHabilidades();
        sistemaCombate.JugadorEligeHabilidad(hormigaActual, habilidad, objetivo);
    }

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

    public void MostrarResultado(bool victoria)
    {
        OcultarHabilidades();
        panelResultado.SetActive(true);
        textoResultado.text = victoria ? "Victoria!" : "Derrota...";
        int siguiente = GameManager.Instancia.EscenarioActual + 1;
        botonSiguiente.gameObject.SetActive(victoria && siguiente <= 3);
    }

    // Boton salir del combate: vuelve al mapa sin guardar resultado
    public void OnBotonSalirCombate()
    {
        GameManager.Instancia.IrAMapa();
    }

    public void OnBotonSiguiente()
    {
        GameManager.Instancia.GuardarEscenario(GameManager.Instancia.EscenarioActual + 1);
        GameManager.Instancia.IrASeleccion();
    }

    public void OnBotonRepetir() { GameManager.Instancia.IrASeleccion(); }
    public void OnBotonMenu() { GameManager.Instancia.IrAMenuPrincipal(); }
}
"@
[System.IO.File]::WriteAllText($ruta, $codigo, [System.Text.Encoding]::UTF8)
Write-Host "OK: $((Get-Item $ruta).Length) bytes"
