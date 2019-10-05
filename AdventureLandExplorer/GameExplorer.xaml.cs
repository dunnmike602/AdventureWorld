using System.Windows;
using SharedControls;

namespace AdventureLandExplorer
{
    public partial class GameExplorer : Window
    {
        public GameExplorer()
        {
            InitializeComponent();
            
            SetupErrors();
        }

        private void SetupErrors()
        {
            Application.Current.DispatcherUnhandledException += CurrentDispatcherUnhandledException;
        }

        private void CurrentDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            TaskDialogService.ShowApplicationError(this, e.Exception);
           
            e.Handled = true;
        }
    }
}
