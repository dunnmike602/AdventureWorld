using System.Windows;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Controls.Helpers;
using SharedControls;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;

namespace DiagramDesigner.Controls
{
    public partial class ConversationEditor : ITypeEditor
    {
        private Npc CurrentNpc { get; set; }

        public ConversationEditor()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AdventureDesigner.Instance.GetProjectSupportDirName()))
            {
                TaskDialogService.ShowWarning(Parent as Window, "You must save the project before you can add Conversations.");
                return;
            }

            ConversationHelper.OpenEditor(CurrentNpc);
        }

        public FrameworkElement ResolveEditor(PropertyItem propertyItem)
        {
            CurrentNpc = (Npc) propertyItem.Instance;
            
            return this;
        }

    }
}
