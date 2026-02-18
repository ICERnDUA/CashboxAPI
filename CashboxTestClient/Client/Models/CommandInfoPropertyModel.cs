using CashboxCommands.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class CommandInfoPropertyModel<T>
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private string _stationDateTime;
        public string StationDateTime
        {
            get { return _stationDateTime; }
            set { _stationDateTime = value; }
        }
        private ObservableCollection<string> _infoProperty = new ObservableCollection<string>();
        public ObservableCollection<string> TextData
        {
            get { return _infoProperty; }
            set { _infoProperty = value; }
        }
        public CommandInfoPropertyModel(T value, string[] infoProperty)
        {
            Value = value;
            foreach (var info in infoProperty)
            {
                TextData.Add(info); 
            }

            StationDateTime = (value as BaseCommand).StationDateTime.ToString();


        }
        public CommandInfoPropertyModel(T value, string infoProperty)
        {
            try
            {
                Value = value;
                TextData.Add(infoProperty);
                if (value is BaseCommand)
                    StationDateTime = (value as BaseCommand).StationDateTime.ToString();
                else                    
                    StationDateTime = DateTime.Now.ToString();
            }
            catch
            {
            }
        }

    }
}
