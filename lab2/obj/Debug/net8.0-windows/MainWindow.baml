   M S B A M L   `   `   `  ����?  ;lab2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:lab2lab2  5   	       lab2.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:lab2     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � 
���OO/� � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   `�  `� 
Background
 �OO/�$��30��61   $	��Top=�6=   .�5      c� $	��100��6   $��30��6"   .@�5      o� .@�5      m� $
��File��6   .@�5      m� -   5        $
��Open��5   #   61   m� 5      -   5        $
��Exit��5   #   61   5      5      5      5      R� 5   
   -   5         R�Name$ treeView��5      $��1��6%   $��5q�62   ��6=   ��  � 	Resources 5         TreeViewItemContextMenu m�Name��%�   &	        �� 5      .@�5      m� -   5        $��Delete��5      6/   m� 5      $ openMenuItem��6H   -   5        $
��Open��5      6H   m� 5       $ createMenuItem��6L   -   5        $��Create��5       6L   5!      5"      ��5#      �� 5$       �� 
TargetType" Q�6   .�5%      ��  �� Property �@�ContextMenu �� Value# [  64   5&      5(      �� 5*   
   $��2��6   
�������.@�5+      �� .��5,      �� -   5        	 ��Name$	 
statusText��5,      5-      5.      �� 50   
   $��1��6   $
��Auto=�6$   $
��Auto=�6I   $��300,10,10,10q�6l   .��51      �� -   5        $	 fileContentTextBlock��51      $
��Wrap=�66   52      53      54      