   M S B A M L   `   `   `  ����M  ICourseWorkPharmacy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:CourseWorkPharmacyCourseWorkPharmacy  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   +    %CourseWorkPharmacy.PrescriptionWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:CourseWorkPharmacy     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	     � Title$  Prescriptions��5	   	   $	��720��6   $	��900��6,   ���	#FFF3F3F3$	��720��5   	   $	��430��5   	    � 	Resources 5         FocusVisual �� Property �� Value J� SnapsToDevicePixels �� StrokeDashArray  Button.Static.Background �� Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border 	 Button.Disabled.Foreground 
 ButtonStyle1 �� 
TargetType �� 
TargetType	 ��Name
 ��Name �� Triggers P� Property P� Value �� 
TargetName��%8  &	        &	 �     &	 �     &	 �     &	     &	 6    &	 Y    &	 |    &	 �    &		 �    &	
 �    7   7  7  7  7  7  7  7  7  7 	 �� 5   
   .�5      ��  � �� 5      �� 5      .R�5      �� $��2q�6$    . #��� ��6J   $��1��6�   $	 1 2R�6�   5      5      5      �� 5   
   $ 	#FFDDDDDD��6;   �� 5   
   $ 	#FF707070��67   �� 5   
   $ 	#FFBEE6FD��6>   �� 5   
   $ 	#FF3C7FB1��6:   �� 5   
   $ 	#FFC4E5F6��6<   �� 5   
   $ 	#FF2C628B��68   �� 5   
   $ 	#FFA0A0A0��6=   �� 5   
   $ 	#FFADB2B5��69   �� 5    
   $ White��6=   �� 5!   
   " ��6%   .�5"      ��  �@�FocusVisualStyle8   61   �� 5#       � ��8  6+   �� 5$       � ��8  6,   �� 5%       � ��# � ��6+   �� 5&       � ��$ 1q�60   �� 5'       � ��$ Center=�6;   �� 5(       � ��$ Center=�69   �� 5)       � ��$ 1q�6(   �� 5*       � �� 5+      �� 5,      " ��6&   .R�5-      �� $	 border��6!   #��z��61   #��z��6]   #��z��6�    . ._�5.      �� $
 contentPresenter��6/   |�.  #��z��6[   #��z��6�   ��. # z 6�   #��z��6  5/       50      P� 51       �@��IsDefaulted . . �52   "   �� $ border��6@   # � ��6T    � ��53      P� 54       �@J�IsMouseOver . . �55   "   �� $ border��6?   8   � ��6S   �� 56   "   $ border��6@   8   � ��6T   57      P� 58       � �� . . �59   "   �� $ border��6?   8   � ��6S   �� 5:   "   $ border��6@   8   � ��6T   5;      P� 5<       � {� .  . �5=   "   �� $ border��6?   8   � ��6S   �� 5>   "   $ border��6@   8   � ��6T   �� 5?   "   $ contentPresenter��6K   8 	  � ��6i   5@      5B      5D      5E      5F      .��5G      �  � ColumnDefinitions 5H   
   �� 5I      $��* �6   �� 5J      $	��250 �6   .+�5L   
   ]� $��0��6   $��5q�6$   .+�5M      �� $	��Top=�6   .��6)   Prescriptions List6=   &   System.Windows.Controls.DataGrid  5N      -   5          Name$ PrescriptionsDataGrid��5O   �   $
��Auto=�5N   Q   ��.  $	��Top=�5O   =      CanUserAddRows .     AutoGenerateColumns .  $��5q�6�   $��0��6�      Columns 5P      0  *System.Windows.Controls.DataGridTextColumn  5Q      ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly .    Header$
 Name��6i      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6w   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *Patient.Name+ �� Mode$ OneWay=�6w     5R       . $ 	Medicines��6w   $ * 6�    6;   �� *PrescriptedMedicinesString+$ OneWay=�6�     5S      $ Reccomendations��6Z   $ 3* 6s    6)   �� *Reccomendations+$ TwoWay=�6s   5U      5V      �� 5X   
   -   5         ��Name$ 	InfoStack��5X      $��1��6(   $��5q�68   .+�5Y      �� .��6   Choose Window6#   �� 5Z      $��0 0 0 10q�6    �� Padding$ 10q�6(   $
��auto��65   ���	#FFABADB3$��1q�6[   ._�5[      �� .+�5\      �  5]      �� 5^      $��* �6/   �� 5_      $��* �6/   .+�5a      �� -   5         ��Name$ GoMedicinesButton��5a   B   $��0 0 5 0q�6!   $��0��62   $
��0 10q�6}   ���	#FF808BFF$��	Medicines��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6�   $��0q�6  ��6�   Z� 6  �� 5b      -   5        $ GoPatientsButton��5b   1   $��1��6!   $
��0 10q�6k   ���	#FF808BFF$��Patients��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6�   $��0q�6�   ��6�   Z� 6�   5c      5d      5e      �� 5f      .��6   Prescription Generation6-   �� 5g      $��0 0 0 10q�6   $ 10q�6'   $
��auto��64   ���	#FFABADB3$��1q�6Z   ._�5h      �� .+�5i      �  5j      �� 5k      $��* �6/   .+�5m      �� -   5        $ GeneratePrescription��5m   B   $��0 0 5 0q�6!   $��0��62   $
��0 10q�6�   ���	#FF808BFF$ ��Auto-generate Prescription��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6  $��0q�6)  ��6  Z� 6)  5o      5p      5q      �� 5r      .��6   Editing Panel6#   �� 5s      $ 10q�6   $
��auto��6"   ���	#FFABADB3$��1q�6H   ._�5t      �� .+�5v      �� Patient Select60   �� 5w      -   5        $��PatientComboBox��5w   C    �� SelectedIndex$ 0��6   $��0 5 0 15q�61   ��5x      �� 5y      .R�5z   "   �� ��6,   �� *Name+5{   #   5|      5~      �� 5�      Medicines Select62   �� 5�      -   5        $��MedicinesBox��5�   ^   $��Visible=�5�      ��.  $��80��5�   =   $��0 5 0 10q�6L   ��5�      �� 5�      " ��6$   .�5�   "   ��  � |� .  6>   5�      5�      �� 5�      Prescripted reccomendations6=   �� 5�      -   5         ��Name$ ReccomendationsBox��5�   2    �� TextWrapping$
 Wrap=�6   $��0 5 0 15q�6p   $��90��6�   � 5�       5�      �� 5�      $��* �6/   �� 5�      $��* �6/   .+�5�      �� -	   5        $ 	AddButton��5�   B   $��0 0 5 0q�6!   $��0��62   $
��0 10q�6m   ���	#FF49FF3D$	��Add��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6�   $��0q�6�   ��6�   Z� 6�   �� 5�      -
   5        $ DeleteButton��5�   1   $��1��6!   $
��0 10q�6c   ���	#FFFF3D3D$��Delete��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6�   $��0q�6�   ��6�   Z� 6�   5�      5�      5�      � 5�      $��0 10 0 0q�6    5�      �� 5�      $��* �6'   �� 5�      $��* �6'   .+�5�      �� -   5        $ DeserializeButton��5�   �   #��[
 6   $��0 0 3 0q�6�   $
��0 10q�6�   ���	#FFFF3D3D$��0��6�   $��Undo Changes��6	  $��Franklin Gothic Book)�6   
�������$��14(�6U  $��0q�6z  {�6?   ��  �� Path$$ ArePrescriptionsChangesUnsaved�$ OneWay=���6c  Z� 6z  �� 5�      -   5        $ SerializeButton��5�   �   #��[
 6   $��3 0 0 0q�6�   $
��0 10q�6�   ���	#FF49FF3D$��1��6�   $��Save Changes��6  $��Franklin Gothic Book)�6  
�������$��14(�6Q  $��0q�6v  {�6?   �� $$ ArePrescriptionsChangesUnsaved�$ OneWay=���6_  Z� 6v  5�      �� 5�      -   5        $ PrintButton��5�   ;   #��[
 6   $��0 10 0 0q�6k   $
��0 10q�6}   ���	#FF8E3DFF$��1��6�   $��Print Prescriptions��6�   $��Franklin Gothic Book)�6�   
�������$��14(�6  $��0q�6+  ��6  Z� 6+  �� 5�      -   5          ��Name$  
ActionInfo��5�   *   $��0 5 0 0 q�6   6@   5�      5�      5�      