﻿#pragma checksum "..\..\MainPanel.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "BFB206F3847363B74437B3355B4B44B98AD02701AD7AE3F8CDF7C850D8ED05CB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

using MuzeumInz;
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


namespace MuzeumInz {
    
    
    /// <summary>
    /// MainPanel
    /// </summary>
    public partial class MainPanel : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\MainPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExhibitionsBtn2;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\MainPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExhibitionsBtn1;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\MainPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExhibitsHistoryBtn;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\MainPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InventoryBtn;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\MainPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReportsBtn1;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\MainPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LogoutBtn;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\MainPanel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label exitClick;
        
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
            System.Uri resourceLocater = new System.Uri("/MuzeumInz;component/mainpanel.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainPanel.xaml"
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
            
            #line 8 "..\..\MainPanel.xaml"
            ((MuzeumInz.MainPanel)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Move_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ExhibitionsBtn2 = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\MainPanel.xaml"
            this.ExhibitionsBtn2.Click += new System.Windows.RoutedEventHandler(this.ExhibitionsBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ExhibitionsBtn1 = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\MainPanel.xaml"
            this.ExhibitionsBtn1.Click += new System.Windows.RoutedEventHandler(this.ExhibitsBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ExhibitsHistoryBtn = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\MainPanel.xaml"
            this.ExhibitsHistoryBtn.Click += new System.Windows.RoutedEventHandler(this.HistoryBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.InventoryBtn = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\MainPanel.xaml"
            this.InventoryBtn.Click += new System.Windows.RoutedEventHandler(this.Inventory_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ReportsBtn1 = ((System.Windows.Controls.Button)(target));
            
            #line 85 "..\..\MainPanel.xaml"
            this.ReportsBtn1.Click += new System.Windows.RoutedEventHandler(this.Reports_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.LogoutBtn = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\MainPanel.xaml"
            this.LogoutBtn.Click += new System.Windows.RoutedEventHandler(this.LogoutBtn_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.exitClick = ((System.Windows.Controls.Label)(target));
            
            #line 113 "..\..\MainPanel.xaml"
            this.exitClick.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.exitClick_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

