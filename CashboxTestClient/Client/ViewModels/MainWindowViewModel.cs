using CashboxCommands.Commands.Items;
using CashboxCommands.Commands.ItemsDiscountCommand;
using CashboxCommands.Converters;
using CashboxCommands.Data;
using Client.Enums;
using Client.Events;
using Client.Interfaces;
using Client.Logger;
using Client.Models;
using Client.SignalR;
using Client.Views;
using ExpertIp.Translations;
using Newtonsoft.Json;
using Prism.Navigation.Regions;
using Server.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Controls;

namespace Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        private ConnectedManager _connectedManager;
        private INetworkDAO _networkDAO;
        private IEventAggregator _eventAggregator;
        private StringToCommandConverter _stringToCommandConverter = new StringToCommandConverter();
        private CommandToStringConverter _commandToStringConverter = new CommandToStringConverter();
        private ValidationCommands _validationCommands = new ValidationCommands();
        public MainWindowViewModel(IEventAggregator eventAggregator, INetworkDAO networkDAO, IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _networkDAO = networkDAO;
            _connectedManager = new ConnectedManager();
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<SendStringJsonEvent>().Subscribe(OnReceivedString);
            _eventAggregator.GetEvent<NetworkSettingsCloseEvent>().Subscribe(CloseRegion);
            OnLoadedCommand = new DelegateCommand(OnLoad);
            StartStopConnectionCommand = new DelegateCommand(OnStartStopConnection);
            OpenSettingsCommand = new DelegateCommand(OnOpenSettings);
            var culture = ConvertTest(SelectedLanguage);
            ExpertIp.Translations.Translations.Instance.CurrentLanguage = culture;
            ConnectionStatus = Translations.Instance.Translate("lblStop", Translations.Instance.CurrentLanguage);
        }

        private ObservableCollection<CommandInfoPropertyModel<BaseCommand>> _commands = new ObservableCollection<CommandInfoPropertyModel<BaseCommand>>();
        public ObservableCollection<CommandInfoPropertyModel<BaseCommand>> Commands
        {
            get { return _commands; }
            set { SetProperty(ref _commands, value); }
        }

        private string _selectedLanguage = LanguageEnums.Українська.ToString();
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { SetProperty(ref _selectedLanguage, value, ChengeLanguage); }
        }
        private string _connectionStatus;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set { SetProperty(ref _connectionStatus, value); }
        }
        private void ChengeLanguage()
        {
            var culture = ConvertTest(SelectedLanguage);
            ExpertIp.Translations.Translations.Instance.CurrentLanguage = culture;

            foreach (var command in Commands)
            {
                try
                {
                    string[] str = new string[1];
                    switch (command.Value)
                    {
                        case BaseItemCommand:
                            {
                                str = _commandToStringConverter.ConvertToArray(command.Value as BaseItemCommand, culture);
                                break;
                            }
                        case BaseItemsDiscountCommand:
                            {
                                str = _commandToStringConverter.ConvertTotalItemsCommand(command.Value as BaseItemsDiscountCommand, culture);
                                break;
                            }
                        default:
                            {
                                str = new string[] { _commandToStringConverter.ConvertToString(command.Value, culture) };
                                break;
                            }
                    }
                    command.TextData.Clear();
                    foreach (string s in str)
                        command.TextData.Add(s);
                }
                catch (Exception ex)
                {
                    Logger.LoggerExceptoin.WriteLogExceptons(ex);
                }
            }



        }      

        public DelegateCommand OpenSettingsCommand { get; private set; }
        private void OnOpenSettings()
        {
            _regionManager.RequestNavigate("SettingsRegion", nameof(ServerConnectSettingsView));
        }
        public DelegateCommand OnLoadedCommand { get; private set; }
        private void OnLoad()
        {
            var settings = _networkDAO.ReadNetworkData();
            if (settings != null)
            {
                _connectedManager.InitConnection();
            }
            else
            {
                _regionManager.RequestNavigate("SettingsRegion", nameof(ServerConnectSettingsView));

            }

        }
        public void CloseRegion()//???
        {
            _connectedManager.StopConnection();
            _connectedManager.InitConnection();
            _connectedManager.StartConnection();

            _regionManager.Regions["SettingsRegion"].RemoveAll();
        }
       
        private void OnReceivedString(object json)
        {

            try
            {
                bool valid = _validationCommands.Validate(json);
                if (!valid)
                {
                    return;
                }
                var com = _stringToCommandConverter.ConvertedObject(json.ToString());

                if (com is BaseItemCommand)
                {
                    try
                    {
                        string[] strArray = _commandToStringConverter.ConvertToArray(com as BaseItemCommand, ConvertTest(SelectedLanguage));
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Commands.Add(new CommandInfoPropertyModel<BaseCommand>(com, strArray));
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ConvertToString");
                        LoggerExceptoin.WriteLogExceptons(ex, com);
                    }

                }
                else if (com is BaseItemsDiscountCommand)
                {
                    try
                    {
                        string[] strArray = _commandToStringConverter.ConvertTotalItemsCommand(com as BaseItemsDiscountCommand, ConvertTest(SelectedLanguage));
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Commands.Add(new CommandInfoPropertyModel<BaseCommand>(com, strArray));
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ConvertToString");
                        LoggerExceptoin.WriteLogExceptons(ex, com);
                    }
                }
                else
                    try
                    {
                        string str = _commandToStringConverter.ConvertToString(com, ConvertTest(SelectedLanguage));

                        System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Commands.Add(new CommandInfoPropertyModel<BaseCommand>(com, str));
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("ConvertToString");
                        LoggerExceptoin.WriteLogExceptons(ex, com);
                    }

            }
            catch (Exception ex)
            {
                LoggerExceptoin.WriteLogExceptons(ex);
            }



        }
        public DelegateCommand StartStopConnectionCommand { get; private set; }
        private void OnStartStopConnection()
        {
            if (ConnectionStatus == Translations.Instance.Translate("lblStart", Translations.Instance.CurrentLanguage))
            {
                _connectedManager.StartConnection();
                ConnectionStatus = Translations.Instance.Translate("lblStop", Translations.Instance.CurrentLanguage);
            }
            else
            {
                _connectedManager.StopConnection();
                ConnectionStatus = Translations.Instance.Translate("lblStart", Translations.Instance.CurrentLanguage);
            }
        }
        private string ConvertTest(string languageEnums)
        {
            return languageEnums switch
            {
                "Українська" => "uk-UA",
                "English" => "en-GB",
                "Русский" => "ru-RU",
                _ => "en_GB",
            };
        }


    }
}
