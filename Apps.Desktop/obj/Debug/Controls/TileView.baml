   M S B A M L   `   `   `  ����B  >Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:DesktopDesktop  5      3clr-namespace:Desktop.ControlsDesktop.Controls  5      -clr-namespace:Desktop.TilesDesktop.Tiles  5          Desktop.TileView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      L HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null(i http://imagin.tech/imagin/common  5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      'localclr-namespace:Desktop     5      3Controlsclr-namespace:Desktop.Controls     5      -Tilesclr-namespace:Desktop.Tiles     5        � 	Resources  5	      "  Imagin.Common.Data.Reference   TileView   Data!  Imagin.Common.Data.Ancestor>  8Imagin.Common.Converters.ColorToSolidColorBrushConverter # ColorToSolidColorBrushConverter0  *Imagin.Common.Converters.FileNameConverter  FileNameConverter/  )Imagin.Common.Converters.SecondsConverter  SecondsConverter5  /Imagin.Common.Converters.SortDirectionConverter  SortDirectionConverter��%�   &	        &	 6     &	 I     &	 \     &	 o       5
   
    6'     *M� *local:TileView++  5   
   6,     5   
   6     5   
   6     5   
   6#   5      .��5      � .+�5   
   �   Brush.Window.Background#���  5       J� Opacity 5      "  Imagin.Common.Markup.Options  *TileOpacity+!   Desktop.Controls.HeaderView  5   
      HeaderAlignment 5      �� *Data.HeaderAlignment+ �� Source# [     HeaderPlacement 5      �� *Data.HeaderPlacement+# [     Header 5      �� 5      ��5      ��   5       �� DataType	   Desktop.Tiles.ClockTile%
  Imagin.Common.Controls.LabelBox 
  
MouseEvent	 
  Label
 �� Mode � ToolTip �� RelativeSource!   Desktop.Tiles.CountDownTile$  Imagin.Common.Controls.TextBox   Placeholder   Desktop.Tiles.ImageTile$  Imagin.Common.Controls.FileBox   
BrowseMode   	CanBrowse   CanValidate   IconVisibility   BrowseButtonVisibility   Label �� 	Converter �� UpdateSourceTrigger   Desktop.Tiles.NoteTile   Desktop.Tiles.FolderTile��%�  (��       5   '   *,	 +)(�� �     5"   '   *, +)(�� �    5)   '   *, +)7  (�� �    55   '   *, +)(�� j    5<   '   *, +)7  �� 5      " 	 6'   .R�5      
  $ 	MouseDown=�5   !   	 5   !   �� *
DateTime+��5   !     *ClockDateTimeFormat+$
 TwoWay=� 5    !   �� *Label+$
 OneWay=� �� *Self+5!      �� 5"      "  6'   .R�5#      
  $ 	MouseDown=�5$   !   $ Title��5%   !   ��5&   !   �� *Title+$
 TwoWay=� 5'   !   �� *Text+$
 OneWay=� �� *Self+5(      �� 5)      "  6'   .R�5*        $
 Open=�5+   !    .  . $ Visible=�5.   !   $ File or folder path��50   !    .  5/   !   �� *Path+8   ��52   !   �� *Path+$
 TwoWay=�$ 	LostFocus=� 53   !   �� *Path+54      �� 55      "  6'   .R�56      
  $ 	MouseDown=�57   !   $ Title��58   !   ��59   !   �� *Title+$
 TwoWay=� 5:   !   �� *Text+$
 OneWay=� �� *Self+5;      �� 5<      "  6'   .R�5=         . $ 
OpenFolder=�5?   !    .  . $ Visible=�5B   !   $ Folder path��5D   !    5C   !   �� *Path+8   ��5E   !   �� *Path+$
 TwoWay=�$ 	LostFocus=� 5F   !   �� *Path+5G      5H      5I         Body 5K      �� 5L      ��6!   ��   5M       (� Stretch (� StretchDirection"  Imagin.Common.Controls.Clock   
PathStroke �� ConverterParameter �� StringFormat&   Imagin.Common.Controls.Slideshow   BackgroundOpacity   Interval   PauseOnMouseOver   Path    
Transition! �� AcceptsReturn*  $Imagin.Common.Linq.ControlExtensions"   FontSizeWheel# �� TextWrapping$  Imagin.Common.Controls.Browser$   
IsReadOnly%   ItemBackground&   
ItemBorder'   ItemForeground(   ItemMouseOverBackground)   ItemMouseOverBorder*   ItemMouseOverForeground#+   ItemSelectedActiveBackground,   ItemSelectedActiveBorder#-   ItemSelectedActiveForeground%.   ItemSelectedInactiveBackground!/   ItemSelectedInactiveBorder%0   ItemSelectedInactiveForeground1   ItemSize2   	GroupName3   Path4   SortDirection5   SortName6   ViewFileExtensions7   ViewHiddenItems��%<  (��       5N   '   *,	 +)(�� �     5X   '   *, +)7  (�� .    5d   '   *, +)(�� �    5l   '   *, +)(�� $    5x   '   *, +)7  �� 5N      " 	 6'   .R�5O      (� $��0��5P   !   $ Uniform=�5Q   !   $
 Both=�5R   !   .��5S   "     $��5q�5T   %   
 ���� 5U   %   5V      5W      �� 5X      "  6'   .R�5Y      (� $ Uniform=�6&   $
 Both=�68   .��5Z   "   
  $��36(�5[   %   $��Center=�5\   %   $ 	MouseDown=�5^   %   $��Center=�5a   %   	 5]   %   �� *Date+8   $ 1����5_   %   �� *Date+$
 TwoWay=�$ M/d/yyyy h:mm tt��$ 	LostFocus=� 5`   %   �� *Label+$
 OneWay=� �� *Self+5a   %   5b      5c      �� 5d      "  6'   .R�5e        $ 1Q�5f   !    5g   !   �� *
Interval+ 5h   !     *SlideshowPauseOnMouseOver+ 5i   !   �� *Path+  5j   !   �� *
Transition+5k      �� 5l      "  6'   .R�5m        ! . " . $��Stretch=�5r   !   $
# Wrap=�5t   !   $��Stretch=�5u   !   $	��Top=�5v   !   ��5o   !     *NoteFontFamily+$
 OneWay=���5p   !     *NoteFontSize+$
 OneWay=���5s   !   �� *Text+$
 TwoWay=�5v   !   5w      �� 5x      "  6'   .R�5y        -   5        
������ $��0q�5{   !   $ . $% Transparent��5   !   $& Transparent��5�   !   $
' #FFF��5�   !   $( 	#33FFFFFF��5�   !   $) 	#44FFFFFF��5�   !   $
* #FFF��5�   !   $+ 	#55FFFFFF��5�   !   $, 	#66FFFFFF��5�   !   $
- #FFF��5�   !   $. 	#11FFFFFF��5�   !   $/ 	#22FFFFFF��5�   !   $
0 #FFF��5�   !   1 5�   !     *
ItemSize+$
 TwoWay=�2 5�   !   �� *	GroupName+3 5�   !   �� *Path+$
 TwoWay=�4 5�   !   �� *SortDirection+8   5 5�   !   �� *
SortName+6 5�   !     *ViewFileExtensions+7 5�   !     *ViewHiddenItems+5�      5�      5�      5�      5�      5�      5�      