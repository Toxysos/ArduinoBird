using System;
using System.IO.Ports;
using UnityEngine;

public class SerialHandler : MonoBehaviour
{
    
    private SerialPort _serial;

    // Common default serial device on a Windows machine
    [SerializeField] private string serialPort = "COM1";
    [SerializeField] private int baudrate = 115200;
    
    private int _r = 255;
    private int _g = 0;
    private int _b = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _serial = new SerialPort(serialPort,baudrate);
        // Guarantee that the newline is common across environments.
        _serial.NewLine = "\n";
        // Once configured, the serial communication must be opened just like a file : the OS handles the communication.
        _serial.Open();
        
    }

    // Update is called once per frame
    void Update()
    {
        // set color to red, blue, green if the corresponding key is pressed just to test
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetColor(255, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetColor(0, 255, 0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetColor(0, 0, 255);
        }
        
        GetDistance();
    }
    
    public void GetDistance()
    {
        // Prevent blocking if no message is available as we are not doing anything else
        // Alternative solutions : set a timeout, read messages in another thread, coroutines, futures...
        if (_serial.BytesToRead <= 0) return;
        
        // Trim leading and trailing whitespaces, makes it easier to handle different line endings.
        // Arduino uses \r\n by default with `.println()`.
        var message = _serial.ReadLine().Trim();
        
        Debug.Log("Distance: " + message);
    }
    
    
    public void SetColor(int r, int g, int b)
    {
        _r = r;
        _g = g;
        _b = b;
        
        
        try
        {
            if (serialPort != null && _serial.IsOpen)
            {
                // Send RGB values to Arduino
                string colorData = $"{_r},{_g},{_b};";
                _serial.Write(colorData);

                Debug.Log($"Color Sent: R={_r}, G={_g}, B={_b}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error writing to serial port: " + e.Message);
        }
    }
    
    private void OnDestroy()
    {
        _serial.Close();
    }
    
}
