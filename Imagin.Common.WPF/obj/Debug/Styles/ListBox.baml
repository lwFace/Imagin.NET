   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri4   .Imagin.Common.Converters.ObjectToTypeConverter   ObjectToTypeConverter �� 
TargetType �� BasedOn �� Property �� Value*   $Imagin.Common.Linq.ControlExtensions �� StrokeDashArray J� SnapsToDevicePixels  
Brush.Text �� 
TargetType	 ��Name
 �� Mode �� RelativeSource �� Padding �� Triggers.   (Imagin.Common.Linq.ListBoxItemExtensions P� Property P� Value �� 
TargetName �� Triggers  Brush.DropTarget.Border ]� 
Conditions �� Property �� Value 0 ,Brush.ListBoxItem.Selected.Active.Background 0 ,Brush.ListBoxItem.Selected.Active.Foreground ) %Brush.ListBoxItem.Selected.Background ) %Brush.ListBoxItem.Selected.Foreground * &Brush.ListBoxItem.MouseOver.Background * &Brush.ListBoxItem.MouseOver.Foreground 	 Style.ListView.ScrollViewer � ColumnDefinitions � RowDefinitions 
 Brush.GridView.Background �� Columns! �� ColumnHeaderContainerStyle �� ColumnHeaderTemplate# �� ColumnHeaderTemplateSelector �� AllowsColumnReorder �� ColumnHeaderContextMenu �� ColumnHeaderToolTip �� CanContentScroll-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation  �� Orientation! �� ScrollableWidth" � Maximum# �� ViewportWidth$ �� ViewportSize% �� HorizontalOffset& � Value,' �� %ComputedHorizontalScrollBarVisibility( �� ScrollableHeight) �� ViewportHeight* �� VerticalOffset*+ �� #ComputedVerticalScrollBarVisibility  Brush.ListBox.Background  Brush.ListBox.Border/   )Imagin.Common.Linq.ItemsControlExtensions*   $Imagin.Common.Linq.ListBoxExtensions, ��Name- ^� 
Conditions. �� Binding/ �� 	Converter�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      5
Converters&clr-namespace:Imagin.Common.Converters  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+%�  &	        '��      7�'�� e    7��&		 �    '��     7   7   7 	 '�� 3    7��  5      6'   �� 5      " ��6)   8   .�5   
   ��  � ��
 ���� 6'   �� 5   
    � ��$ 1q�6,   �� 5   
    �@ FadeIn . 6:   �� 5   
    �@�FocusVisualStyle 5      �� 5      .�5      ��  � �� 5      �� 5      .R�5   "   �� $��1��5   %   
���   �$	 1 2R�5   %    . 5   %   5      5      5      5!      �� 5"   
    � ��# �  6'   �� 5#   
    �@J�Opacity$ 0Q�6$   �� 5$   
    � ��$
 16,8q�6$   �� 5%   
    � �� 5&      �� 5'      " ��6"   .R�5(      � .+�5)      �� $	 	Rectangle��5*      $��1��5,      
���   �$	 1 2R�5.       . $y�	Collapsed=�50      ��5+      �� *ActualHeight+$
 OneWay=� �� *TemplatedParent+��51      �� *ActualWidth+$
 OneWay=� �� *TemplatedParent+�� 52      $��PART_Border��53      #��z��54      #��z��55      #��z��56      $��1q�57      # z��58       . ._�5:      �� 5;      5<       5=      P� 5>       �@ LastSelected . . �5?      �� $ 	Rectangle��6%    � y�$ Visible=�6R   5@      5B      5D       5E   
   P� 5F       �@ IsDraggingOver . . �5G      ��  � ��# �  60   5H      ]� 5I       5J      �� 5K       �@��
