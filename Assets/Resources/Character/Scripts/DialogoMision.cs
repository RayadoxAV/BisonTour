using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoMision : MonoBehaviour
{
    GameObject player = null;
    GameObject secretary = null;
    GameObject pago = null;
    public float distanciaUmbral = 2.0f;
    public string mensaje = "Bienvendo al tec, pase a caja para pagar su constancia";
    public string dinero = "50$";
    public GUIStyle estiloMensaje;
    public bool mostrarMensaje = false;
    public bool mensajeExito = false;
    private Transform posicionPago;
    private Transform posicionJugador;

    void Start()
    {
        player = GameObject.Find("Player");
        secretary = GameObject.Find("Secretary");
        pago = GameObject.Find("pago");
        posicionPago = pago.GetComponent<Transform>();
        posicionJugador = player.GetComponent<Transform>();
    }

    void Update()
    {
        // Verificar si los objetos se encontraron correctamente
        if (player != null && secretary != null)
        {
            // Acceder a la posición de los objetos
            Vector3 posicionPlayer = player.transform.position;
            Vector3 posicionSecretary = secretary.transform.position;

            // Calcular la distancia entre los objetos
            float distancia = Vector3.Distance(posicionPlayer, posicionSecretary);

            // Comprobar si la distancia está por debajo del umbral
            if (distancia < distanciaUmbral)
            {
                // Los objetos están cerca
                mostrarMensaje = true;
            }
            else
            {
                mostrarMensaje = false;
            }

            Vector3 coordenadasPago = posicionPago.position;
            Vector3 coordenadasPlayer = posicionJugador.position;

            float distanciaPago = Vector3.Distance(coordenadasPago, coordenadasPlayer);
            float distanciaUmbralPago = 1.0f; // Define el umbral de distancia deseado

            if (distanciaPago < distanciaUmbralPago)
            {
                dinero = "0$";
                mensajeExito = true;
                mensaje = "Su constancia ha sido pagada";
            }
            
        }
    }

    private void OnGUI()
    {
        Vector2 centroPantalla = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 labelSize = estiloMensaje.CalcSize(new GUIContent(mensaje));
        Vector2 labelPosition = centroPantalla - (labelSize / 2);

        if (mostrarMensaje)
        {
            
           GUI.Label(new Rect(labelPosition.x, labelPosition.y, labelSize.x, labelSize.y), mensaje, estiloMensaje);
            
           
        }

        GUI.Label(new Rect(10, 10, 200, 20), dinero, estiloMensaje);
    }

    
}