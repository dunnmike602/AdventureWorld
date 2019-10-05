using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using DiagramDesigner.Symbols.Helpers;

namespace DiagramDesigner.ToolboxImpl
{
    // Represents a selectable item in the Toolbox/>.
    public class ToolboxItem : ContentControl
    {
        // caches the start point of the drag operation
        private Point? _dragStartPoint;

        static ToolboxItem()
        {
            // set the key to reference the style for this control
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ToolboxItem), new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            _dragStartPoint = new Point?(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _dragStartPoint = null;
            }

            var stencil = (Grid) Content;

            var roomDesigner = (Symbols.DesignerCanvas)Application.Current.MainWindow.FindName("RoomDesigner");
            var objectDesigner = (Symbols.DesignerCanvas)Application.Current.MainWindow.FindName("ObjectDesigner");

            if(stencil.ToolTip.ToString() == "Room")
            {
                roomDesigner.AllowDrop = true;
                objectDesigner.AllowDrop = false;
            }
            else if (stencil.ToolTip.ToString() == "Object" || stencil.ToolTip.ToString() == "Container" || stencil.ToolTip.ToString() == "Npc")
            {
                roomDesigner.AllowDrop = true;
                objectDesigner.AllowDrop = true;
            }

            if (_dragStartPoint.HasValue)
            {
                // XamlWriter.Save() has limitations in exactly what is serialized,
                // see SDK documentation; short term solution only;
                var xamlString = XamlWriter.Save(Content);

                var dataObject = new DragObject
                    {
                        Xaml = xamlString,
                        StencilObjectType = AdventureObjectHelper.ConvertName(stencil.ToolTip.ToString()),
                };

                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);

                e.Handled = true;
            }
        }
    }
}
