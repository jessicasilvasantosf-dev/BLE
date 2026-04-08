# Unity Android BLE - PUC-SP

<p align="center">
  <b>Projeto educacional de Bluetooth Low Energy para Unity Android</b><br>
    <i>Adaptado para os alunos da Pontifícia Universidade Católica de São Paulo (PUC-SP)</i>
</p>

<p align="center">
    <img src="https://i.imgur.com/fL3ybma.png" style="width:40%;">
</p>

## ?? Sobre o Projeto

Este projeto é uma **versão adaptada e comentada** do [Unity Android Bluetooth Low Energy](https://github.com/Velorexe/Unity-Android-Bluetooth-Low-Energy), modificado especificamente para fins didáticos no curso da PUC-SP.

### ?? Objetivos de Aprendizado

- Compreender comunicação **Bluetooth Low Energy (BLE)** em dispositivos Android
- Integrar **sensores externos** (ex: ESP32, Arduino) com Unity
- Desenvolver aplicações **VR/AR** que interagem com hardware físico
- Aplicar padrões de projeto (Command Pattern, Observer Pattern, Singleton)
- Gerenciar permissões Android em tempo de execução

---

## ? Funcionalidades

### Operações BLE Suportadas

? **Descobrir dispositivos** - Scan de dispositivos BLE próximos  
? **Conectar/Desconectar** - Gerenciamento de conexão com dispositivo  
? **Escrever dados** - Envio de comandos para o dispositivo (ex: acender LED)  
? **Ler dados** - Leitura de valores de características  
? **Subscrever notificações** - Receber dados em tempo real (ex: sensores)  

### Recursos Adicionais (Modificações PUC-SP)

?? **Documentação completa em português** com XMLDoc  
?? **Scripts de exemplo comentados** para aprendizado  
??? **Gerenciamento de permissões** Android 12+  
?? **Sistema de comunicação bidirecional** simplificado  
?? **Integração com New Input System** do Unity  
?? **Interface de usuário intuitiva** para testes  

---

## ??? Arquitetura do Projeto

### Como Funciona

```
???????????????????????????????????????????????????????????????
?         Unity (C#)         ?
?  ??????????????????  ????????????????  ??????????????????  ?
?  ?   BleManager   ????  BleAdapter  ????  BleCommand    ?  ?
?  ?   (Singleton)  ?  ?  (Observer)  ?  ?  (Commands)    ?  ?
?  ??????????????????  ????????????????  ??????????????????  ?
?       ?      ?            ?
?        ????????????????????    ?
?          ?      ?
?  ????????????????????????              ?
?        ?  Android JNI Bridge  ?           ?
?????????????????????????           ?
???????????????????????????????????????????????????????????????
             ?
???????????????????????????????????????????????????????????????
?            Android BLE Stack            ?
?  ?????????????????????????????????????????????????????????? ?
?  ?  BluetoothAdapter ? BluetoothGatt ? Characteristics   ? ?
?  ?????????????????????????????????????????????????????????? ?
???????????????????????????????????????????????????????????????
        ?
              ?
              ??????????????????
    ?  Dispositivo   ?
      ?  BLE Externo   ?
     ? (ESP32/Arduino)?
 ??????????????????
```

### Componentes Principais

#### 1. **BleManager** (`Assets/Scripts/BLE/BleManager.cs`)
- Singleton que gerencia todas as operações BLE
- Fila de comandos para execução sequencial
- Interface entre Unity e plugin Android

#### 2. **BleAdapter** (`Assets/Scripts/BLE/BleAdapter.cs`)
- Recebe callbacks do plugin Android via `SendMessage`
- Converte dados JSON em eventos .NET
- Distribui eventos para os comandos apropriados

#### 3. **BleCommand** (`Assets/Scripts/BLE/Commands/Base/BleCommand.cs`)
- Classe abstrata para todas as operações BLE
- Implementa padrão Command com timeout
- Subclasses: `DiscoverDevices`, `ConnectToDevice`, `SubscribeToCharacteristic`, etc.

#### 4. **ExampleBleInteractor** (`Assets/Example/Scripts/ExampleBleInteractor.cs`) ? NOVO
- Script de exemplo completo e comentado
- Demonstra fluxo completo: Scan ? Connect ? Subscribe
- Interface visual para testes

#### 5. **GerenciarComunicacao** (`Assets/Example/Scripts/GerenciarComunicacao.cs`) ? NOVO
- Gerencia comunicação bidirecional com dispositivo
- Sistema de callbacks para receber dados
- Envia comandos com fragmentação automática

---

## ?? Como Usar

### 1?? Pré-requisitos

- **Unity 2020.3+** (testado com 2021.3 LTS)
- **Android SDK** com API Level 21+ (Android 5.0)
- **Dispositivo Android físico** (BLE não funciona no emulador)
- **Dispositivo BLE** (ESP32, Arduino Nano 33 BLE, etc.)

### 2?? Configuração Inicial

1. Clone o repositório:
   ```bash
   git clone https://github.com/masterrey/BLE.git
   cd BLE
   ```

2. Abra o projeto no Unity

3. Configure as permissões Android:
   - O projeto já está configurado com as permissões necessárias
   - Verifique `Assets/Plugins/Android/AndroidManifest.xml`

4. Configure seu dispositivo BLE:
   ```csharp
   // No ExampleBleInteractor, digite o nome do seu dispositivo
   private string nomeBlueTooth = "ESP32_BLE"; // Mude para o nome do seu dispositivo
   ```

### 3?? Testando a Conexão

1. **Prepare seu dispositivo BLE**
   - Programe um ESP32/Arduino com serviço BLE
   - Use UUIDs padrão: Serviço `ffe0`, Característica `ffe1`

2. **Build para Android**
   - File ? Build Settings ? Android
   - Build and Run

3. **Execute o aplicativo**
   - Permita acesso Bluetooth quando solicitado
   - Digite o nome do dispositivo BLE
   - Clique em "Scan"
   - Aguarde a conexão automática

### 4?? Enviando e Recebendo Dados

#### Enviar comando para o dispositivo:
```csharp
GerenciarComunicacao gc = FindObjectOfType<GerenciarComunicacao>();
gc.Enviar("LED:ON"); // Liga LED no ESP32
```

#### Receber dados do dispositivo:
```csharp
void Start()
{
    GerenciarComunicacao gc = FindObjectOfType<GerenciarComunicacao>();
    gc.RegistraRecebedor(ProcessarDados);
}

void ProcessarDados(string[] dados)
{
    // ESP32 enviou: "25.5;60.2\n" (temperatura;umidade)
    float temperatura = float.Parse(dados[0]); // 25.5
    float umidade = float.Parse(dados[1]);     // 60.2
    
  Debug.Log($"Temperatura: {temperatura}°C");
  Debug.Log($"Umidade: {umidade}%");
}
```

---

## ?? Estrutura de Pastas

```
BLE/
??? Assets/
?   ??? Example/       # ?? Exemplos para alunos
?   ?   ??? Scenes/           # Cenas de demonstração
?   ?   ??? Scripts/        # Scripts de exemplo comentados
?   ?       ??? ExampleBleInteractor.cs      # ? Exemplo completo
?   ?       ??? GerenciarComunicacao.cs      # ? Gerenciador de dados
?   ?       ??? DeviceButton.cs              # Botão de dispositivo
?   ?       ??? NaoDestruirNoCarregamento.cs # Persistência entre cenas
?   ?
?   ??? Scripts/
?   ?   ??? BLE/              # Sistema BLE principal
?   ?   ?   ??? BleManager.cs
?   ?   ?   ??? BleAdapter.cs
?   ?   ?   ??? Commands/     # Comandos BLE
?   ?   ?   ?   ??? Base/
?   ?   ?   ?   ?   ??? BleCommand.cs
?   ?   ?   ? ??? ConnectToDevice.cs
?   ?   ?   ?   ??? DiscoverDevices.cs
?   ?   ?   ?   ??? ReadFromCharacteristic.cs
?   ?   ?   ???? SubscribeToCharacteristic.cs
?   ?   ?   ?   ??? WriteToCharacteristic.cs
?   ?   ?   ??? Extension/
?   ?   ?       ??? UuidHelper.cs
?   ?   ?
?   ?   ??? ExemploNovoInputSystem.cs        # ?? Exemplo Input System
?   ?   ??? InicialVerificaPermissoes/       # ? Sistema de permissões
?   ?
?   ??? Plugins/
?       ??? Android/ # Plugin nativo Android
?           ??? AndroidManifest.xml
?           ??? AndroidBlePlugin.aar
?
??? README.md    # ?? Este arquivo
```

---

## ?? Exemplos de Código para Alunos

### Exemplo 1: Conectar e Ler Sensor de Temperatura

```csharp
using Android.BLE;
using Android.BLE.Commands;
using UnityEngine;

public class SensorTemperatura : MonoBehaviour
{
    private GerenciarComunicacao comunicacao;
    
    void Start()
    {
  // Encontra o gerenciador de comunicação
        comunicacao = FindObjectOfType<GerenciarComunicacao>();
        
        // Registra callback para receber dados
        comunicacao.RegistraRecebedor(ProcessarTemperatura);
    }
    
    void ProcessarTemperatura(string[] dados)
    {
        // ESP32 envia: "TEMP:25.5\n"
   if (dados[0].StartsWith("TEMP:"))
        {
            string valorStr = dados[0].Replace("TEMP:", "");
         float temperatura = float.Parse(valorStr);
       
      Debug.Log($"??? Temperatura: {temperatura}°C");
          
    // Atualizar UI, mudar cor do ambiente, etc.
 AtualizarVisualizacao(temperatura);
    }
    }
    
    void AtualizarVisualizacao(float temp)
    {
        // Exemplo: muda cor do ambiente baseado na temperatura
        if (temp > 30f)
      RenderSettings.fogColor = Color.red;    // Quente
        else if (temp < 15f)
   RenderSettings.fogColor = Color.blue;   // Frio
else
            RenderSettings.fogColor = Color.white;  // Normal
    }
}
```

### Exemplo 2: Controlar LED Remoto

```csharp
using UnityEngine;

public class ControladorLED : MonoBehaviour
{
    private GerenciarComunicacao comunicacao;
    
    void Start()
    {
        comunicacao = FindObjectOfType<GerenciarComunicacao>();
    }
    
    // Chame este método de um botão UI
    public void LigarLED()
    {
comunicacao.Enviar("LED:ON");
        Debug.Log("?? LED ligado!");
    }
    
    public void DesligarLED()
    {
        comunicacao.Enviar("LED:OFF");
 Debug.Log("?? LED desligado!");
    }
    
    public void AjustarBrilho(int intensidade) // 0-255
    {
        comunicacao.Enviar($"LED:PWM:{intensidade}");
   Debug.Log($"?? Brilho ajustado para {intensidade}");
    }
}
```

### Exemplo 3: Sistema de Batimentos Cardíacos (VR Saúde)

```csharp
using UnityEngine;
using UnityEngine.UI;

public class MonitorCardíaco : MonoBehaviour
{
    [SerializeField] private Text textoFrequencia;
    [SerializeField] private Image coracao; // Imagem que pulsa
    
    private GerenciarComunicacao comunicacao;
    private float frequenciaAtual = 0f;
    
    void Start()
  {
        comunicacao = FindObjectOfType<GerenciarComunicacao>();
        comunicacao.RegistraRecebedor(ProcessarBatimento);
    }
    
    void ProcessarBatimento(string[] dados)
    {
        // Sensor envia: "HR:72\n" (Heart Rate: 72 bpm)
        if (dados[0].StartsWith("HR:"))
        {
  string valorStr = dados[0].Replace("HR:", "");
            frequenciaAtual = float.Parse(valorStr);
            
          AtualizarUI();
      AnimarCoracao();
   }
    }
    
  void AtualizarUI()
    {
        textoFrequencia.text = $"?? {frequenciaAtual} BPM";
    
        // Alerta se frequência anormal
        if (frequenciaAtual > 120f || frequenciaAtual < 50f)
        {
   textoFrequencia.color = Color.red;
    Debug.LogWarning($"?? Frequência cardíaca anormal: {frequenciaAtual} BPM");
        }
        else
 {
      textoFrequencia.color = Color.green;
        }
    }

    void AnimarCoracao()
    {
        // Faz o coração pulsar no ritmo dos batimentos
        float intervalo = 60f / frequenciaAtual; // Tempo entre batimentos
        LeanTween.scale(coracao.gameObject, Vector3.one * 1.2f, intervalo * 0.3f)
.setEaseInOutSine()
      .setOnComplete(() => {
          LeanTween.scale(coracao.gameObject, Vector3.one, intervalo * 0.7f);
        });
    }
}
```

---

## ?? Código ESP32 de Exemplo

```cpp
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

// UUIDs (devem coincidir com o Unity)
#define SERVICE_UUID     "0000ffe0-0000-1000-8000-00805f9b34fb"
#define CHARACTERISTIC_UUID "0000ffe1-0000-1000-8000-00805f9b34fb"

BLECharacteristic *pCharacteristic;
bool deviceConnected = false;

// Callback de conexão
class ServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
 deviceConnected = true;
      Serial.println("? Cliente Unity conectado!");
    }
    
    void onDisconnect(BLEServer* pServer) {
    deviceConnected = false;
        Serial.println("? Cliente Unity desconectado!");
  BLEDevice::startAdvertising(); // Reinicia advertising
    }
};

// Callback para receber dados do Unity
class CharacteristicCallbacks: public BLECharacteristicCallbacks {
    void onWrite(BLECharacteristic *pChar) {
        std::string value = pChar->getValue();
        
        if (value.length() > 0) {
            Serial.print("?? Recebido do Unity: ");
   Serial.println(value.c_str());
   
        // Processa comandos
    if (value == "LED:ON\n") {
   digitalWrite(LED_BUILTIN, HIGH);
       Serial.println("?? LED ligado");
            }
   else if (value == "LED:OFF\n") {
     digitalWrite(LED_BUILTIN, LOW);
             Serial.println("?? LED desligado");
    }
        }
    }
};

void setup() {
    Serial.begin(115200);
    pinMode(LED_BUILTIN, OUTPUT);
    
    // Inicializa BLE
    BLEDevice::init("ESP32_BLE"); // Nome que aparece no Unity
    
    // Cria servidor BLE
  BLEServer *pServer = BLEDevice::createServer();
 pServer->setCallbacks(new ServerCallbacks());
    
    // Cria serviço
    BLEService *pService = pServer->createService(SERVICE_UUID);
    
    // Cria característica (READ, WRITE, NOTIFY)
    pCharacteristic = pService->createCharacteristic(
     CHARACTERISTIC_UUID,
        BLECharacteristic::PROPERTY_READ |
  BLECharacteristic::PROPERTY_WRITE |
        BLECharacteristic::PROPERTY_NOTIFY
    );
    
    pCharacteristic->setCallbacks(new CharacteristicCallbacks());
    pCharacteristic->addDescriptor(new BLE2902());
    
    // Inicia serviço e advertising
    pService->start();
    
    BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
    pAdvertising->addServiceUUID(SERVICE_UUID);
    pAdvertising->start();
    
    Serial.println("?? BLE Server iniciado! Aguardando conexão Unity...");
}

void loop() {
    // Envia temperatura a cada 2 segundos
    if (deviceConnected) {
        float temperatura = random(20, 30) + random(0, 100) / 100.0;
 
  String dados = String(temperatura) + ";100.0\n"; // temp;umidade
    pCharacteristic->setValue(dados.c_str());
pCharacteristic->notify();
   
        Serial.print("?? Enviado para Unity: ");
        Serial.println(dados);
        
        delay(2000);
    }
}
```

---

## ?? Problemas Comuns e Soluções

### 1. "Dispositivo não encontrado"
- ? Verifique se o Bluetooth está ligado no celular
- ? Certifique-se de que o nome do dispositivo está **exatamente igual** (case-sensitive)
- ? Aproxime o celular do dispositivo BLE (máximo 10 metros)
- ? Reinicie o ESP32/Arduino

### 2. "Permissões negadas"
- ? Vá em Configurações ? Apps ? [Seu App] ? Permissões
- ? Ative "Localização" e "Dispositivos Próximos"
- ? No Android 12+, a permissão de localização é obrigatória para BLE

### 3. "Conecta mas não recebe dados"
- ? Verifique se o UUIDs do serviço/característica estão corretos
- ? Confirme que o ESP32 está enviando dados com `notify()`
- ? Verifique se registrou o callback com `RegistraRecebedor()`

### 4. "Dados recebidos cortados ou estranhos"
- ? BLE limita a 20 bytes por transmissão
- ? Use delimitadores claros (`;`, `\n`)
- ? Adicione `\n` no final de cada mensagem
- ? Use `EnviarDadosGrandes()` para mensagens longas

### 5. "Aplicativo fecha ao conectar"
- ? Verifique logs no Logcat (Android Studio)
- ? Certifique-se de estar rodando em dispositivo físico (não emulador)
- ? Verifique se o plugin `.aar` está na pasta correta

---

## ?? Documentação Adicional

### Artigos Recomendados
- [Creating an Android BLE plugin for Unity](https://velorexe.com/posts/unity-bluetooth-low-energy/) - Artigo original do criador
- [Bluetooth Low Energy - Documentação Android](https://developer.android.com/guide/topics/connectivity/bluetooth-le)
- [ESP32 BLE Arduino Tutorial](https://randomnerdtutorials.com/esp32-bluetooth-low-energy-ble-arduino-ide/)

### Vídeos Úteis
- [Unity Android BLE Tutorial](https://www.youtube.com/results?search_query=unity+android+ble)
- [ESP32 BLE Basics](https://www.youtube.com/results?search_query=esp32+ble+tutorial)

---

## ????? Para Professores

### Sugestões de Exercícios

1. **Básico**: Conectar ao ESP32 e fazer um LED piscar via Unity
2. **Intermediário**: Criar interface que mostra temperatura em tempo real
3. **Avançado**: Desenvolver aplicação VR que responde a sensor de batimentos cardíacos
4. **Projeto Final**: Sistema completo de reabilitação com múltiplos sensores

### Avaliação Sugerida
- Conexão bem-sucedida (20%)
- Envio/recebimento de dados (30%)
- Interface de usuário (20%)
- Documentação do código (15%)
- Criatividade na aplicação (15%)

---

## ?? Contribuindo

Alunos e professores são encorajados a contribuir com melhorias!

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

---

## ?? Créditos

### Projeto Original
- **Unity Android Bluetooth Low Energy** por [Velorexe](https://github.com/Velorexe)
- Repositório original: https://github.com/Velorexe/Unity-Android-Bluetooth-Low-Energy

### Adaptação PUC-SP
- Documentação em português
- Scripts de exemplo educacionais
- Sistema de permissões Android 12+
- Exemplos de integração com sensores

---

## ?? Contato

### Dúvidas sobre o Projeto
- Crie uma **Issue** neste repositório
- Email: degenerexe.code@gmail.com

### Projeto Original
- Discord: Velorexe#8403
- Website: [velorexe.com](https://velorexe.com)

---

## ?? Licença

Este projeto mantém a licença do projeto original. Livre para uso educacional.

---

<p align="center">
    <b>Desenvolvido para os alunos da PUC-SP ??</b><br>
 <i>Bons estudos e mãos à obra! ??</i>
</p>
