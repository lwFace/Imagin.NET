   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri*  $System.Windows.Controls.DataGridCell �� 
TargetType �� Property �� Value !  Brush.DataGridCell.Foreground �� 
TargetType � 	Resources �� Padding J� SnapsToDevicePixels)  #System.Windows.Controls.DataGridRow	 �� BasedOn*   $Imagin.Common.Linq.ControlExtensions   Brush.DataGridRow.Foreground;   5Imagin.Common.Converters.BooleanToVisibilityConverter  Key
 ��Name �� StrokeDashArray?  9System.Windows.Controls.Primitives.SelectiveScrollingGrid � ColumnDefinitions � RowDefinitions?  9System.Windows.Controls.Primitives.DataGridCellsPresenter   
ItemsPanelA  ;System.Windows.Controls.Primitives.DataGridDetailsPresenter   DetailsVisibility$   SelectiveScrollingOrientation;  5System.Windows.Controls.SelectiveScrollingOrientation   Vertical �� ConverterParameter&	   System.Windows.Controls.DataGrid# 	  RowDetailsScrollingConverter �� 	Converter �� RelativeSource �� AncestorType:
  4System.Windows.Controls.Primitives.DataGridRowHeader7  1System.Windows.Controls.DataGridHeadersVisibility
   Row! 	  HeadersVisibilityConverter �� Triggers.   (Imagin.Common.Linq.DataGridRowExtensions P� Property P� Value �� 
TargetName �� Triggers ]� 
Conditions �� Property  �� Value * &Brush.DataGridRow.MouseOver.Background & "Brush.DataGridRow.MouseOver.Border ) %Brush.DataGridRow.Selected.Background % !Brush.DataGridRow.Selected.Border ) %Brush.DataGridRow.Selected.Foreground 0 ,Brush.DataGridRow.Selected.Active.Background ,	 (Brush.DataGridRow.Selected.Active.Border 0
 ,Brush.DataGridRow.Selected.Active.Foreground % !Brush.DataGridRow.Even.Background ! Brush.DataGridRow.Even.Border $  Brush.DataGridRow.Odd.Background   Brush.DataGridRow.Odd.Border $  Style.DataGridColumnHeader.Thumb $  Brush.DataGridColumnHeader.Thumb  Style.DataGridColumnHeader=  7System.Windows.Controls.Primitives.DataGridColumnHeader % !Brush.DataGridColumnHeader.Border ) %Brush.DataGridColumnHeader.Foreground! F�Name" J� RenderTransformOrigin# �� Stretch$ n�Name% � Cursor& �� Angle ) %Brush.DataGridColumnHeader.Background 3 /Brush.DataGridColumnHeader.MouseOver.Background / +Brush.DataGridColumnHeader.MouseOver.Border 3 /Brush.DataGridColumnHeader.MouseOver.Foreground 1 -Brush.DataGridColumnHeader.Pressed.Background - )Brush.DataGridColumnHeader.Pressed.Border 1 -Brush.DataGridColumnHeader.Pressed.Foreground' �� Path( �� Orientation  Brush.DataGrid.Background  Brush.DataGrid.Border ' #Brush.DataGrid.GridLines.Horizontal+   %Imagin.Common.Linq.DataGridExtensions/   )Imagin.Common.Linq.ItemsControlExtensions-  'System.Windows.Input.KeyboardNavigation % !Brush.DataGrid.GridLines.Vertical) �� 
