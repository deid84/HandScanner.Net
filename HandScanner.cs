/// <summary>
/// Name: HandScanner
/// Desc: Class for scanner communication management
/// Rel:  1.0_DF
/// Date: 02/03/2021
///
/// Changelog:
/// 1.0 - first release
/// </summary>

using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace HandScanner
{
    public class HandScanner
    {
        #region Public Declarations
        /*-----------------------------------------------------------------------------+
        |                              Public Declarations                             |
        +-----------------------------------------------------------------------------*/

        #endregion

        #region Private Declarations
        /*-----------------------------------------------------------------------------+
        |                             Private Declarations                             |
        +-----------------------------------------------------------------------------*/
        // Private constants
        private const int _timeoutAnswer_ms = 500;

        // Private variables
        private readonly int _baudRate;
        private readonly string _portName;
        private SerialPort _serialPort;
        private readonly Queue<string> _value;
        #endregion

        #region General Properties
        /*-----------------------------------------------------------------------------+
        |                              General Properties                              |
        +-----------------------------------------------------------------------------*/
        public bool Enabled { private get; set; }
        public Queue<string> Value { get => _value; }
        #endregion

        #region Constructor & Desctructor
        /*-----------------------------------------------------------------------------+
        |                           Constructor & Destructor                           |
        +-----------------------------------------------------------------------------*/
        public HandScanner(string portName,
                           int baudRate)
        {
            // Assign com port and baudrate
            _portName = portName;
            _baudRate = baudRate;
            // Init queue
            _value = new Queue<string>();
        }
        #endregion

        #region Public Methods
        /*-----------------------------------------------------------------------------+
        |                                Public Methods                                |
        +-----------------------------------------------------------------------------*/
        public bool Connect(int dataBit = 8,
                            Parity parity = Parity.None,
                            StopBits stopBit = StopBits.One,
                            Handshake handShake = Handshake.None)
        {
            try
            {
                // Init serial port properties
                _serialPort = new SerialPort()
                {
                    PortName = _portName,
                    BaudRate = _baudRate,
                    DataBits = dataBit,
                    StopBits = stopBit,
                    Parity = parity,
                    Handshake = handShake,
                    NewLine = "\r",
                    ReadTimeout = _timeoutAnswer_ms,
                };
                // Add handler for data receiving
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                // Open serial port
                _serialPort.Open();
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
        #endregion

        #region Private Methods
        /*-----------------------------------------------------------------------------+
        |                               Private Methods                                |
        +-----------------------------------------------------------------------------*/
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            // Check if scanner is enabled
            if (Enabled)
            {
                _value.Enqueue(_serialPort.ReadLine());
            }
            else
            {
                // Discard readen value
                _serialPort.DiscardInBuffer();
            }
        }
        #endregion
    }
}
