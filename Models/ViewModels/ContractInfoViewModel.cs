using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ViewModels
{
    public class ContractInfoViewModel
    {
        public int contractId { get; set; }
        public string contractName { get; set; }
        public string schoolName { get; set; }
        public DateTime? contractTime { get; set; }
    }
}
