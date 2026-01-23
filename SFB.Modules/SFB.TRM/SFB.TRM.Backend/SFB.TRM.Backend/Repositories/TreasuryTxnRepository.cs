using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.TRM;
using SFB.Shared.Backend.Repositories;
using SFB.TRM.Shared.Sealed;

namespace SFB.TRM.Backend.Repositories
{
    public class TreasuryTxnRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        public Task<ETreasuryTxn> CreateTxn(ETreasuryTxn txn)
        {
            txn.StatusCode = TreasuryStatus.Pendiente.Code;
            Context.TRMTreasuryTxn.Add(txn);
            return Task.FromResult(txn);
        }

        public async Task<ETreasuryTxn?> AnularByOrigin(string modOrigin, int txnOrigin)
        {
            var txn = await Context.TRMTreasuryTxn
                .FirstOrDefaultAsync(t => t.ModOrigin == modOrigin && t.TxnOrigin == txnOrigin && !t.Delete);

            if (txn is null)
            {
                return null;
            }

            txn.StatusCode = TreasuryStatus.Anulado.Code;
            Context.TRMTreasuryTxn.Update(txn);
            return txn;
        }
    }
}
