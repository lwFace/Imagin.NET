   M S B A M L   `   `   `  ���� R  NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType �� Property �� Value �� 
TargetType � ColumnDefinitions � RowDefinitions ��Name �� CanContentScroll ��Name	 �� ScrollableHeight
 � Maximum �� VerticalOffset � Value �� ViewportHeight �� ViewportSize* �� #ComputedVerticalScrollBarVisibility �� ScrollableWidth �� Orientation �� HorizontalOffset �� ViewportWidth, �� %ComputedHorizontalScrollBarVisibility   Brush.ScrollBar.Background ��Name ]�Name ]� Thumb n�Name �Name ]� IncreaseRepeatButton ��Name J� Opacity ]� DecreaseRepeatButton �� Triggers P� 
SourceName  P� Property! P� Value ( $Brush.ScrollBar.MouseOver.Background" �� 
TargetName ' #Brush.ScrollBar.Dragging.Background�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      %e  '��       '�� �    �� "  ��.���  � �� . ��  � ��$
 Auto=���  � �� �� " ��.R��� #��z��#��z��#��z��._��  �� $��* ��� $
��Auto � �� $��* ��� $
��Auto �.+��� $ PART_ScrollContent�� . #��z���� $ PART_VerticalScrollBar��$��0��$��1��$��Right=�$��Right=�$��0q�#
 z	 # z # z #y�z �� $ PART_HorizontalScrollBar��$��1��$��0��$��0q�#
 z $ 
Horizontal=�# z $��Bottom=�$��Bottom=�# z #y�z �� "  ��.���  �@��IsFlicksEnabled .  ��  � ��# �   ��  � ��
 ���� ��  � ��$ 0q���  � �� �� " ��.R��� $ PART_Border��#��z��#��z��#��z��._�]� $ 
PART_Track��$��0��|�.  #��z��#��z�� n� $ 
PART_Thumb��#��z������ "  n�.���  � �� �� " n�.R�� $
 Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��
������ �� $ CornerScrollBarRectangle��$��Stretch=�$��Stretch=�$
��Auto��$
��Auto��#��z�� �� $ PART_Up��$��ScrollBar.PageDownCommand��$ 0Q�|�.   �� $ 	PART_Down��$��ScrollBar.PageUpCommand��$ 0Q�|�.   P� $ 
PART_Thumb��  �@J�IsMouseOver! . . ��� # �  $" 
PART_Thumb�� � ��P� $ 
PART_Thumb��  �@n�
IsDragging! . . ��� # �  $" 
PART_Thumb�� � ��P�   � {�! .  . ��� $" 
PART_Thumb�� � y�$ 	Collapsed=�P�   �@��Orientation$! Vertical=�. ���  � ��$
 Auto����  � ��$ 8����  � ��$ 5����  � ��$ 5���� $" 
PART_Track�� � �� . P�   �@��Orientation$! 
Horizontal=�. ���  � ��$ 5����  � ��$ 8����  � ��$ 5����  � ��$
 Auto���� $" 	PART_Down�� � ��$ ScrollBar.PageLeftCommand���� $" 
PART_Thumb�� �@�Tag$ 
Horizontal���� $" 
PART_Track�� � �� .  �� $" PART_Up�� � ��$  ScrollBar.PageRightCommand��