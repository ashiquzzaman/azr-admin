using System;
using System.Transactions;

namespace AzR.Utilities
{
    /// <summary>
    /// This class is meant to be a wrapper around TransactionScope, to avoid problems with escalating transactions among different versions of SQL Server.
    /// Please refer to http://jeffmlakar.wordpress.com/2011/12/07/differences-in-escalation-to-distributed-transaction-coordinator-starting-in-sql-server-2008/ 
    /// for more information.
    /// </summary>
    public sealed class TransactionWrapper : IDisposable
    {
        #region Private Members

        private TransactionScope transactionScope;

        #endregion

        #region Constructors

        public TransactionWrapper()
        {
            if (GeneralHelper.GetConfigValue("TransactionSupportEnabled", true))
            {
                transactionScope = new TransactionScope();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Commits the transaction
        /// </summary>
        public void Complete()
        {
            if (transactionScope != null)
            {
                transactionScope.Complete();
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Rolls back the transaction
        /// </summary>
        public void Dispose()
        {
            if (transactionScope != null)
            {
                transactionScope.Dispose();
            }
        }

        #endregion
    }
}