ResourceId* �� TypeInTargetAssembly ' #Brush.DataGrid.SelectAll.Foreground+ ��Name(   "Imagin.Common.Controls.MaskedImage,   Source-   SourceColor 1  -Brush.DataGrid.SelectAll.MouseOver.Foreground /! +Brush.DataGrid.SelectAll.Pressed.Foreground�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5	   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+%{$  '        '  �    7�&	 �    &	     7  '
  �    '	  w    7  (�� h    5�     $") DataGridSelectAllButtonStyle��"* 	 )�� 5      "  6*   .�5   
   ��  � ��
 ���� 6'   �� 5   
    � ��$ 0q�6,   �� 5   
    � ��# �   6(   �� 5   
    � ��$ 13(�6%   �� 5   
    � ��$ 5q�6$   �� 5   
    � ��$ Stretch=�67   �� 5   
    � ��$ Center=�65   �� 5   
    � �� 5      �� 5      "  6"    5      ���� 5      '��       " ��6    .�5      ��  � ��$ CharacterEllipsis=�6=   5      5      .R�5      �� #��z��5      #��z��5      #��z��5      # z��5        . ._�5"      �� # z 5#      #��z��5$      #��z��5%      #��z��5&      5'      5(      5*      57      �� 59      "  6)   8	   .�5:   
   ��  � ��$ 0q�6,   �� 5;   
    �@ FadeIn . 6:   �� 5<   
    � ��# �  63   �� 5=   
    �@J�SnapsToDevicePixels . 60   �� 5>   
    �@=�ErrorTemplate 65   Z� �� 5?   
    �@ ValidationErrorTemplate 5@      �� 5A      .R�5B      �� 
���  ��$��2,0,0,0q�5C      $��!��5D      $��Center=�5E      5F      5H      �� 5I   
    � �� 5J      �� 5K      "  6"    5L      ��  5M       BooleanToVisibilityConverter 6B   5N      .R�5O      �� #��z��5P      #��z��5Q      #��z��5R      # z 5S      ._�5T      � .+�5U      �� $
 	Selection��5V   !   $��1��5W   !   ���	#DD000000$	 1 2R�5Y   !    . 5[        5\      $��1q�65    5]   "   �� 5^   &   $
��Auto �67   �� 5_   &   $��* �67    5a   "   �� 5b   &   $��* �64   �� 5c   &   $
��Auto �64   .+�5e   "     $��1��5f   %   #��z 5g   %   # z 5h   %     5i   "   $��1��5j   %   $��1��5k   %   #y�z 5m   %    5l   %   �� *AreRowDetailsFrozen+# Z # Z  �� " 	 5m   %   
  5n   "   $��2��64   $ Vertical=�5o   %   y�5p   %   �� *HeadersVisibility+# Z # Z  �� " 	 5q      5r      5s       5t      P� 5u       �@ SelectionVisibility$ 	Collapsed=�6\   . �5v      �� $ 	Selection��6%    � y�$ 	Collapsed=�6R   5w      P� 5x       �@ SelectionVisibility$ Visible=�6\   . �5y      �� $ 	Selection��6%    � y�$ Visible=�6R   5z      5|      5~       5   
   ]� 5�       5�      �� 5�       �@J�IsMouseOver  . 67   �� 5�       �@ 
IsSelected  .  66   ./�5�      ��  � ��# �  6/   �� 5�       � ��# �  60   5�      ]� 5�       5�      �� 5�       �@ 
IsSelected  . 66   �� 5�       �@��IsSelectionActive  .  6F   ./�5�      ��  � ��# �  6/   �� 5�       � ��# �  60   �� 5�       � ��# �  6/   5�      ]� 5�       5�      �� 5�       �@ 
IsSelected  . 66   �� 5�       �@��IsSelectionActive  . 6F   ./�5�      ��  � ��# �  6/   �� 5�       � ��# � 	 60   �� 5�       � ��# � 
 6/   5�      ]� 5�       5�      �� 5�       �@��AlternationIndex$  0��6I   �� 5�       �@J�IsMouseOver  .  67   �� 5�       �@ 
IsSelected  .  66   ./�5�      ��  � ��# �  6/   �� 5�       � ��# �  60   5�      ]� 5�       5�      �� 5�       �@��AlternationIndex$  1��6I   �� 5�       �@J�IsMouseOver  .  67   �� 5�       �@ 
IsSelected  .  66   ./�5�      ��  � ��# �  6/   �� 5�       � ��# �  60   5�      5�      �� 5�      " n�65   .�5�   
   ��  �@�OverridesDefaultStyle . 62   �� 5�   
    � ��# �  6'   �� 5�   
    � ��$ Center=�67   �� 5�   
    � ��$ 18��6"   �� 5�   
    � �� 5�      �� 5�      " n�6"   .R�5�      �� 
������ ._�5�      �� #��z��5�      #��z��5�      $��1��5�      5�      5�      5�      5�      �� 5�      "  6/   .�5�   
   ��  � ��# �  6(   �� 5�   
    � ��$ 1,0,1,1q�6,   �� 5�   
    �@�Cursor$
 Hand��6#   �� 5�   
    � ��# �  6'   �� 5�   
    � ��$ Center=�67   �� 5�   
    � ��$ -1,0,0,0q�6#   �� 5�   
    � ��$ 0��6%   �� 5�   
    � ��$ 30��6&   �� 5�   
    � ��$
 10,0q�6$   �� 5�   
    �@J�SnapsToDevicePixels . 60   �� 5�   
    � ��$ Center=�65   �� 5�   
    � �� 5�      �� 5�      "  6"   .R�5�      �  5�      �� 5�      $��* �6/   �� 5�      $
