using UnityEngine;
using TMPro;

public class ReceberCor : MonoBehaviour
{
    public TextMeshProUGUI texto;

    void Start()
    {
        Debug.Log("Scene 2 iniciou");

        var gc = FindFirstObjectByType<GerenciarComunicacao>();

        if (gc != null)
        {
            Debug.Log("✅ GerenciarComunicacao encontrado");
            gc.RegistraRecebedor(OnDadosRecebidos);
        }
        else
        {
            Debug.LogError("❌ GerenciarComunicacao NÃO encontrado");
        }
    }

    void OnDadosRecebidos(string[] dados)
    {
        Debug.Log("📨 Dados chegaram");

        if (dados.Length > 0)
        {
            string cor = dados[0];

            Debug.Log("🎨 Cor recebida: " + cor);

            texto.text = cor;
        }
    }
}