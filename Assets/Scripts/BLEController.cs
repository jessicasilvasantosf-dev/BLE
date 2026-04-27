using UnityEngine;
using Android.BLE;
using Android.BLE.Commands;
using System.Text;

public class BLEController : MonoBehaviour
{
    public ReceberCor receiver;

    string deviceName = "cromia";
    string deviceAddress;

    string serviceUUID = "1234";
    string characteristicUUID = "5678";

    void Start()
    {
        BleManager.Instance.Initialize();

        DiscoverDevices scan = new DiscoverDevices(OnDeviceFound, 10000);
        BleManager.Instance.QueueCommand(scan);
    }

    void OnDeviceFound(string name, string address)
    {
        Debug.Log("Encontrou: " + name);

        if (name == deviceName)
        {
            deviceAddress = address;

            ConnectToDevice connect = new ConnectToDevice(address, OnConnected);
            BleManager.Instance.QueueCommand(connect);
        }
    }

    void OnConnected(string address)
    {
        Debug.Log("Conectado: " + address);

        // ✅ CALLBACK CORRETO (byte[])
        SubscribeToCharacteristic subscribe = new SubscribeToCharacteristic(
            address,
            serviceUUID,
            characteristicUUID,
            (byte[] data) =>
            {
                string texto = Encoding.UTF8.GetString(data);

                Debug.Log("Recebido: " + texto);

                if (receiver != null)
                    receiver.AtualizarCor(texto);
            }
        );

        BleManager.Instance.QueueCommand(subscribe);
    }
}