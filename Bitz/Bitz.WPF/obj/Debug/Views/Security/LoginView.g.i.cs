﻿#pragma checksum "..\..\..\..\Views\Security\LoginView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5F4FA6EBB46974B6EAD69458FAAF3865"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Converters;
using FirstFloor.ModernUI.Windows.Navigation;
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


namespace Bitz.Views.Security {
    
    
    /// <summary>
    /// LoginView
    /// </summary>
    public partial class LoginView : FirstFloor.ModernUI.Windows.Controls.ModernDialog, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\Views\Security\LoginView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox xUsername;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Views\Security\LoginView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox xPassword;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Views\Security\LoginView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button xLoginButton;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Views\Security\LoginView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button xCancelButton;
        
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
            System.Uri resourceLocater = new System.Uri("/Bitz;component/views/security/loginview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Security\LoginView.xaml"
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
            this.xUsername = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.xPassword = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.xLoginButton = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\Views\Security\LoginView.xaml"
            this.xLoginButton.Click += new System.Windows.RoutedEventHandler(this.xLoginButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.xCancelButton = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\Views\Security\LoginView.xaml"
            this.xCancelButton.Click += new System.Windows.RoutedEventHandler(this.xCancelButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

