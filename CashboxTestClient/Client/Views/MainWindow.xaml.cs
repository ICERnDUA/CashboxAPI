using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }


        //GAVNO
        private void CashboxWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ((INotifyCollectionChanged)CommandListBox.ItemsSource).CollectionChanged +=
     (s, e) =>
     {
         if (e.Action ==
             System.Collections.Specialized.NotifyCollectionChangedAction.Add)
         {
             CommandListBox.ScrollIntoView(CommandListBox.Items[CommandListBox.Items.Count - 1]);
         }
     };
        }
    }
}
