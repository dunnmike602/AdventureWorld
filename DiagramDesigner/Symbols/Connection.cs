using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DiagramDesigner.Interfaces;

namespace DiagramDesigner.Symbols
{
    public class Connection : Control, ISelectable, INotifyPropertyChanged
    {
        private Adorner _connectionAdorner;

        public Guid ID { get; set; }

        // source connector
        private Connector _source;

        public Connector Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    if (_source != null)
                    {
                        _source.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPositionChanged);
                        _source.Connections.Remove(this);
                    }

                    _source = value;

                    if (_source != null)
                    {
                        _source.Connections.Add(this);
                        _source.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPositionChanged);
                    }

                    UpdatePathGeometry();
                }
            }
        }

        // sink connector
        private Connector sink;

        public Connector Sink
        {
            get => sink;
            set
            {
                if (sink != value)
                {
                    if (sink != null)
                    {
                        sink.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPositionChanged);
                        sink.Connections.Remove(this);
                    }

                    sink = value;

                    if (sink != null)
                    {
                        sink.Connections.Add(this);
                        sink.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPositionChanged);
                    }

                    UpdatePathGeometry();
                }
            }
        }

        // connection path geometry
        private PathGeometry pathGeometry;

        public PathGeometry PathGeometry
        {
            get => pathGeometry;
            set
            {
                if (pathGeometry != value)
                {
                    pathGeometry = value;
                    UpdateAnchorPosition();
                    OnPropertyChanged("PathGeometry");
                }
            }
        }

        // between source connector position and the beginning 
        // of the path geometry we leave some space for visual reasons; 
        // so the anchor position source really marks the beginning 
        // of the path geometry on the source side
        private Point anchorPositionSource;

        public Point AnchorPositionSource
        {
            get => anchorPositionSource;
            set
            {
                if (anchorPositionSource != value)
                {
                    anchorPositionSource = value;
                    OnPropertyChanged("AnchorPositionSource");
                }
            }
        }

        // slope of the path at the anchor position
        // needed for the rotation angle of the arrow
        private double anchorAngleSource = 0;

        public double AnchorAngleSource
        {
            get => anchorAngleSource;
            set
            {
                if (anchorAngleSource != value)
                {
                    anchorAngleSource = value;
                    OnPropertyChanged("AnchorAngleSource");
                }
            }
        }

        // analogue to source side
        private Point anchorPositionSink;

        public Point AnchorPositionSink
        {
            get => anchorPositionSink;
            set
            {

                anchorPositionSink = value;
                OnPropertyChanged("AnchorPositionSink");
            }
        }

        // analogue to source side
        private double anchorAngleSink = 0;

        public double AnchorAngleSink
        {
            get => anchorAngleSink;
            set
            {
                if (anchorAngleSink != value)
                {
                    anchorAngleSink = value;
                    OnPropertyChanged("AnchorAngleSink");
                }
            }
        }

        private ArrowSymbol sourceArrowSymbol = ArrowSymbol.None;

        public ArrowSymbol SourceArrowSymbol
        {
            get => sourceArrowSymbol;
            set
            {
                if (sourceArrowSymbol != value)
                {
                    sourceArrowSymbol = value;
                    OnPropertyChanged("SourceArrowSymbol");
                }
            }
        }

        public ArrowSymbol sinkArrowSymbol = ArrowSymbol.Arrow;

        public ArrowSymbol SinkArrowSymbol
        {
            get => sinkArrowSymbol;
            set
            {
                if (sinkArrowSymbol != value)
                {
                    sinkArrowSymbol = value;
                    OnPropertyChanged("SinkArrowSymbol");
                }
            }
        }

        // specifies a point at half path length
        private Point labelPosition;

        public Point LabelPosition
        {
            get => labelPosition;
            set
            {
                if (labelPosition != value)
                {
                    labelPosition = value;
                    OnPropertyChanged("LabelPosition");
                }
            }
        }

        // if connected, the ConnectionAdorner becomes visible
        private bool isSelected;
      
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (isSelected)
                    {
                        ShowAdorner();
                    }
                    else
                    {
                        HideAdorner();
                    }
                }
            }
        }

        public Connection(Connector source, Connector sink)
        {
            ID = Guid.NewGuid();
            Source = source;
            Sink = sink;
            //  Unloaded += new RoutedEventHandler(Connection_Unloaded);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            // usual selection business
            if (VisualTreeHelper.GetParent(this) is DesignerCanvas designer)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    if (IsSelected)
                    {
                        designer.SelectionService.RemoveFromSelection(this);
                    }
                    else
                    {
                        designer.SelectionService.AddToSelection(this);
                    }
                }
                else if (!IsSelected)
                {
                    designer.SelectionService.SelectItem(this);
                }

                Focus();
            }

            SetPropertyGridToCurrentSelectedItem();

            e.Handled = true;
        }

        void OnConnectorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            // whenever the 'Position' property of the source or sink Connector 
            // changes we must update the connection path geometry
            if (e.PropertyName.Equals("Position"))
            {
                UpdatePathGeometry();
            }
        }

        private void UpdatePathGeometry()
        {
            if (Source != null && Sink != null)
            {
                var geometry = new PathGeometry();
                var linePoints = OrthogonalPathFinder.GetConnectionLine(Source.GetInfo(), Sink.GetInfo(), true);
                if (linePoints.Count > 0)
                {
                    //  var figure = new PathFigure {StartPoint = linePoints[0]};
                    // linePoints.Remove(linePoints[0]);
                    // figure.Segments.Add(new PolyLineSegment(linePoints, true));
                    // geometry.Figures.Add(figure);

                    //    PathGeometry = geometry;

                    var rcp = new RoundedCornersPolygon {ArcRoundness = 20};
                    foreach (Point p in linePoints)
                    {
                        rcp.Points.Add(p);
                    }

                    this.PathGeometry = (PathGeometry)rcp.Data;
                }
            }
        }

        private void UpdateAnchorPosition()
        {
            // the PathGeometry.GetPointAtFractionLength method gets the point and a tangent vector 
            // on PathGeometry at the specified fraction of its length
            PathGeometry.GetPointAtFractionLength(0, out var pathStartPoint, out var pathTangentAtStartPoint);
            PathGeometry.GetPointAtFractionLength(1, out var pathEndPoint, out var pathTangentAtEndPoint);
            PathGeometry.GetPointAtFractionLength(0, out var pathMidPoint, out _);

            // get angle from tangent vector
            AnchorAngleSource =
                Math.Atan2(-pathTangentAtStartPoint.Y, -pathTangentAtStartPoint.X) * (180 / Math.PI);
            AnchorAngleSink = Math.Atan2(pathTangentAtEndPoint.Y, pathTangentAtEndPoint.X) * (180 / Math.PI);

            // add some margin on source and sink side for visual reasons only
            pathStartPoint.Offset(-pathTangentAtStartPoint.X * 1.5, -pathTangentAtStartPoint.Y * 1.5);
            pathEndPoint.Offset(pathTangentAtEndPoint.X * 1.5, pathTangentAtEndPoint.Y * 1.5);

            AnchorPositionSource = pathStartPoint;
            AnchorPositionSink = pathEndPoint;
            LabelPosition = pathMidPoint;
        }

        public void SetPropertyGridToCurrentSelectedItem()
        {
            AdventureDesigner.Instance.ShowPropertiesForObject(ID);
        }

        private void ShowAdorner()
        {
            // the ConnectionAdorner is created once for each Connection
            if (_connectionAdorner == null)
            {
                var designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

                var adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    _connectionAdorner = new ConnectionAdorner(designer, this);
                    adornerLayer.Add(_connectionAdorner);
                }
            }

            _connectionAdorner.Visibility = Visibility.Visible;
        }

        internal void HideAdorner()
        {
            if (_connectionAdorner != null)
            {
                _connectionAdorner.Visibility = Visibility.Collapsed;
            }
        }

        public void CleanUpConnection()
        {
            // remove event handler
            Source = null;
            Sink = null;
            // remove adorner
            if (_connectionAdorner != null)
            {
                var designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

                var adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(_connectionAdorner);
                    _connectionAdorner = null;
                }
            }
        }

        // we could use DependencyProperties as well to inform others of property changes
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
