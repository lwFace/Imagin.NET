   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_4clr-namespace:Imagin.Common.Globalization.Extensions&Imagin.Common.Globalization.Extensions  5      ?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.DataImagin.Common.Data  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5	      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri   Style.TextBox �� 
TargetType �� BasedOn �� Property  Brush.TextBox.Background �� Value  Brush.TextBox.Border  Brush.TextBox.Caret � DataContext �� RelativeSource.  (System.Windows.Input.ApplicationCommands
   Cut	 m� Command
 m� InputGestureText9   3Imagin.Common.Globalization.Extensions.LocExtension m� Icon    Imagin.Common.Markup.Image   Copy   Paste  
Brush.Text  Brush.TextBox.Selection �� Triggers P� Property P� Value " Brush.TextBox.MouseOver.Border   Brush.TextBox.Focused.Border $  Brush.TextBox.Disabled.Selection$   Imagin.Common.Controls.TextBox �� Mode � 	Resources;   5Imagin.Common.Converters.BooleanToVisibilityConverter 	 Key)   #Imagin.Common.Controls.MaskedButton   Source � ToolTip  
 BooleanToVisibilityConverter �� 	Converter:   4Imagin.Common.Converters.StringToVisibilityConverter J� Opacity J� SnapsToDevicePixels  StringToVisibilityConverter �� ConverterParameter6   0Imagin.Common.Converters.InverseBooleanConverter-	   'Imagin.Common.Controls.MaskedToggleMenu/
   )Imagin.Common.Controls.MaskedToggleButton 
  Source d� 	IsChecked  InverseBooleanConverter%   Imagin.Common.Controls.LabelBox#   Imagin.Common.Controls.UriBox'   !Imagin.Common.Controls.VersionBox#   Imagin.Common.Controls.UpDown   SourceHeight   SourceWidth'   !Imagin.Common.Controls.ByteUpDown*   $Imagin.Common.Controls.DecimalUpDown)   #Imagin.Common.Controls.DoubleUpDown)   #Imagin.Common.Controls.SingleUpDown(   "Imagin.Common.Controls.Int16UpDown(   "Imagin.Common.Controls.Int32UpDown(   "Imagin.Common.Controls.Int64UpDown*   $Imagin.Common.Controls.UDoubleUpDown)   #Imagin.Common.Controls.UInt16UpDown)   #Imagin.Common.Controls.UInt32UpDown)   #Imagin.Common.Controls.UInt64UpDown+   %Imagin.Common.Controls.DateTimeUpDown  Brush.Popup.Background  Brush.Popup.BorderBrush+   %Imagin.Common.Controls.TimeSpanUpDown'   !Imagin.Common.Controls.TimeUpDown,   &Imagin.Common.Controls.AlphaNumericBox$   Imagin.Common.Controls.FileBox   SourceColor(   "Imagin.Common.Controls.MaskedImage  	Brush.Red �� Binding!    Imagin.Common.Data.Ancestor �� Value  Brush.Green%!   Imagin.Common.Controls.RegexBox  �� 
TargetType&"    Imagin.Common.Controls.TokenView*#   $Imagin.Common.Linq.TextBoxExtensions3$   -Imagin.Common.Linq.FrameworkElementExtensions�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      :l4clr-namespace:Imagin.Common.Globalization.Extensions  5      .local$clr-namespace:Imagin.Common.Controls  5      5
Converters&clr-namespace:Imagin.Common.Converters  5      )Data clr-namespace:Imagin.Common.Data  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5	        5
      �� 5   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+%"  &	        7�'�� #    7   '  E    7   7 
 7  7  '  �    7 '  r    7 '  �    7 '  �    7 '  �    7 '  �    7 '  �    7 '  	    7 '  +    7 '  M    7 '  o    7 '  �    7 '  �    7 '  �    7 '  �    7 '      7 '  t    7 '  �    7 '  �    7 '  �    7 7 '!  `    7 '�� �    7   '"  l    7���� 5      " ��6"   8   .�5   
   ��  � ��# �  6'   �� 5   
    � ��# �  6(   �� 5   
    � ��$ 1q�6,   �� 5   
    �@��
