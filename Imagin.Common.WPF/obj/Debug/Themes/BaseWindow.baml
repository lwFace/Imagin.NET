   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      7 clr-namespace:Imagin.Common.DataImagin.Common.Data  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089)    #Imagin.Common.Controls.WindowButton  �� 
TargetType �� Property �� Value �� 
TargetType(   "Imagin.Common.Controls.MaskedImage   SourceColor   Source �� Mode �� RelativeSource �� UpdateSourceTrigger	   SourceHeight
   SourceWidth �� Triggers P� Property P� Value *  &Brush.Window.Button.Checked.Background & "Brush.Window.Button.Checked.Border * &Brush.Window.Button.Checked.Foreground ]� 
Conditions �� Property �� Value , (Brush.Window.Button.MouseOver.Background ( $Brush.Window.Button.MouseOver.Border , (Brush.Window.Button.MouseOver.Foreground " Brush.Window.Button.Background  Brush.Window.Button.Border " Brush.Window.Button.Foreground *	 &Brush.Window.Button.Pressed.Background &
 "Brush.Window.Button.Pressed.Border * &Brush.Window.Button.Pressed.Foreground'   !Imagin.Common.Controls.BaseWindow  Brush.Window.Background  
Brush.Text)   #Imagin.Common.Linq.WindowExtensions&   System.Windows.Media.TextOptions J� SnapsToDevicePixels � 	Resources"   Imagin.Common.Data.Reference  Key   Data!   Imagin.Common.Data.Ancestor'   !Imagin.Common.Linq.GridExtensions   Rows;   5Imagin.Common.Converters.GridLengthArrayTypeConverter   Columns ��Name   TitleTemplate*	   $Imagin.Common.Data.AncestorReference   Buttons'
  !System.Windows.Shell.WindowChrome 
  IsHitTestVisibleInChrome �� Orientation   Name   MinimizeCommand � ToolTip    Imagin.Common.Markup.Image   MaximizeCommand   RestoreCommand    CloseCommand! F�Name" �� Stretch  Brush.Window.Content.Border " Window.Content.BorderThickness# �� Padding$ �� Triggers% �� 
TargetName& �� DataType' 
  CaptionHeight( 
  CornerRadius) 
  GlassFrameThickness* 
  ResizeBorderThickness  Brush.Window.Active.Border  Brush.Window.Border-   'Imagin.Common.Controls.CollectionWindow+ �� BasedOn�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      )Data clr-namespace:Imagin.Common.Data  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5      %$!  '         '  %    '  a     7 �� 5      "    60   .�5	   
   ��  � ��$ 1q�6,   �� 5
   
    �@�Cursor$
 Hand��6#   �� 5   
    � ��$ 25��6#   �� 5   
    � ��$ Center=�67   �� 5   
    � ��$ Center=�65   �� 5   
    � ��$ 30��6"   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      �� #��z��5      #��z��5      #��z��5      ._�5        #��z��5      # z��5      #��z��5       5      �� *	Content+$ OneWay=� �� *TemplatedParent+$ PropertyChanged=�	 5      �� *ContentSize.Height+ �� *TemplatedParent+
 5      �� *ContentSize.Width+ �� *TemplatedParent+5      5      5      5        5!   
   P� 5"       �@  	IsChecked . . �5#      ��  � ��# �   6/   �� 5$       � ��# �  60   �� 5%       � ��# �  6/   5&      ]� 5'       5(      �� 5)       �@  	IsChecked .  65   �� 5*       �@J�IsMouseOver . 67   ./�5,      ��  � ��# �  6/   �� 5-       � ��# �  60   �� 5.       � ��# �  6/   5/      ]� 50       51      �� 52       �@  	IsChecked .  65   �� 53       �@J�IsMouseOver .  67   ./�55      ��  � ��# �  6/   �� 56       � ��# �  60   �� 57       � ��# �  6/   58      ]� 59       5:      �� 5;       �@  	IsChecked .  65   �� 5<       � �� . 65   ./�5>      ��  � ��# � 	 6/   �� 5?       � ��# � 
 60   �� 5@       � ��# �  6/   5A      5C      �� 5E      "   6.   .�5F   
   ��  �@�AllowsTransparency .  6/   �� 5G   
    � ��# �  6'   �� 5H   
    � ��$ 1q�6,   �� 5I   
    � ��# �  6'   �� 5J   
    �@ Extends . 6:   �� 5K   
    �@ TextFormattingMode$ Display=�6;   �� 5L   
    �@�ShowInTaskbar . 6*   �� 5M   
    � �� 5N      �� 5O      "  6"   .R�5P      �� #��z��5Q      #��z��5R      #��z��5S      # z 5T       5U      ��  5V      
BaseWindow  6@     *M� *local:BaseWindow++5W      ._�5X      � $ Auto,* 6   .+�5Y      � $��0��5Z   !   $  Auto,*,Auto,Auto,Auto,Auto 5[   !   $��32��5\   !   .+�5]   "   �� $ 	PART_Icon��5_   %   $��0��5^   %   $��10,0,0,0q�5`   %   �� 5a   "   $��1��5b   %   #��z 5d   %   $��10,0,0,0q�5e   %   ��5c   %   	  *M� *local:BaseWindow++5e   %   �� 5g   "   $��2��5h   %   #��z 5i   %   $	��Top=�5j   %    . ��5l   &   �� 5m   *   .R�5n   .   �� $ 
Horizontal=�69   5o   +   5q   #      5r   "   $ PART_Minimize��5s   %   $��3��5t   %   #��Z 5u   %   $ Minimize��5x   %   $	��Top=�5y   %    . ��5v   %   ��  �� *TemplatedParent+��5w   %     *Imagin.Common.WPFImages/Minus.png+5z   %      5{   "   $ PART_Maximize��5|   %   $��4��5}   %   #��Z 5~   %   $ Maximize��5�   %   $	��Top=�5�   %    . ��5   %   ��  �� *TemplatedParent+��5�   %     *Imagin.Common.WPFImages/Maximize.png+5�   %      5�   "   $ PART_Restore��5�   %   $��4��5�   %   #��Z 5�   %   $ Restore��5�   %   $	��Top=�5�   %    . ��5�   %   ��  �� *TemplatedParent+��5�   %     *Imagin.Common.WPFImages/Restore.png+5�   %      5�   "   $ 
PART_Close��5�   %   $��5��5�   %   #��Z  5�   %   $ Close��5�   %   $	��Top=�5�   %    . ��5�   %   ��  �� *TemplatedParent+��5�   %     *Imagin.Common.WPFImages/X.png+5�   %   5�      F� 5�      $! PART_ResizeGrip��5�   !   $��1��5�   !   �����@B ��� ���- ��� �@KL ��� ���j ��� �@T� ��� ��ا ��� Ѐ� @T� р� �ا Ѐ�[ @T� р�[ �ا Ѐ�� @T� р�� �ا �@KL ��[ ���j ��[ �@T� ��[ ��ا ��[ Ѐ�[ @KL р�[ ��j Ѐ�� @KL р�� ��j �@T� �� ��ا �� Ѐ�� @B р�� ��- $��12��5�   !   $��Right=�5�   !   $��0,0,4,4q�5�   !   #��z��5�   !   $��1��5�   !   $
" None=�5�   !   $��Bottom=�5�   !   $y�	Collapsed=�5�   !   $��12��5�   !   �� 5�      $��1��5�   !   #���  5�   !   #���  5�   !   $��2q�5�   !   ## z��5�   !   ._�5�   "   �� .u�5�   &   �� #��z��5�   )   #��z��5�   )   #��z��5�   )   5�   #   5�      5�      5�      $ 5�      ]� 5�       5�      �� 5�   "    �@�WindowState$ 	Maximized=�6C   �� 5�   "    �@�
ResizeMode$ CanMinimize=�6B   ./�5�      �� $% PART_Maximize��6%    � y�$ 	Collapsed=�6V   �� 5�      $% PART_Restore��6%    � y�$ Visible=�6U   5�      ]� 5�       5�      �� 5�   "    �@�WindowState$ Normal=�6C   �� 5�   "    �@�
ResizeMode$ CanMinimize=�6B   ./�5�      �� $% PART_Maximize��6%    � y�$ Visible=�6V   �� 5�      $% PART_Restore��6%    � y�$ 	Collapsed=�6U   5�      ]� 5�       5�      �� 5�   "    �@�WindowState$ 	Maximized=�6C   �� 5�   "    �@�
ResizeMode$ 	CanResize=�6B   ./�5�      �� $% PART_Maximize��6%    � y�$ 	Collapsed=�6V   �� 5�      $% PART_Restore��6%    � y�$ Visible=�6U   5�      ]� 5�       5�      �� 5�   "    �@�WindowState$ Normal=�6C   �� 5�   "    �@�
ResizeMode$ 	CanResize=�6B   ./�5�      �� $% PART_Maximize��6%    � y�$ Visible=�6V   �� 5�      $% PART_Restore��6%    � y�$ 	Collapsed=�6U   5�      ]� 5�       5�      �� 5�   "    �@�WindowState$ 	Maximized=�6C   �� 5�   "    �@�
ResizeMode$ CanResizeWithGrip=�6B   ./�5�      �� $% PART_Maximize��6%    � y�$ 	Collapsed=�6V   �� 5�      $% PART_Restore��6%    � y�$ Visible=�6U   5�      ]� 5�       5�      �� 5�   "    �@�WindowState$ Normal=�6C   �� 5�   "    �@�
ResizeMode$ CanResizeWithGrip=�6B   ./�5�      �� $% PART_Maximize��6%    � y�$ Visible=�6V   �� 5�      $% PART_Restore��6%    � y�$ 	Collapsed=�6U   5�      ]� 5�       5�      �� 5�   "    6<   Z�  �@�Icon�� 5�   "    6A   Z�  �@ 	TitleIcon./�5�      �� $% 	PART_Icon��6%    � y�$ 	Collapsed=�6R   5�      P� 5�       62   Z�  �@�Icon. �5�      �� $% 	PART_Icon��6%    � �� 5�   "   �� 5�   &   .R�5�   *     $	 16Q�5�   -   $
 16Q�5�   -    5�   -     *	TitleIconM� *local:BaseWindow++5�   -   5�   '   5�      5�      P� 5�       67   Z�  �@ 	TitleIcon. �5�      �� $% 	PART_Icon��6%    � �� 5�   "   �� 5�   &   .R�5�   *   �� $��16��5�   -   $��16��5   -   ��5�   -     *IconM� *local:BaseWindow++5   -   5  '   5     5     P� 5      �@�
