   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      -clr-namespace:Imagin.CommonImagin.Common  5      M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089M3clr-namespace:System.ComponentModel;assembly=SystemSystem.ComponentModel 5      ?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      _4clr-namespace:Imagin.Common.Globalization.Converters&Imagin.Common.Globalization.Converters  5	      7 clr-namespace:Imagin.Common.DataImagin.Common.Data  5
      K2clr-namespace:System.Windows.Input;assembly=SystemSystem.Windows.Input 5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri;   5Imagin.Common.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter1   +Imagin.Common.Converters.CamelCaseConverter  CamelCaseConverter6   0Imagin.Common.Converters.DataGridLengthConverter  DataGridLengthConverter3   -Imagin.Common.Converters.DisplayNameConverter  DisplayNameConverter7   1Imagin.Common.Converters.ObjectToBooleanConverter  ObjectToBooleanConverter:   4Imagin.Common.Converters.ObjectToVisibilityConverter  ObjectToVisibilityConverter4   .Imagin.Common.Converters.ObjectToTypeConverter  ObjectToTypeConverter0   *Imagin.Common.Converters.ToStringConverter  ToStringConverter7	   1Imagin.Common.Converters.StringToBooleanConverter  StringToBooleanConverterA
   ;Imagin.Common.Converters.ValueEqualsParameterMultiConverter &	 "ValueEqualsParameterMultiConverter?   9Imagin.Common.Globalization.Converters.TranslateConverter 
 TranslateConverter 
 FadeIn �� TargetProperty W� From	 W� To g� Duration g� AccelerationRatio g� DecelerationRatio)   #Imagin.Common.Controls.PropertyGrid �� 
TargetType&   System.Windows.Controls.DataGrid	 �� BasedOn
 �� Property �� Value  Brush.Border*  $System.Windows.Controls.DataGridCell - )Brush.PropertyGrid.Description.Background ) %Brush.PropertyGrid.Description.Border  
