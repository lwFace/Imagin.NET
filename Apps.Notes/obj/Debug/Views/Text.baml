   M S B A M L   `   `   `  ����@  <Notes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:NotesNotes  5          Notes.TextViewW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089L HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;  5Imagin.Common.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter3  -Imagin.Common.Converters.BulletMultiConverter  BulletMultiConverter/  )Imagin.Common.Converters.HasFlagConverter  HasFlagConverter,  &Imagin.Common.Converters.NullConverter  NullConverter4  .Imagin.Common.Converters.ObjectToTypeConverter  ObjectToTypeConverter/  )Imagin.Common.Converters.SecondsConverter  SecondsConverter5  /Imagin.Common.Converters.SortDirectionConverter  SortDirectionConverter0  *Imagin.Common.Converters.ToStringConverter  ToStringConverter  DataTemplate.List'	  !Imagin.Common.Linq.GridExtensions  	  Rows;
  5Imagin.Common.Converters.GridLengthArrayTypeConverter � 	Resources"  Imagin.Common.Data.Reference 	 Key   Data 
 Style.Border.Bottom)  #Imagin.Common.Controls.MaskedButton � ToolTip   Source   Imagin.Common.Markup.Image/  )Imagin.Common.Linq.ItemsControlExtensions   SortDirection �� 	Converter   SortName �� RelativeSource	 � 	ResourcesA  ;Imagin.Common.Converters.DateTimeStringFormatMultiConverter@  :Imagin.Common.Converters.DecimalStringFormatMultiConverter7  1Imagin.Common.Converters.DecimalToStringConverter
 	  Columns(  "Imagin.Common.Linq.PanelExtensions   VerticalContentAlignment ��Name d� 	IsChecked �� Mode �� UpdateSourceTrigger'  !Imagin.Common.Controls.BulletView  
Brush.Text   Bullet  List �� Source   Count"  Imagin.Common.Markup.Options   Value  Line �� 
TargetType �� Triggers �� Binding   Notes.Attributes   Bullet �� ConverterParameter �� Value �� Property �� Value ��Name �� CornerRadius j� Stretch �� ImageSource%  Imagin.Common.Controls.LabelBox   Name*  $Imagin.Common.Linq.ControlExtensions!   FontSizeWheel"   FontSizeWheelMaximum#   FontSizeWheelMinimum$ �� TextWrapping%   
IsEditable& �� BasedOn  DecimalToStringConverter'   Label % !DecimalStringFormatMultiConverter( `� 	Converter) �� Path   Notes.Columns*   Decimal+ �� Orientation, �� StringFormat-   DateTime. �� Triggers/   Check0 �� 
TargetName1   Image  DataTemplate.Text$  Imagin.Common.Controls.TextBox2 �� AcceptsReturn3   Placeholder*  $Imagin.Common.Linq.TextBoxExtensions
4   Tab5   SelectAllOnFocus6   TabSpace7   
CaretIndex8   SelectionLength9   SelectionStart  DataTemplate.Markdown&   Imagin.Common.Controls.SplitView: �� DataType   Notes.TextDocument   
Notes.List   Notes.Markdown   
Notes.Note   5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      localclr-namespace:Notes  5      (i http://imagin.tech/imagin/common  5      %�  &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r     &	 �     &	 �     7  7  7  7  7  7  7  7  7 7  7  7  7 7  7  7 7  7  7  7  7  &	 Z    7 &	 ]    (�� �    5     *, +)7  7  7  7    5      6%     5      6     5	      6     5
      6     5      6     5      6     5      6     5      6   �� 5      .R�5   
   � $  Auto,*
 6    5      ��  5      List	  6+   ��   5      	Lines	  6,   �� *Lines+5      .+�5      �� $��0��5      #��� 
 5      ._�5        $
��Left=�5      $��5q�5      $	 Add��5      $��Center=�5      ��5      �� *
AddCommand+ 5        *NotesImages/Plus.png+5      5       �� 5!      $��1��5"      $��Disabled=�5#      $��0,5,10,0q�5$      .��5%      �� ��5&      �� *Lines+ 5'      �� *SortDirection+8    5(      �� *
SortName+8  ��5)      �� 5*      .R�5+      �� 5,      ��5.      �� 5/      .R�50      �� ��6-   ��  51   "   ��  52   &   Line	  6?   ��  �� *TemplatedParent+53   #   ��54   "   �� 55   &   	 56   *   ��  57   .   &"DateTimeStringFormatMultiConverter	 6S     58   .   %!DecimalStringFormatMultiConverter	 6R     59   .   DecimalToStringConverter	 6I   5:   +   .R�5;   *   � $
 Auto,Auto,Auto,*,Auto
 5<   -   $ Center=�5=   -   $��10,0,0,5q�5>   -   .+�5?   .   �� $ PART_CheckBox��5@   1   $��0��5A   1   $��0,0,10,0q�5C   1    5B   1   �� *	Checked+$ TwoWay=�$ PropertyChanged=�5C   1     5D   .   $��1��5E   1   #���  5J   1   $��0,0,10,0q�5K   1    5F   1   �� *Data.Bullets+8   5G   1   �� *
