using System.Windows;

namespace UndoSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ViewModel Model => (ViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.DataGrid.RemoveAt(this.Table.SelectedIndex);
        }
    }
}
