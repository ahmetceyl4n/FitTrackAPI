using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Domain.Common
{
    public abstract class ISoftDelete
    {
        bool isDeleted { get; set; }
    }
}
