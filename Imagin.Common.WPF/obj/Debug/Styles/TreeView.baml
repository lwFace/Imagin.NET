   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri4   .Imagin.Common.Converters.IntToBooleanConverter   IntToBooleanConverter4   .Imagin.Common.Converters.ObjectToTypeConverter  ObjectToTypeConverter �� 
TargetType �� Property  Brush.TreeView.Background �� Value  Brush.TreeView.Border  
Brush.Text+   %Imagin.Common.Linq.TreeViewExtensions*   $Imagin.Common.Linq.ControlExtensions/   )Imagin.Common.Linq.TreeViewItemExtensions �� Mode �� 
TargetType � ColumnDefinitions � RowDefinitions	 d�Name . *Brush.TreeViewItem.ToggleButton.Empty.Fill 0 ,Brush.TreeViewItem.ToggleButton.Empty.Stroke
 �� 	ClickMode d� 	IsChecked �� RelativeSource(   "Imagin.Common.Controls.MaskedImage  Name   SourceColor J� RenderTransformOrigin J� SnapsToDevicePixels   Source    Imagin.Common.Markup.Image �� Angle �� Triggers P� Property P� Value N� EnterActions �� 
TargetName �� TargetProperty	 W� To g� Duration N� ExitActions �� Padding.   (Imagin.Common.Controls.TreeViewRowHeader  Name �� BasedOn ��Name  �� Value!	   Imagin.Common.Controls.Line! �� Binding" �� 	Converter# �� 
TargetName$ ]� 
Conditions% �� Property& �� Value 7 3Brush.TreeViewItem.ToggleButton.Filled.Checked.Fill 9 5Brush.TreeViewItem.ToggleButton.Filled.Checked.Stroke /	 +Brush.TreeViewItem.ToggleButton.Filled.Fill 1
 -Brush.TreeViewItem.ToggleButton.Filled.Stroke' �� 
SourceName + 'Brush.TreeViewItem.MouseOver.Background ' #Brush.TreeViewItem.MouseOver.Border + 'Brush.TreeViewItem.MouseOver.Foreground 1 -Brush.TreeViewItem.Selected.Active.Background - )Brush.TreeViewItem.Selected.Active.Border 1 -Brush.TreeViewItem.Selected.Active.Foreground * &Brush.TreeViewItem.Selected.Background & "Brush.TreeViewItem.Selected.Border * &Brush.TreeViewItem.Selected.Foreground( �� Triggers  Brush.DropTarget.Border%
   Imagin.Common.Controls.TreeView1   +Imagin.Common.Controls.TreeViewColumnHeader % !Brush.DataGridColumnHeader.Border 3 /Brush.DataGridColumnHeader.MouseOver.Background / +Brush.DataGridColumnHeader.MouseOver.Border 3 /Brush.DataGridColumnHeader.MouseOver.Foreground 1 -Brush.DataGridColumnHeader.Pressed.Background - )Brush.DataGridColumnHeader.Pressed.Border 1 -Brush.DataGridColumnHeader.Pressed.Foreground;   5Imagin.Common.Controls.TreeViewColumnHeadersPresenter ) %Brush.DataGridColumnHeader.Background,   &Imagin.Common.Controls.TreeViewContent  Brush.Border  Brush.TabControl.Border  H1�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5	   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+�� 5
   
    6      *Imagin.Common.WPFStyles/GridSplitter.xaml+%"  &	        &	      'R� &     'Q� �    7�7  7  '
  �    7R�'  �    '       '  F    7
 7��  5      6'     5      6'   �� 5      " R�6&   .�5   
   ��  � ��# �  6'   �� 5   
    � ��# �  6(   �� 5   
    � ��$ 1q�6,   �� 5   
    � ��# �  6'   �� 5   
    � ��$ 5q�6$   �� 5   
    �@ SelectionMode$ Single=�6B   �� 5   
    �@%�IsVirtualizing . 6=   �� 5   
    �@%�VirtualizationMode$ 	Recycling=�6A   5      �� 5      " Q�6*   .�5   
   ��  �@�OverridesDefaultStyle . 62   �� 5   
    � ��$ 1q�6,   �� 5   
    �@�Cursor$
 Hand��6#   �� 5   
    �@ FadeIn . 6:   �� 5    
    � ��# �  61   �� 5!   
    � ��$ Stretch=�67   �� 5"   
    �@ 
