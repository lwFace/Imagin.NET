   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.DataImagin.Common.Data  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089;    5Imagin.Common.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter:   4Imagin.Common.Converters.ObjectToVisibilityConverter  ObjectToVisibilityConverter-   'Imagin.Common.Controls.MaskedToggleMenu  �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Padding � ColumnDefinitions(   "Imagin.Common.Controls.MaskedImage  Name/   )Imagin.Common.Controls.MaskedToggleButton   SourceColor	   SourceColor
   SourceHeight   SourceHeight   SourceWidth   SourceWidth ��Name   ContentMargin �� 	Converter � Cursor   ButtonSource   Source   ButtonSourceColor   MenuButtonToolTip � ToolTip   MenuButtonVisibility(   "Imagin.Common.Data.TemplatedParent �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName �� RelativeSource �� AncestorType P� Property  P� Value�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      )Data clr-namespace:Imagin.Common.Data  5      %�
  &	        &	      '  &     7     5      6.     5      6-   �� 5
      "   64   .�5   
   ��  � ��
 ���� 6'   �� 5   
    �@�Cursor$
 Hand��6#   �� 5   
    � �� 5      �� 5      "  6"   .R�5      �� $ PART_Border��5      #��z��5      #��z��5      #��z��5      # z��5      ._�5      �  5      �� 5   "   $
��Auto �63   �� 5   "   $
��Auto �63   �� 5   "   $
��Auto �63   .+�5        $ 
PART_Image��5   !   $��0q�5   !   $��0q�5    !   #	 z 5!   !   # z
 5"   !   # z 5#   !   #��z��5$   !   �� 5%      $ PART_Content��5'   !   $��1��5&   !   #��z��5(   !   #��z 5)   !   #��z��5*   !   y�5+   !   �� *	Content+8     5-      $ PART_MaskedImage��5/   !   $��2��5.   !   $
 Hand��50   !   # z 51   !   #	 z 52   !   # z 55   !   #��z��56   !   #y�z 57   !    53   !     *ButtonSize.Height+ 54   !     *ButtonSize.Width+57   !   58      59       5:      ]� 5;       5<      �� 5=   "    �@d�	IsChecked$
 TrueY�6A   ./�5?      �� $ PART_Border��6%    6Q   �� *CheckedToolTip+ �� "   �@�ToolTip5@      ]� 5A       5B      �� 5C   "    �@d�	IsChecked$
 TrueY�6A   �� 5D   "    6F   Z�  �@ CheckedToolTip./�5F      �� $ PART_Border��6%    6Q   �� *	ToolTip+ �� "   �@�ToolTip5G      ]� 5H       5I      �� 5J   "    �@d�	IsChecked$
 TrueY�6A   ./�5L      �� $ PART_Content��6%    6R   �� *CheckedContent+ �� "   � ��5M      ]� 5N       5O      �� 5P   "    �@d�	IsChecked$
 TrueY�6A   �� 5Q   "    6F   Z�  �@ CheckedContent./�5S      �� $ PART_Content��6%    6R   �� *	Content+ �� "   � ��5T      ]� 5U       5V      �� 5W   "    �@d�	IsChecked$
 TrueY�6A   ./�5Y      �� $ 
PART_Image��6%    6O   �� *CheckedSource+ �� "   �@ Source5Z      ]� 5[       5\      �� 5]   "    �@d�	IsChecked$
 TrueY�6A   �� 5^   "    6E   Z�  �@ CheckedSource./�5`      �� $ 
PART_Image��6%    6O   �� *Source+ �� "   �@ Source5a      P� 5b       �@d�	IsChecked$  FalseY�67   . �5c      �� $ PART_Content��6%    6R   �� *	Content+ �� "   � ���� 5d      $ 
PART_Image��6%    6O   �� *Source+ �� "   �@ Source�� 5e      $ PART_Border��6%    6Q   �� *	ToolTip+ �� "   �@�ToolTip5f      5h      5j      �� 5k   
    � ��$ Center=�6.   �� 5l   
    � ��$ Center=�65   5m      5n      