   M S B A M L   `   `   `  ����@  <Paint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PaintPaint  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089L HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null  �� DataType    Paint.HistogramPanel � 	Resources>  8Imagin.Common.Converters.ColorToSolidColorBrushConverter   Key"  Imagin.Common.Markup.Options # ColorToSolidColorBrushConverter �� 	Converter �� Mode � 	Resources"  Imagin.Common.Data.Reference   Data � ContextMenu � DataContext  HistogramViewModel �� Source	 m� IsCheckable
 m� 	IsChecked m� Command �� Stretch J� Opacity � Points   BooleanToVisibilityConverter�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      localclr-namespace:Paint  5      (i http://imagin.tech/imagin/common  5      %�  (��       5      *,  +)7  7  7  7  7  7  7  7  7  7  7  7  �� 6   "    6    5   
   ��  5      #ColorToSolidColorBrushConverter  60   5	      .R�5
   
   � ��6     *HistogramBackground+8   $ OneWay=� 5      ��  5      HistogramViewModel   69   �� *.+5       5      �� 5       6   �� *Data+8  .@�5      m� $
��Blue��5      	 . 
 5      �� *
ShowBlue+$ TwoWay=�m� 5      $��Green��5      	 . 
 5      �� *	ShowGreen+$ TwoWay=�m� 5      $	��Red��5      	 . 
 5      �� *	ShowRed+$ TwoWay=��� 5      m� 5      $��	Luminance��5      	 . 
 5       �� *ShowLuminance+$ TwoWay=�m� 5!      $��
Saturation��5"      	 . 
 5$      �� *ShowSaturation+$ TwoWay=��� 5%      m� 5&      $��Refresh��5(       5'      �� *RefreshCommand+5(      5)      .+�5+      � $
 Fill=�5/      ��5,        *HistogramBlue+8   5-        *HistogramOpacity+ 5.      �� *Histogram.BluePoints+y�50      �� *
ShowBlue+8  $ OneWay=�� 51      $
 Fill=�55      ��52        *HistogramGreen+8   53        *HistogramOpacity+ 54      �� *Histogram.GreenPoints+y�56      �� *	ShowGreen+8  $ OneWay=�� 57      $
 Fill=�5;      ��58        *HistogramRed+8   59        *HistogramOpacity+ 5:      �� *Histogram.RedPoints+y�5<      �� *	ShowRed+8  $ OneWay=�� 5=      $
 Fill=�5A      ��5>        *HistogramLuminance+8   5?        *HistogramOpacity+ 5@      �� *Histogram.LuminancePoints+y�5B      �� *ShowLuminance+8 	 $ OneWay=�� 5C      $
 Fill=�5G      ��5D        *HistogramSaturation+8 
  5E        *HistogramOpacity+ 5F      �� *Histogram.SaturationPoints+y�5H      �� *ShowSaturation+8  $ OneWay=�5I      5J      5K      