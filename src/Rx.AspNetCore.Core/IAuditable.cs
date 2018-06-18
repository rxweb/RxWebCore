using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Core.Data
{
    public interface IAuditable
    {
        Nullable<int> CreatedBy { get; set; }
        Nullable<System.DateTime> CreatedOn { get; set; }
        Nullable<int> UpdatedBy { get; set; }
        Nullable<System.DateTime> UpdatedOn { get; set; }
    }
}
