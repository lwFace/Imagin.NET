   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5	      7 clr-namespace:Imagin.Common.DataImagin.Common.Data  5
      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      =#clr-namespace:Imagin.Common.StorageImagin.Common.Storage  5      &     Imagin.Common.Controls.Navigator   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      U QGongSolutions.WPF.DragDrop, Version=1.1.0.0, Culture=neutral, PublicKeyToken=nullgurn:gong-wpf-dragdrop  5      0local$clr-namespace:Imagin.Common.Controls  5      7
Converters&clr-namespace:Imagin.Common.Converters  5	      +Data clr-namespace:Imagin.Common.Data  5
      +Linq clr-namespace:Imagin.Common.Linq  5      1Storage#clr-namespace:Imagin.Common.Storage  5        � 	Resources  5      "   Imagin.Common.Data.Reference   	Navigator   Data!   Imagin.Common.Data.Ancestor;   5Imagin.Common.Converters.BooleanToVisibilityConverter   BooleanToVisibilityConverter0   *Imagin.Common.Converters.FileNameConverter  FileNameConverter0   *Imagin.Common.Converters.ToStringConverter  ToStringConverter��%�   &	        &	 7     &	 J     &	 ]       5   
    6+     *M� *local:Navigator++  5   
   62     5   
   6'     5   
   6'   5      .��5      %   Imagin.Common.Controls.TreeView  -   5          Name$ PART_TreeView��5   	    J� 	AllowDrop . $��0q�5   	   )  #GongSolutions.Wpf.DragDrop.DragDrop   IsDropTarget .  /   )Imagin.Common.Linq.ItemsControlExtensions   CanDragSelect .  $��Disabled=�5    	    � DataContext 5   	     *local:Navigator+   DropHandler 5   	   �� *DropHandler+��5   	   �� *Items+  5!   
    �� DataType%	   Imagin.Common.Storage.Container	 �� ItemsSource
 �Name � ColumnDefinitions d� 	IsChecked �� 	Converter �� Source&
    Imagin.Common.Controls.Thumbnail 
 Name 
  Path �� Mode ��Name)   #Imagin.Common.Controls.ProgressRing �� Triggers �� Value �� Binding �� 
