   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri;   5Imagin.Common.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter �� 
TargetType �� BasedOn �� Property  Brush.ComboBox.Background �� Value  Brush.ComboBox.Border  Brush.ComboBox.Foreground �� PanningMode �� PanningRatio-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation	 �� 
TargetType
 � ColumnDefinitions d�Name �� 	ClickMode d� 	IsChecked �� Mode �� RelativeSource ��Name F�Name J� RenderTransformOrigin �� 	Converter �� ConverterParameter �� AncestorType �� Triggers P� Property P� Value N� EnterActions �� 
TargetName �� TargetProperty	 W� To g� Duration N� ExitActions ��Name  J� IsHitTestVisible! �� SelectionBoxItem" �� Path# �� UpdateSourceTrigger$   Imagin.Common.Controls.TextBox$  Name% �� 
IsReadOnly& �� 
IsReadOnly'   Placeholder( ��Name) J� Opacity* �Name+ � AllowsTransparency, �� IsDropDownOpen- �Name. J� SnapsToDevicePixels/ � ActualWidth0 �� MaxDropDownHeight & "Brush.ComboBox.DropDown.Background " Brush.ComboBox.DropDown.Border+   %Imagin.Common.Linq.ComboBoxExtensions1 �� 
TargetName2 ]� 
Conditions3 �� Property4 �� Value5 �� Triggers ' #Brush.ComboBox.MouseOver.Background # Brush.ComboBox.MouseOver.Border ' #Brush.ComboBox.MouseOver.Foreground %	 !Brush.ComboBox.Checked.Background !
 Brush.ComboBox.Checked.Border % !Brush.ComboBox.Checked.Foreground*   $Imagin.Common.Controls.FontFamilyBox(   "Imagin.Common.Controls.FontSizeBox)   #Imagin.Common.Controls.FontStyleBox*   $Imagin.Common.Controls.FontWeightBox(	   "Imagin.Common.Controls.TimeZoneBox-
   'Imagin.Common.Controls.TreeViewComboBox ! Brush.ComboBoxItem.Foreground6 �� Padding * &Brush.ComboBoxItem.Selected.Background * &Brush.ComboBoxItem.Selected.Foreground + 'Brush.ComboBoxItem.MouseOver.Background�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5	   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+%Z  &	        '��      7�7   '  �    7��'  �    7��'  �    7��'      7��'	  ;    7��'
  �    7��'�� 7      5      6.   �� 5      " ��6&   8   .�5   
   ��  � ��# �  6'   �� 5   
    � ��# �  6(   �� 5   
    � ��$ 1q�6,   �� 5   
    � ��# �  6'   �� 5   
    � �� 5      �� 5      .R�5      �� $
 None=�5      $ 0Q�5      $ 	Contained=�5      5      5      �� 5   
    � ��$	 7,6q�6$   �� 5   
    � �� . 6:   �� 5   
    � ��$
 Auto=�6G   �� 5    
    �@��PanningMode$
 None=�65   �� 5!   
    � ��$
 Auto=�6E   �� 5"   
    �@J�SnapsToDevicePixels . 60   �� 5#   
    � ��$
 Auto��6"   �� 5$   
    �@�OverridesDefaultStyle . 62   �� 5%   
    � �� 5&      �� 5'      "	 ��6"   .R�5(      � 
 5)      �� 5*      $��* �6/   �� 5+      $��15 �6/   .+�5-      d� $ PART_ToggleButton��5/      $��2��5.      #��z��50      #��z��51      #��z��52      #��z��53      |�.  $ Press=�56       55      �� *IsDropDownOpen+$ TwoWay=� �� *TemplatedParent+��57      �� 58   "   "	 d�62   .R�59   &   � 
 5:   *   �� 5;   .   �� 5<   .   $��20 �6?   .+�5>   *   �� $ Border��5?   -   $��2��5@   -   #��z��5A   -   #��z��5B   -   #��z��5C   -   F� 5D   *   $ 
PART_Arrow��5F   -   $��1��5E   -   =����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	#��z��5H   -   $��Center=�5I   -   $ 0.5, 0.5&�5J   -   $��Center=�5K   -   y�5L   -   �� *
IsReadOnly+8  $ 1�� �� " ��z�5M   .   �� 5N   2   5P   +   5Q   '    5R   &   P� 5S   *    �@d�	IsChecked$
 TrueY�6G    5T   .   �� 5U   2   .��5V   6   �� .��5W   :   W� $ 
