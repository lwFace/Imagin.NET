   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      C&clr-namespace:Imagin.Common.ConvertersImagin.Common.Converters  5      '    !Imagin.Common.Controls.Downloader   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      6local$clr-namespace:Imagin.Common.Controls     5      =
Converters&clr-namespace:Imagin.Common.Converters     5      -   5          >�Name$  PART_Downloader��5       � 	Resources 5      1   +Imagin.Common.Converters.FileSpeedConverter   FileSpeedConverter/   )Imagin.Common.Converters.SecondsConverter  SecondsConverter��%:   &	        &	        5   
   6(     5   
   6&   5      .��5      � 
���  �� � DataContext 5   	   ��  �� ElementName$ PART_Downloader��.+�5   
   �� $��15(�5      $��Center=�5      ��5      �� *
Progress+ �� StringFormat$
 {0}%��5      (   "Imagin.Common.Controls.ProgressBar  5   
   $��1��5      $��Stretch=�5      $��Center=�5      $
��0,10q�5      $��15��5       � Value 5      �� *
Progress+5      �� 5   
   $��2��5       $��Center=�5"       � ToolTip$ Progress��5#      $��0,0,0,3q�5$      ��5!      �� *	Processed+5$      �� 5%   
   $��3��5&      $��Center=�5(      $ Speed��5)      $��0,0,0,3q�5*      ��5'      �� *Speed+ �� 	Converter# [  5*      �� 5+   
   $��4��5,      $��Center=�5.      $ 	Remaining��5/      ��5-      �� *	Remaining+# [ 5/      50      51      