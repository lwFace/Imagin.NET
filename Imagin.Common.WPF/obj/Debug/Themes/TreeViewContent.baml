   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0894    .Imagin.Common.Converters.ObjectToTypeConverter   ObjectToTypeConverter,   &Imagin.Common.Controls.TreeViewContent  �� 
TargetType �� Property �� Value/   )Imagin.Common.Linq.TreeViewItemExtensions K� IsItemsHost �� 
TargetType �� Padding � ColumnDefinitions   	MenuWidth   ContentWidth	   MenuBorderThickness
   MenuBackground   MenuBorderBrush   MenuPadding �� ResizeBehavior �� ResizeDirection   SplitterStyle   SplitterVisibility   ContentPadding   ContentBorderThickness   ContentBackground   ContentBorderBrush � RowDefinitions ��Name   ContentHeaderTemplate   ContentHeaderVisibility+   %Imagin.Common.Controls.TransitionView  Name R� SelectedItem   ContentTemplate   ContentTemplateSelector   ContentTransition   
Transition �� Triggers  P� Value! P� Property" �� 
TargetName# �� Mode$ �� Value% �� Binding& �� 	Converter' �� RelativeSource( � DataContext) �� AncestorType* �� Path�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      )Linq clr-namespace:Imagin.Common.Linq  5      %�	  &	        '       7      5      6'   �� 5	      "   63   .�5
   
   ��  � ��
 ���� 6'   �� 5   
    � ��$ 0q�6,   �� 5   
    � ��$ Stretch=�60   �� 5   
    �@ CollapseSiblingsOnClick .  6P   �� 5   
    �@ ExpandOnClick .  6F   �� 5   
    � �� 5      �� 5      .R�5      $�  . 6-   5      5      �� 5   
    � ��$ Visible=�6G   �� 5   
    � ��$ Visible=�6E   �� 5   
    �@J�SnapsToDevicePixels . 60   �� 5   
    � ��$ Stretch=�6.   �� 5   
    �@%�IsVirtualizing . 6=   �� 5   
    �@%�VirtualizationMode$ 	Recycling=�6A   �� 5   
    � �� 5      �� 5      "  6"   .R�5      �� #��z��5       #��z��5!      #��z��5"      # z��5#      ._�5$      �  5%      �� 5&   "   #��z 63   �� 5'   "   #��z 63   .+�5)      �� $��Stretch=�5*   !   #��z	 5+   !   #��z
 5,   !   #��z 5-   !   |�.  ��. .��50   "   �� #��z 51   %   $��Stretch=�52   %   53      �� 55      $��Right=�56   !   $ CurrentAndNext=�57   !   $ Columns=�58   !   #��z 59   !   $��Stretch=�5:   !   #y�z 5;   !   �� 5=      $��1��5>   !   # z 5?   !   #��z 5@   !   #��z 5A   !   #��z 5B   !   ._�5C   "   �  5D   &   �� 5E   *   $
��Auto �68   �� 5F   *   $��* �68   .+�5H   &   �� $ PART_ContentHeader��5I   )   #��z 5J   )   #y�z 5K   )     5L   &   $ PART_Content��5N   )   $��1��5M   )   #��z 5O   )   #��z 5P   )   #��z 5Q   )   # z 5R   )   5S   #   5T      5U      5V       5W      P� 5X        6C   Z� ! �@ ContentHeaderTemplate. �5Y      �� $" PART_ContentHeader��6%    � �� 5Z   "   �� 5[   &   .R�5\   *   �� ��6;   �� $# OneWay=�5]   '   5_      5`      P� 5a        6:   Z� ! �@R�SelectedItem. �5b      �� $" PART_Content��6%    � y�$ 	Collapsed=�6U   �� 5c      $" PART_ContentHeader��6%    � y�$ 	Collapsed=�6[   5d      �� 5e      "$ Q�6�   % 6&   �� *SelectedItem+8&   ' �� *Self+.a�5f      �� $" PART_Content��6%    6R   Z�  � ���� 5g      $" PART_Content��6%    � �� 5h   "   �� 5i   &   .R�5j   *   �� ( 5k   -   �� *SelectedItem+' �� ")  ��5l   -   �� $/* )(Linq:FrameworkElementExtensions.Content)�$# OneWay=�5m   '   5o      5p      5r      5t      5u      5v      