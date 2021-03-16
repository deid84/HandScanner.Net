/// <summary>
/// Name: HandScanner
/// Desc: Class for scanner communication management
/// Rel:  0.2
/// Date: 16/03/2021
///
/// Changelog:
/// 1.0 - first release
/// 1.1 - added baudrate enumerations
///       added connected bool flag
///       all parameters requested in constructor
///       added 2 optional parameters (newLine default \r && timeout default 500ms)
///       added event delegate
///       removed queue
/// </summary>

using System;
using System.IO.Ports;
using System.Net.Sockets;   // Actually unused

namespace HandScanner
{
    public class HandScanner
    {
        #region Public Declarations
        /*-----------------------------------------------------------------------------+
        |                              Public Declarations                             |
        +-----------------------------------------------------------------------------*/
        // Public enumerations
        public enum Baudrate
        {
            Baudrate9600 = 9600,
            Baudrate14400 = 14400,
            Baudrate19200 = 19200,
            Baudrate38400 = 38400,
            Baudrate57600 = 57600, 
            Baudrate115200 = 115200
        }

        // Public Events
        public event EventHandler DataScanned;
        #endregion



        #region Private Declarations
        /*-----------------------------------------------------------------------------+
        |                             Private Declarations                             |
        +-----------------------------------------------------------------------------*/
        // Private constants
        private const int _timeoutAnswer_ms = 500;

        // Private variables
        private SerialPort _serialPort;
        #endregion



        #region General Properties
        /*-----------------------------------------------------------------------------+
        |                              General Properties                              |
        +-----------------------------------------------------------------------------*/
        public bool Connected { get; private set; }
        public bool EventEnabled { get; set; }
        public string Value { get; private set; }
        #endregion



        #region Constructor & Desctructor
        /*-----------------------------------------------------------------------------+
        |                           Constructor & Destructor                           |
        +-----------------------------------------------------------------------------*/
        public HandScanner(string portName,
                           int baudRate,
                           int dataBit = 8,
                           Parity parity = Parity.None,
                           StopBits stopBit = StopBits.One,
                           Handshake handShake = Handshake.None,
                           string newLine = "\r",
                           int timeout = _timeoutAnswer_ms)
        {
            // Instantiate serial port object
            _serialPort = new SerialPort()
            {
                PortName = portName,
                BaudRate = baudRate,
                DataBits = dataBit,
                StopBits = stopBit,
                Parity = parity,
                Handshake = handShake,
                NewLine = newLine,
                ReadTimeout = timeout,
            };
        }
        #endregion



        #region Public Methods
        /*-----------------------------------------------------------------------------+
        |                                Public Methods                                |
        +-----------------------------------------------------------------------------*/
        public bool Connect()
        {
            try
            {
                // Add handler for data receiving
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                // Open serial port
                _serialPort.Open();
                // Set connection flag
                Connected = true;
                // Return false
                return false;
            }
            catch
            {
                // Delete the object
                _serialPort = null;
                // Return true
                return true;
            }
        }



        public bool Disconnect()
        {
            if (_serialPort != null)
            {
                try
                {
                    // Close the serial port and delete the object
                    _serialPort.Close();
                    _serialPort = null;
                    // Reset connection flag
                    Connected = false;
                    // Return False
                    return false;
                }
                catch
                {
                    // Return true  
                    return true;
                }
            }
            else
            {
                // Return False
                return false;
            }
        }



        public bool Read(ref string value)
        {
            try
            {
                if (_serialPort.BytesToRead != 0)
                {
                    value = _serialPort.ReadLine();
                }      
                // Return false
                return false;
            }
            catch
            {
                // Return true  
                return true;
            }           
        }
        #endregion



        #region Private Methods
        /*-----------------------------------------------------------------------------+
        |                               Private Methods                                |
        +-----------------------------------------------------------------------------*/
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // If Event is enabled use event handler
            if (EventEnabled)
            {
                Value =_serialPort.ReadLine();
                DataScanned?.Invoke(this, e);
            }
        }
        #endregion
    }
}
