﻿#pragma checksum "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "26E76E8E63359135B2C5FD443FFBC09B3504C1CF"
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
using Imagin.Common.Colour.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Data;
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


namespace Imagin.Common.Colour.Controls {
    
    
    /// <summary>
    /// ColorPlane
    /// </summary>
    public partial class ColorPlane : Imagin.Common.Colour.Controls.ColorViewBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Imagin.Common.Colour.Controls.ColorPlane PART_ColorPlane;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image PART_Image;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Imagin.Common.Colour.Controls.ColorSelection PART_SelectionA;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Imagin.Common.Colour.Controls.ColorSelection PART_SelectionB;
        
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
            System.Uri resourceLocater = new System.Uri("/Imagin.Common.WPF;component/colour/controls/colorpicker/colorplane.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
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
            this.PART_ColorPlane = ((Imagin.Common.Colour.Controls.ColorPlane)(target));
            return;
            case 2:
            this.PART_Image = ((System.Windows.Controls.Image)(target));
            
            #line 30 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
            this.PART_Image.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.OnMouseDown);
            
            #line default
            #line hidden
            
            #line 31 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
            this.PART_Image.MouseMove += new System.Windows.Input.MouseEventHandler(this.OnMouseMove);
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\..\..\Colour\Controls\ColorPicker\ColorPlane.xaml"
            this.PART_Image.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.OnMouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.PART_SelectionA = ((Imagin.Common.Colour.Controls.ColorSelection)(target));
            return;
            case 4:
            this.PART_SelectionB = ((Imagin.Common.Colour.Controls.ColorSelection)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

