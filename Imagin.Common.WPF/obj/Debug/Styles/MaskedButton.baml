   M S B A M L   `   `   `  ����L  HImagin.Common.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Imagin.Common.ControlsImagin.Common.Controls  5      7 clr-namespace:Imagin.Common.LinqImagin.Common.Linq  5      ;"clr-namespace:Imagin.Common.MarkupImagin.Common.Markup  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source    Imagin.Common.Markup.Uri   FadeInAnimation �� TargetProperty W� From	 W� To g� Duration g� AccelerationRatio g� DecelerationRatio)   #Imagin.Common.Controls.MaskedButton �� 
TargetType	 �� BasedOn
 �� Property �� Value6   0Imagin.Common.Converters.DoubleSizeTypeConverter  
Brush.Text �� Triggers P� Property P� Value ]� 
Conditions �� Property �� Value  Brush.Image  Brush.Image.MouseOver  Brush.Image.Pressed"   Imagin.Common.Controls.Crumb3   -Imagin.Common.Linq.FrameworkElementExtensions :� RoutedEvent*   $Imagin.Common.Linq.ControlExtensions  Brush.DropTarget.Border ) %Brush.BreadCrumb.MouseOver.Background % !Brush.BreadCrumb.MouseOver.Border ) %Brush.BreadCrumb.MouseOver.Foreground '	 #Brush.BreadCrumb.Pressed.Background #
 Brush.BreadCrumb.Pressed.Border ' #Brush.BreadCrumb.Pressed.Foreground�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .local$clr-namespace:Imagin.Common.Controls  5      )Linq clr-namespace:Imagin.Common.Linq  5      -Markup"clr-namespace:Imagin.Common.Markup  5        5      �� 5   
    6      *Imagin.Common.WPFStyles/FrameworkElement.xaml+%r	  &	        '  �     7�'  g    7 7   �� 5      .��5   
   W� $ Opacity�6   $ 0X�6>   $	 1.0X�6G   $ 0:0:0.5D�6P   $	 0.4Q�6c   $	 0.4Q�6{   5      �� 5      "  60   8	   .�5   
   �� 
 � ��
 ���� 6'   �� 5   
   
 �@ 
ButtonSize$ 12,12 6'   �� 5   
   
 �@ ButtonSource$P Jpack://application:,,,/Imagin.Common.WPF;component/Images/TriangleDown.png��6)   �� 5   
   
 � ��# �  6'    5   
   P� 5       �@ MenuButtonVisibility$ Visible=�66   . �5      �� 
 �@ ContentMargin$ 5,4,0,4q�62   5      P� 5       �@ MenuButtonVisibility$ 	Collapsed=�66   . �5      �� 
 �@ ContentMargin$	 5,4q�62   5      P� 5       �@ 	IsChecked . . �5      �� 
 � �� �#007acc6/   �� 5      
 � �� �#FFF6/   �� 5      
 �@ SourceColor �#FFF60   5      ]� 5        5!      �� 5"       �@ MenuButtonVisibility$ Visible=�6@   �� 5#       � �� 63   Z� ./�5%      �� 
 �@ ContentMargin$ 0q�62   5&      ]� 5'       5(      �� 5)       �@ 	IsChecked .  65   �� 5*       �@J�IsMouseOver .  67   �� 5+       � �� .  65   ./�5-      �� 
 �@ SourceColor# �  60   5.      ]� 5/       50      �� 51       �@ 	IsChecked .  65   �� 52       �@J�IsMouseOver . 67   �� 53       � �� .  65   ./�55      �� 
 �@ SourceColor# �  60   56      ]� 57       58      �� 59       �@ 	IsChecked .  65   �� 5:       � �� . 65   ./�5<      �� 
 �@ SourceColor# �  60   5=      5?      �� 5A      "  6   8	   .�5B   
   �� 
 � ��
 ���� 6(   �� 5C   
   
 � ��$ 1q�6,   �� 5D   
   
 �@ CornerRadius$ 2��6I    5E   
   :� 5F      $ local:Crumb.Loaded��6   .[�5G      �� 8�� 6"   5H      P� 5I       �@ IsDraggingOver . . �5J      �� 
 � ��# �  60   5K      P� 5L       �@ IsDraggingOver .  . �5M      �� 
 � ��
 ���� 60   5N      P� 5O       �@J�IsMouseOver . . �5P      �� 
 � ��# �  6/   �� 5Q      
 � ��# �  60   �� 5R      
 � ��# �  6/   5S      P� 5T       � �� . . �5U      �� 
 � ��# � 	 6/   �� 5V      
 � ��# � 
 60   �� 5W      
 � ��# �  6/   5X      5Z      5[      