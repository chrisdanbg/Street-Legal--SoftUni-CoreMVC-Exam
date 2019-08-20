using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreetLegal.Helpers
{
    public class ErrorMessageHelper
    {
        public string PurchaseCompletedMessage { get; } = "Purchase Completed!";

        public string NotEnoughFundsMessage { get; } = "Insufficient funds!";
        public object SaveChangesErrorMessage { get; } = "Error Saving Changes";

        public object SaveChangesSuccsessMessage { get; } = "Changes Saved!";
        public object SoldMessage { get; } = "Car Sold";
    }
}
