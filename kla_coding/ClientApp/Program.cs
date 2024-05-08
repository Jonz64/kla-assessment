using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main(string[] args)
    {
        TcpClient client = new TcpClient("localhost", 8888);
        NetworkStream stream = client.GetStream();

        // Prompt the user to enter a numeric value
        Console.Write("\nEnter a dollar amount (0.00 - 999,999,999.99): ");
        string numericValue = Console.ReadLine();

        // Send the numeric value to the server
        byte[] requestData = Encoding.ASCII.GetBytes(numericValue);
        stream.Write(requestData, 0, requestData.Length);
        // Console.WriteLine($"Sent to server: {numericValue}");

        // Receive and display the converted result from the server
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string result = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine(result + "\n\n");

        stream.Close();
        client.Close();
    }
}
