using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashLoanShop.Model
{
    public class CustomerLoan
    {

        public long Id { get; set; }

        public int CustomerId { get; set; }

        public int ShopStoreId { get; set; }

        public decimal LoanAmountApplied { get; set; }

        public DateTime NextPayDate { get; set; }

        public string LateInterestRate { get; set; }

        public bool IsLoanApproved { get; set; }

        public decimal LoanAmountApproved { get; set; }

        public string PaymentOption { get; set; }

        public decimal DueAmount { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal AdminFee { get; set; }

        public string CustomerName { get; set; }

        public string LoanStatus { get; set; }

        public int StatusUpdatedby { get; set; }

        public string LoanType { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string StoreAddress { get; set; }

        public string OverDueReason { get; set; }
        public string ModeofPayment { get; set; }

        public int? OverdueCount { get; set; }
        public decimal? RemainingAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string PartialPayment { get; set; }
        public string LoanDeniedReason { get; set; }
        public decimal? OverDueLoanAmount { get; set; }
        public decimal? LateInterestCharge { get; set; }
        public decimal? NSFCharge { get; set; }
        public string Settlement { get; set; }
        public decimal? LastDueAmount { get; set; }
        public decimal PartialAmount { get; set; }
        public string LastStatus { get; set; }
        public DateTime PartialLoanCreatedDate { get; set; }
        public string PartialPaymentMethod { get; set; }
    }

    public class LoanPartialPayment
    {

        public int Id { get; set; }

        public int LoanId { get; set; }

        public decimal PartialAmount { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Createdby { get; set; }
        public decimal? IntrestCharge { get; set; }
        public decimal? DueAmount { get; set; }
        public string PartialPaymentMethod { get; set; }
    }

    public enum LoanType
    {
        Not_Selected=0,
        Weekly = 1,
        Bi_Weekly = 2,
        Twice_Monthly = 3,
        Monthly = 4,
        No_Fix_Date = 5

    }
    public class CustomerLoanException
    {

        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public int? LoanId { get; set; }
    }
    public class LoanDenyReason
    {

        public int Id { get; set; }

        public int? LoanId { get; set; }

        public string DenyReason { get; set; }
    }
}
