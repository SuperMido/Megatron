using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Megatron.Data.DBInit
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}