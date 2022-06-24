using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Enums
{
    public enum Message : byte
    {
        AddXp = 1,
        SyncLevel,
        Caster = 3
    }
}