IsSelected 6C   �� *
IsSelected+$ TwoWay=��� 5#   
    �@ ExpandOnClick . 6F   �� 5$   
    �@ CollapseSiblingsOnClick . 6P   �� 5%   
    �@ SelectOnRightClick . 6K   �� 5&   
    �@J�Opacity$ 0Q�6$   �� 5'   
    � ��$	 5,3q�6$   �� 5(   
    � �� 5)      �� 5*      " Q�6"   .R�5+      �  5,      �� 5-      $��15��6/   $��15 �6=   �� 5.      $
��Auto �6/   �� 5/      $��* �6/    51      �� 52      $
��Auto �6,   �� 53      .+�56      d� $	 PART_Expander��57      #���  58      #���  59      $��1q�5:      $��16��5;      $��16��5<      |�.  $
 Press=�5?       5>      �� *
IsExpanded+ �� *TemplatedParent+��5@      �� 5A   "   " d�6(   .�5B   &   ��  �@�OverridesDefaultStyle . 6N   �� 5C   &    � ��$ Center=�6Q   �� 5D   &    � �� 5E   *   �� 5F   .   " d�6>   .R�5G   2     $ PART_MaskedImage��5H   5   # z��5J   5   #��z��5K   5   #��z��5L   5   $ 0.5, 0.5&�5M   5    .  5I   5     *Imagin.Common.WPFImages/ArrowDownSmall.png+z�5O   6   �� 5P   :   $	 -90Q�6J   5R   3    5S   2   P� 5T   6    �@d�	IsChecked$
 TrueY�6S    5U   :   �� 5V   >   .��5W   B   �� .��5X   F   W� $ PART_MaskedImage��6V   $4 .(Path.RenderTransform).(RotateTransform.Angle)�6   $ 0X�6�   $ 0:0:0.4D�6�   5Y   C   5Z   ?    5\   :   �� 5]   >   .��5^   B   �� .��5_   F   W� $ PART_MaskedImage��6V   $4 .(Path.RenderTransform).(RotateTransform.Angle)�6   $	 -90X�6�   $ 0:0:0.4D�6�   5`   C   5a   ?   5c   7   5e   /   5g   '   5h   #   5j      �� 5k      $��PART_Border��5n      $��1��5l      $��2��5m      #��z��5o      #��z��5p      #��z��5q      # z��5r       . ._�5t        $ PART_Header��5u   !   #��z��5v   !   ��5w   "   �� 5x   &   "  6,   8   6Z   5z      5{      �� 5|      $ 	ItemsHost��5�      $��1��5}      $��1��5~      $��2��5      5�      5�       5�      �� 5�      "  	 6x   ! 6&   �� $ OneWay=�8"  .a�5�      �� $# PART_Expander��6%    � y�$ 	Collapsed=�6V   �� 5�       � {� .  6:   5�      �� 5�      "  ��6x   ! 6&   �� $ OneWay=�8"  .a�5�      �� $# PART_Expander��6%    � y�$ 	Collapsed=�6V   �� 5�       � {� .  6:   5�      ]� 5�      $ 5�      �� 5�   "   % �@��HasItems& . 6@   �� 5�   "   % �@Q�
IsExpanded& . 6B   ./�5�      �� $# 	ItemsHost��6%    � y�$ Visible=�6R   �� 5�      $# PART_Expander��6%   # �  6V    � ���� 5�      $# PART_Expander��6%   # �  6W    � ��5�      ]� 5�      $ 5�      �� 5�   "   % �@��HasItems& . 6@   �� 5�   "   % �@Q�
IsExpanded& .  6B   ./�5�      �� $# 	ItemsHost��6%    � y�$ 	Collapsed=�6R   �� 5�      $# PART_Expander��6%   # � 	 6V    � ���� 5�      $# PART_Expander��6%   # � 
 6W    � ��5�      ]� 5�      $ 5�      �� 5�   "   % �@��HasItems& .  6@   �� 5�   "   % �@Q�
IsExpanded& . 6B   ./�5�      �� $# 	ItemsHost��6%    � y�$ Visible=�6R   5�      ]� 5�      $ 5�      �� 5�   "   % �@��HasItems& .  6@   �� 5�   "   % �@Q�
IsExpanded& .  6B   ./�5�      �� $# 	ItemsHost��6%    � y�$ 	Collapsed=�6R   5�      ]� 5�      $ 5�      �� 5�   "   % �@ 
IsSelected& .  6^   �� 5�   "   $' PART_Border��6,   % �@J�IsMouseOver& . 6\   ./�5�      �� $# PART_Border��6%   # �  6T    � ���� 5�      $# PART_Border��6%   # �  6U    � ���� 5�      # �  6E    � ��5�      ]� 5�      $ 5�      �� 5�   "   % �@ 
IsSelected& . 6^   �� 5�   "   % �@��IsSelectionActive& . 6R   ./�5�      �� $# PART_Border��6%   # �  6T    � ���� 5�      $# PART_Border��6%   # �  6U    � ���� 5�      # �  6E    � ��5�      ]� 5�      $ 5�      �� 5�   "   % �@ 
IsSelected& . 6^   �� 5�   "   % �@��IsSelectionActive& .  6R   ./�5�      �� $# PART_Border��6%   # �  6T    � ���� 5�      $# PART_Border��6%   # �  6U    � ���� 5�      # �  6E    � ��5�      5�      5�      ( 5�   
   P� 5�       �@ IsDraggingOver . . �5�      ��  � ��# �  60   5�      P� 5�       �@ 
IsSelected . . �5�      ��  � ��# �  6/   �� 5�       � ��# �  69   5�      5�      �� 5�      " 
 6,   8   .�5�   
   ��  �@
 ColumnHeaderHeight$ 30Q�6/   5�      �� 5�      "  68   .�5�   
   ��  � ��
 ���� 6'   �� 5�   
    � ��# �  6(   �� 5�   
    � ��$ 1,0,1,1q�6,   �� 5�   
    � ��# �  6'   �� 5�   
    � ��$	 5,7q�6$   ( 5�   
   P� 5�       �@J�IsMouseOver . . �5�      ��  � ��# �  6/   �� 5�       � ��# �  60   �� 5�       � ��# �  6/   5�      P� 5�       � �� . . �5�      ��  � ��# �  6/   �� 5�       � ��# �  60   �� 5�       � ��# �  6/   5�      5�      �� 5�      "  6B   .�5�   
   ��  � ��# �  6'   5�      �� 5�      "  63   8   .�5�   
   ��  � ��$ 1q�6,   �� 5�   
    � ��# �  6(   �� 5�   
    �@ ContentBorderBrush# �  6/   �� 5�   
    �@ ContentBorderThickness$ 1q�63   �� 5�   
    �@ ContentHeaderTemplate 5�      �� 5�      .R�5�      �� $��0,0,0,10q�5�      $��0,0,0,7q�5�      #���  5�      ��5�      �� $ OneWay=�5�      5�      5      �� 5  
    �@ ContentHeaderVisibility$ Visible=�64   �� 5  
    �@ ContentPadding$ 10q�6+   �� 5  
    �@ ContentWidth$	 70* �6)   �� 5  
    �@ 	MenuWidth$	 30* �6&   �� 5  
    �@ SplitterStyle 5     �� 5     " ��6   8  .�5     ��  �@��ShowsPreview . 65   �� 5	      � ��$ 4��6.   5
     5     �� 5  
    �@ SplitterVisibility$ Visible=�6/   5     5     