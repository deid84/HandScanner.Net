# HandScanner .Net
## Simple general purpose barcode scanner communication class

HandScanner .Net is a simple lightweight class writte in C# that helps programmers to easily implement any kind of barcode reader that supports these communication protocols:

- RS-232 | RS-485
- TcP/IP (in next upgrades)

## Features

- Easy to implement in any net framework version up to 4.7.1
- Higly configurable

## Installation

Actually there is no dll for this project, in order to use the class you need to:

- Clone the repository
- Open your project into Visual Studio
- Go to solution explorer
- Right click on your solution name -> Add -> Existing item
- Select the HandScanner.cs file from the cloned repository

## Using the class

Once imported in the project, the first thing to do is to instantiate a scanner object passing the com port and the baudrate used from the scanner
```sh
Scanner = new HandScanner("COM1", 9600);
```

Then open communication once required, if function return false, everything goes smoothly.

```sh
if (Scanner.Connect()){
    // Handle connections error here
}
```

Connect() function has 4 optional parameters that you could ignore if your connection has the following specs:

- 8 data bits
- no parity bit
- 1 stop bit
- no handshake

If your instruments has different parameter you can connect passing all customized parameters in this way

```sh
if (Scanner.Connect(8, System.IO.Ports.Parity.Even, System.IO.Ports.StopBits.Two, System.IO.Ports.Handshake.RequestToSend)){
    // Handle connections error here
}
```

Once connected the class uses an event handler to put into a Queue all the values readen from the scanner. Queue is a First in First out collection. You're free to check the queue every time its length is greater than 0

```sh
// Read every time scanner reads a value and put it into a textbox
if (Scanner.Value.Count != 0){
    textbox.Text = Value.Dequeue();
}

// Read after a given time all values in the queue respecting the received order and put it into a textbox separating it with "-"
foreach(string item in scDeviceManager.Scanner.Value){
    textbox.Text += item + "-";
}
```

Using Dequeue() command you're able to read values and clear buffer at the same time.

To disconnect the scanner simply use the disconnect function (no parameters required), if function returns false all worked properly

```sh
if (Scanner.Connect()){
    // Handle connections error here
}
```
## Thank you
This is my first repository so any help even to improve this job will be really appreciated. Feel free to suggest improvements (also for a better wiki).

Deid84

## License

GPL-3.0 License