CaretBrush# �  6'   �� 5   
    �@�ContextMenu 5      �� 5       6   �� *PlacementTarget+ �� *Self+.@�5      m� #	 Z 5      $
 Ctrl + X��5      ��5      �� *ActualFontFamily+��5        *Imagin.Common.WPF:Main:Cut+ 5        *Imagin.Common.WPFImages/Cut.png+5      m� 5      #	 Z 5      $
 Ctrl + C��5!      ��5      �� *ActualFontFamily+��5        *Imagin.Common.WPF:Main:Copy+ 5         *Imagin.Common.WPFImages/Copy.png+5!      m� 5"      #	 Z 5#      $
 Ctrl + V��5'      ��5$      �� *ActualFontFamily+��5%        *Imagin.Common.WPF:Main:Paste+ 5&        *Imagin.Common.WPFImages/Paste.png+5'      5(      5*      �� 5+   
    � ��$ 13(�6%   �� 5,   
    � ��# �  6'   �� 5-   
    � ��$ Stretch=�67   �� 5.   
   + �@��#IsInactiveSelectionHighlightEnabled .  6@   �� 5/   
    � ��$ 6q�6$   �� 50   
    �@��SelectionBrush# �  6+   �� 51   
    �@��SelectionOpacity$	 0.4Q�6-   �� 52   
    � ��$ Hidden=�6G   �� 53   
    �@J�SnapsToDevicePixels . 60   �� 54   
    � ��$ Center=�65    55   
   P� 56       �@J�IsMouseOver . . �57      ��  � ��# �  60   58      P� 59       �@J�	IsFocused . . �5:      ��  � ��# �  60   5;      P� 5<       �@��
IsReadOnly . . �5=      ��  �@��SelectionBrush# �  63   �� 5>       �@J�Opacity$	 0.6Q�6,   5?      P� 5@       � {� .  . �5A      ��  �@��SelectionBrush# �  63   5B      5D      �� 5H      " ��6%   8   6C   �� 5J      "  6+   8   .�5K   
   ��  �@ ActualFontFamily 6-   �� *
FontFamily+$ OneTime=� �� *Self+�� 5L   
    �@ ClearButtonTemplate 5M      �� 5N       5O      ��  5P       BooleanToVisibilityConverter	 6B   5Q      .R�5R        |�.  $J Dpack://application:,,,/Imagin.Common.WPF;component/Images/XRound.png��5V      $��0,0,5,0q�5W      $ Clear��5X       5S      �� *Data+��5T      �� *ClearCommand+y�5Y      �� *	IsEnabled+8   �� *Self+5Z      5\      �� 5]   
    �@ EnterButtonTemplate 5^      �� 5_      .R�5`        |�.  $��0,0,5,0q�5d      $M Gpack://application:,,,/Imagin.Common.WPF;component/Images/NextArrow.png��5e      $ Enter��5f       5a      �� *Data+��5b      �� *EnterCommand+5f      5g      5i      �� 5j   
    �@ PlaceholderTemplate 5k      �� 5l       5m      ��  5n      StringToVisibilityConverter	 6A   5o      .R�5p      �� $	 0.4Q�5z       5q      �� *Data+��5r      �� *ActualFontFamily+��5s      �� *
FontSize+��5t      �� *FontStretch+��5u      �� *	FontStyle+��5v      �� *
FontWeight+��5w      �� *
Foreground+��5x      �� *HorizontalContentAlignment+��5y      �� *	Padding+ 5{      �� *SnapsToDevicePixels+��5|      �� *Placeholder+��5}      �� *TextTrimming+��5~      �� *TextWrapping+��5      �� *VerticalContentAlignment+y�5�      �� *Text+8  $ 1��5�      5�      �� 5�   
    �@ ToggleButtonTemplate 5�      �� 5�       5�      ��  5�      InverseBooleanConverter	 6=   5�      .R�5�      	  |�.  $��0,0,5,0q�5�      $J Dpack://application:,,,/Imagin.Common.WPF;component/Images/Toggle.png��5�      $��Center=�5�       5�      �� *Data+ 5�      �� *MaskCharacters+8  5�      5�      5�       5�   
   P� 5�       �@ MaskCharacters . . �5�      ��  � ��$T Npack://application:,,,/Imagin.Common.WPF;component/Fonts/Password.ttf#Password)�6/   5�      5�      �� 5�      "  6,   8   .�5�   
   ��  � ��$ 4q�6$    5�   
   P� 5�       �@ 
IsEditable . . �5�      ��  �@�Cursor$ IBeam��6+   �� 5�       � |� . 6.   5�      P� 5�       �@ 
IsEditable .  . �5�      ��  �@�ContextMenu 60   Z� �� 5�       �@�Cursor$
 Hand��6+   �� 5�       � |� .  6.   5�      5�      �� 5�      "  6*   8   6M   �� 5�      "  6.   8   6U   �� 5�      "  6*   8   .�5�   
   ��  �@ DownButtonTemplate 5�      �� 5�      .R�5�        $
��Down��5�      |�.  $M Gpack://application:,,,/Imagin.Common.WPF;component/Images/ArrowDown.png��5�      $ 9Q�5�      $ 9Q�5�      $ Decrease��5�       5�      �� *Data+$ OneWay=���5�      �� *DownCommand+5�      5�      5�      �� 5�   
    �@ UpButtonTemplate 5�      �� 5�      .R�5�        $��Up��5�      |�.  $K Epack://application:,,,/Imagin.Common.WPF;component/Images/ArrowUp.png��5�      $ 9Q�5�      $ 9Q�5�      $ Increase��5�       5�      �� *Data+$ OneWay=���5�      �� *	UpCommand+5�      5�      5�      5�      �� 5�      "  6.   8   6U   �� 5�      "  61   8   6[   �� 5�      "  60   8   6Y   �� 5�      "  60   8   6Y   �� 5�      "  6/   8   6W   �� 5�      "  6/   8   6W   �� 5�      "  6/   8   6W   �� 5�      "  61   8   6[   �� 5�      "  60   8   6Y   �� 5�      "  60   8   6Y   �� 5�      "  60   8   6Y   �� 5�      "  62   8   .�5�   
   ��  �@ DropDownStretch$
 None=�6,   �� 5�   
    �@ DropDownStyle 5�      �� 5�      " ��6   .�5�      ��  � ��# �  63   �� 5�       � ��# �  64   �� 5�       � ��$ 1q�68   5�      5�      �� 5�   
    �@ StaysOpenOnSelection . 61   5�      �� 5�      "  62   8   6]   �� 5�      "  6.   8   .�5   
   ��  �@ DropDownStyle 5     �� 5     " ��6   .�5     ��  � ��# �  63   �� 5      � ��# �  64   �� 5      � ��$ 1q�68   5     5     5	     �� 5     "  63   8   6_   �� 5     "  6+   8   .�5  
   ��  �@ BrowseButtonTemplate 5     �� 5     .R�5       |�.  $��0,0,5,0q�5     $J Dpack://application:,,,/Imagin.Common.WPF;component/Images/Folder.png��5      5     �� *Data+$ OneWay=���5     �� *BrowseCommand+ 5     �� *
Foreground+5     5     5     �� 5  
    �@ BrowseButtonToolTip$ Browse��60   �� 5  
    �@ BrowseButtonVisibility . 63   �� 5  
    �@ 
BrowseMode$ OpenFile=�6'   �� 5   
    �@ BrowseTitle$ 	Browse...��6(   �� 5!  
    �@ ValidateTemplate 5"     �� 5#     .R�5$       $��0,0,5,0q�6(   ��5%     �� 5&     "  6$   8  .�5'  "   ��  �@ Source 6;     *Imagin.Common.WPFImages/XRound.png+�� 5(  "    �@ SourceColor# �  6@    5)  "   �� 5*  &    62      *	IsValidM� *local:FileBox++ True6l   .a�5+  *   ��  �@ Source 6C     *Imagin.Common.WPFImages/CheckmarkRound.png+�� 5,  *    �@ SourceColor# �  6H   5-  '   5/     51     52     54     �� 55  
    �@ ValidateToolTip 6,   Z� 56     �� 58     " ! 6,   8   6Q   �� 5<     " ��6)   8   .�5=  
   ��  �@�OverridesDefaultStyle . 62   �� 5>  
    � ��$
 Left=�67   �� 5?  
    � ��$ 10q�6$   �� 5@  
    � ��$ Center=�65   �� 5A  
    � ��$
 Auto=�6E   �� 5B  
    � �� 5C     �� 5D     "  ��6"   .R�5E     �� $��Bd��5F     #��z��5G     #��z��5H     #��z��5I      . ._�5K     �� $��PART_ContentHost��5L     # z 5M     5N     5O     5Q     5R     �� 5T     " " 6-   8   .�5U  
   ��  �@# EnableCopyCommand .  6E   �� 5V  
    �@# EnableCutCommand .  6D   �� 5W  
    �@# EnablePasteCommand .  6F   �� 5X  
    �@$ EnableContextMenu .  6N   �� 5Y  
    � ��$	 3,7q�6$   5Z     5[     