﻿#pragma checksum "..\..\..\Controls\ScriptWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2B78C576EB8F4B95C887549B52C8333ED2572198"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DiagramDesigner.AdventureWorld.Domain;
using DiagramDesigner.Controls.Behaviours;
using FontAwesome.WPF;
using FontAwesome.WPF.Converters;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Search;
using Syncfusion;
using Syncfusion.UI.Xaml.Controls.DataPager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.RowFilter;
using Syncfusion.UI.Xaml.TreeGrid;
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


namespace DiagramDesigner.Controls {
    
    
    /// <summary>
    /// ScriptWindow
    /// </summary>
    public partial class ScriptWindow : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 45 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView ScriptExplorer;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem AllScriptsContainer;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdValidateAll;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.Windows.Tools.Controls.TabControlExt ScriptTab;
        
        #line default
        #line hidden
        
        
        #line 135 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdObjectBrowser;
        
        #line default
        #line hidden
        
        
        #line 145 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ScriptName;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal ICSharpCode.AvalonEdit.TextEditor CodeTextEditor;
        
        #line default
        #line hidden
        
        
        #line 155 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Errors;
        
        #line default
        #line hidden
        
        
        #line 162 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdRun;
        
        #line default
        #line hidden
        
        
        #line 164 "..\..\..\Controls\ScriptWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdValidate;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AdventureWorldDesigner;component/controls/scriptwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\ScriptWindow.xaml"
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
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\Controls\ScriptWindow.xaml"
            ((DiagramDesigner.Controls.ScriptWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ScriptWindowOnLoaded);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ScriptExplorer = ((System.Windows.Controls.TreeView)(target));
            
            #line 45 "..\..\..\Controls\ScriptWindow.xaml"
            this.ScriptExplorer.AddHandler(System.Windows.Controls.TreeViewItem.SelectedEvent, new System.Windows.RoutedEventHandler(this.ScriptExplorerOnSelected));
            
            #line default
            #line hidden
            return;
            case 5:
            this.AllScriptsContainer = ((System.Windows.Controls.TreeViewItem)(target));
            return;
            case 6:
            this.CmdValidateAll = ((System.Windows.Controls.Button)(target));
            
            #line 76 "..\..\..\Controls\ScriptWindow.xaml"
            this.CmdValidateAll.Click += new System.Windows.RoutedEventHandler(this.CmdCompileAllClick);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ScriptTab = ((Syncfusion.Windows.Tools.Controls.TabControlExt)(target));
            return;
            case 8:
            
            #line 98 "..\..\..\Controls\ScriptWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenFileClick);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 101 "..\..\..\Controls\ScriptWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SaveFileClick);
            
            #line default
            #line hidden
            return;
            case 10:
            this.CmdObjectBrowser = ((System.Windows.Controls.Button)(target));
            
            #line 135 "..\..\..\Controls\ScriptWindow.xaml"
            this.CmdObjectBrowser.Click += new System.Windows.RoutedEventHandler(this.CmdObjectBrowserClick);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 138 "..\..\..\Controls\ScriptWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.QuickSearchClick);
            
            #line default
            #line hidden
            return;
            case 12:
            this.ScriptName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 13:
            this.CodeTextEditor = ((ICSharpCode.AvalonEdit.TextEditor)(target));
            
            #line 150 "..\..\..\Controls\ScriptWindow.xaml"
            this.CodeTextEditor.TextChanged += new System.EventHandler(this.CodeTextEditorOnTextChanged);
            
            #line default
            #line hidden
            return;
            case 14:
            this.Errors = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.CmdRun = ((System.Windows.Controls.Button)(target));
            
            #line 162 "..\..\..\Controls\ScriptWindow.xaml"
            this.CmdRun.Click += new System.Windows.RoutedEventHandler(this.CmdRunClick);
            
            #line default
            #line hidden
            return;
            case 16:
            this.CmdValidate = ((System.Windows.Controls.Button)(target));
            
            #line 164 "..\..\..\Controls\ScriptWindow.xaml"
            this.CmdValidate.Click += new System.Windows.RoutedEventHandler(this.CmdCompileClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 2:
            
            #line 35 "..\..\..\Controls\ScriptWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CmdExpandClick);
            
            #line default
            #line hidden
            break;
            case 3:
            
            #line 38 "..\..\..\Controls\ScriptWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CmdCollapseClick);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

