   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType �� Property �� Value �� 
TargetType � ColumnDefinitions � RowDefinitions ��Name �� CanContentScroll ��Name	 �� ScrollableHeight
 � Maximum �� VerticalOffset � Value �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility �� ScrollableWidth �� Orientation �� HorizontalOffset �� ViewportWidth, �� %ComputedHorizontalScrollBarVisibility   Brush.ScrollBar.Background ��Name ]�Name ]� Thumb n�Name �Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton �� Triggers P� 
SourceName  P� Property! P� Value ( $Brush.ScrollBar.MouseOver.Background" �� 
TargetName ' #Brush.ScrollBar.Dragging.Background�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %�  '��       '�� �    �� 5      "  ��6*   .�5   
   ��  � �� . 6-   �� 5   
    � ��$ Hidden=�6:   �� 5   
    � ��$
 Auto=�68   �� 5   
    � �� 5	      �� 5
      " ��6"   .R�5      �� #��z��5      #��z��5      #��z��5      ._�5      �  5      �� 5   "   $��* �63   �� 5   "   $
��Auto �63    5      �� 5   "   $��* �60   �� 5   "   $
��Auto �60   .+�5      �� $ PART_ScrollContent��5   !    . #��z��5   !   5      �� 5      $ PART_VerticalScrollBar��5!   !   $��0��5   !   $��1��5    !   $��Right=�5"   !   $��Right=�5#   !   $��0q�5$   !   #
 z	 5%   !   # z 5&   !   # z 5'   !   #y�z 5(   !   �� 5)      $ PART_HorizontalScrollBar��5,   !   $��1��5*   !   $��0��5+   !   $��0q�5-   !   #
 z 5.   !   $ 
Horizontal=�5/   !   # z 50   !   $��Bottom=�51   !   $��Bottom=�52   !   # z 53   !   #y�z 54   !   55      56      57      59      5:      �� 5<      "  ��6   .�5=   
   ��  �@��IsFlicksEnabled .  63   �� 5>   
    �@��IsFlicksEnabled .  63   �� 5?   
    � ��
 ���� 6'   �� 5@   
    � ��$ 0q�6,   �� 5A   
    � |� .  6&   �� 5B   
    � ��# �   6'   �� 5C   
    �@�OverridesDefaultStyle . 62   �� 5D   
    � �� 5E      �� 5F      " ��6"   .R�5G      �� $ PART_Border��5H      #��z��5I      #��z��5J      #��z��5K      ._�5L      ]� $ 
PART_Track��5M      $��0��5N      |�.  #��z��5P      #��z��5Q       5R      n� 5S   "   $ 
PART_Thumb��5T   %   #��z��5U   %   |�.  ��5W   &   �� 5X   *   "  n�60   .�5Y   .   ��  � �� 5Z   2   �� 5[   6   " n�6F   .R�5\   :   � $
 Grid��6?   .+�5]   >   �� $��Stretch=�5^   A   $��Stretch=�5_   A   $
��Auto��5`   A   $
��Auto��5a   A   
������ 5b   A   �� 5c   >   $ CornerScrollBarRectangle��5d   A   $��Stretch=�5e   A   $��Stretch=�5f   A   $
��Auto��5g   A   $
��Auto��5h   A   #��z��5i   A   5j   ;   5k   7   5m   /   5n   +   5p   #    5r      �� 5s   "   $ PART_Up��5t   %   $��ScrollBar.PageDownCommand��5u   %   $ 0Q�5v   %   |�.  5w   %    5y      �� 5z   "   $ 	PART_Down��5{   %   $��ScrollBar.PageUpCommand��5|   %   $ 0Q�5}   %   |�.  5~   %   5�      5�       5�      P� 5�      $ 
PART_Thumb��6"     �@J�IsMouseOver! . . �5�      �� # �  6;   $" 
PART_Thumb��6z    � ��5�      P� 5�      $ 
PART_Thumb��6"     �@n�
IsDragging! . . �5�      �� # �  6;   $" 
PART_Thumb��6y    � ��5�      P� 5�        � {�! .  . �5�      �� $" 
PART_Thumb��6%    � y�$ 	Collapsed=�6S   5�      P� 5�        �@��Orientation$! Vertical=�69   . �5�      ��  � ��$
 Auto��67   �� 5�       � ��$ 8��69   �� 5�       � ��$ 5��69   �� 5�       � ��$ 5��66   �� 5�      $" 
PART_Track��6%    � �� . 6\   5�      P� 5�        �@��Orientation$! 
Horizontal=�69   . �5�      ��  � ��$ 5��67   �� 5�       � ��$ 8��6:   �� 5�       � ��$ 5��6:   �� 5�       � ��$
 Auto��66   �� 5�      $" 	PART_Down��6%    � ��$ ScrollBar.PageLeftCommand��6O   �� 5�      $" 
PART_Thumb��6%    �@�Tag$ 
Horizontal��6L   �� 5�      $" 
PART_Track��6%    � �� .  6\   �� 5�      $" PART_Up��6%    � ��$  ScrollBar.PageRightCommand��6M   5�      5�      5�      5�      5�      