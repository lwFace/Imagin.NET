   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      7 clr-namespace:Imagin.Common.DataImagin.Common.Data  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5	      ?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5
      %    Imagin.Common.Controls.Explorer   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =
Converters&clr-namespace:Imagin.Common.Converters     5      1Data clr-namespace:Imagin.Common.Data     5      1Linq clr-namespace:Imagin.Common.Linq     5	      6local$clr-namespace:Imagin.Common.Controls     5
        � 	Resources  5      0   *Imagin.Common.Converters.FileSizeConverter   FileSizeConverter9   3Imagin.Common.Converters.Int32ToVisibilityConverter  Int32ToVisibilityConverter4   .Imagin.Common.Converters.ObjectToTypeConverter  ObjectToTypeConverter.   (Imagin.Common.Converters.PluralConverter  PluralConverter<   6Imagin.Common.Converters.ValueEqualsParameterConverter ! ValueEqualsParameterConverterA   ;Imagin.Common.Converters.ValueEqualsParameterMultiConverter & "ValueEqualsParameterMultiConverter"   Imagin.Common.Data.Reference  Browser   Data �� ElementName  Explorer �� RelativeSource �� AncestorType%   Imagin.Common.Controls.Explorer��%  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �       5   
   6'     5   
   60     5   
   6+     5   
   6%     5   
   63     5   
   68     5   
    6)   �� $ PART_Browser��  5   
    6*   ��  �� "  5      .��5      � '	   !Imagin.Common.Linq.GridExtensions 	  Rows;
   5Imagin.Common.Converters.GridLengthArrayTypeConverter$ Auto,*
 5   	    � DataContext 5   	   �� *Data+ �� Source# [ .+�5   
   '   !Imagin.Common.Controls.AddressBar  '   !Imagin.Common.Controls.AddressBar   	Refreshed$ OnRefreshed��5#      $��0��5      $��5q�5       	   	Favorites	 5      �� *	Favorites+
 �� Mode$
 OneWay=�   History 5      �� *	History+$
 TwoWay=�   Path 5!      �� *Path+   Progress 5"      �� *Items.Progress+$ PART_Browser��5#      � 5�   
   $��1��5�       	  Columns$ 30*,Auto,70*
 5�      .+�5�      &    Imagin.Common.Controls.Navigator  $��0��5�      ���	#22000000$��0,0,1,0q�5�      $��0,0,10,0q�5�         	ViewFiles .     Path 5�      �� *Path+$
 TwoWay=�   Root 5�      �� *Root+$
 TwoWay=�   ViewHiddenItems 5�      �� *ViewHiddenItems+$
 TwoWay=��� 5�      $��1��5�      
������  �� ResizeBehavior$ PreviousAndNext=�5�       �� ResizeDirection$ Columns=�5�       �� ShowsPreview . $��5��5�      � 5�      $��2��5�      $ *,Auto
 5�      .+�5�      $   Imagin.Common.Controls.Browser  -   5          Name$ PART_Browser��5�      $   Imagin.Common.Controls.Browser   
FileOpened$ OnFileOpened��5�      $��0��5�         OpenFilesOnClick .   5�      �� *Data+# [    FileExtensions 5�      �� *FileExtensions+$
 OneWay=�   	GroupName 5�      �� *	GroupName+$
 TwoWay=�   History 5�      �� *	History+$
 TwoWay=�   ItemSize 5�      �� *
ViewSize+$
 TwoWay=�   Path 5�      �� *Path+$
 TwoWay=�   	Selection 5�      �� *	Selection+$
 TwoWay=�   SortDirection 5�      �� *SortDirection+$
 TwoWay=�    SortName  5�      �� *
SortName+$
 TwoWay=�!   View! 5�      �� *View+$
 TwoWay=�"   	ViewFiles" 5�      �� *	ViewFiles+$
 TwoWay=�#   ViewFileExtensions# 5�      �� *ViewFileExtensions+$   ViewHiddenItems$ 5�      �� *ViewHiddenItems+5      �� 5     $��1��5     ���	#22000000$��0,1,0,0q�5     $��10,5,10,2.5q�5     ._�5     � .+�5     �� $��	0,7.5,0,5q�5     % J� Opacity$	% 0.4Q�5	     �� 5
     �6"   �� *Items.Count+$ PART_Browser��$
 OneWay=�& �� StringFormat$& 	 {0} item���� 6�   �6�   �� *Items.Count+$ PART_Browser��' �� 	Converter#' [ $
 OneWay=� (�� 5     �6#   �� *Length+#' [  $ PART_Browser��$
 OneWay=�)5     �� 5     $��Right=�5     $��	0,7.5,0,5q�5     $	% 0.4Q�5     y�5     �� *SelectionCount+$ PART_Browser��#' [ �� 5     �6"   �� *SelectionCount+$ PART_Browser��$
 OneWay=�$& 	 {0} item���� 6�   �6�   �� *SelectionCount+$ PART_Browser��#' [ $
 OneWay=� �� 6�   $�selected��6   (�� 5     �6#   �� *SelectionLength+#' [  $ PART_Browser��$
 OneWay=�)5     5     5     5     5     5     5     