Brush.Text � RowDefinitions �� 	Converter �� Mode � ToolTip �� RelativeSource(  "System.Windows.Controls.GroupStyle   ContainerStyle �� 
TargetType 9� 
IsExpanded!   Imagin.Common.Data.Ancestor'   !Imagin.Common.Linq.GridExtensions   Columns;   5Imagin.Common.Converters.GridLengthArrayTypeConverter �� Triggers �� Binding �� Value  
Brush.Link �� StringFormat!   Imagin.Common.Controls.Line   Orientation   Panel>  8System.Windows.Controls.Primitives.DataGridRowsPresenter)  #System.Windows.Controls.DataGridRow :� RoutedEvent ^� 
Conditions �� Binding �� Value ]� 
Conditions  �� Property % !Brush.DataGridRow.Even.Background ! Brush.DataGridRow.Even.Border $  Brush.DataGridRow.Odd.Background   Brush.DataGridRow.Odd.Border! P� Property" P� Value.   (Imagin.Common.Linq.DataGridRowExtensions/   )Imagin.Common.Linq.ItemsControlExtensions+   %Imagin.Common.Linq.DataGridExtensions)   #Imagin.Common.Controls.ProgressRing# �� DataType(   "Imagin.Common.Controls.MemberModel$ `� 	Converter% �� Path"   Imagin.Common.Markup.Options& ��Name' �� AncestorType( �� Triggers) �� UpdateSourceTrigger* �� 
TargetName ! Brush.GridSplitter.Background+ � 	Resources"   Imagin.Common.Data.Reference  Key,   Data-   Rows. � DataContext  BasePropertyGrid/ �� Source  Style.Border.Bottom(   "Imagin.Common.Linq.PanelExtensions0   Spacing1   SpacingExcept2   VerticalContentAlignment/   )Imagin.Common.Controls.MaskedToggleButton3   Source4   Menu-  'System.Windows.Data.CompositeCollection5 �� 
Collection$    Imagin.Common.Markup.Enumerate3!   -Imagin.Common.Controls.PropertyGridSearchName6 `� Mode$"   Imagin.Common.Controls.TextBox7 "  IsClearEnabled8 "  Placeholder #   Imagin.Common.Markup.Image9 m� IsCheckable: m� StaysOpenOnClick; m� 	IsChecked2$   ,Imagin.Common.Controls.PropertyGridGroupName1%   +Imagin.Common.Controls.PropertyGridSortName-&  'System.ComponentModel.ListSortDirection< m� Command= �� ConverterParameter)'   #Imagin.Common.Controls.MaskedButton> '  Source? �� ContentStringFormat@ � ColumnDefinitionsA K� ZIndexB J� SnapsToDevicePixelsC �� ResizeDirection.(   (Imagin.Common.Controls.PropertyCrumbViewD (  RouteE (  CrumbTemplateF (  SeparatorTemplate()   "Imagin.Common.Controls.MaskedImageG )  SourceH )  SourceHeightI )  SourceWidth)*   #Imagin.Common.Controls.PropertyViewJ *  Property$K *  ReferencePropertyStringFormat L *  ReferencePropertyTemplate(M *  !ReferencePropertyTemplateSelector�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      &Commonclr-namespace:Imagin.Common  5      FComponentModel3clr-namespace:System.ComponentModel;assembly=System  5      1Controls$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      D_Converters4clr-namespace:Imagin.Common.Globalization.Converters  5	      )Data clr-namespace:Imagin.Common.Data  5
      <Input2clr-namespace:System.Windows.Input;assembly=System  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5   
    6      *Imagin.Common.WPFStyles/DataGrid.xaml+�� 5   
    6      *Imagin.Common.WPFStyles/Menu.xaml+�� 5   
    6      *Imagin.Common.WPFStyles/TextBlock.xaml+%�<  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �     &	 �     &		 �     &	
 �     &	 �     '  Y    7 7 7  7  7  7 7  7 
 7 
 7��7  7  7 7  7  7m�7  7  7 	 7  7m�7  7  7 	 7  7m�7  7  7 	 7  7  7 	 7  7   7   7   7  7  7  7  7 
 7 
 7  7     5      6.     5      6$     5      6)     5      6&     5      6*     5      6-     5      6'     5      6#   	  5      6*   
  5      64     5      6%   �� 5!      .��5"   
   W� $ Opacity�6   $ 0X�6>   $	 1.0X�6G   $ 0:0:0.5D�6P   $	 0.4Q�6c   $	 0.4Q�6{   5#      �� 5%      "  60   8	   .�5&   
   �� 
 �@ AutoGenerateColumns .  60   �� 5'   
   
 � ��
 ���� 6'   �� 5(   
   
 � ��# �  6(   �� 5)   
   
 � ��$ 1q�6,   �� 5*   
   
 �@ CanUserAddRows .  6+   �� 5+   
   
 �@ CanUserDeleteRows .  6.   �� 5,   
   
 �@ CanUserReorderColumns .  62   �� 5-   
   
 �@ CanUserResizeColumns . 61   �� 5.   
   
 �@ CanUserResizeRows .  6.   �� 5/   
   
 �@ 	CellStyle 50      �� 51      "  6   8	  62   53      �� 54   
   
 �@ DescriptionBorderStyle 55      �� 56      " ��6   .�57      �� 
 � ��# �  63   �� 58      
 � ��# �  64   �� 59      
 � ��$ 0,1,0,0q�68   �� 5:      
 � ��$ 36��62   �� 5;      
 � ��# �  6?   �� 5<      
 �@��Padding$ 10q�60   5=      5?      �� 5@   
   
 �@ DescriptionTemplate 5A      �� 5B      .R�5C      �  5D      �� 5E      $
��Auto �6,   �� 5F      $
��Auto �6,   .+�5H      �� $
��Bold"�5I      $��0,0,0,7q�5J      $��CharacterEllipsis=�5L      ��5K      �� *DisplayName+8  $ OneWay=� 5M      �� *Text+$ OneWay=� �� *Self+�� 5N      $��1��5O      $��CharacterEllipsis=�5Q      $
��Wrap=�5R      ��5P      �� *Description+$ OneWay=� 5S      �� *Text+$ OneWay=� �� *Self+5T      5U      5W      �� 5X   
   "
 �@ EnableColumnVirtualization . 67   �� 5Y   
   
 �@ EnableRowVirtualization . 64   �� 5Z   
   
 � ��# �  6'   �� 5[   
   
 �@ DefaultCategoryName$ General��60   �� 5\   
   
 �@ DefaultGroupStyle 5]        5^       5_      �� 5`      " ��6    .�5a      �� 
 � �� 5b   "   �� 5c   &   " ��66   .R�5d   *   9� $��Stretch=�5f   -    5e   -     *GroupVisibilityM� *local:PropertyGrid++$ OneWay=���5g   .   � 5h   2   $ Auto,* 67   .+�5i   6   �� ��6E   �� ��5j   :   �� 5k   >   " ��6D    5l   B   �� 5m   F    6R   �� *Name+8   True6�   .a�5n   J   �� 
 � �� 5o   N   �� 5p   R   .R�5q   V   �� $��15(�6`   �� 5r   Z   #���  6�   �6^   �� *Name+$ OneWay=�6�    �� 5s   Z   �6^   �� *	ItemCount+$ OneWay=�$ ({0})��5t   W   5u   S   5w   K   5x   G   �� 5y   F    6R   �� *Name+8  	 False6�   .a�5z   J   �� 
 � �� 5{   N   �� 5|   R   .R�5}   V   �� $��15(�6`   �� 5~   Z   #���  6�   �6^     *DefaultCategoryNameM� *local:PropertyGrid++$ OneWay=�6�    �� 5   Z   �6^   �� *	ItemCount+$ OneWay=�$ ({0})��5�   W   5�   S   5�   K   5�   G   5�   ?   5�   7     5�   6   $��1��5�   9   $��5,0,0,0q�5�   9   $ 
Horizontal=�5�   9   5�   3   .��5�   .   �� 5�   +   5�   '   5�      5�       5�      �� 5�      .R�5�        5�      5�      5�      �� 5�   
   
 � �� 5�      �� 5�      "  6   8	  .�5�      �� 
 � ��$ 0q�68    5�      :� 5�      $ DataGridRow.Loaded��6'   .[�5�      �� 8�� 6.   5�      ^� 5�       5�      �� 5�   "    6,   �� *
Lockable+ True6I   �� 5�   "    6,   �� *Locked+ True6G   .0�5�      �� 
 � {� .  6:   5�      ^� 5�       5�      �� 5�   "    6,   �� *
Lockable+ True6I   �� 5�   "    6,   �� *Locked+	 False6G   .0�5�      �� 
 � {� . 6:   5�      �� 5�       6&   �� *
Lockable+	 False6C   .a�5�      �� 
 � {� . 6:   5�      ]� 5�       5�      �� 5�   "     �@��AlternationIndex$ 0��6U   ./�5�      �� 
 � ��# �  6;   �� 5�      
 � ��# �  6<   5�      ]� 5�       5�      �� 5�   "     �@��AlternationIndex$ 1��6U   ./�5�      �� 
 � ��# �  6;   �� 5�      
 � ��# �  6<   5�      P� 5�      ! �@ 
IsSelected" . . �5�      �� 
 �@ SelectionVisibility$ Visible=�6_   5�      5�      5�      �� 5�   
   
 �@ CanDragSelect .  6F   �� 5�   
   
 �@ Extends .  6<   �� 5�   
   
 �@ LoaderTemplate 5�      �� 5�      .R�5�        5�      5�      �� 5�   
   
 � ��$ 0q�6#   �� 5�   
   
 �@ NameAboveValue .  6+   �� 5�   
   
 �@ NameTemplate 5�      �� 5�      "#  6   .R�5�      �� $��CharacterEllipsis=�5�      $��Center=�5�      ��5�      `� 5�      8$  .1�5�   "   �� $% DisplayName�6*     5�   "   $% Language�61   5�       5�      `� 5�      8$  .1�5�   "   �� $% DisplayName�6*     5�   "   $% Language�61   5�      ��5�      �� 5�      " ��6$   8	 	  5�   "   �� 5�   &   " ��6S    62   �� *TemplateType+.a�5�   *   �� 
 � ��$  ��6A   �� 5�   *   
 �@�ToolTip$  ��6D   5�   '   5�      5�      5�      5�      �� 5�   
   
 � ��$ 0q�6$   �� 5�   
   
 �@ PropertyCrumbTemplate 5�      �� 5�      .R�5�      �� $��CharacterEllipsis=�5      ��5�      �� 8 
 5      5     5     �� 5  
   !
 �@ ReferencePropertyTemplate 5     �� 5     .R�5     � $ *,Auto 5     .+�5	     �� $& 	TextBlock��5
     $��0��5     $��CharacterEllipsis=�5     $��Center=�5      5     �� *Text+$ OneWay=� �� *Self+�� 5     $��1��5     $	��...��5     $��Center=�5     ��5     �� *ForwardCommand+ �� "'  ��5     �� 5     5     ( 5     �� 5     $
 True��6o    6&   �� *IsIndeterminate+$) PropertyChanged=�.a�5     �� $* 	TextBlock��6%   
 � ��$ (multiple values)��6L   5     �� 5     $ False��6o    6&   �� *IsIndeterminate+$) PropertyChanged=�.a�5     �� $* 	TextBlock��6%    6L   �� *Value+8  
 � ��5     5     5      �� 5!  
   
 � ��$
 Auto=�6G   �� 5"  
   
 � ��$
 Auto=�6E   �� 5#  
   
 �@ SelectionMode$ Single=�6*   �� 5$  
   
 �@ SplitterStyle 5%     �� 5&     " ��6   .�5'     �� 
 � ��# �  63   �� 5(     
 � ��$ 3��6/   �� 5)     
 � ��$ Stretch=�6<   �� 5*     
 �@��ResizeBehavior$ PreviousAndNext=�67   �� 5+     
 �@��ResizeDirection$
 Rows=�68   �� 5,     
 � ��$	 Top=�6:   �� 5-     
 � ��$
 Auto��6.   5.     50     �� 51  
   
 � �� 52     �� 53     "  6"   .R�54     �� #��z��55     #��z��56     #��z��57     + 58     ��  59     BasePropertyGrid , 6G   ��  �� "'  �� 5;     '        "  6$   8	  .�5<  "   �� 
 � ��$
 10,1q�6<   5=     5>     ._�5?     � $- Auto,*,Auto,Auto 5A     . 5@     �� *Data+8/  .+�5C     �� $��30��5D  !   #���  5E  !   y�5F  !   �� *HeaderVisibility+$ OneWay=�._�5G  "   � $	��5,0q�5I  %   $  Auto,*,Auto,Auto,Auto,Auto 5J  %   $0 0,0,5,0q�5K  %   $
1 Last=�5L  %   $2 Center=�5M  %   {�5H  %   �� *Source+8  .+�5P  &     $��0��5Q  )   $J3 Dpack://application:,,,/Imagin.Common.WPF;component/Images/Search.png��5R  )   $ Search��5S  )   4 5T  *   �� 5U  .   . 6:   �� *PlacementTarget.DataContext+ �� *Self+��5V  2     5W  6   �� 5X  :   5 6N      *local:PropertyGridSearchName+5Y  7   + 5[  2   ���� 5\  6   'm�       " m�6<   8	  .�5]  :   �� 
 �@m�Command 6T   �� *Data.SearchCommand+8/  �� 5^  :   
 �@m�CommandParameter 6]   �� $ OneWay=��� 5_  :   
 �@m�StaysOpenOnClick . 6]    5`  :   �� 5a  >   " ! 6�    6J   �� 8  .a�5b  B   �� 
 �@m�	IsChecked 5c  F   `� 5d  J   8$  $6 OneWay=�6�   .1�5e  N   �� $% Data.SearchName�6V   8/  6m   �� 5f  N   $% .�6V   5g  K   5i  C   5j  ?   5l  7   5m  3   5n  /   5p  '   "  5q  &   $��1��5r  )   
������ $��0q�5t  )   7 . $��0q�5v  )   $8 Search��5w  )   $ Search��5y  )   ��5x  )   �� *Search+$ TwoWay=�$) PropertyChanged=�5y  )   �� 5|  &   $��2��5}  )   $��Center=�5  )   ��5~  )   �� *HeaderButtons+��5�  *   �� 5�  .   .R�5�  2   �� ��6C   �� $ OneWay=�5�  /   5�  '     5�  &   $��3��5�  )   $ Group��5�  )   3 5�  )   #  *Imagin.Common.WpfImages/Category.png+4 5�  *   �� 5�  .   . 6:   �� *PlacementTarget.DataContext+ �� *Self+��5�  2     5�  6   m� 5�  :   $��Collapse��6C   9 . : . ; 5�  =   �� *GroupVisibility+5�  =   �� 5�  :   �� 5�  :   5 6N      *local:PropertyGridGroupName+5�  7   + 5�  2   ���� 5�  6   'm�       " m�6<   8	  .�5�  :   �� 
 �@m�Command 6T   �� *Data.GroupCommand+8/  �� 5�  :   
 �@m�CommandParameter 6]   �� $ OneWay=��� 5�  :   
 �@m�StaysOpenOnClick . 6]    5�  :   �� 5�  >   " $ 6�    6J   �� 8  .a�5�  B   �� 
 �@m�	IsChecked 5�  F   `� 5�  J   8$  $6 OneWay=�6�   .1�5�  N   �� $% Data.GroupName�6V   8/  6l   �� 5�  N   $% .�6V   5�  K   5�  C   5�  ?   5�  7   5�  3   5�  /   5�  '     5�  &   $��4��5�  )   $
 Sort��5�  )   3 5�  )   #  *Imagin.Common.WpfImages/Sort.png+4 5�  *   �� 5�  .   . 6:   �� *PlacementTarget.DataContext+ �� *Self+��5�  2     5�  6   �� 5�  :   5 6N      *local:PropertyGridSortName+�� 5�  :   �� 5�  :   5 6N      *" ComponentModel:ListSortDirection+5�  7   + 5�  2   ���� 5�  6   'm�       " m�6<   8	  .�5�  :   �� 
 �@m�Command 6T   �� *Data.SortCommand+8/  �� 5�  :   
 �@m�CommandParameter 6]   �� $ OneWay=��� 5�  :   
 �@m�StaysOpenOnClick . 6]    5�  :   �� 5�  >   " % 6�    6J   �� 8  .a�5�  B   �� 
 �@m�	IsChecked 5�  F   `� 5�  J   8$  $6 OneWay=�6�   .1�5�  N   �� $% Data.SortName�6V   8/  6k   �� 5�  N   $% .�6V   5�  K   5�  C   5�  ?   �� 5�  >   " & 6�    6J   �� 8  .a�5�  B   �� 
 �@m�	IsChecked 5�  F   `� 5�  J   8$  $6 OneWay=�6�   .1�5�  N   �� $% Data.SortDirection�6V   8/   6p   �� 5�  N   $% .�6V   5�  K   5�  C   5�  ?   5�  7   5�  3   5�  /   5�  '     5�  &   $��5��5�  )   $ Options��5�  )   3 5�  )   #  *Imagin.Common.WpfImages/ArrowDown.png+4 5�  *   �� 5�  .   . 6:   �� *PlacementTarget.DataContext+ �� *Self+.@�5�  2   m� $��Reset��6;   < 5�  5   �� *ResetCommand+5   /   5  '   5  #   5     �� 5     $��1��5  !   $��Center=�5
  !   $��Center=�5  !   ��5	  !   �� *LoaderTemplate+y�5  !   �� *	Loading+8 ! � 5     $��1��5  !   $- Auto,Auto,* 5  !   y�5  !   �� *	Loading+8 " $= 1��.+�5  "   � $ Auto,* 5  %   $��
