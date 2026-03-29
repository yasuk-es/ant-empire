using UnityEngine;

// Controla los botones del menú principal
// Escena: MenuPrincipal
public class MenuPrincipalUI : MonoBehaviour
{
    // Llamado cuando el jugador presiona el botón "Jugar"
    public void OnBotonJugar()
    {
        // Va al mapa para elegir escenario
        GameManager.Instancia.IrAMapa();
    }

    // Llamado cuando el jugador presiona el botón "Salir"
    public void OnBotonSalir()
    {
        // Cierra la aplicación (en el editor solo detiene el play)
        Application.Quit();
    }
}
