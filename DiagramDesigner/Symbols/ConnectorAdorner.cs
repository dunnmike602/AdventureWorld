﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DiagramDesigner.Symbols.Helpers;

namespace DiagramDesigner.Symbols
{
    public class ConnectorAdorner : Adorner
    {
        private PathGeometry _pathGeometry;
        private readonly DesignerCanvas _designerCanvas;
        private readonly Connector _sourceConnector;
        private readonly Pen _drawingPen;
        private DesignerItem _hitDesignerItem;

        private DesignerItem HitDesignerItem
        {
            get => _hitDesignerItem;
            set
            {
                if (_hitDesignerItem != value)
                {
                    if (_hitDesignerItem != null)
                        _hitDesignerItem.IsDragConnectionOver = false;

                    _hitDesignerItem = value;

                    if (_hitDesignerItem != null)
                        _hitDesignerItem.IsDragConnectionOver = true;
                }
            }
        }

        private Connector _hitConnector;

        private Connector HitConnector
        {
            get => _hitConnector;
            set
            {
                if (_hitConnector != value)
                {
                    _hitConnector = value;
                }
            }
        }

        public ConnectorAdorner(DesignerCanvas designer, Connector sourceConnector)
            : base(designer)
        {
            _designerCanvas = designer;
            _sourceConnector = sourceConnector;
            _drawingPen = new Pen(Brushes.LightSlateGray, 1) {LineJoin = PenLineJoin.Round};
            Cursor = Cursors.Cross;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (HitConnector != null)
            {
                var sourceConnector = _sourceConnector;
                var sinkConnector = HitConnector;

                if (AdventureObjectHelper.CanConnect(sourceConnector.ParentDesignerItem, sinkConnector.ParentDesignerItem))
                {
                    var newConnection = new Connection(sourceConnector, sinkConnector);

                    Panel.SetZIndex(newConnection, _designerCanvas.Children.Count);
                    _designerCanvas.Children.Add(newConnection);

                    if (_designerCanvas.GetCanvasType() == CanvasType.AdventureDesigner)
                    {
                        ConnectionLinkerHelper.ProcessExit(sourceConnector, sinkConnector, newConnection);
                    }
                    else if (_designerCanvas.GetCanvasType() == CanvasType.Conversation)
                    {
                        ConnectionLinkerHelper.ProcessConversationLink(sourceConnector, sinkConnector);
                    }
                }
            }

            if (HitDesignerItem != null)
            {
                HitDesignerItem.IsDragConnectionOver = false;
            }

            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }

            var adornerLayer = AdornerLayer.GetAdornerLayer(_designerCanvas);

            adornerLayer?.Remove(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                {
                    CaptureMouse();
                }

                HitTesting(e.GetPosition(this));
                _pathGeometry = GetPathGeometry(e.GetPosition(this));
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, _drawingPen, _pathGeometry);

            // without a background the OnMouseMove event would not be fired
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        private PathGeometry GetPathGeometry(Point position)
        {
            var geometry = new PathGeometry();

            ConnectorOrientation targetOrientation;
            if (HitConnector != null)
                targetOrientation = HitConnector.Orientation;
            else
                targetOrientation = ConnectorOrientation.None;

            var pathPoints = OrthogonalPathFinder.GetConnectionLine(_sourceConnector.GetInfo(), position, targetOrientation);

            if (pathPoints.Count > 0)
            {
                //  var figure = new PathFigure {StartPoint = pathPoints[0]};
                //  pathPoints.Remove(pathPoints[0]);
                // figure.Segments.Add(new PolyLineSegment(pathPoints, true));
                // geometry.Figures.Add(figure);

                var rcp = new RoundedCornersPolygon {ArcRoundness = 20};
                foreach (var p in pathPoints)
                {
                    rcp.Points.Add(p);
                }

                geometry = (PathGeometry)rcp.Data;
            }

            return geometry;
        }

        private void HitTesting(Point hitPoint)
        {
            var hitConnectorFlag = false;

            var hitObject = _designerCanvas.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                   hitObject != _sourceConnector.ParentDesignerItem &&
                   hitObject.GetType() != typeof(DesignerCanvas))
            {
                if (hitObject is Connector)
                {
                    HitConnector = hitObject as Connector;
                    hitConnectorFlag = true;
                }

                if (hitObject is DesignerItem)
                {
                    HitDesignerItem = hitObject as DesignerItem;
                    if (!hitConnectorFlag)
                        HitConnector = null;
                    return;
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            HitConnector = null;
            HitDesignerItem = null;
        }
    }
}
