using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.PriceTracker.Services.Types;

public record SearchFilter
{
    public string Category { get; set; }
    public string Material { get; set; }

}
