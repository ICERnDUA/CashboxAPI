using CashboxCommands.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Events
{
    public class BaseEvent:PubSubEvent<BaseCommand>
    {
    }
}
