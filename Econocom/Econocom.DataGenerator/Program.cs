using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Econocom.DataGenerator.Entities.Entities;
using Econocom.DataGenerator.Entities.Interfaces;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Econocom.DataGenerator
{
    internal class Program
    {
        #region properties

        private static IConfiguration Configuration { get; set; }
        public static string Input { get; set; }

        public static IEnumerable<IData> PanicData { get; set; }
        public static IEnumerable<IData> DoorData { get; set; }
        public static IEnumerable<IData> FridgeData { get; set; }
        public static IEnumerable<IData> PlugData { get; set; }

        public static DeviceClient DeviceClient { get; set; }

        #endregion

        #region program

        static void Main()
        {
            BuildConfig();
            BuildTestDataSets();
            BuildDeviceClient();

            Console.WriteLine("Data sets ready, select action:");
            Console.WriteLine($"- 'PANI' = Transfer 'Panic'-data. (Amount: {PanicData.Count()})");
            Console.WriteLine($"- 'DOOR' = Transfer 'Door'-data. (Amount: {DoorData.Count()})");
            Console.WriteLine($"- 'FRID' = Transfer 'Fridge'-data. (Amount: {FridgeData.Count()})");
            Console.WriteLine($"- 'PLUG' = Transfer 'Smart Plug'-data. (Amount: {PlugData.Count()})");
            Console.WriteLine("- 'EXIT' = To exit console.");
            Console.WriteLine("");

            do
            {
                Input = Console.ReadLine()?.ToUpper();

                switch (Input)
                {
                    case "PANI":
                        Task.Run(async () => await TransferData(PanicData));
                        break;

                    case "DOOR":
                        Task.Run(async () => await TransferData(DoorData));
                        break;

                    case "FRID":
                        Task.Run(async () => await TransferData(FridgeData));
                        break;

                    case "PLUG":
                        Task.Run(async () => await TransferData(PlugData));
                        break;

                    default:
                        Console.WriteLine($"Input could not be processed.");
                        Console.WriteLine($"Select a new keyword:");
                        break;
                }

            } while (Input != "EXIT");
        }

        #endregion

        #region private methods

        private static void BuildConfig()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }

        private static void BuildTestDataSets()
        {
            PanicData = GeneratePanicData();
            DoorData = GenerateDoorData();
            FridgeData = GenerateFridgeData();
            PlugData = GenerateSmartPlugData();
        }

        private static void BuildDeviceClient()
        {
            DeviceClient = DeviceClient.CreateFromConnectionString(Configuration["connectionstring"]);
        }

        private static async Task TransferData(IEnumerable<IData> dataset)
        {
            foreach (var data in dataset)
            {
                await DeviceClient
                    .OpenAsync();

                var message = new Message(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));

                await DeviceClient
                    .SendEventAsync(message);

                Console.Write(".");
            }

            Console.WriteLine("\n\nTransfer complete, select a new keyword:");
        }

        private static IEnumerable<IData> GeneratePanicData()
        {
            return File.ReadAllLines(Configuration["testdata:panic"])
                .Select(a => a.Split(';'))
                .Select(data => new PanicData
                {
                    DeviceId = data[0],
                    Type = data[1],
                    Payload = new PanicDataPayload
                    {
                        LastSingleClick = int.Parse(data[2]),
                        SourceId = data[3]
                    }
                });
        }

        private static IEnumerable<IData> GenerateDoorData()
        {
            return new List<IData>();
        }

        private static IEnumerable<IData> GenerateFridgeData()
        {
            return new List<IData>();
            ;
        }

        private static IEnumerable<IData> GenerateSmartPlugData()
        {
            return new List<IData>();
            ;
        }

        #endregion
    }
}