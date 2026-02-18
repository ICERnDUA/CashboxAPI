using CashboxCommands.Commands.Items;
using CashboxCommands.Commands.ItemsDiscountCommand;
using CashboxCommands.Converters;
using CashboxCommands.Data;
using Client.Logger;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Server.Modules
{
    public class ValidationCommands
    {
        public bool Validate(object command)
        {

            return JsonConvert.DeserializeObject<BaseCommand>( command.ToString()) != null;
            //try 
            //{ 
            //        var stringToCommandConverter = new StringToCommandConverter();
            //        var commandToStringConverter = new CommandToStringConverter();

            //        var com = stringToCommandConverter.ConvertedObject(JsonConvert.DeserializeObject(command.ToString()).ToString());

            //        if (com is BaseItemCommand)
            //        {
            //            try
            //            {
            //                string[] strArray = commandToStringConverter.ConvertToArray(com as BaseItemCommand, "");
            //                return true;
            //            }
            //            catch (Exception ex)
            //            {
            //                LoggerExceptoin.WriteLogExceptons(ex, com);
            //                return false;
            //            }

            //        }
            //    else if (com is BaseItemsDiscountCommand)
            //    {
            //        try
            //        {
            //            string[] strArray = commandToStringConverter.ConvertTotalItemsCommand(com as BaseItemsDiscountCommand, "");
            //            return true;
            //        }
            //        catch (Exception ex)
            //        {
            //            LoggerExceptoin.WriteLogExceptons(ex, com);
            //            return false;
            //        }
            //    }
            //    else
            //        try
            //        {
            //            string str = commandToStringConverter.ConvertToString(com, "");
            //            return true;
            //        }
            //        catch (Exception ex)
            //        {
            //            LoggerExceptoin.WriteLogExceptons(ex, com);
            //            return false;
            //        }

            //}
            //catch (Exception ex)
            //{
            //    LoggerExceptoin.WriteLogExceptons(ex, command);
            //    return false;
            //}
        }
    }
}
