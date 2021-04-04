using MagicHome;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TcpServer : MonoBehaviour
{
    #region инициализаци€
    [SerializeField] private int port = 9998;
    [SerializeField] private int tick = 0;
    [SerializeField] private TcpListener server;
    [SerializeField] private bool isFirstInit = false;
    [SerializeField] private string serverIp = "";

    //светодиодна€ лента
    [SerializeField] string ledAddress = "192.168.88.160";
    [SerializeField] private LEDLight light;
    public LEDLight Light
    {
        get
        {
            return light;
        }
    }

    //private WebSocket server;

    public void Start()
    {
        server = new TcpListener(IPAddress.Any, port);
        server.Start();

        //StartCoroutine("ServerTick");

        Debug.Log("—ервер запущен. ќжидание..");

        //server = new WebSocket()

        ConnectToLed();

        if (isFirstInit)
        {
            
            //работа со спотами
            //SetSpotServer(serverIp, port);
        }



    }

    #endregion

    #region работа сервера
    public TcpClient AcceptClients()
    {
        Debug.Log("Searching requests...");
        return server.AcceptTcpClient();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tick++;
        if (tick > 60)
        {
            tick = 0;
            //TcpClient client = AcceptClients();
            //NetworkStream stream = client.GetStream();

        }
    }

    private IEnumerator ServerTick()
    {
        TcpClient client = AcceptClients();
        if (client.Connected)
        {
            NetworkStream stream = client.GetStream();
        }
        yield return new WaitForSeconds(1);
    }

    private async void SendPostRequest(string url, Dictionary<string, string> values)
    {
        var data = new FormUrlEncodedContent(values);
        using var client = new HttpClient();
        var response = await client.PostAsync(url, data);
        Debug.Log(response.Content.ReadAsStringAsync().Result);
    }
    #region работа со спотами
    private void SetSpotServer(string ip, int port)
    {
        Dictionary<string, string> values = new Dictionary<string, string>
        {
            {"version", "4"},
            {"ssid", "SIDORCOOL"},
            {"password", "twgok4ever"},
            {"serverName", ip},
            {"port", $"{port}"},
        };
        SendPostRequest("http://10.10.7.1/ap", values);
    }
    #endregion

    #region работа со светодиодной лентой
    public async Task<UnityEngine.Color> ConnectToLed()
    {
        light = new LEDLight(ledAddress);

        //Debug.Log($"ѕоиск светодиодных лент..");
        //List<LEDLight> lights = await LEDLight.DiscoverAsync();

        //if (lights.Count != 0)
        //{
        //    Debug.Log($"ќбнаружено {lights.Count} лент");
        //    light = lights[0];


        //}
        //else
        //{
        //    Debug.Log($"Ћенты не найдены");
        //}

        Debug.Log($"ѕопытка соединени€");
        await light.ConnectAsync();
        if (light.Connected)
        {
            Debug.Log($"—оединение со светодиодной лентой {light.ToString()} прошло успешно");
            return new UnityEngine.Color(light.Color.Red / 255, light.Color.Red / 255, light.Color.Green / 255);
        }
        else
        {
            Debug.Log($"Ќе удалось подключитьс€ к светодиодной ленте по адресу {ledAddress}!");
        }
        // Connect.
        return new UnityEngine.Color(0, 0, 0);

    }
    #endregion

    #endregion
}
