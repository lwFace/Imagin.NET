   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      =#clr-namespace:Imagin.Common.EffectsImagin.Common.Effects  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089*    $Imagin.Common.Controls.ColorSelector  �� 
TargetType �� Property �� Value �� 
TargetType � 	Resources4   .Imagin.Common.Converters.ByteToDoubleConverter   Key J� SnapsToDevicePixels    BorderStyle)   #Imagin.Common.Controls.CheckerBoard  Name*   $Imagin.Common.Controls.ClippedBorder   Child	 (� Stretch
 �Name � DataContext �� RelativeSource J� Effect)   #Imagin.Common.Effects.ColorXYEffect   ActualModel   Value �� Triggers �� Binding �� Value  ByteToDoubleConverter �� 	Converter ��Name ��Name �� CornerRadius    EllipseSize �� Left �� Mode
 �� Top �� Triggers.   (Imagin.Common.Controls.BaseColorSelector P� Property P� Value �� 
TargetName�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      /Effects#clr-namespace:Imagin.Common.Effects  5      %R  '         7  7  �� 5      "    61   .�5   
   ��  � �� 5	      �� 5
      "   6"    5      ��  5      ByteToDoubleConverter  6;   5      .R�5      � .+�5      ��  . #��z 5      ._�5        $ PART_CheckerBoard��61   . 5   "   (� $
	 Fill=�6*   .��5   &   � $

 Grid��6+   
�������$��1��6�   $��1��6�    69   ��  �� *TemplatedParent+ 5   *     5   .    5   1   �� *Model+ 5   1   �� *Value+��5   *   �� 5   .   "  �64    5   2   �� 5   6    6B   �� *Alpha+ With6\   .a�5   :   ��  �@J�Opacity 6T   �� *
AlphaValue+8   5   7   5!   /   5#   '   5$   #   5%      5&      �� 5'      $ PART_Canvas��5(      }�. .+�5*      �� $ PART_Border��5+   !   
���   �$��1q�5/   !   $	 999��50   !   #��z 51   !   #��z 52   !    5,   !   �� *EllipsePosition.X+$ OneWay=� �� *TemplatedParent+ 5-   !   �� *EllipsePosition.Y+$ OneWay=� �� *TemplatedParent+._�53   "   �� 
�������$��1q�55   %   $	 999��56   %   57      58      59       5:      P� 5;       �@ Alpha$
 With=�63   . �5<      �� $
 Grid��6%    6J   �� *
AlphaValue+8   �� *TemplatedParent+ �@J�Opacity5=      5?      5A      5B      5C      