using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveIP : MonoBehaviour
{
    [SerializeField] private string currentIP = "192.168.1.2";

    public string Ip
    {
        get
        {
            return currentIP;
        }
        set
        {
            currentIP = value;
        }
    }

    public void Save() => PlayerPrefs.SetString("Controller IP", currentIP);
}
