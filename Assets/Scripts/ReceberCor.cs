using UnityEngine;
using TMPro;

public class ReceberCor : MonoBehaviour
{
    public TextMeshProUGUI texto;

    public void AtualizarCor(string cor)
    {
        Debug.Log("Cor recebida: " + cor);
        texto.text = cor;
    }
}