TargetName * &Brush.TreeViewItem.Selected.Foreground �� Value �� Property    Imagin.Common.Storage.File �� ConverterParameter$   Imagin.Common.Storage.Shortcut��%�  (��       5"   '   *,	 +)7  7   7  7  (�� 9    5L   '   *, +)7  7   7   7  7   7  (�� �    5t   '   *, +)7  7   7  �� 5"      " 	 6'   	 6M   �� *Items+.R�5#      � $
 	PART_Grid��6    5$      �� 5%      $
��Auto �6+   �� 5&      $
��Auto �6+   �� 5'      $��* �6+   �� 5(      $
��Auto �6+   .+�5+      �� $��0,0,5,0q�5-       5,      �� *
IsSelected+y�5.      �� *Data.ShowCheckBoxes+8   8  
  5/      $ PART_Thumbnail��51      $��1��50      $��0,0,10,0q�53      $��20��54      $��20��55      $��Center=�56       52      �� *Path+$ OneWay=�56      �� 57      $ 	PART_Text��59      $��2��58      $��0,0,10,0q�5:      $��Center=�5<      ��5;      �� *Path+8  5<        5=      $��3��5>      $��16��5?      $��16��5@      y�5A      �� *Items.IsRefreshing+8  5B       5C      �� 5D      $
 True��6A    6"   �� *
IsSelected+.a�5E      �� $ 	PART_Text��6!   # �  6N    � ��5F      �� 5G      $
 True��6?    6"   �� *
IsHidden+.a�5H      �� $ 	PART_Grid��6!    �@J�Opacity$	 0.6Q�6K   5I      5K      �� 5L      "  6'   .R�5M      � $
 	PART_Grid��6    5N      �� 5O      $
��Auto �6+   �� 5P      $
��Auto �6+   �� 5Q      $��* �6+   .+�5T      �� $��0,0,5,0q�5V       5U      �� *
IsSelected+y�5W      �� *Data.ShowCheckBoxes+8   8  
  5X      $ PART_Thumbnail��5Z      $��1��5Y      $��0,0,10,0q�5\      $��20��5]      $��20��5^      $��Center=�5_       5[      �� *Path+$ OneWay=�5_      �� 5`      $ 	PART_Text��5b      $��2��5a      $��Center=�5c      5d       5e      �� 5f      $ False��6q    6"   �� *Data.ViewFileExtensions+8  .a�5g      �� $ 	PART_Text��6!    6H   �� *Path+8  $ 0�� � ��5h      �� 5i      $
 True��6q    6"   �� *Data.ViewFileExtensions+8  .a�5j      �� $ 	PART_Text��6!    6H   �� *Path+8  $ 1�� � ��5k      �� 5l      $
 True��6A    6"   �� *
IsSelected+.a�5m      �� $ 	PART_Text��6!   # �  6N    � ��5n      �� 5o      $
 True��6?    6"   �� *
IsHidden+.a�5p      �� $ 	PART_Grid��6!    �@J�Opacity$	 0.6Q�6K   5q      5s      �� 5t      "  6'   	 6L   �� *Items+.R�5u      � $
 	PART_Grid��6    5v      �� 5w      $
��Auto �6+   �� 5x      $
��Auto �6+   �� 5y      $��* �6+   .+�5|      �� $��0,0,5,0q�5~       5}      �� *
IsSelected+y�5      �� *Data.ShowCheckBoxes+8   8  
  5�      $ PART_Thumbnail��5�      $��1��5�      $��0,0,10,0q�5�      $��20��5�      $��20��5�      $��Center=�5�       5�      �� *Path+$ OneWay=�5�      �� 5�      $ 	PART_Text��5�      $��2��5�      $��Center=�5�      ��5�      �� *Path+8  $ 0��5�      5�       5�      �� 5�      $
 True��6A    6"   �� *
IsSelected+.a�5�      �� $ 	PART_Text��6!   # �  6N    � ��5�      �� 5�      $
 True��6?    6"   �� *
IsHidden+.a�5�      �� $ 	PART_Grid��6!    �@J�Opacity$	 0.6Q�6K   5�      5�      5�      ��5�   
   �� 5�       �� 
TargetType" Q�6    �� BasedOn# [BQ�6.   .�5�      �� *   $Imagin.Common.Linq.ControlExtensions �@ FadeIn . 6B   �� 5�       �@ FadeOut . 6C   �� 5�      3   -Imagin.Common.Linq.FrameworkElementExtensions �@ ShellContextMenu 6U   �� *Path+-   5        �� 5�       � ��$ 1q�64   �� 5�       �@Q�
IsExpanded 6/   �� *
IsExpanded+�� 5�       �@�ToolTip 6,   �� *	ToolTip+�� 5�       �@J�	AllowDrop . 6.   �� 5�       �@ DropHandler 6;   �� *Data.DropHandler+# [  �� 5�       �@ IsDragSource . 6<   �� 5�       �@ IsDropTarget . 6<   �� 5�       �@ UseDefaultDragAdorner . 6E    �� Triggers 5�      ^� 5�       ^� 
Conditions 5�      �� 5�       �� Binding 6(   �� *Type+ �� Value File6A   �� 5�       6(   �� *Data.ViewFiles+# [  	 False6n   .0�5�      ��  � y�$ 	Collapsed=�67   5�      ^� 5�       5�      �� 5�       6(   �� *
IsHidden+ True6E   �� 5�       6(   �� *Data.ViewHiddenItems+# [  	 False6t   .0�5�      ��  � y�$ 	Collapsed=�67   5�      5�      5�      5�      