PART_Arrow��6J   $4 .(Path.RenderTransform).(RotateTransform.Angle)�6m   $ 90X�6�   $ 0:0:0.4D�6�   5X   7   5Y   3    5[   .   �� 5\   2   .��5]   6   �� .��5^   :   W� $ 
PART_Arrow��6J   $4 .(Path.RenderTransform).(RotateTransform.Angle)�6m   $ 0X�6�   $ 0:0:0.4D�6�   5_   7   5`   3   5b   +   5d   #   5f      �� 5g      $ ContentSite��5h        .  #��z! 5j      $
��Left=�5m      #��z��5n      $��Center=�5o      ��5k      �� $4" .(Linq:ComboBoxExtensions.SelectedItemTemplate)� �� *TemplatedParent+$# PropertyChanged=���5l      �� $<" 6(Linq:ComboBoxExtensions.SelectedItemTemplateSelector)� �� *TemplatedParent+$# PropertyChanged=�5p        5q      $$ TextBox��5r      |�. 
������ $��0q�5u      #��z��5v      $	��8,0q�5w      $��0q�5x      $y�	Collapsed=�5z      #& z% 5|      ' 5y      �� *'%(Linq:ComboBoxExtensions.Placeholder)+ �� *TemplatedParent+��5{      �� *Text+$ TwoWay=� �� *TemplatedParent+$# PropertyChanged=�5|      �� 5}      $( Placeholder��5~        .  $	��7,0q�5�      $	) 0.4Q�5�      $y�	Collapsed=�5�      ��5�      �� *'%(Linq:ComboBoxExtensions.Placeholder)+ �� *TemplatedParent+5�      � 5�      $* Popup��5�      + . |�.  #��z, 5�      $��Bottom=�5�      $��Slide=�5�      .��5�      � $- DropDown��5�   !   . . #��z/ 5�   !   #��z0 5�   !   .+�5�   "   �� $ DropDownBorder��5�   %   #���  5�   %   #���  5�   %   $��1q�5�   %   �� 5�   "   $��1,2,1,1q�6/   . . .��5�   &   �� 5�   #   5�      5�      5�       5�      P� 5�       6Z   Z�  �@ SelectedItemTemplate. �5�      �� $1 ContentSite��6%    6Y   �� *SelectionBoxItemTemplate+ �� *TemplatedParent+ � ��5�      P� 5�       6b   Z� $ �@ SelectedItemTemplateSelector. �5�      �� $1 ContentSite��6%    6a   �� *ItemTemplateSelector+ �� *TemplatedParent+ � ��5�      P� 5�       �@��HasItems .  . �5�      �� $1 DropDownBorder��6%    � ��$	 125��6V   5�      P� 5�       �@��
IsEditable . . �5�      �� $1 TextBox��6%    � y�$ Visible=�6P   �� 5�      $1 ContentSite��6%    � y�$ Hidden=�6T   5�      ]� 5�      2 5�      �� 5�   "   3 �@��
IsEditable4 .  6B   �� 5�   "   4 6E   Z� 3 �@��SelectedValue./�5�      �� $1 Placeholder��6%    � y�$ Visible=�6T   5�      5�      5�      5 5�   
   P� 5�       �@J�IsMouseOver . . �5�      ��  � ��# �  6/   �� 5�       � ��# �  60   �� 5�       � ��# �  6/   5�      P� 5�       �@��
IsEditable . . �5�      ��  � �� .  6.   5�      P� 5�       �@��
IsEditable .  . �5�      ��  �@�Cursor$
 Hand��6+   5�      P� 5�       �@��IsDropDownOpen . . �5�      ��  � ��# � 	 6/   �� 5�       � ��# � 
 60   �� 5�       � ��# �  6/   5�      5�      �� 5�      "  61   8   6[   �� 5�      "  6/   8   6W   �� 5�      "  60   8   6Y   �� 5�      "  61   8   6[   �� 5�      " 	 6/   8   .�5�   
   ��  � �� 5�      �� 5�      .R�5�      �� ��6    �� *DisplayName+5�      5�      5�      �� 5�      " 
 64   8   .�5�   
   ��  �@�OverridesDefaultStyle .  62   5�      �� 5�      " ��6*   .�5�   
   ��  � ��
 ���� 6'   �� 5�   
    � ��# �  6'   �� 5�   
    � ��$
 Left=�67   �� 5�   
    �@�OverridesDefaultStyle . 62   �� 5�   
    � ��$ 6q�6$   �� 5�   
    �@J�SnapsToDevicePixels . 60   �� 5�   
    � �� 5�      �� 5�      "	 ��6"   .R�5�      �� $ Border��5�      #��z��5�      #6 z��5�      . . ._�5�      �� $
��Left=�5�      $��Center=�5�      5�       5�      P� 5�       � {� .  . �5�      ��  �@J�Opacity$	 0.6Q�68   5�      P� 5�       �@��
IsSelected . . �5�      �� # �  6;    � ���� 5�      # �  6;    � ��5�      ]� 5�      2 5�      �� 5�   "   3 �@J�IsMouseOver4 . 6C   �� 5�   "   3 �@��
IsSelected4 .  6B   ./�5�      �� # �  6;    � ��5      5     5     �� 5  
    � ��$ Center=�65   5     5     