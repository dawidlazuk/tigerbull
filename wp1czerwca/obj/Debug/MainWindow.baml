   M S B A M L   `   `   `  ����E  Awp1czerwca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:wp1czerwca
wp1czerwca  5   	       wp1czerwca.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:wp1czerwca     5   	   -   5          � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   ��5	      �� 5
   
    �� ImageSource$ obrazki/tło.jpg��6    � 	Resources 5      '  !wp1czerwca.HeightToWidthConverter   HeightToWidthConverter'   !wp1czerwca.AnimalToImageConverter  AnimalToImageConverter �� 
TargetType �� Property �� Value��%�   &	        &	      '�� &       5   
   6'     5   
   6'   �� 5   
   " ��6   .�5      ��  � ��$ 20(�6)   66   5      5      .��5      � -   5         �Name$
 grid��5       � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      .+�5   
   o� .@�5      m� -   5        $��Nowa gra��5      6)   m� 5      -   5        $��
Ustawienia��5      6+   5      �� 5   
   $��1��6   .+�5      �� 
��� d �$��5��6+   $	��150��6?   $	��100��6K    �� Right$ 20��6X   
	 �� Top$	 20��6j   6|   �� 5       
��� � �$	��150��6%   $	��100��61   $ 20��6>   $	 20��6P   
 J� Opacity$	
 0.5Q�6`   6p   � 5!      $ 20��6   $	 20��6%   $	��150��65   $��90��6A    5"      �� 5#      �� 5$       � ColumnDefinitions 5&      �� 5'      �� 5(      .+�5*      �� $��Center=�6   $��Center=�65   .��6P   Ruchy:6X   �� 5+      $��Center=�6   $��Center=�65   $��1��6P   ��6`   ��  �� Path$ MoveCounter� �� NotifyOnSourceUpdated . 6�   �� 5,      $��Center=�6   $��Center=�65   $��1��6P   .��6]   Tura:6d   (� 5-      $��Center=�6   $��Center=�67   $��1��6R   $��1��6_   .��5.      �� ��6   �� $ Round� �� 	Converter# [  . 5/      50      51      � 53   
   $��1��6    54      �� 55      �� 56      ��6#   ��  �� ElementName$
 grid��$ ActualHeight�# [  �� 57      .+�59      �� -   5        $��Canvas��59      $��1��6#   $��1��63   
��� � �$	
 0.5Q�6S   � 5:      -   5        $��GameGrid��5:      $��1��6#    5;      �� 5<      �� 5=      �� 5>      �� 5?       5A      �� 5B      �� 5C      �� 5D      �� 5E      �� 5F      5H      5I      5J      5K      