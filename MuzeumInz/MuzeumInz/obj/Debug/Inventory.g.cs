﻿#pragma checksum "..\..\Inventory.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "09AB0F34D774F3AB49712FAA0A80FEA925F6429F406FD2A865816742399935E6"
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
    /// Inventory
    /// </summary>
    public partial class Inventory : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\Inventory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExhibitionsBtn2;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\Inventory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExhibitionsBtn1;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\Inventory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ExhibitsHistoryBtn;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\Inventory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button InventoryBtn;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\Inventory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ReportsBtn1;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\Inventory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LogoutBtn;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\Inventory.xaml"
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
            System.Uri resourceLocater = new System.Uri("/MuzeumInz;component/inventory.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Inventory.xaml"
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
            
            #line 8 "..\..\Inventory.xaml"
            ((MuzeumInz.Inventory)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Move_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ExhibitionsBtn2 = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\Inventory.xaml"
            this.ExhibitionsBtn2.Click += new System.Windows.RoutedEventHandler(this.ExhibitionsBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ExhibitionsBtn1 = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\Inventory.xaml"
            this.ExhibitionsBtn1.Click += new System.Windows.RoutedEventHandler(this.ExhibitsBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ExhibitsHistoryBtn = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\Inventory.xaml"
            this.ExhibitsHistoryBtn.Click += new System.Windows.RoutedEventHandler(this.HistoryBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.InventoryBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.ReportsBtn1 = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.LogoutBtn = ((System.Windows.Controls.Button)(target));
            
            #line 102 "..\..\Inventory.xaml"
            this.LogoutBtn.Click += new System.Windows.RoutedEventHandler(this.LogoutBtn_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.exitClick = ((System.Windows.Controls.Label)(target));
            
            #line 113 "..\..\Inventory.xaml"
            this.exitClick.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.exitClick_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

