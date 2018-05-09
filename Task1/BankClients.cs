using System;
using System.Collections.Generic;
using System.Linq;

namespace Task1
{
    public class BankClients
    {
        public List<ClientInfo> Clients { get; set; }

        /// <summary>
        /// Calculates the sum by operation type for the month
        /// </summary>
        /// <param name="month">An integer month</param>
        /// <returns>Dictionary that contains credit and debit sum</returns>
        public Dictionary<string, double> OperationsSumByMonth(int month)
        {
            return this.Clients
                .SelectMany(client => client.Operations)
                .Where(operation => operation.Date.Month == month)
                .GroupBy(operation => operation.OperationType)
                .ToDictionary(g => g.Key, g => g.Sum(operation => operation.Amount));
        }

        /// <summary>
        /// Queries clients who does not have credit operations in specific month
        /// </summary>
        /// <param name="month">An integer month</param>
        /// <returns>List of clients</returns>
        public List<object> ClientsWithoutCreditByMonth(int month)
        {
            return this.Clients
                .Where(client => !client.Operations.Any(
                    operation => operation.Date.Month == month && operation.OperationType == "Credit")
                )
                .Select(client => client.ShortObject())
                .ToList();
        }

        /// <summary>
        /// Queries cleint who has the biggest sum of specific operation type
        /// </summary>
        /// <param name="operationType">An operation type</param>
        /// <returns>ClientInfo short object</returns>
        public object ClientWithBiggestOperationSum(string operationType)
        {
            return this.Clients
                .MaxBy(client => client.Operations
                    .Where(operation => operation.OperationType == operationType)
                    .Sum(operation => operation.Amount))
                .ShortObject();
        }
        
        /// <summary>
        /// Queries client who has the biggest balance by specific date
        /// </summary>
        /// <remarks>
        /// Balance calculates by subtracting the sum of credit operations from the sum of debit operations
        /// </remarks>
        /// <param name="date">A date</param>
        /// <returns>ClientInfo short object</returns>
        public object ClientWithBiggestBalanceByDate(DateTime date)
        {
            return this.Clients
                .MaxBy(client => client.Operations
                    .Where(operation => operation.Date < date)
                    .GroupBy(operation => operation.OperationType)
                    .Select(g =>
                    {
                        var sum = g.Sum(operation => operation.Amount);
                        if (g.Key == "Debit") return sum;
                        if (g.Key == "Credit") return -sum;
                        return 0;
                    })
                    .Sum()
                )
                .ShortObject();
        }
    }
}