10,10,10,5q�5  %   y�5  %   �� *TypeVisibility+.+�5  &   '  $��0��5  )   $��0,-1,5,0q�5  )   $
 Back��5  )   $��Center=�5  )   ��5  )   �� *BackwardCommand+> 5  )   #  *Imagin.Common.WpfImages/ArrowLeftRound.png+y�5  )   �� *	IsEnabled+8 # $ OneWay=� �� *Self+$) Default=��� 5  &   $��1��5   )   $��Center=�5%  )   ��5!  )   �� *Route+? 5"  )   �� *TypeStringFormat+��5#  )   �� *TypeTemplate+��5$  )   �� *TypeTemplateSelector+��5&  *   �� 5'  .   " ��64    5(  2   �� 5)  6    6B   �� *Source+8 $ 	 False6�   .a�5*  :   �� 
 � y�$ 	Collapsed=�6W   5+  7   �� 5,  6    6B   �� *Source+8 %  True6�   .a�5-  :   �� 
 � y� 6W   �� *TypeVisibility+ �� *TemplatedParent+5.  7   50  /   52  '   53  #   � 56  "   $��1��57  %   $��	10,5,10,0q�58  %   y�59  %   �� *Properties.Featured+8 & $ OneWay=�.+�5:  &   �� ��5;  )   �� *Properties.Featured+��5<  *   �� 5=  .   .R�5>  6   � $��0,0,0,5q�5?  9   @ 5@  :   �� 5A  >   $
