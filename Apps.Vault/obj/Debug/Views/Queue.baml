   M S B A M L   `   `   `  ����@  <Vault, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:VaultVault  5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089L HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null  �� DataType    Vault.QueuePanel � 	Resources0  *Imagin.Common.Converters.FileSizeConverter   Key1  +Imagin.Common.Converters.FileSpeedConverter5  /Imagin.Common.Converters.TimeLeftMultiConverter/  )Imagin.Common.Converters.PercentConverter&   System.Windows.Controls.DataGrid   CanUserReorderColumns   CanUserSortColumns+  %Imagin.Common.Linq.DataGridExtensions   DisplayRowNumber   DisplayRowNumberOffset/  )Imagin.Common.Linq.ItemsControlExtensions   IsColumnMenuEnabled   HeadersVisibility   SelectionMode	   Columns4  .System.Windows.Controls.DataGridTemplateColumn,	  &System.Windows.Controls.DataGridColumn
 	  Width5
  /System.Windows.Controls.DataGridLengthConverter 	  Header9  3Imagin.Common.Globalization.Extensions.LocExtension   CellTemplate �� RelativeSource �� AncestorType)  #System.Windows.Controls.DataGridRow � ToolTip  FileSizeConverter �� 	Converter  FileSpeedConverter  TimeLeftMultiConverter `� 	Converter �� Path � Maximum � Minimum �� ElementName � Value ��Name �� Triggers �� Value �� Binding �� 
TargetName �� Property �� Value  PercentConverter �� StringFormat   RowStyle  �� 
TargetType! �� BasedOn" �� Triggers�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      localclr-namespace:Vault  5      (i http://imagin.tech/imagin/common  5      %(  (��       5      *,  +)7  7  7  7  7  7 �� 6   "    6    5   
   ��  5      FileSizeConverter  6"     5	      FileSpeedConverter  6#     5
      TimeLeftMultiConverter  6'     5      PercentConverter  6!   5      .R�5   
     $��0q�5       .  .  . $ 1��5       . $	 All=�5      $ Single=�5      ��5      �� *Queue+	 5        5      $

 Auto
 5       5        *Type+ 5      �� 5      .R�5      �� $��CharacterEllipsis=�5    !   ��5   !   �� *
Foreground+ �� "  ��5   !   �� *
ItemType+ 5!   !   �� *Text+ �� *Self+5"      5$        5%      $
 *
 5'       5&        *Source+ 5(      �� 5)      .R�5*      �� $��CharacterEllipsis=�5-   !   ��5+   !   �� *
Foreground+ �� "  ��5,   !   �� *Source+ 5.   !   �� *Text+ �� *Self+5/      51        52      $
 *
 54       53        *Destination+ 55      �� 56      .R�57      �� $��CharacterEllipsis=�5:   !   ��58   !   �� *
Foreground+ �� "  ��59   !   �� *Destination+ 5;   !   �� *Text+ �� *Self+5<      5>        5?      $

 Auto
 5A       5@        *
SizeRead+ 5B      �� 5C      .R�5D      �� $��CharacterEllipsis=�5G   !   ��5E   !   �� *
Foreground+ �� "  ��5F   !   �� *
SizeRead+8    5H   !   �� *Text+ �� *Self+5I      5K        5L      $

 Auto
 5N       5M        *Size+ 5O      �� 5P      .R�5Q      �� $��CharacterEllipsis=�5T   !   ��5R   !   �� *
Foreground+ �� "  ��5S   !   �� *Size+8   5U   !   �� *Text+ �� *Self+5V      5X        5Y      $

 Auto
 5[       5Z        *Speed+ 5\      �� 5]      .R�5^      �� $��CharacterEllipsis=�5a   !   ��5_   !   �� *
Foreground+ �� "  ��5`   !   �� *Speed+8   5b   !   �� *Text+ �� *Self+5c      5e        5f      $

 Auto
 5h       5g        *Time+ 5i      �� 5j      .R�5k      �� $��CharacterEllipsis=�5n   !   ��5l   !   �� *
Foreground+ �� "  ��5m   !   �� *
Duration+ 5o   !   �� *Text+ �� *Self+5p      5r        5s      $

 Auto
 5u       5t        *
TimeLeft+ 5v      �� 5w      .R�5x      �� $��CharacterEllipsis=�5z   !   ��5y   !   �� *
Foreground+ �� "   5{   !   �� *Text+ �� *Self+��5|   "   `� 5}   &   8  .1�5~   *   �� $ Duration�62   �� 5   *   $ SizeRead�62   �� 5�   *   $
 Size�62   5�   '   5�      5�      5�        5�      $
 *
 5�       5�        *
Progress+ 5�      �� 5�      .R�5�      � .+�5�   "   � $��20��5�   %   $ 1Q�5�   %   $ 0Q�5�   %    5�   %   �� *Text+$ 	PART_Text�� 5�   %   �� *
Progress+�� 5�   "   $ 	PART_Text��5�   %   $��11(�5�   %   $��Center=�5�   %   $��Center=�5�   %   5�       5�      �� 5�   "   $ Inactive��6I    6.   �� *Status+.a�5�   &   �� $ 	PART_Text��6-    � ��$ Pending��6T   5�   #   �� 5�   "   $ Active��6I    6.   �� *Status+.a�5�   &   �� $ 	PART_Text��6-    6T   �� *
Progress+8  $ {0:n2}%�� � ��5�   #   5�      5�       5�      �� 5�      "   6   8!  " 5�      �� 5�       6&   �� *Type+
 Create6?   .a�5�      ��  � y�$ Visible=�6;   5�      �� 5�       6&   �� *Type+
 Delete6?   .a�5�      ��  � y�$ 	Collapsed=�6;   5�      �� 5�       6&   �� *Type+ Move6?   .a�5�      ��  � y�$ Visible=�6;   5�      5�      5�      5�      5�      