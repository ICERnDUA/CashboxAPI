using Client.Events;
using Client.Interfaces;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class ServerConnectSettingsViewModel : BindableBase
    {
        INetworkDAO _networkDAO;
        IEventAggregator _eventAggregator;
        public ServerConnectSettingsViewModel() 
        {
            _networkDAO = ContainerLocator.Container.Resolve<NetworkDAO>();
            _eventAggregator = ContainerLocator.Container.Resolve<IEventAggregator>();
            SaveSettingsCommand = new DelegateCommand(OnSaveSettings);
            CloseSettingsCommand = new DelegateCommand(OnClose);
            OnLoadedCommand = new DelegateCommand(OnLoaded);
        }
        private string _serverIpAddress;
        public string ServerIpAddress
        {
            get { return _serverIpAddress; }
            set { SetProperty(ref _serverIpAddress, value); }
        }
        private int _serverPort;
        public int ServerPort
        {
            get { return _serverPort; }
            set { SetProperty(ref _serverPort, value); }
        }
        private bool _enabled = false;
        public bool Enabled
        {
            get { return _enabled; }
            set { SetProperty(ref _enabled, value); }
        }
        public  DelegateCommand OnLoadedCommand { get; private set; }
        private void OnLoaded()
        {
            var networkData = _networkDAO.ReadNetworkData();
            if (networkData != null)
            {
                Enabled = true;
                ServerIpAddress = networkData.IpAddress;
                ServerPort = networkData.Port;
            }
            else
            {
                Enabled = false;
                ServerIpAddress = "localhost";
                ServerPort = 5000;
            }
        }
       
        public DelegateCommand SaveSettingsCommand { get; private set; }
        private void OnSaveSettings()
        {
            _networkDAO.SaveNetworkData(new NetworkConnections() { IpAddress = ServerIpAddress, Port = ServerPort });
            _eventAggregator.GetEvent<NetworkSettingsCloseEvent>().Publish();
        }
        public DelegateCommand CloseSettingsCommand { get; private set; }
        private void OnClose()
        {
             
            _eventAggregator.GetEvent<NetworkSettingsCloseEvent>().Publish();
        }
    }
}