ResizeMode$ NoResize=�68   . �5     �� $% PART_Maximize��6%    � y�$ 	Collapsed=�6V   �� 5     $% PART_Restore��6%    � y�$ 	Collapsed=�6U   5	     ]� 5      5     �� 5  "    �@�WindowState$ 	Maximized=�6C   �� 5  "    �@�WindowStyle$ 
ToolWindow=�6C   ./�5     �� $% PART_Maximize��6%    � y�$ 	Collapsed=�6V   �� 5     $% PART_Restore��6%    � y�$ 	Collapsed=�6U   5     ]� 5      5     �� 5  "    �@�WindowState$ Normal=�6C   �� 5  "    �@�WindowStyle$ 
ToolWindow=�6C   ./�5     �� $% PART_Maximize��6%    � y�$ 	Collapsed=�6V   �� 5     $% PART_Restore��6%    � y�$ 	Collapsed=�6U   5     ]� 5      5     �� 5  "    �@�
ResizeMode$ CanResizeWithGrip=�6B   �� 5  "    �@�WindowState$ Normal=�6C   ./�5      �� $% PART_ResizeGrip��6%    � y�$ Visible=�6X   5!     5#     5%     �� 5&  
    �@ TitleTemplate 5'     �� 5(     "&  6   .R�5)     �� ��6    �� *
Data.Title+5*     5,     �� 5-  
    �@�UseLayoutRounding . 6.   �� 5.  
    �@
 WindowChrome 5/     
  50     $' 32Q�51     $( 0��52     $) 0,0,0,1q�53     $* 5q�54     56     �� 57  
    �@�WindowStyle$
 None=�6(    58  
   P� 59      �@�IsActive . . �5:     ��  � ��# �  60   5;     P� 5<      �@�IsActive .  . �5=     ��  � ��# �  60   5>     5@     �� 5B     "   64   8+   .�5C  
   ��  �@ ItemTemplate 5D     �� 5E     .R�5F     �� ��6    �� *Name+5G     5I     5J     5K     