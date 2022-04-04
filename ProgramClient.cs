 
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

public class Client
{
	public static void Main(String[] args)
	{

        while (true)
        {
			IPAddress ip_address = IPAddress.Parse("110.120.46.10"); //default
			int port = 8080;
			try
			{
				if (args.Length >= 1)
				{
					ip_address = IPAddress.Parse(args[0]);
				}
			}
			catch (FormatException)
			{
				Console.WriteLine("Invalid IP address entered. Using default IP of: "
													+ ip_address.ToString());
			}
			try
			{
				Console.WriteLine("Attempting to connect to server at IP address: {0} port: {1}",
													ip_address.ToString(), port);
				TcpClient client = new TcpClient(ip_address.ToString(), port);
				Console.WriteLine("Connection successful!");
				StreamReader reader = new StreamReader(client.GetStream());
				StreamWriter writer = new StreamWriter(client.GetStream());
				string s = String.Empty;
				while (!s.Equals("Exit"))
				{
					//Console.Write("Enter a string to send to the server: ");

					s = "Device 1, I ma device one"; //Console.ReadLine();
					Console.WriteLine();
					writer.WriteLine(s);
					writer.Flush();
					if (!s.Equals("Exit"))
					{
						String server_string = reader.ReadLine();
						Console.WriteLine(server_string);
					}
				}
				reader.Close();
				writer.Close();
				client.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		
	} // end Main()
} // end class definition