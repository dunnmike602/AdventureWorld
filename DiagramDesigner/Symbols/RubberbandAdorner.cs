using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DiagramDesigner.Interfaces;

namespace DiagramDesigner.Symbols
{
    public class RubberbandAdorner : Adorner
    {
        private Point? startPoint;
        private Point? endPoint;
        private Pen rubberbandPen;

        private DesignerCanvas designerCanvas;

        public RubberbandAdorner(DesignerCanvas designerCanvas, Point? dragStartPoint)
            : base(designerCanvas)
        {
            this.designerCanvas = designerCanvas;
            startPoint = dragStartPoint;
            rubberbandPen = new Pen(Brushes.LightSlateGray, 1);
            rubberbandPen.DashStyle = new DashStyle(new double[] { 2 }, 1);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();

                endPoint = e.GetPosition(this);
                UpdateSelection();
                InvalidateVisual();
            }
            else
            {
                if (IsMouseCaptured) ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            // release mouse capture
            if (IsMouseCaptured) ReleaseMouseCapture();

            // remove this adorner from adorner layer
            var adornerLayer = AdornerLayer.GetAdornerLayer(designerCanvas);
            if (adornerLayer != null)
                adornerLayer.Remove(this);

            e.Handled = true;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            // without a background the OnMouseMove event would not be fired!
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (startPoint.HasValue && endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, rubberbandPen, new Rect(startPoint.Value, endPoint.Value));
        }

        private void UpdateSelection()
        {
            designerCanvas.SelectionService.ClearSelection();

            var rubberBand = new Rect(startPoint.Value, endPoint.Value);
            foreach (Control item in designerCanvas.Children)
            {
                var itemRect = VisualTreeHelper.GetDescendantBounds(item);
                var itemBounds = item.TransformToAncestor(designerCanvas).TransformBounds(itemRect);

                if (rubberBand.Contains(itemBounds))
                {
                    if (item is Connection)
                        designerCanvas.SelectionService.AddToSelection(item as ISelectable);
                    else
                    {
                        var di = item as DesignerItem;
                        if (di.ParentId == Guid.Empty)
                            designerCanvas.SelectionService.AddToSelection(di);
                    }
                }
            }
        }
    }
}