Data.Count+8  ��5H   1     *
FontFamily+$ OneWay=���5I   1     *
FontSize+$ OneWay=� 5L   1   �� *Data+8  ��5M   2   �� 5N   6   "  6<    5O   :   �� 5P   >    6J   �� *Data.Attributes+8  # Z 8  	 False6�   .a�5Q   B   ��  � y�$ 	Collapsed=�6_   5R   ?   5T   7   5V   /   �� 5W   .   $ 
PART_Image��5Y   1   -   5        $��2��5X   1   ���	#22000000$��1q�5[   1   }�. $	 999��5]   1   $��72��5^   1   $��0,0,10,0q�5_   1   $��72��5a   1   ��5b   2   �� 5c   6   $ UniformToFill=�6A    6Y   �� *Image+8  5e   /   � 5f   .   $��3��63   .+�5g   2     $  PART_LabelBox��5h   5   ! . $" 72��5l   5   $# 8��5m   5   $
$ Wrap=�5p   5   ��5i   5     *
FontFamily+$ OneWay=���5j   5     *
FontSize+$ TwoWay=�% 5n   5   �� *	Editing+$ TwoWay=���5o   5   �� *Text+$ TwoWay=�$ PropertyChanged=�.��5p   I   ��5q   6   �� 5r   :   "  6@   8&  .�5s   >   -   5        5u   ;   .��5v   H   5w   3   5x   /   �� 5y   .   $��4��5z   1   $��5,0,0,0q�5{   1   .+�5|   2     ! . $" 72��5�   5   $# 8��5�   5   $��Right=�5�   5   $
$ Wrap=�5�   5   ��5}   5     *
FontFamily+$ OneWay=���5~   5     *
FontSize+$ TwoWay=���5�   5   �� *	Decimal+8 	 $ TwoWay=�$ 	LostFocus=�.��5�   I   ' 5�   6   `� 5�   :   8( 
 1�5�   >   �� 5�   B   $) Decimal�6J   �� 5�   B   $) Data.DecimalFormat�6J   8  6d   5�   ;   .��5�   H   ��5�   6   �� 5�   :   "  6@   8&  .�5�   >   -   5        �� 5�   >    � y�$ 	Collapsed=�6[    5�   >   �� 5�   B   # Z* 6�    6N   �� *
Data.Value+8  .a�5�   F   ��  � y�$ Visible=�6c   5�   C   5�   ;   .��5�   H   5�   3   �� 5�   2   $��Right=�5�   5   $+ 
Horizontal=�5�   5   .+�5�   6     ! . $" 72��5�   9   $# 8��5�   9   $
$ Wrap=�5�   9   ��5�   9     *
FontFamily+$ OneWay=���5�   9     *
FontSize+$ TwoWay=�' 5�   9   �� *
DateTime+8  $ 1����5�   9   �� *
DateTime+$ TwoWay=�$, M/d/yyyy h:mm tt��$ 	LostFocus=�.��5�   M   ��5�   :   �� 5�   >   "  6D   8&  .�5�   B   -   5        �� 5�   B    � y�$ 	Collapsed=�6_    5�   B   �� 5�   F   # Z- 6�    6R   �� *
Data.Value+8  .a�5�   J   ��  � y�$ Visible=�6g   5�   G   5�   ?   .��5�   L   5�   7   5�   3   5�   /   5�   +   . 5�   *   �� 5�   .   $ False��6�    6:   �� *Data.Attributes+8  # Z/ 8  .a�5�   2   �� $0 PART_CheckBox��69    � y�$ 	Collapsed=�6j   5�   /   �� 5�   .   $ False��6�    6:   �� *Data.Attributes+8  # Z1 8  .a�5�   2   �� $0 
PART_Image��69    � y�$ 	Collapsed=�6g   5�   /   5�   '   5�      5�      5�      5�      5�      5�      �� 5�      .R�5�   
     2 . $��0q�5�      ! . $" 72��5�      $# 8��5�      $��10q�5�      $3 Write something...��5�      ��. $
��Auto=�5�      $
��Auto=�5�      4 . 5 . $��Stretch=�5�      $	��Top=�5�      ��5�        *
FontFamily+$ OneWay=���5�        *
FontSize+$ TwoWay=�6 5�        *Tab+7 5�      �� *
CaretIndex+$ OneWay=�8 5�      �� *SelectionLength+$ TwoWay=�$ PropertyChanged=�9 5�      �� *SelectionStart+$ TwoWay=�$ PropertyChanged=���5�      �� *Text+$ TwoWay=�$ PropertyChanged=�$ 5�        *
TextWrap+.��5�   ,   ��5�      �� 5�      "  6   8&   .�5�      -   5        5�      .��5�      5�      5�      �� 5      .R�5  
     5     �� 5     ":  6   .R�5  
   �� ��6   ��  5     ���� 5     ":  6   (��       *, +).R�5     �� ��6%   �� ��5	     �� 5
     " ��6$    5  "   �� 5  &    62   �� *Valid+ True6L   .a�5  *   ��  � ��8   6L   5  '   �� 5  &    62   �� *Valid+	 False6L   .a�5  *   ��  � ��8  6L   5  '   5     5     5     �� 5     ":  6   (��       *, +).R�5     �� 8�� ��6%   �� 69   5     �� 5     ":  6   (��       *, +).R�5     �� 8�� ��6%   �� 69   5     5     5     5     5      