﻿#pragma checksum "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4A5B893D3ACF6114FD797FF4B688C5E4CB15ADB4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Imagin.Common;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Media.Controls;
using Imagin.Common.Media.Models;
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


namespace Imagin.Common.Media.Controls {
    
    
    /// <summary>
    /// ColorPicker
    /// </summary>
    public partial class ColorPicker : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Imagin.Common.Media.Controls.ColorPicker PART_ColorPicker;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox PART_Models;
        
        #line default
        #line hidden
        
        
        #line 226 "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl PART_CMYK;
        
        #line default
        #line hidden
        
        
        #line 306 "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle PART_OldRectangle;
        
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
            System.Uri resourceLocater = new System.Uri("/Imagin.Common.WPF;component/media/controls/colorpicker/colorpicker.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.PART_ColorPicker = ((Imagin.Common.Media.Controls.ColorPicker)(target));
            return;
            case 2:
            this.PART_Models = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.PART_CMYK = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 4:
            this.PART_OldRectangle = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 5:
            
            #line 323 "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnRevert);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 327 "..\..\..\..\..\Media\Controls\ColorPicker\ColorPicker.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnSave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

