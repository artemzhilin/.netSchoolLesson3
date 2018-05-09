using System.Collections.Generic;
using System.Linq;

namespace Task1
{
    public class ClientInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public List<Operation> Operations { get; set; }

        /// <summary>
        /// Formats client info short object
        /// </summary>
        /// <returns>Anonymous object</returns>
        public object ShortObject()
        {
            return new
            {
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName,
                FirstDebitDate = GetFirstDebitDate()
            };
        }
        
        public override string ToString()
        {   
            return this.ShortObject().ToString();
        }

        private string GetFirstDebitDate()
        {
            var firstDebit = this.Operations
                .OrderBy(operation => operation.Date)
                .FirstOrDefault(operation => operation.OperationType=="Debit");
            return (firstDebit != null ? firstDebit.Date.ToString("G") : "Unspecified");
        }
    }
}