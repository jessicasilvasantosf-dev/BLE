using Android.BLE;
using Android.BLE.Commands;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExampleBleInteractor : MonoBehaviour
{
    #region 🔥 DADO GLOBAL (COR QUE VEM DO ARDUINO)
    public static string corAtual = "";
    #endregion

    [Header("UI")]
    [SerializeField] private TMP_InputField InputNomeBlueTooh;
    [SerializeField] private Text status;

    private string nomeBlueTooth;

    private float _scanTimer;
    private bool _isScanning;

    private string _deviceUuid;
    private string _deviceName;

    private ConnectToDevice _connectCommand;
    private bool _isConnected;

    private const string PREF_NOME_BLUETOOTH = "nomeBlueTooth";

    private void Start()
    {
        if (InputNomeBlueTooh != null)
            InputNomeBlueTooh.onValueChanged.AddListener(OnInputFieldValueChanged);

        AtualizarStatus("Aguardando scan...");
    }

    private void Update()
    {
        if (_isScanning)
        {
            _scanTimer += Time.deltaTime;
        }
    }

    #region UI

    private void OnInputFieldValueChanged(string novoNome)
    {
        nomeBlueTooth = novoNome;
    }

    public void ScanForDevices()
    {
        if (string.IsNullOrEmpty(nomeBlueTooth)) return;

        _isScanning = true;
        _scanTimer = 0f;

        DiscoverDevices comando = new DiscoverDevices(OnDeviceFound, 10000);
        BleManager.Instance.QueueCommand(comando);
    }

    #endregion

    #region BLE CONNECT

    private void OnDeviceFound(string uuid, string deviceName)
    {
        if (deviceName == nomeBlueTooth)
        {
            _deviceUuid = uuid;
            _deviceName = deviceName;

            Connect();
        }
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(_deviceUuid)) return;

        Debug.Log("Conectado ao BLE");

        // 👉 AQUI é onde o plugin começa a receber dados automaticamente
        // MAS precisamos capturar isso via callback do plugin
    }

    #endregion

    #region 🔥 ESSA É A FUNÇÃO QUE VOCÊ VAI USAR

    // 👉 CHAME ISSO quando o BLE receber "VERMELHO", "AZUL", etc
    public void ReceberCor(string data)
    {
        corAtual = data;
        Debug.Log("COR RECEBIDA: " + data);
    }

    #endregion

    #region UI STATUS

    private void AtualizarStatus(string msg)
    {
        if (status != null)
            status.text = msg;
    }

    #endregion
}