using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Controla la pantalla de seleccion de hormigas
// El jugador elige hasta 3 hormigas libremente (sin restriccion de rol)
// Si clickea una ya seleccionada, se deselecciona
public class SeleccionUI : MonoBehaviour
{
    // Hormigas actualmente seleccionadas (maximo 3)
    private List<Hormiga> seleccionadas = new List<Hormiga>();

    // Boton para confirmar la seleccion
    [SerializeField] private Button botonConfirmar;

    // Texto que muestra cuantas hormigas lleva seleccionadas
    [SerializeField] private TextMeshProUGUI textoSeleccion;

    // Botones de cada hormiga para cambiar su color al seleccionar
    [SerializeField] private Button[] botonesHormigas;

    void Start()
    {
        botonConfirmar.interactable = false;
        ActualizarTexto();
    }

    // Llamado cuando el jugador toca una hormiga
    public void OnSeleccionarHormiga(int indice)
    {
        Hormiga hormiga = GameManager.Instancia.RosterCompleto[indice];

        // Si ya estaba seleccionada, la deselecciona
        if (seleccionadas.Contains(hormiga))
        {
            seleccionadas.Remove(hormiga);
            ActualizarColorBoton(indice, false);
        }
        else
        {
            // Solo agrega si hay menos de 3 seleccionadas
            if (seleccionadas.Count < 3)
            {
                seleccionadas.Add(hormiga);
                ActualizarColorBoton(indice, true);
            }
        }

        ActualizarTexto();
        // Activa confirmar cuando hay exactamente 3
        botonConfirmar.interactable = seleccionadas.Count == 3;
    }

    // Cambia el color del boton para indicar si esta seleccionado o no
    void ActualizarColorBoton(int indice, bool seleccionado)
    {
        if (botonesHormigas == null || indice >= botonesHormigas.Length) return;
        var imagen = botonesHormigas[indice].GetComponent<Image>();
        if (imagen == null) return;
        // Seleccionado: amarillo | Deseleccionado: blanco
        imagen.color = seleccionado ? new Color(1f, 0.9f, 0.3f) : Color.white;
    }

    // Actualiza el texto de seleccion
    void ActualizarTexto()
    {
        textoSeleccion.text = "Seleccionadas: " + seleccionadas.Count + "/3";
    }

    // Confirmar: guarda seleccion y va al combate
    public void OnBotonConfirmar()
    {
        GameManager.Instancia.GuardarSeleccion(seleccionadas);
        GameManager.Instancia.IrACombate();
    }

    // Volver al mapa
    public void OnBotonVolver()
    {
        GameManager.Instancia.IrAMapa();
    }
}
