using System.Windows;

namespace UndoSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DataModel Model => (DataModel)DataContext;
        private DebugModel DebugModel => (DebugModel)DebugLog.DataContext;

        public MainWindow()
        {
            InitializeComponent();
            Model.DebugModel = DebugModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.DataGrid.RemoveAt(this.Table.SelectedIndex);
        }

        private void Undo_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            DebugModel.Undo();
        }

        private void Undo_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DebugModel.UndoStackSize > 0;
        }

        private void Redo_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            DebugModel.Redo();
        }

        private void Redo_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DebugModel.RedoStackSize > 0;
        }        
    }
}
