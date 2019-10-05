#pragma checksum "..\..\..\Controls\MultiTextEditorView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0098DF8DEF878B16FC719E7F6E90588D9A7236E2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Syncfusion;
using Syncfusion.Windows;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DiagramDesigner.Controls
{


    /// <summary>
    /// MultiTextEditorView
    /// </summary>
    public partial class MultiTextEditorView : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AdventureWorldDesigner;component/controls/multitexteditorview.xaml", System.UriKind.Relative);

#line 1 "..\..\..\Controls\MultiTextEditorView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:

#line 8 "..\..\..\Controls\MultiTextEditorView.xaml"
                    ((DiagramDesigner.Controls.MultiTextEditorView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControlLoaded);

#line default
#line hidden
                    return;
                case 2:
                    this.AddButton = ((System.Windows.Controls.Button)(target));

#line 23 "..\..\..\Controls\MultiTextEditorView.xaml"
                    this.AddButton.Click += new System.Windows.RoutedEventHandler(this.AddButtonClick);

#line default
#line hidden
                    return;
                case 3:

#line 24 "..\..\..\Controls\MultiTextEditorView.xaml"
                    ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OkButtonClick);

#line default
#line hidden
                    return;
                case 4:
                    this.CancelButton = ((System.Windows.Controls.Button)(target));
                    return;
                case 5:
                    this.TextTabControl = ((Syncfusion.Windows.Tools.Controls.TabControlExt)(target));

#line 29 "..\..\..\Controls\MultiTextEditorView.xaml"
                    this.TextTabControl.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.TabControlSelectionChanged);

#line default
#line hidden

#line 29 "..\..\..\Controls\MultiTextEditorView.xaml"
                    this.TextTabControl.TabClosed += new Syncfusion.Windows.Tools.Controls.TabClosedEventHandler(this.TextTabControlOnTabClosed);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }
    }
}

