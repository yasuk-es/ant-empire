using UnityEngine;

// Controla los botones del mapa de escenarios
// Escena: Mapa
// El jugador elige uno de los 3 escenarios disponibles
public class MapaUI : MonoBehaviour
{
    // Llamado cuando el jugador presiona el botón del Escenario 1 (Slimes)
    public void OnBotonEscenario1()
    {
        // Guarda el escenario elegido y va a selección de hormigas
        GameManager.Instancia.GuardarEscenario(1);
        GameManager.Instancia.IrASeleccion();
    }

    // Llamado cuando el jugador presiona el botón del Escenario 2 (Larvas)
    public void OnBotonEscenario2()
    {
        GameManager.Instancia.GuardarEscenario(2);
        GameManager.Instancia.IrASeleccion();
    }

    // Llamado cuando el jugador presiona el botón del Escenario 3 (Arañas)
    public void OnBotonEscenario3()
    {
        GameManager.Instancia.GuardarEscenario(3);
        GameManager.Instancia.IrASeleccion();
    }

    // Llamado cuando el jugador presiona el botón "Volver al Menú"
    public void OnBotonVolver()
    {
        GameManager.Instancia.IrAMenuPrincipal();
    }
}
