 
 
using System.Net;
using System.Net.Sockets;
 
using ServerX;
using MongoDB.Driver;
 
using StackExchange.Redis;
using Newtonsoft.Json;

public class Server
{
	
	 
	
/*
	private static void ShowServerNetworkConfig()
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface adapter in adapters)
		{
			Console.WriteLine(adapter.Description);
			Console.WriteLine("\tAdapter Name: " + adapter.Name);
			Console.WriteLine("\tMAC Address: " + adapter.GetPhysicalAddress());
			IPInterfaceProperties ip_properties = adapter.GetIPProperties();
			UnicastIPAddressInformationCollection addresses = ip_properties.UnicastAddresses;
			foreach (UnicastIPAddressInformation address in addresses)
			{
				Console.WriteLine("\tIP Address: " + address.Address);
			}
		}
		Console.ForegroundColor = ConsoleColor.White;
	}
*/
	public static void Main()
	{
		

		/*var dbClient = new MongoClient("mongodb://127.0.0.1:27017");

		IMongoDatabase db = dbClient.GetDatabase("testdb");
		var cars = db.GetCollection<BsonDocument>("cars");
			*/

		TcpListener listener = null;
		try
		{
			//ShowServerNetworkConfig();
			listener = new TcpListener(IPAddress.Any, 8080);
			listener.Start();
			Console.WriteLine("Server started...");
			while (true)
			{
				/*Console.WriteLine("Waiting for incoming client connections...");
				TcpClient client = listener.AcceptTcpClient();
				 
				Console.WriteLine("Accepted new client connection...");
				Thread t = new Thread(ProcessClientRequests);
				t.Start(client);
				Thread.Sleep(1);*/
				TcpClient client = listener.AcceptTcpClient();
				HttpProcessor processor = new HttpProcessor(client);
				Thread thread = new Thread(new ThreadStart(processor.Process));
				thread.Start();
				Thread.Sleep(1);



			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
		finally
		{
			if (listener != null)
			{
				listener.Stop();
			}
		}
	} // end Main()

	

	 /*public static void ReadData()
	{
		
		var cache = RedisConnectorHelper.Connection.GetDatabase();
		var devicesCount = 2;
		var values = cache.ListLeft`1`1($"Device_Status:");

		List<Message> list = new List<Message>();
		foreach (var value in values)
			list.Add(new Message { message = value});

		//Console.WriteLine($"Valor={value}");
		 var dbClient = new MongoClient("mongodb://127.0.0.1:27017");

		IMongoDatabase db = dbClient.GetDatabase("xenotic");

		var cars = db.GetCollection<Message>("car");
		cars.InsertMany(car); 
		 BsonDocument document = new BsonDocument();
		if (list != null)
		{
			
			document.Add("list", new BsonArray(list));
			//var json = document.ToJson();
			cars.InsertMany(document);
			cache.KeyDelete($"Device_Status:");
		} 



	}*/

	 
	public static void Transfer()
	{
		

	}


} // end class definition

public class HttpProcessor
{
	private TcpClient client;
	private StreamReader reader;
	private StreamWriter writer;

	public HttpProcessor(TcpClient client)
	{
		this.client = client;
		this.reader = null;
		this.writer = null;
	}

	public void Process()
	{
		reader = new StreamReader(client.GetStream());
		writer = new StreamWriter(client.GetStream());

		try
		{
			 
			string s = String.Empty;
			while (!(s = reader.ReadLine()).Equals("Exit") || (s == null))
			{
				//Console.WriteLine("From client -> " + s);
				writer.WriteLine("From server -> " + s);

				Message mes = new Message { key = "Device", message = s };

				SaveBigData(mes);

				ReadData();
				writer.Flush();
			}

			reader.Close();
			writer.Close();
			client.Close();
			Console.WriteLine("Client connection closed!");
		}
		catch (IOException)
		{
			Console.WriteLine("Problem with client communication. Exiting thread.");
		}
		finally
		{
			if (client != null)
			{
				client.Close();
			}
		}

		client.Close();
	}


	public static void SaveBigData(object messageModel)
	{

		var cache = RedisConnectorHelper.Connection.GetDatabase();
		Console.WriteLine("jasdhgfjshgdfjd" + JsonConvert.SerializeObject(messageModel));
		cache.ListRightPush($"Device_Status:", JsonConvert.SerializeObject(messageModel));

	}

	[Obsolete]
	public static void ReadData()
	{
		IDatabase db = RedisConnectorHelper.Connection.GetDatabase();
		EndPoint endPoint = RedisConnectorHelper.Connection.GetEndPoints().First();
		RedisKey[] keys = RedisConnectorHelper.Connection.GetServer(endPoint).Keys(pattern: "*").ToArray();
		/*  for (int i=0; i< keys.Length; i++)
		  {
			  //Console.WriteLine("keyyyyyyyyyyy" + keys[i].);
		  }*/
	}
}

 