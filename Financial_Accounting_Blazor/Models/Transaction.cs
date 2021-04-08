using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Financial_Accounting_Blazor.Models
{
    public class Transaction 
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int sum { get; set; }
        public DateTime DateTime { get; set; }
    }
}
