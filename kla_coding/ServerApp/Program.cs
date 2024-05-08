using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static DollarConverter;

class Server
{
    static async Task Main(string[] args)
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("Server started. Waiting for connections...");

        try
        {
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");
                HandleClientAsync(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server error: {ex.Message}");
        }
        finally
        {
            server.Stop();
        }
    }

    static async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string requestData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                
                // Process request and send response
                string response = "";
                try 
                {
                    response = ConvertDecimalToWords(requestData);
                }
                catch 
                {
                    response = "Error: Invalid Input. Please enter a valid number.";
                }

                // Send response to client
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                await stream.WriteAsync(responseData, 0, responseData.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }
}
