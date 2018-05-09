using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Task1
{
    internal class Program
    {
        private const string BankClientsFilePath = "bankClients.json";

        public static int Main(string[] args)
        {
            if (!File.Exists(BankClientsFilePath))
            {
                Console.WriteLine($"File {BankClientsFilePath} does not exist\nPress any key");
                Console.ReadKey();
                return 1;
            }
            var bankClients = new BankClients();
            try
            {
                using (var reader = new StreamReader(BankClientsFilePath))
                {
                    bankClients.Clients = JsonConvert.DeserializeObject<List<ClientInfo>>(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not read from file {BankClientsFilePath}\n{e.Message}\nPress any key");
                Console.ReadKey();
                return 1;
            }
            try
            {
                var bankReport = new
                {
                    OperationsSumByApril = bankClients.OperationsSumByMonth(4),
                    ClientsWithoutCreditInApril = bankClients.ClientsWithoutCreditByMonth(4),
                    ClientWithBiggestDebitSum = bankClients.ClientWithBiggestOperationSum("Debit"),
                    ClientWithBiggestCreditSum = bankClients.ClientWithBiggestOperationSum("Credit"),
                    ClientWithBiggestBalanceByMay = bankClients.ClientWithBiggestBalanceByDate(new DateTime(2018, 5, 1))
                };
                SaveToJson(bankReport, "bankReport.json");
                
                Console.WriteLine($"Sum by operation type in april:\nCredit: {bankReport.OperationsSumByApril["Credit"]}; Debit: {bankReport.OperationsSumByApril["Debit"]}");
                Console.WriteLine("\nClients without credit in april:");
                bankReport.ClientsWithoutCreditInApril.ForEach(Console.WriteLine);
                Console.WriteLine($"\nClient with biggest debit sum:\n{bankReport.ClientWithBiggestDebitSum}");
                Console.WriteLine($"\nClient with biggest credit sum:\n{bankReport.ClientWithBiggestCreditSum}");
                Console.WriteLine($"\nClient with biggest balance by 01.05:\n{bankReport.ClientWithBiggestBalanceByMay}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Bank report error\n{e.Message}\nPress any key");
                Console.ReadKey();
                return 1;
            }

            Console.WriteLine("Finished. Press any key");
            Console.ReadKey();
            return 0;
        }

        public static void SaveToJson(object target, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                var jsonObject = JsonConvert.SerializeObject(target);
                writer.WriteLine(jsonObject);
            }
        }
    }
}