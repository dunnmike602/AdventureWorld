﻿#pragma checksum "..\..\ObjectBrowser.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "16C6723DC8D74AD26265FD03DDD14C9B07D91B63"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FontAwesome.WPF;
using FontAwesome.WPF.Converters;
using ReflectionHelper.Core;
using Syncfusion;
using Syncfusion.Windows;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace MLDComputing.ObjectBrowser {
    
    
    /// <summary>
    /// ObjectBrowser
    /// </summary>
    public partial class ObjectBrowser : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 35 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Progress;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar LoadProgress;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Syncfusion.Windows.Tools.Controls.ComboBoxAdv SearchObjectBrowser;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdSearch;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CmdCancel;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView SearchExplorer;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView ObjectExplorer;
        
        #line default
        #line hidden
        
        
        #line 179 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView DetailsPanel;
        
        #line default
        #line hidden
        
        
        #line 205 "..\..\ObjectBrowser.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SummaryPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/MLDComputing.ObjectBrowser;component/objectbrowser.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ObjectBrowser.xaml"
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
            
            #line 13 "..\..\ObjectBrowser.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MemberViewMenuItemOnClick);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 20 "..\..\ObjectBrowser.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchMenuItemOnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 27 "..\..\ObjectBrowser.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ExplorerMenuItemOnClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Progress = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.LoadProgress = ((System.Windows.Controls.ProgressBar)(target));
            return;
            case 6:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.SearchObjectBrowser = ((Syncfusion.Windows.Tools.Controls.ComboBoxAdv)(target));
            return;
            case 8:
            this.CmdSearch = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\ObjectBrowser.xaml"
            this.CmdSearch.Click += new System.Windows.RoutedEventHandler(this.CmdSearchOnClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.CmdCancel = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\ObjectBrowser.xaml"
            this.CmdCancel.Click += new System.Windows.RoutedEventHandler(this.CmdCancelOnClick);
            
            #line default
            #line hidden
            return;
            case 10:
            this.SearchExplorer = ((System.Windows.Controls.TreeView)(target));
            
            #line 73 "..\..\ObjectBrowser.xaml"
            this.SearchExplorer.AddHandler(System.Windows.Controls.TreeViewItem.SelectedEvent, new System.Windows.RoutedEventHandler(this.SearchExplorerOnSelected));
            
            #line default
            #line hidden
            return;
            case 12:
            this.ObjectExplorer = ((System.Windows.Controls.TreeView)(target));
            
            #line 133 "..\..\ObjectBrowser.xaml"
            this.ObjectExplorer.AddHandler(System.Windows.Controls.TreeViewItem.SelectedEvent, new System.Windows.RoutedEventHandler(this.ObjectExplorerOnSelected));
            
            #line default
            #line hidden
            return;
            case 14:
            this.DetailsPanel = ((System.Windows.Controls.ListView)(target));
            
            #line 178 "..\..\ObjectBrowser.xaml"
            this.DetailsPanel.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.DetailsPanelOnSelectionChanged);
            
            #line default
            #line hidden
            return;
            case 15:
            this.SummaryPanel = ((System.Windows.Controls.TextBlock)(target));
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
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 11:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.TreeViewItem.SelectedEvent;
            
            #line 76 "..\..\ObjectBrowser.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.EventSetterOnHandler);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 13:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.TreeViewItem.SelectedEvent;
            
            #line 136 "..\..\ObjectBrowser.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.EventSetterOnHandler);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

