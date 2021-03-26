using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using MagicHome;
public class ColorMaker : MonoBehaviour
{
    [SerializeField] string address = "192.168.88.160";
    [SerializeField] float red = 1;
    [SerializeField] float green = 1;
    [SerializeField] float blue = 1;
    [SerializeField] Image currentImage;
    [SerializeField] Sprite turnOn;
    [SerializeField] Sprite turnOff;

    private LEDLight light;

    public float Red
    {
        set
        {
            red = value;
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
        currentImage.color = new UnityEngine.Color(red, green, blue);
        await SendColor();
    }

    public async void TurnAround()
    {
        if (light.Power)
        {
            ChangeSprite(turnOff);
            await light.TurnOffAsync();
            Debug.Log("Лента отключена");
        }
        else
        {
            ChangeSprite(turnOn);
            await light.TurnOnAsync();
            Debug.Log("Лента включена");
        }

    }

    private void SetStartButtonSprite()
    {
        if (light.Power)
            ChangeSprite(turnOff);
        else
            ChangeSprite(turnOn);
    }

    private void ChangeSprite(Sprite _sprite) => GameObject.Find(@"Turn On\Off").GetComponent<Button>().image.sprite = _sprite;

    public async void Test()
    {
        var discoveredLights = await MagicHome.LEDLight.DiscoverAsync();

        if (true)
        {

        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        ConnectToLed();
        SetStartButtonSprite();
        ReloadImageColor();
    }

    private void ConnectToLed()
    {
        light = new LEDLight(address);

        // Connect.
        light.ConnectAsync();
        Debug.Log("Соединение со светодиодной лентой прошло успешно");
        red = light.Color.Red / 255;
        blue = light.Color.Blue / 255;
        green = light.Color.Green / 255;

    }

    private async Task SendColor()
    {
        if (light.Power)
        {
            (byte r, byte g, byte b) color = ConvertColor();
            Debug.Log("Передан цвет в светодиодную ленту");
            await light.SetColorAsync(color.r, color.g, color.b);
        }

    }

    private (byte r, byte g, byte b) ConvertColor()
    {
        byte r = (byte)Mathf.Round(red * 255);
        byte g = (byte)Mathf.Round(green * 255);
        byte b = (byte)Mathf.Round(blue * 255);
        return (r,g,b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
