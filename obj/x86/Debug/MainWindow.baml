   M S B A M L   `   `   `  ����X  THelixToolkit.Wpf, Version=2.20.2.0, Culture=neutral, PublicKeyToken=52aa3500039caf0dM8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.WpfHelixToolkit.Wpf  5   	   J FStereoStructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:StereoStructureStereoStructure 5   	       StereoStructure.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   QHelixToolkit8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.Wpf     5   	   Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  5   	   )localclr-namespace:StereoStructure  5   	   -   5          � Title$  StereoStructure��5
   	   $��680.958��6!   $��1029.265��62    � 
ResizeMode$ CanMinimize=�6C    � Icon$ icons/icon.png��6s   .��5      � .+�5   
   o� $��Center=�6   $��20��6,   $	��Top=�68   ���	#FF605E5E$	��0,1q�6h   .@�5      m� $��20��6    � Cursor$
 Hand��6#   
����������	#FF151414$	��1,0q�6\   ��5      �� 5      -   5        $��Model��5       $��Model��6-   $��Right=�6:   $��Center=�6V   ���	#FF2121216q   .@�5      m� -   5        $��20��5   0   $
 Hand��6<   ��5      �� 5      -   5        $��
SelectFile��5   $   $
��Open��66   $��Right=�6B   $��Center=�6^   ���	#FF2121216y   5      m� 5      -   5        $
 Hand��5   .   $��20��6<   ��5      �� 5      -   5        $��	ScanVideo��5   $   $��Create from video��65   $��Right=�6N   $��Center=�6j   5      m� 5      -   5         m�Name$ saveAsButton��5      $
 Hand��61   $��
Save as...��6?   {�.  6n   5      m� 5      -   5        $ SettingsButton��5   %   $
 Hand��6   
�������$��20��6P   ���	#FF151414$	��1,0q�6t   ��5       �� 5!      -	   5        $��SettingsTextBlock��5!       $��Settings��69   $��Right=�6I   $��Center=�6e   .@�5#      m� -
   5        $��LogsItem��5#      $
��Logs��6+    m� 	IsChecked . $
 Hand��6e   m� 5$      -   5        $��CorItem��5$      $��Correspondences��6*    . $
 Hand��6T   6b   m� 5%      -   5        $��EditItem��5%      {�.  $��	Edit mode��6=   $
 Hand��6P    m� InputGestureText$ E��6u   m� 5&      -   5        $��AccuracyItem��5&      $��Accuracy��6/   $
 Hand��6A   .@�5'      m� -   5        $ LowAccuracy��5'      $
 Hand��64   $	��Low��6B    . 6`   m� 5(      -   5        $ MediumAccuracy��5(      $
 Hand��67   $��Medium��6E   6U   m� 5)      -   5        $ HighAccuracy��5)      $
 Hand��65   $
��High��6C   6Q   5*      m� 5+      -   5        $��LangItem��5+      $��Language��6+   $
 Hand��6=   .@�5,      m� -   5        $ RU��5,      $
 Hand��6+   $��Russian��69   6J   m� 5-      -   5        $ EN��5-      $
 Hand��6+   $��English��69   6J   5.      m� 5/      -   5        $��AdvancedItem��5/      $��Advanced...��6/   $
 Hand��6D   6R   50      m� 52      $
 Hand��6   
�������$��20��68   ���	#FF151414$	��1,0q�6\   ��53      �� 54      -   5        $��About��54       $��About��6-   $��Right=�6:   $��Center=�6V   .@�56      m� -   5        $��Author��56      $��Author��6)   $
 Hand��69   $��
0,0,22.8,0q�6\   m� 57      -   5        $	��FAQ��57      $��
How to use��6&   $
 Hand��6:   6H   58      59      &    HelixToolkit.Wpf.HelixViewport3D  5:   
   -   5        $��scene��5:   '      ZoomExtentsWhenLoaded . $��0,22,0.2,0.2q�6Q      Children. 5;      $   HelixToolkit.Wpf.DefaultLights  g� 5<      -   5        model3d��	 g� Content	 5=      � 5>      
 � Material
 5?      u� 5@      -   5        matDiffuseMain�� u� Brush 5A   "   �� 5B   &    �� Color$
 Gray��66   5D      5F      5H      (   "HelixToolkit.Wpf.GridLinesVisual3D  5I      -   5        Grid��   Width$	 500Q�5I   ;      Length$	 500Q�6G      MinorDistance$ 20Q�6T      MajorDistance$ 1Q�6g      	Thickness$	 0.3Q�6y   5J      "  FontAwesome.WPF.ImageAwesome  5K   
   -   5          Name$
 wait��5K         Icon$ Refresh=�6(      Spin .  J� Opacity$ 0Q�6C   $��	0,10,10,0q�6O   $��30��6b   $��30��6m   $��Right=�6y   $	��Top=�6�   5L      5M      