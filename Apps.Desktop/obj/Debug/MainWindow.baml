   M S B A M L   `   `   `  ����B  >Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:DesktopDesktop  5      L HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullzOclr-namespace:Imagin.Common.Globalization.Converters;assembly=Imagin.Common.WPF&Imagin.Common.Globalization.Converters 5          Desktop.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      'localclr-namespace:Desktop     5      f
ConvertersOclr-namespace:Imagin.Common.Globalization.Converters;assembly=Imagin.Common.WPF     5      \ XHardcodet.Wpf.TaskbarNotification, Version=1.0.5.0, Culture=neutral, PublicKeyToken=null0	Hardcodet http://www.hardcodet.net/taskbar  5	      (i http://imagin.tech/imagin/common  5
      ;  5Imagin.Common.Globalization.Engine.LocalizeDictionary    DesignCulture$  en��5      D  >Imagin.Common.Globalization.Providers.ResxLocalizationProvider   DefaultDictionary$
 Main��5      )  #Imagin.Common.Linq.WindowExtensions   AlwaysOnBottom . $��0q�5       � 
ResizeMode$ NoResize=�5       � ShowInTaskbar .  � WindowState$ 	Maximized=�5         DefaultAssembly 5      '  !Imagin.Common.Markup.AssemblyName   � 	Resources 5      >  8Imagin.Common.Converters.ColorToSolidColorBrushConverter #  ColorToSolidColorBrushConverter0  *Imagin.Common.Converters.ObjectIsConverter  ObjectIsConverter4  .Imagin.Common.Converters.ObjectToTypeConverter  ObjectToTypeConverter/  )Imagin.Common.Converters.HasFlagConverter  HasFlagConverterA	  ;Imagin.Common.Converters.ValueEqualsParameterMultiConverter & "ValueEqualsParameterMultiConverter?
  9Imagin.Common.Globalization.Converters.TranslateConverter  TranslateConverter  Style.Thumb �� 
TargetType	 �� Property
 �� Value �� 
TargetType � Cursor  ContextMenu.TaskbarIcon �� BasedOn �� Triggers �� Value   Desktop.Screen �� Binding �� 	Converter&   Imagin.Common.Data.MainViewModel'  !Imagin.Common.Linq.GridExtensions   Columns;  5Imagin.Common.Converters.GridLengthArrayTypeConverter �� StringFormat)  #Imagin.Common.Controls.MaskedButton   Source   Imagin.Common.Markup.Image+  %Imagin.Common.Linq.MenuItemExtensions-  'System.Windows.Data.CompositeCollection m� Command m� Icon �� 
Collection�� 5   
   %�  &	        &	      &	 .     &	 E     &	 \     &	 s     &	 �     &	 l    7m�7    5      5        5      5        5      5        5      5      	  5      5      
  5!      5"      �� 5$      " n�6(   .�5%      �� 	 �@�OverridesDefaultStyle
 . 6:   �� 5&      	 � ��
 5'      �� 5(      " n�6*   .R�5)      �� ���#007acc# z 5+   %   5,      5-      5/      50      �� 52       53      ���� 54      'm�       " m�6   8    55      �� 56      "  6o    6*   �� 8  .a�57   "   �� 	 �@m�Command
 6<     *SelectScreenCommand+�� 58   "   	 �@m�CommandParameter
 6E   �� �� 59   "   	 � ��
 5:   &   �� 5;   *   .R�5<   .   � $ *,Auto 63   .+�5=   2   �� ��6<   �� *Index+$ 
Screen {0}��  5>   2   $��1��5?   5   $��5,0,0,0q�5B   5   ��5@   5     *RemoveScreenCommand+��5A   5   ��  5C   5     *	DesktopImages/Trash.png+5D   /   5E   +   5G   #   �� 5H   "   	 �@m�IsCheckable
 . 6@   �� 5I   "   	 �@ 	GroupName$
 Screens��6S   �� 5J   "   	 �@ SelectionMode$
 Single=�6W   5K      5M      5N      ��5O        5P      m� 5Q      $
��Draw��6#    5R      �� *DrawCommand+ 5S        *	DesktopImages/Pencil.png+�� 5T      �� 5U       6.     *	Screens+�� 5V      m� 5W      -   5        $��About��5W   #    5Y        *	DesktopImages/Info.png+m� 5Z      -   5        $��Options��5Z   #    5\        *	DesktopImages/Options.png+�� 5]      m� 5^      -   5        $
��Exit��5^   #    5`        *	DesktopImages/X.png+5a      5c      5d      ��5f      �� 5g   
   " �6   .�5h      �� 	 �@�AllowsTransparency
 . 63   �� 5i      	 �@�OverridesDefaultStyle
 . 66   �� 5j      	 �@ Extends
 . 6;   �� 5k      	 � ��
 5l      �� 5m      " �6&   .R�5n      � .+�5o      �� .u�5p   "   �� 5q      5r      5s      5u      �� 5v      	 �@�WindowStyle$

 None=�6,   5w       � TaskbarItemInfo 5y      *  $System.Windows.Shell.TaskbarItemInfo  5z   
      Description 6   �� *TaskbarItemDescription+   ThumbButtonInfos 5|      *  $System.Windows.Shell.ThumbButtonInfo     Description$ Previous screen��5~         Command 5}      �� *LeftScreenCommand+   ImageSource 5        *	DesktopImages/ArrowLeft.png+  5�      $ 
