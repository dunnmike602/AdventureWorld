using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DiagramDesigner.Symbols
{
    public class Connector : Control, INotifyPropertyChanged
    {
        // drag start point, relative to the DesignerCanvas
        private Point? _dragStartPoint;

        public ConnectorOrientation Orientation { get; set; }

        // center position of this Connector relative to the DesignerCanvas
        private Point _position;
        public Point Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        // the DesignerItem this Connector belongs to;
        // retrieved from DataContext, which is set in the
        // DesignerItem template
        private DesignerItem _parentDesignerItem;

        public DesignerItem ParentDesignerItem => _parentDesignerItem ?? (_parentDesignerItem = DataContext as DesignerItem);

        // keep track of connections that link to this connector
        private List<Connection> _connections;

        public List<Connection> Connections => _connections ?? (_connections = new List<Connection>());

        public Connector()
        {
            // fired when layout changes
            LayoutUpdated += ConnectorLayoutUpdated;            
        }

        // when the layout changes we update the position property
        private void ConnectorLayoutUpdated(object sender, EventArgs e)
        {
            var designer = GetDesignerCanvas(this);
            if (designer != null)
            {
                //get centre position of this Connector relative to the DesignerCanvas
                Position = TransformToAncestor(designer).Transform(new Point(Width / 2, Height / 2));
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            var canvas = GetDesignerCanvas(this);
            if (canvas != null && canvas.CanHaveConnectors)
            {
                // position relative to DesignerCanvas
                _dragStartPoint = e.GetPosition(canvas);
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                _dragStartPoint = null;
            }

            // but if mouse button is pressed and start point value is set we do have one
            if (_dragStartPoint.HasValue)
            {
                var connectionAdorner= CreateConnectionAdorner();

                if (connectionAdorner != null)
                {
                    e.Handled = true;
                }
            }
        }

        public ConnectorAdorner CreateConnectionAdorner()
        {
            var canvas = GetDesignerCanvas(this);

            if (canvas != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(canvas);

                if (adornerLayer != null)
                {
                    var adorner = new ConnectorAdorner(canvas, this);
                    adornerLayer.Add(adorner);
                    return adorner;
                }
            }

            return null;
        }

        internal ConnectorInfo GetInfo()
        {
            return new ConnectorInfo
            {
                DesignerItemLeft = Canvas.GetLeft(ParentDesignerItem),
                DesignerItemTop = Canvas.GetTop(ParentDesignerItem),
                DesignerItemSize = new Size(ParentDesignerItem.ActualWidth, ParentDesignerItem.ActualHeight),
                Orientation = Orientation,
                Position = Position
            };
        }

        // iterate through visual tree to get parent DesignerCanvas
        private DesignerCanvas GetDesignerCanvas(DependencyObject element)
        {
            while (element != null && !(element is DesignerCanvas))
            {
                element = VisualTreeHelper.GetParent(element);
            }

            return element as DesignerCanvas;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
