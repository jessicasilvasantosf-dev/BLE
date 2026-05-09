using UnityEngine;
using TMPro;

public class ReceberCor : MonoBehaviour
{
    public TextMeshProUGUI texto;

    void Start()
    {
        texto.text = "Scene 2 iniciou";

        var gc = FindFirstObjectByType<GerenciarComunicacao>();

        if (gc != null)
        {
            texto.text = "GC encontrado";

            gc.RegistraRecebedor(OnDadosRecebidos);
        }
        else
        {
            texto.text = "GC NÃO encontrado";
        }
    }

    void OnDadosRecebidos(string[] dados)
    {
        if (dados.Length > 0)
        {
            string cor = dados[0];

            texto.text = cor;
        }
    }
}