��Auto �6O   �� 5B  >   $��* �6O   .+�5D  :   �� $��0��5E  =   $��0,0,10,0q�5F  =   $��CharacterEllipsis=�5G  =   $��Center=�5H  =   y�5I  =   �� *Data.NameColumnVisibility+8/ ' ��5J  >   `� 5K  B   8$ ( .1�5L  F   �� $% DisplayName�6N     5M  F   $% Language�6U   5N  C    5P  >   `� 5Q  B   8$ ) .1�5R  F   �� $% DisplayName�6N     5S  F   $% Language�6U   5T  C   5V  ;   �� 5W  :   $��1��5X  =   ��5Y  =   �� ��5Z  =   �� *Data.ValueTemplate+8/ * 5[  7   5\  /   ��5^  *   �� 5_  .   .R�5`  2   �� 5a  /   5c  '   5d  #   �� 5g  "   $��2��5h  %   $A 1��5i  %   |�.  .��5k  &   �� #��z��5l  )   #B zB 5m  )   5n  #   5o     �� 5r     $��2��5s  !   $
C Rows=�5t  !   ��5u  !   �� *SplitterStyle+y�5v  !   �� *SplitterVisibility+�� 5x     $��3��5y  !   ��5z  !   �� *DescriptionBorderStyle+y�5{  !   �� *DescriptionVisibility+._�5|  "   �� ��5}  %   �� *Properties.Active+? 5~  %   �� *DescriptionStringFormat+��5  %   �� *DescriptionTemplate+��5�  %   �� *DescriptionTemplateSelector+��5�  &   �� 5�  *   " ��60    5�  .   �� 5�  2    6>   �� *Properties.Active+ 6d   Z� .a�5�  6   �� 
 � y�$ 	Collapsed=�6S   5�  3   5�  +   5�  #   5�     5�     5�     5�     5�     �� 5�  
   
 �@ TypeTemplate 5�     �� 5�     .R�5�     (  D 5�     �� E 5�     �� *PropertyCrumbTemplate+ �� "'  F 5�     �� 5�     .R�5�  "   )  $JG Dpack://application:,,,/Imagin.Common.WPF;component/Images/ArrowP.png��5�  %   $H 6Q�5�  %   $I 6Q�5�  %   5�     5�     5�     5�     �� 5�  
   
 �@ ValueTemplate 5�     �� 5�     .R�5�     � $- Auto,* 6   $��2q�6=   .+�5�     �� $��0��5�     $��0,0,0,5q�5�     ��5�     �� ��5�       *NameTemplateM� *local:PropertyGrid++y�5�       *NameAboveValueM� *local:PropertyGrid++8 + *  5�     $��1��5�     J 5�     �� K 5�       *ReferencePropertyStringFormatM� *local:PropertyGrid++L 5�       *ReferencePropertyTemplateM� *local:PropertyGrid++M 5�       *#!ReferencePropertyTemplateSelectorM� *local:PropertyGrid++5�     5�     5�     �� 5�  
    
 �@%�IsContainerVirtualizable . 6G   �� 5�  
   
 �@%�IsVirtualizing . 6=   �� 5�  
   
 �@%�VirtualizationMode$ 	Recycling=�6A   5�     5�     