��Auto �6/   .+�5�      �� $��2��5�      #��z��5�      #��z��5�      #��z��5�      �� 5�      $	��6,3q�5�      $��Center=�5�      F� 5�      $! 	SortArrow��5�      $��1��5�      "����        �@B     � � @B 	#��z��5�      $��6��5�      $��0,0,8,0q�5�      $" 0.5,0.4&�5�      $
# Fill=�5�      $��Center=�5�      $y�	Collapsed=�5�      $��8��5�      n� 5�      $$ PART_RightHeaderGripper��5�      $��2��5�      $% SizeWE��5�      $��	-8,0,-9,0q�5�      $��Right=�5�      8��  5�      5�       5�      P� 5�       �@ SortDirection$ 	AscendingX�6;   . �5�      �� $ 	SortArrow��6%    � y�$ Visible=�6R   �� 5�      $ 	SortArrow��6%    � z� 5�   "   �� 5�   &   $	& 180Q�66   5�      5�      P� 5�       �@ SortDirection$ 
DescendingX�6;   . �5�      �� $ 	SortArrow��6%    � y�$ Visible=�6R   5�      5�      5      5<  
   P� 5=      �@ SortDirection 6/   Z� . �5>     ��  � ��# �  6/   5?     P� 5@      �@J�IsMouseOver . . �5A     ��  � ��# �  6/   �� 5B      � ��# �  60   �� 5C      � ��# �  6/   5D     P� 5E      � �� . . �5F     ��  � ��# �  6/   �� 5G      � ��# �  60   �� 5H      � ��# �  6/   5I     5K     �� 5M     " 
 6/   .�5N  
   ��  � ��
 ���� 6'   �� 5O  
    � ��$	 5,0q�6$   �� 5P  
    � ��$ 0q�6,   �� 5Q  
    � �� 5R     �� 5S     " 
 6"   .R�5T     � .+�5U     �� #��z��5W     #��z��5X     # z��5Y     ��5V     ��  �� " 	 $' HorizontalGridLinesBrush�._�5Z     �� $( 
Horizontal=�6)   .+�5[  "   �� # z 5\  %   $��Center=�5]  %   5^     5_     5`      5a     P� 5b      �@J�IsMouseOver . . �5c     ��  � �� �	#110000006;   5d     P� 5e      �@
 IsRowSelected . . �5f     ��  � �� �	#55FFFFFF6;   5g     5i     5k     5l     �� 5n     " 	 6&   .�5o  
   ��  �@��AlternationCount$ 2��6-   �� 5p  
    �@	 AutoGenerateColumns .  60   �� 5q  
    � ��# �  6'   �� 5r  
    � ��# �  6(   �� 5s  
    � ��$ 1q�6,   �� 5t  
    �@	 CanUserAddRows .  6+   �� 5u  
    �@	 CanUserDeleteRows .  6.   �� 5v  
    �@	 CanUserReorderColumns . 62   �� 5w  
    �@	 CanUserResizeColumns . 61   �� 5x  
    �@	 CanUserResizeRows .  6.   �� 5y  
    �@	 CanUserSortColumns . 6/   �� 5z  
    �@	 ColumnHeaderStyle8   6.   �� 5{  
    �@	 GridLinesVisibility$
 None=�60   �� 5|  
    �@	 HeadersVisibility$ Column=�6.   �� 5}  
     �@	 HorizontalGridLinesBrush# �  65   �� 5~  
    � ��$ Stretch=�60   �� 5  
    �@ Extends . 6<   �� 5�  
    �@ CanDragSelect . 6F   �� 5�  
    �@ IsColumnMenuEnabled . 6L   �� 5�  
   # �@ SelectNoneOnEmptySpaceClick . 6T   �� 5�  
    �@ DirectionalNavigation$ Cycle=�6E   �� 5�  
    � ��$
 Auto=�6E   �� 5�  
    �@	 SelectionMode$ Extended=�6*   �� 5�  
    �@	 SelectionUnit$ FullRow=�6*   �� 5�  
   % �@��IsSynchronizedWithCurrentItem$
 TrueY�6C   �� 5�  
    � ��$ Stretch=�6.   �� 5�  
    �@	 VerticalGridLinesBrush# �  63   �� 5�  
    �@%�IsVirtualizing . 6=   �� 5�  
    �@%�VirtualizationMode$ 	Recycling=�6A   5�     �� 5�     " ��6{   .�5�  
   ��  � ��# �  6'   �� 5�  
    � ��$ 0q�6,   �� 5�  
    �@�Cursor$
 Hand��6#   �� 5�  
    � ��# �  6'   �� 5�  
    �@�OverridesDefaultStyle . 62   �� 5�  
    � ��$ 0,0,4,4q�6$   �� 5�  
    � �� 5�     �� 5�     " ��6"   .R�5�     �� $+ PART_Border��5�     #��z��5�     #���  5�     $��0,0,1,1q�5�     ._�5�       $��Right=�5�     #��z��5�     $Q, Kpack://application:,,,/Imagin.Common.WPF;component/Images/RightTriangle.png��5�     #- z��5�     $��Bottom=�5�     5�      5�     P� 5�      �@J�IsMouseOver . . �5�     �� $ PART_Border��6%   # �  6U    � ��5�     P� 5�      � �� . . �5�     �� $ PART_Border��6%   # �  6U    � ��5�     5�     5�     �� 5�  
    �@J�SnapsToDevicePixels . 60    5�  
   P� 5�      �@J�IsMouseOver . . �5�     ��  � ��# �  6/   �� 5�      � ��# �   6/   5�     P� 5�      � �� . . �5�     ��  � ��# �  6/   �� 5�      � ��# � ! 6/   5�     5�     5�     