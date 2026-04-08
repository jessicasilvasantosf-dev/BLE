using System;
using TMPro;
using UnityEngine;

public class ControlesCenaDois : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI recebidos;

    Action<String> Enviador;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject gm = GameObject.Find("Comunicacao");
        GerenciarComunicacao gc = gm.GetComponent<GerenciarComunicacao>();
        gc.RegistraRecebedor(Receber);
        Enviador = gc.Enviar;//Os dados são enviados pelo script que está na primeira cena
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Receber(string[] dados)
    {
        recebidos.text = dados[0];
    }
    
    public void Enviar(string dados)
    {
        Enviador(dados);
    }
}