Add screen��5�       5�      �� *AddScreenCommand+ 5�        *	DesktopImages/Plus.png+  5�      $ Next screen��5�       5�      �� *RightScreenCommand+ 5�        *	DesktopImages/ArrowRight.png+5�       � Title 5�      `� 5�   
    `� 	Converter# [ 6   .1�5�      ��   �� Source  6   *  $Imagin.Common.Markup.AssemblyProduct  "  Imagin.Common.Markup.Options  5�      ! �� Path$! Language�6   5�      .��5�      � -   5        " �Name$
" Grid��5�      .+�5�   
   3  -Hardcodet.Wpf.TaskbarNotification.TaskbarIcon  -   5        #  Name$# TaskbarIcon��5�      $ � ContextMenu#$ [ 5�      %   
IconSource$7% 1pack://application:,,,/Desktop;component/Logo.ico��5�      &   MenuActivation$& LeftOrRightClick=�5�      $y�Visible=�5�      '   ToolTipText' 5�        5�      � 5�   
   .+�5�      +  %Imagin.Common.Controls.TransitionView  ��5�        *Screen+(   
Transition( 5�        *ScreenTransition+��5�      �� 5�      ) �� DataType")  6#   .R�5�      �� ��6'   �� ��5�      �� 5�   "   " �6(   .�5�   &   �� 	 �@��Left
 6D   �� *
Position.X+* �� Mode$* TwoWay=��� 5�   &   	 �@��Top
 6C   �� *
Position.Y+$* TwoWay=��� 5�   &   	 � ��
 6?   �� *Size.Height+$* TwoWay=��� 5�   &   	 � ��
 6>   �� *
Size.Width+$* TwoWay=��� 5�   &   *  $Imagin.Common.Linq.ControlExtensions	 �@ FadeIn
 . 6S   �� 5�   &   	 �@ FadeOut
 . 6T   �� 5�   &   	 �@ IsResizable
 .  6X   �� 5�   &   	 �@ 
ResizeSnap
 6W     *
TileSnap+�� 5�   &   	 �@ ResizeThumbStyle#
 �  6]    5�   &   ^� 5�   *   + ^� 
Conditions+ 5�   .   �� 5�   2   , �� Binding, 6<   �� *
IsSelected+- �� Value- True6[   �� 5�   2   , 6<   �� *
IsLocked+	- False6Y   .0�5�   .   �� 	 �@ IsResizable
 . 6`   5�   +   5�   #   ��5�      �� 5�   "   .R�5�   &   ,  &Imagin.Common.Controls.DraggableCanvas  .   
SnapToGrid. 68     *
TileSnap+5�   #   ��5�      �� 5�   "   .R�5�   &   �  5�   *   "  Imagin.Common.Data.Reference  Tile/   Data��%&   &	         5�   .   / 6G   �� 5�   +   .+�5�   *      Desktop.TileView  0  Name$0 PART_TileView��5�   -   1   HeaderAlignment1 5�   -     *TileHeaderAlignment+2   HeaderPlacement2 5�   -     *TileHeaderPlacement+$ 5�   .   �� 5�   2   3 � DataContext3 5�   5   �� *PlacementTarget.DataContext+4 �� RelativeSource4 �� *Self+.@�5�   6   m� $
��Lock��5�   9   5 m� IsCheckable5 . 6 m� StaysOpenOnClick6 . 7 m� 	IsChecked7 5�   9   �� *
IsLocked+$* TwoWay=�8 �� UpdateSourceTrigger$8 PropertyChanged=�5�   9   �� 5�   6   m� 5�   6   $��Remove��5�   9    5�   9     *RemoveTileCommand+9 m� CommandParameter9 5�   9   �� 5�   9   5�   3   ��5�   .   �� 5�   2   "  68   .�5�   6   �� 	 � ��$
 1q�6X    5�   6   �� 5�   :    6F   �� *
IsSelected+ True6e   .a�5�   >   �� 	 � �� 	 Brush.Window.Active.Border#
 � 	 6\   5�   ;   �� 5�   :    6F   �� *
IsSelected+	 False6e   .a�5�   >   �� 	 � �� 
 Brush.Window.Border#
 � 
 6\   5�   ;   5�   3   5�   +   5�   '   5�   #   5�      5�      5�      � 5�      -   5        $" 	PART_Grid��5�      $y�	Collapsed=�5�      .+�5�      ,  &Imagin.Common.Controls.SelectionCanvas  -   5        :  Name$: PART_SelectionCanvas��5�      ���	#CC000000$��Stretch=�5�      $��Stretch=�5�      �� 5�      $��Center=�5�      $��0,0,0,10q�5�      $��20,10q�5�        Button.Green#���  5�      $��Bottom=�5�      ��5�      �� *CancelCommand+.��5�      �� ; �� Orientation$; 
Horizontal=�6!   .+�5�      (  "Imagin.Common.Controls.MaskedImage  $��0,0,10,0q�5�      <   SourceColor< �#FFF=   SourceHeight$= 16Q�5�      >   SourceWidth$> 16Q�5�      ?   Source? 5�        *	DesktopImages/Pencil.png+5�      �� 5      $��Cancel��6$   5     5     5     5     5     5     