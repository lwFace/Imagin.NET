   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri-   'Imagin.Common.Controls.MaskedToggleMenu �� 
TargetType �� BasedOn �� Property �� Value6   0Imagin.Common.Converters.DoubleSizeTypeConverter   Brush.Image/   )Imagin.Common.Controls.MaskedToggleButton  
Brush.Text �� Triggers P� Property P� Value  Brush.Image.Checked " Brush.Image.Checked.Foreground	 ]� 
Conditions
 �� Property �� Value  Brush.Image.MouseOver  Brush.Image.Pressed�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+%�  '        7��� 5
      "  64   8   .�5   
   ��  � ��$ 0q�6,   �� 5   
    � ��
 ���� 6'   �� 5   
    �@ 
ButtonSize$ 12,12 6'   �� 5   
    �@ ButtonSource$P Jpack://application:,,,/Imagin.Common.WPF;component/Images/TriangleDown.png��6)   �� 5   
    �@ ButtonSourceColor# �   6.   �� 5   
    �@ ContentMargin$	 5,0q�6*   �� 5   
    � ��# �  6'   �� 5   
    � ��$ 0q�6$   �� 5   
    �@ SourceColor# �   6(    5   
   P� 5       �@ MenuVisibility$ Visible=�60   . �5      ��  � ��# �  6/   �� 5       �@ ButtonSourceColor# �  66   �� 5       � ��# �  6/   �� 5       �@ SourceColor# �  60   5      ]� 5      	 5      �� 5      
 �@J�IsMouseOver . 67   �� 5      
 �@d�	IsChecked$ FalseY�65   ./�5       ��  �@ SourceColor# �  60   5!      ]� 5"      	 5#      �� 5$      
 �@ MenuVisibility$ 	Collapsed=�6:   �� 5%      
 �@d�	IsChecked$
 TrueY�65   ./�5'      ��  �@ SourceColor# �  60   5(      P� 5)       � �� . . �5*      ��  �@ SourceColor# �  60   5+      5-      5.      