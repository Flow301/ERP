using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.Domain.Entities;

public record class Category
{
    public int IdCategory { get; set; }
    public string CategoryName { get; set; } = null!; 
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
