   M S B A M L   `   `   `  ����@  <Paint, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PaintPaint  5          Paint.NewWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      L HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null(i http://imagin.tech/imagin/common  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      %localclr-namespace:Paint     5        � SizeToContent$  Height=�5
       � Title$	 New��5      $	��512��5       � WindowStartupLocation$ CenterScreen=�5       � 	Resources 5      0  *Imagin.Common.Converters.FileSizeConverter   FileSizeConverter��%   &	          5   
   6   5      .��5      � '  !Imagin.Common.Linq.GridExtensions   Rows;  5Imagin.Common.Converters.GridLengthArrayTypeConverter$ Auto,*,Auto 5   	    � DataContext 5   	   !  Imagin.Common.Data.Ancestor  *M� *local:NewWindow++.+�5   
   � $��0��5      $��10q�5      $# Auto,Auto,Auto,Auto,Auto,Auto 5      (  "Imagin.Common.Linq.PanelExtensions   Spacing$ 0,0,0,10q�5         SpacingExcept$
 Last=�5      .+�5      �� $��Preset��6   $��0��5      $��0q�5      .��5      �    Columns$ *,Auto,Auto 5      $ 0,0,5,0q�5       $
 Last=�5!      .+�5"      �� -   5        $��0��5#      $��0q�5$      ��5%      "  Imagin.Common.Markup.Options  *DocumentPresets+	 �� SelectedValue	 5&      �� *SelectedPreset+
 �� Mode$
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=���5(      �� 5)      .R�5*   "   �� ��5+   %   �� *Name+$
 OneWay=�5,      5.      �� 5/      $��1��50      $
��Save��52      $��Center=�53      ��51      �� *SavePresetCommand+53      �� 54      $��2��55      $��Delete��57      $��Center=�58      ��56      �� *DeletePresetCommand+58      59      5:      �� 5;      $
��Name��6   $��1��5<      $��0q�5=      .��5>      $  Imagin.Common.Controls.TextBox  $��0q�5?         Placeholder$
 Name��5@      ��5A      �� *DocumentPreset.Name+$
 TwoWay=�$ PropertyChanged=�5B      �� 5C      $��2��5D      $��
10,10,-5,5q�5E      ��5F      �� 5G      �� 5H      $
�Size��6    (�� 6.   �62   �� *DocumentPreset.Size+ �� 	Converter# [  $
 OneWay=�)5I      .��5K      � $ 20*,80*,Auto 5L      $ 	Auto,Auto 5M      !   HorizontalContentAlignment$ Stretch=�5N      $ 0,0,5,5q�5O         VerticalContentAlignment$ Center=�5P      .+�5Q      �� $��0��5R      $��0��5S      $��Height:��5T      3  -Imagin.Common.Linq.FrameworkElementExtensions   HorizontalAlignment$ RightX�5U      $	  Imagin.Common.Controls.UnitBox	  5V      $��1��5W      $��0��5X       	  StringFormat$ N3��5Z      $��Center=�5]       	  
Resolution 5Y      �� *DocumentPreset.Resolution+$
 OneWay=� 	  Unit 5[      �� *Unit+$
 TwoWay=� 	  Value 5\      �� *DocumentPreset.Height+$
 TwoWay=�5]      �� 5_      $��1��5`      $��0��5a      $��Width:��5b      $ RightX�5c      	  5d      $��1��5e      $��1��5f      $ N3��5h      $��Center=�5k       5g      �� *DocumentPreset.Resolution+$
 OneWay=� 5i      �� *Unit+$
 TwoWay=� 5j      �� *DocumentPreset.Width+$
 TwoWay=�5k      -
  'Imagin.Common.Controls.MaskedToggleMenu
  5m      $��2��5n      $��0��5o      $��2��5p      /  )Imagin.Common.Controls.MaskedToggleButton   CheckedToolTip$ Unlink��5q         Source$< 6pack://application:,,,/Paint;component/Images/Link.png��5s       � ToolTip$
 Link��5t       d� 	IsChecked 5r      �� *DocumentPreset.Link+$
 TwoWay=�5t      5u      5v      �� 5w      $��
Resolution��6   $��3��5x      $��0q�5y      .��5z      )  #Imagin.Common.Controls.SingleUpDown  $��0q�5{      �  }Imagin.Common.Controls.UpDown`1[[System.Single, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 5|      �� *DocumentPreset.Resolution+$
 TwoWay=�$ PropertyChanged=�5}      �� 5~      $��Format��6   $��4��5      $��0q�5�      .��5�      �� $��0q�5�      ��5�      $  Imagin.Common.Markup.Enumerate  *M� *i:PixelFormat++	 5�      �� *DocumentPreset.PixelFormat+$
 TwoWay=�$ PropertyChanged=�5�      �� 5�      $
��Fill��6   $��5��5�      .��5�      &   Imagin.Common.Controls.ColorChip  $��32��5�      $��Stretch=�5�      $��Stretch=�5�      $
��Auto��5�         Color 5�      �� *DocumentPreset.Background+$
 TwoWay=�$ PropertyChanged=�5�      5�      5�      �� 5�   
   $��2��5�      ���	#11000000._�5�      �� $ 0,0,5,0q�5�      $
 Last=�5�      $��Center=�5�      $��10q�5�       �� Orientation$ 
Horizontal=�5�      .+�5�      �� -   5        $��Create��5�       �� 	IsDefault . $��Center=�5�      �� 5�      -   5        $��Cancel��5�       �� IsCancel . $��Center=�5�      5�      5�      5�      5�      