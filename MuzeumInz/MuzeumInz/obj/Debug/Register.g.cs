﻿#pragma checksum "..\..\Register.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "33676647B3574E630377DF31A197CC8A7A13918DA25DC6BB05FBE4CFE9FBF068"
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
    /// Register
    /// </summary>
    public partial class Register : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Register1;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox register_loginTxt;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button register_loginBtn;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label exitClick;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button register_signBtn;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Register.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox register_passTxt;
        
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
            System.Uri resourceLocater = new System.Uri("/MuzeumInz;component/register.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Register.xaml"
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
            this.Register1 = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.register_loginTxt = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.register_loginBtn = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\Register.xaml"
            this.register_loginBtn.Click += new System.Windows.RoutedEventHandler(this.register_loginBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.exitClick = ((System.Windows.Controls.Label)(target));
            
            #line 17 "..\..\Register.xaml"
            this.exitClick.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.exitClick_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.register_signBtn = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\Register.xaml"
            this.register_signBtn.Click += new System.Windows.RoutedEventHandler(this.register_signBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.register_passTxt = ((System.Windows.Controls.PasswordBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
