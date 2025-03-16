using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ideageek.Subscribly.Core.Entities
{
    public class Enum
    {
    }

    public enum GroupType
    {
        [Description("None")]
        None = 0,
        [Description("Home")]
        Home = 1,
        [Description("Trip")]
        Trip
    }
}
