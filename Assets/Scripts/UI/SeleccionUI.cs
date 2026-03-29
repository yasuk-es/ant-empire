using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controla la pantalla de selección de hormigas
// El jugador debe elegir exactamente 1 Tanque, 1 Luchador y 1 Soporte
// Escena: Seleccion
public class SeleccionUI : MonoBehaviour
{
    // Hormigas actualmente seleccionadas (máximo 3, una por rol)
    private List<Hormiga> seleccionadas = new List<Hormiga>();

    // Botón para confirmar la selección e ir al combate
    [SerializeField] private Button botonConfirmar;

    // Texto que muestra cuántas hormigas lleva seleccionadas
    [SerializeField] private TextMeshProUGUI textoSeleccion;

    void Start()
    {
        // Al inicio el botón de confirmar está desactivado
        botonConfirmar.interactable = false;
        ActualizarTexto();
    }

    // Llamado cuando el jugador toca una tarjeta de hormiga
    // Recibe el índice de la hormiga en el roster del GameManager
    public void OnSeleccionarHormiga(int indice)
    {
        Hormiga hormiga = GameManager.Instancia.RosterCompleto[indice];

        // Verifica si ya hay una hormiga del mismo rol seleccionada
        bool yaHayDeEseRol = seleccionadas.Exists(h => h.Rol == hormiga.Rol);

        if (yaHayDeEseRol)
        {
            // Reemplaza la hormiga del mismo rol
            seleccionadas.RemoveAll(h => h.Rol == hormiga.Rol);
        }

        // Agrega la hormiga seleccionada
        seleccionadas.Add(hormiga);

        // Actualiza el texto y el botón
        ActualizarTexto();

        // Activa el botón solo si hay exactamente 3 hormigas (una de cada rol)
        botonConfirmar.interactable = seleccionadas.Count == 3;
    }

    // Actualiza el texto que muestra la selección actual
    void ActualizarTexto()
    {
        textoSeleccion.text = $"Seleccionadas: {seleccionadas.Count}/3";
    }

    // Llamado cuando el jugador presiona "Confirmar"
    public void OnBotonConfirmar()
    {
        // Guarda la selección en el GameManager y va al combate
        GameManager.Instancia.GuardarSeleccion(seleccionadas);
        GameManager.Instancia.IrACombate();
    }

    // Llamado cuando el jugador presiona "Volver"
    public void OnBotonVolver()
    {
        GameManager.Instancia.IrAMapa();
    }
}