IsSelected . 66   �� 5L       �@��IsSelectionActive . 6F   ./�5N      ��  � ��# �  6/   �� 5O       � ��# �  6/   5P      ]� 5Q       5R      �� 5S       �@��
IsSelected . 66   �� 5T       �@��IsSelectionActive .  6F   ./�5V      ��  � ��# �  6/   �� 5W       � ��# �  6/   5X      ]� 5Y       5Z      �� 5[       �@��
IsSelected .  66   �� 5\       �@J�IsMouseOver . 67   ./�5^      ��  � ��# �  6/   �� 5_       � ��# �  6/   5`      5b      �� 5d      " ��6*   8   6M   �� 5f      " ��60   .�5g   
   ��  � �� . 6-   �� 5h   
    � ��$
 Auto=�68   �� 5i   
    � �� 5j      �� 5k      " ��6"   .R�5l      �� #��z��5m      #��z��5n      #��z��5o      ._�5p      �  5q      �� 5r   "   $��* �63   �� 5s   "   $
��Auto �63    5u      �� 5v   "   $
��Auto �60   �� 5w   "   $��* �60   �� 5x   "   $
��Auto �60   .+�5{      �� $��2��5|   !   #��� 
 5}   !   �� 5~      $��2��5   !   $��Stretch=�5�   !   # z 5�   !    5�   !   �� *TemplatedParent.View.Columns+ �� *TemplatedParent+ 5�   !   �� *1/TemplatedParent.View.ColumnHeaderContainerStyle+ �� *TemplatedParent+ 5�   !   �� *+)TemplatedParent.View.ColumnHeaderTemplate+ �� *TemplatedParent+ 5�   !   �� *31TemplatedParent.View.ColumnHeaderTemplateSelector+ �� *TemplatedParent+ 5�   !   �� **(TemplatedParent.View.AllowsColumnReorder+ �� *TemplatedParent+ 5�   !   �� *.,TemplatedParent.View.ColumnHeaderContextMenu+ �� *TemplatedParent+ 5�   !   �� **(TemplatedParent.View.ColumnHeaderToolTip+ �� *TemplatedParent+5�   !   �� 5�      $!��PART_ScrollContentPresenter��5�   !   $��1��5�   !   # z��5�   !   $ Local=�5�   !   �� 5�      $��PART_HorizontalScrollBar��5�   !   $  
Horizontal=�5�   !   $��2��5�   !   #" z! 5�   !   #$ z# 5�   !   #& z% 5�   !   #y�z' 5�   !   �� 5�      $��PART_VerticalScrollBar��5�   !   $��1��5�   !   $��1��5�   !   #" z( 5�   !   #$ z) 5�   !   #& z* 5�   !   #y�z+ 5�   !   5�      5�      5�      5�      5�      �� 5�      " ��6%   .�5�   
   ��  � ��# �  6'   �� 5�   
    � ��# �  6(   �� 5�   
    � ��$ 1q�6,   �� 5�   
    � ��$ 5q�6$   �� 5�   
   # �@ SelectNoneOnEmptySpaceClick . 6T   �� 5�   
    �@ EnableLastSelected . 6F   �� 5�   
    � ��$ Disabled=�6G   �� 5�   
    � ��$ Disabled=�6E   �� 5�   
   % �@��IsSynchronizedWithCurrentItem$
 TrueY�6C   �� 5�   
    �@%�IsVirtualizing . 6=   �� 5�   
   " �@%�IsVirtualizingWhenGrouping . 6I   �� 5�   
    �@%�
ScrollUnit$ Pixel=�69   �� 5�   
    �@%�VirtualizationMode$ 	Recycling=�6A   �� 5�   
    �@ DirectionalNavigation$ Cycle=�6E   �� 5�   
    � �� 5�      �� 5�      " ��6"   .R�5�      �� $, PART_ScrollViewer��5�      #��z��5�      #��z��5�      #��z��5�      .��5�      � .+�5�      �� #��z��6-   5�      5�       5�      ^� 5�      - 5�      �� 5�   "   " ��6�   . 6,   �� $
 OneWay=�8/    �� *Self+6�   �� 5�   "   " ��6�   . 6,   �� *View+$
 OneWay=�8/   �� *Self+6�   .0�5�      �� $ PART_ScrollViewer��6%   8   � ��6U   5�      P� 5�       �@��
IsGrouping . . �5�      ��  � �� .  6N   5�      5�      5�      5�      �� 5�      " ��6&   8   6E   5�      