using System;
using System.Collections.Generic;
using System.Transactions;

namespace log4net.Extensions.DistributedTransactionCoordination
{
    public class DistrubutedTransactionContextProvider : IContextProvider
    {
        private readonly string _key;

        public DistrubutedTransactionContextProvider()
            : this(Constants.DefaultContextName)
        {
        }

        public DistrubutedTransactionContextProvider(string key)
        {
            _key = key;
        }

        public KeyValuePair<string, object> GetContext()
        {
            return Transaction.Current.TransactionInformation.DistributedIdentifier != Guid.Empty
                ? new KeyValuePair<string, object>(_key,
                    Transaction.Current.TransactionInformation.DistributedIdentifier)
                : (new DefaultContextProvider()).GetContext();
        }
    }
}