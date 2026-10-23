using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Domain.Entities;

public class Province { 
    public int IdProvince { get; set; } 
    public string ProvinceName { get; set; } = null!; 
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>(); 
}
