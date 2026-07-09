using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework11
{
    interface IItemUpgradeable
    {
        bool upgrade();
    }

    interface IInventoryUpgradeable
    {
        void upgrade(int size);
    }
}
