using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Labb5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserModel SelectedUser { get; set; }
        private ListBox SelectedListBox { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
