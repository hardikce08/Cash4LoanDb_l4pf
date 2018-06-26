using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashLoanShop.Model
{
    public class Company
    {
      
        public int Id { get; set; }
      
        public string Name { get; set; }
      
        public string Address { get; set; }
      
        public string City { get; set; }
      
        public string Province { get; set; }
 
        public string PostCode { get; set; }
    
        public string Phone { get; set; }
      
        public string BankTransitNumber { get; set; }
      
        public string BankAccountNumber { get; set; }
       
        public string Status { get; set; }
      
        public DateTime? CreatedDate { get; set; }
      
        public int? CreatedBy { get; set; }

    }

    public class CompanyStore
    {

        public int Id { get; set; }

        public string Address { get; set; }
        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public string PostCode { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string Businessname { get; set; }
    }
}
