   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri �� 
TargetType �� BasedOn �� Property   Brush.CheckBox.Background �� Value  Brush.CheckBox.Border  
Brush.Text �� 
TargetType � 	Resources:   4Imagin.Common.Converters.ObjectToVisibilityConverter  Key � ColumnDefinitions	 ��Name  Brush.CheckBox.Tick.Fill
 ��Name �� Mode  ObjectToVisibilityConverter �� 	Converter �� RelativeSource �� Triggers P� Value P� Property �� 
TargetName �� Triggers�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      5
Converters&clr-namespace:Imagin.Common.Converters  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+%�  '��       7�7  �� 5
      " ��6&   8   .�5   
   ��  � ��# �   6'   �� 5   
    � ��# �  6(   �� 5   
    � ��$ 1q�6,   �� 5   
    � }� .  6)   �� 5   
    � ��# �  6'   �� 5   
    � ��$ 13(�6%   �� 5   
    � ��$
 Left=�67   �� 5   
    � ��$ Center=�65   �� 5   
    � �� 5      �� 5      " ��6"    5      ��  5      ObjectToVisibilityConverter 6A   5      .R�5      �� ._�5      � 
������  5      �� 5   "   $
��Auto �63   �� 5   "   $��* �63   .+�5       �� #��z��5!   !   #��z��5"   !   #��z��5#   !   $��16��5$   !   $��16��5%   !   ._�5&   "   �� $	 	PART_Tick��5'   %   #���  5(   %   $��10��5)   %   $��10��5*   %   $y�	Collapsed=�5+   %   ._�5,   &   �� $
 PART_MaskedImage��5-   )   #���  5.   )   $��3��5/   )   $��10��50   )   $��10��51   )   52   #   53      �� 54      $��1��55   !   $��10,0,0,0q�56   !   #��z��57   !   #��z��58   !   y�59   !   �� *	Content+$ OneWay=�8   �� *TemplatedParent+5:      5;       5<      P� 5=       67   Z�  �@d�	IsChecked. �5>      �� $ 	PART_Tick��6%    � ��
 ���� 6R   �� 5?      $ 	PART_Tick��6%    � y�$ Visible=�6R   �� 5@      $ PART_MaskedImage��6%    � y�$ Visible=�6Y   5A      P� 5B       �@d�	IsChecked$
 TrueY�67   . �5C      �� $ 	PART_Tick��6%   # �  6R    � ���� 5D      $ 	PART_Tick��6%    � y�$ Visible=�6R   �� 5E      $ PART_MaskedImage��6%    � y�$ 	Collapsed=�6Y   5F      P� 5G       �@d�	IsChecked$ FalseY�67   . �5H      �� $ 	PART_Tick��6%    � y�$ 	Collapsed=�6R   5I      5K      5M       5N   
   P� 5O       � {� . . �5P      ��  �@�Cursor$
 Hand��6+   5Q      P� 5R       � {� .  . �5S      ��  �@�Cursor$ Arrow��6+   5T      5V      5W      