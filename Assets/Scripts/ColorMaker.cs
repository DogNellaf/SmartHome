using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using MagicHome;
public class ColorMaker : MonoBehaviour
{
    [SerializeField] float red = 1;
    [SerializeField] float green = 1;
    [SerializeField] float blue = 1;
    [SerializeField] float brightness = 1;
    [SerializeField] Image currentImage;
    [SerializeField] Sprite turnOn;
    [SerializeField] Sprite turnOff;

    [SerializeField] private TcpServer server;

    public float Red
    {
        set
        {
            red = value;
            ReloadImageColor();
        }
    }

    public float Brightness
    {
        set
        {
            brightness = value;
            ReloadImageColor();
        }
    }

    public float Green
    {
        set
        {
            green = value;
            ReloadImageColor();
        }
    }

    public float Blue
    {
        set
        {
            blue = value;
            ReloadImageColor();
        }
    }

    private async void ReloadImageColor()
    {
        currentImage.color = new UnityEngine.Color(red, green, blue, brightness);
        await SendColor();
    }

    public async void TurnAround()
    {
        if (server.Light.Power)
        {
            ChangeSprite(turnOff);
            await server.Light.TurnOffAsync();
            Debug.Log("Лента отключена");
        }
        else
        {
            ChangeSprite(turnOn);
            await server.Light.TurnOnAsync();
            Debug.Log("Лента включена");
        }

    }

    private void SetStartButtonSprite()
    {
        if (server.Light.Power)
            ChangeSprite(turnOff);
        else
            ChangeSprite(turnOn);
    }

    private void ChangeSprite(Sprite _sprite) => GameObject.Find(@"Turn On\Off").GetComponent<Button>().image.sprite = _sprite;

    // Start is called before the first frame update
    public void Start()
    {
        if (server.Light.Connected)
        {
            SetStartButtonSprite();
            ReloadImageColor();
        }

    }

    private async Task SendColor()
    {
        if (server.Light.Power)
        {
            (byte r, byte g, byte b) color = ConvertColor();
            //Debug.Log("Передан цвет в светодиодную ленту");
            await server.Light.SetColorAsync(color.r, color.g, color.b);
            await server.Light.SetBrightnessAsync(brightness);
        }

    }

    private (byte r, byte g, byte b) ConvertColor()
    {
        byte r = (byte)Mathf.Round(red * 255);
        byte g = (byte)Mathf.Round(green * 255);
        byte b = (byte)Mathf.Round(blue * 255);
        return (g,r,b);
    }
}
