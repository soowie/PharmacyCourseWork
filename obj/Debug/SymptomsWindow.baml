   M S B A M L   `   `   `  ����M  ICourseWorkPharmacy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:CourseWorkPharmacyCourseWorkPharmacy  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   '    !CourseWorkPharmacy.SymptomsWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:CourseWorkPharmacy     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	     � Title$  Symptoms��5	   	   $	��660��6   $	��700��6'   ���	#FFF3F3F3$	��660��5   	   $	��430��5   	    � 	Resources 5         dataFromEnum S� 
MethodName S� 
ObjectType  System.Enum S� MethodParameters M� TypeName  FocusVisual �� Property �� Value J� SnapsToDevicePixels	 �� StrokeDashArray  Button.Static.Background
 �� Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background 	 Button.Disabled.Border 
 Button.Disabled.Foreground  ButtonStyle1 �� 
TargetType �� 
TargetType ��Name ��Name �� Triggers P� Property P� Value �� 
TargetName��%�  &	        &	 m     &	     &	 :    &	 ]    &	 �    &	 �    &	 �    &	 �    &		     &	
 /    &	 N    7  7  7  7  7  7  7  7  7 	 7 
 S� 5   
   $ 	GetValues��62   "  5       5      M� 5      $ local:Gender��6   5      �� 5   
   .�5      ��  � �� 5      �� 5      .R�5      �� $��2q�6$    . #��� ��6J   $��1��6�   $		 1 2R�6�   5      5      5      �� 5   
   $
 	#FFDDDDDD��6;   �� 5   
   $
 	#FF707070��67   �� 5   
   $
 	#FFBEE6FD��6>   �� 5    
   $
 	#FF3C7FB1��6:   �� 5!   
   $
 	#FFC4E5F6��6<   �� 5"   
   $
 	#FF2C628B��68   �� 5#   
   $
 	#FFA0A0A0��6=   �� 5$   
   $
 	#FFADB2B5��69   �� 5%   
   $
 White��6=   �� 5&   
   " ��6%   .�5'      ��  �@�FocusVisualStyle8   61   �� 5(       � ��8  6+   �� 5)       � ��8  6,   �� 5*       � ��# � ��6+   �� 5+       � ��$ 1q�60   �� 5,       � ��$ Center=�6;   �� 5-       � ��$ Center=�69   �� 5.       � ��$ 1q�6(   �� 5/       � �� 50      �� 51      " ��6&   .R�52      �� $ border��6!   #��z��61   #��z��6]   #��z��6�    . ._�53      �� $ contentPresenter��6/   |�.  #��z��6[   #��z��6�   ��. # z 6�   #��z��6  54       55      P� 56       �@��IsDefaulted . . �57   "   �� $ border��6@   # � ��6T    � ��58      P� 59       �@J�IsMouseOver . . �5:   "   �� $ border��6?   8   � ��6S   �� 5;   "   $ border��6@   8   � ��6T   5<      P� 5=       � �� . . �5>   "   �� $ border��6?   8   � ��6S   �� 5?   "   $ border��6@   8   � ��6T   5@      P� 5A       � {� .  . �5B   "   �� $ border��6?   8   � ��6S   �� 5C   "   $ border��6@   8   � ��6T   �� 5D   "   $ contentPresenter��6K   8 	  � ��6i   5E      5G      5I      5J      5K      .��5L      �  � ColumnDefinitions 5M   
   �� 5N      $��* �6   �� 5O      $	��250 �6   .+�5Q   
   ]� $��0��6   $��5q�6$   .+�5R      �� $	��Top=�6   .��6)   Symptoms List68   �� 5S      -   5         ��Name$ SymptomsList��5U      $	��Top=�5S      $��Visible=�6,   ��.  
���������5V      �� 5W      .R�5X      ��  �� Orientation$ 
Horizontal=�6%   .+�5Y      �� ��6(   �� 5Z      5[      5]      5^      �� 5`   
   $��1��6   $��5q�6%   .+�5a      �� .��6   Editing Panel6#   �� 5b       �� Padding$ 10q�6   $
��auto��6"   ���	#FFABADB3$��1q�6H   ._�5c      �� .+�5e      �� 	Symptom6)   �� 5f      -   5         ��Name$ 
SymptomBox��5f      $��2q�62   $��0 5 0 15q�6>   6R   � 5h       5i      �� 5j      $��* �6/   �� 5l      $��* �6/   .+�5n      �� -   5         ��Name$ 	AddButton��5n   B   $��0 0 5 0q�6!   $��0��62   $
��0 10q�6n   ���	#FF49FF3D$	��Add��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6�   $��0q�6�   ��6�   Z� 6�   �� 5o      -   5        $ DeleteButton��5o   1   $��2��6!   $
��0 10q�6c   ���	#FFFF3D3D$��Delete��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6�   $��0q�6�   ��6�   Z� 6�   5p      5q      5r      � 5s      $��0 10 0 0q�6    5t      �� 5u      $��* �6'   �� 5v      $��* �6'   .+�5x      �� -   5        $ DeserializeButton��5x   �   #��[ 6   $��0 0 3 0q�6�   $
��0 10q�6�   ���	#FFFF3D3D$��0��6�   $��Undo Changes��6  $��Franklin Gothic Book)�6  
�������$��14(�6P  $��0q�6u  {�6?   ��  �� Path$ AreSymptomsChangesUnsaved� �� Mode$ OneWay=���6^  Z� 6u  �� 5y      -   5        $ SerializeButton��5y   �   #��[ 6   $��3 0 0 0q�6�   $
��0 10q�6�   ���	#FF49FF3D$��1��6�   $��Save Changes��6   $��Franklin Gothic Book)�6  
�������$��14(�6L  $��0q�6q  {�6?   �� $ AreSymptomsChangesUnsaved�$ OneWay=���6Z  Z� 6q  5z      �� 5{      -   5         ��Name$ 
ActionInfo��5{      6.   5}      5~      5      