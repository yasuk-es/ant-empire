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
        // En el editor detiene el Play, en el dispositivo cierra la app
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
