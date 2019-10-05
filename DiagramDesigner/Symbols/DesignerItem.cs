using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using AdventureLandCore.Extensions;
using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Controls;
using DiagramDesigner.Helpers;
using DiagramDesigner.Interfaces;
using DiagramDesigner.Symbols.Helpers;
using DiagramDesigner.ToolboxImpl;

namespace DiagramDesigner.Symbols
{
    //These attributes identify the types of the named parts that are used for templating
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class DesignerItem : ContentControl, ISelectable, IGroupable
    {
        public Guid ID { get; }

        public Guid ParentId
        {
            get => (Guid) GetValue(ParentIdProperty);
            set => SetValue(ParentIdProperty, value);
        }

        public static readonly DependencyProperty ParentIdProperty =
            DependencyProperty.Register("ParentId", typeof(Guid), typeof(DesignerItem));

        public ObjectType StencilObjectType
        {
            get => (ObjectType) GetValue(StencilObjectTypeProperty);
            set => SetValue(StencilObjectTypeProperty, value);
        }

        public static readonly DependencyProperty StencilObjectTypeProperty =
            DependencyProperty.Register("StencilObjectType", typeof(ObjectType), typeof(DesignerItem));

        public bool IsGroup
        {
            get => (bool) GetValue(IsGroupProperty);
            set => SetValue(IsGroupProperty, value);
        }

        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(DesignerItem));

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected",
                typeof(bool),
                typeof(DesignerItem),
                new FrameworkPropertyMetadata(false));

        // can be used to replace the default template for the DragThumb
        public static readonly DependencyProperty DragThumbTemplateProperty =
            DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        public static ControlTemplate GetDragThumbTemplate(UIElement element)
        {
            return (ControlTemplate) element.GetValue(DragThumbTemplateProperty);
        }

        public static void SetDragThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragThumbTemplateProperty, value);
        }

        // can be used to replace the default template for the ConnectorDecorator
        public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached("ConnectorDecoratorTemplate", typeof(ControlTemplate),
                typeof(DesignerItem) );

        public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate) element.GetValue(ConnectorDecoratorTemplateProperty);
        }

        public static void SetConnectorDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectorDecoratorTemplateProperty, value);
        }

        // while drag connection procedure is ongoing and the mouse moves over 
        // this item this value is true; if true the ConnectorDecorator is triggered
        // to be visible, see template
        public bool IsDragConnectionOver
        {
            get => (bool) GetValue(IsDragConnectionOverProperty);
            set => SetValue(IsDragConnectionOverProperty, value);
        }

        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register("IsDragConnectionOver",
                typeof(bool),
                typeof(DesignerItem),
                new FrameworkPropertyMetadata(false));

        static DesignerItem()
        {
            // set the key to reference the style for this control
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesignerItem),
                new FrameworkPropertyMetadata(typeof(DesignerItem)));
        }

        public DesignerItem(Guid id)
        {
            ID = id;
            Loaded += DesignerItem_Loaded;
        }

        public DesignerItem() : this(Guid.NewGuid())
        {
        }

        public DesignerCanvas ParentCanvas => Parent as DesignerCanvas;

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            // update selection
            if (VisualTreeHelper.GetParent(this) is DesignerCanvas designer)
            {
                SetPropertyGridToCurrentSelectedItem(ParentCanvas);

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

            e.Handled = false;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (!(e.Data.GetData(typeof(DragObject)) is DragObject dragObject) || string.IsNullOrEmpty(dragObject.Xaml))
            {
                return;
            }

            var content = XamlReader.Load(XmlReader.Create(new StringReader(dragObject.Xaml)));

            if (content == null)
            {
                return;
            }

            if ((dragObject.StencilObjectType == ObjectType.ConversationText &&
                 StencilObjectType == ObjectType.ConversationStart) ||
                (dragObject.StencilObjectType == ObjectType.ConversationResponse &&
                 StencilObjectType == ObjectType.ConversationText) ||
                (dragObject.StencilObjectType == ObjectType.ConversationText &&
                 StencilObjectType == ObjectType.ConversationResponse) ||
                (dragObject.StencilObjectType == ObjectType.Room && StencilObjectType == ObjectType.Room))
            {

                dragObject.ParentDesignerItem = this;
            }
            else if (dragObject.StencilObjectType == ObjectType.PlaceableObject || dragObject.StencilObjectType ==
                                                                           ObjectType.Container
                                                                           || dragObject.StencilObjectType ==
                                                                           ObjectType.Npc)
            {

                var newItem = new DesignerItem {Content = content, StencilObjectType = dragObject.StencilObjectType};

                // This new item will go in the Object Designer
                var objectDesigner = (DesignerCanvas) Application.Current.MainWindow.FindName("ObjectDesigner");
                objectDesigner.Children.Add(newItem);

                var desiredSize = dragObject.DesiredSize.Value;
                newItem.Width = desiredSize.Width;
                newItem.Height = desiredSize.Height;

                var newObject = AdventureObjectHelper.ProcessNewAdventureObject(newItem) as PlaceableObject;

                var parentObject = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(ID);

                // Only container objects and rooms can be parents
                if (parentObject is PlaceableObject parentPlaceableObject &&
                    parentPlaceableObject is Container || parentObject is Room)
                {
                    newObject.Parent = AdventureGameDesignerViewModel.Instance.FindObjectByGuid(ID);
                }

                e.Handled = true;
            }
        }

        public void SetPropertyGridToCurrentSelectedItem(DesignerCanvas designerCanvas)
        {
            if (designerCanvas.GetCanvasType() == CanvasType.Conversation)
            {
                ConversationDesigner.ConversationDesigner.Instance.ShowPropertiesForObject(ID);
            }
            else if (designerCanvas.GetCanvasType() == CanvasType.AdventureDesigner)
            {
                AdventureDesigner.Instance.ShowPropertiesForObject(ID);
            }
        }

        private void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (Template?.FindName("PART_ContentPresenter", this) is ContentPresenter contentPresenter &&
                VisualTreeHelper.GetChild(contentPresenter, 0) is UIElement contentVisual &&
                Template.FindName("PART_DragThumb", this) is DragThumb thumb &&
                GetDragThumbTemplate(contentVisual) is ControlTemplate template) thumb.Template = template;
        }

        public bool HasBeenPositioned { get; set; }

        public int CountConnectorsFromThis()
        {
            return ParentCanvas.Children.OfType<Connection>()
                .Count(connector => connector.Source.ParentDesignerItem.ID == ID);
        }

        public Connector GetConnector(string name)
        {
            var cd = Template.FindName("PART_ConnectorDecorator", this) as Control;

            var connectors = new List<Connector>();
            DependancyObjectHelper.GetConnectors(cd, connectors);

            return connectors.First(con => con.Name.IsEqualTo(name));
        }
    }
}