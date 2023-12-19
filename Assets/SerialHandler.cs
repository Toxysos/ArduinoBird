using System;
using System.IO.Ports;
using UnityEngine;

public class SerialHandler : MonoBehaviour
{
    
    private SerialPort _serial;

    // Common default serial device on a Windows machine
    [SerializeField] private string serialPort = "COM1";
    [SerializeField] private int baudrate = 115200;
    
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
        // Prevent blocking if no message is available as we are not doing anything else
        // Alternative solutions : set a timeout, read messages in another thread, coroutines, futures...
        if (_serial.BytesToRead <= 0) return;
        
        // Trim leading and trailing whitespaces, makes it easier to handle different line endings.
        // Arduino uses \r\n by default with `.println()`.
        var message = _serial.ReadLine().Trim();
        
        Debug.Log("Message received: " + message);

    }

    public void SetLed(bool newState)
    {
        _serial.WriteLine(newState ? "LED ON" : "LED OFF");
    }
    
    private void OnDestroy()
    {
        _serial.Close();
    }
}
