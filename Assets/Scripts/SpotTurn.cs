using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class SpotTurn : MonoBehaviour
{
    [SerializeField] private string ip = "192.168.88.1";
    // Start is called before the first frame update
    void TurnOffTheSpot()
    {
        var Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket.ReceiveTimeout = 1000;

        IPAddress Ip = IPAddress.Parse(ip);
        //Ep = new IPEndPoint(Ip, PORT);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
