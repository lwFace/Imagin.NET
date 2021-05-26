﻿#pragma checksum "..\..\ControlView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CCC25F0B6596A10A220086A51E7E519858AD8749B342908DD6B4D0CDC5A65B34"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Demo;
using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Collections;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Effects;
using Imagin.Common.Globalization;
using Imagin.Common.Globalization.Engine;
using Imagin.Common.Globalization.Extensions;
using Imagin.Common.Globalization.Providers;
using Imagin.Common.Input;
using Imagin.Common.Interactivity;
using Imagin.Common.Linq;
using Imagin.Common.Markup;
using Imagin.Common.Math;
using Imagin.Common.Media;
using Imagin.Common.Media.Conversion;
using Imagin.Common.Media.Models;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using Imagin.Common.Text;
using Imagin.Common.Time;
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


namespace Demo {
    
    
    /// <summary>
    /// ControlView
    /// </summary>
    public partial class ControlView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\ControlView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox PART_ListBox;
        
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
            System.Uri resourceLocater = new System.Uri("/Demo;component/controlview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ControlView.xaml"
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
            this.PART_ListBox = ((System.Windows.Controls.ListBox)(target));
            
            #line 19 "..\..\ControlView.xaml"
            this.PART_ListBox.Loaded += new System.Windows.RoutedEventHandler(this.OnLoaded);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

