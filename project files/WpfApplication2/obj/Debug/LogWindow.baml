   M S B A M L   `   `   `  ����J  FWpfApplication2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:StatsBRStatsBR  5   	       StatsBR.LogWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:StatsBR     5   	   -   5          �Name$  BRLogWindow��5   	    � Title$ BR LogWindow��5   	   $	��450��6   $	��500��6+   
������� � WindowStartupLocation$ CenterOwner=�6J   .��5
      � ���#c8cfc3$��3q�6    .+�5   
   �� .+�5      � ���	#FFD2EAF1 � ShowGridLines .   � RowDefinitions 5      �� 5      $��0.75* �6$   �� 5      $��0.25* �6$   .+�5      �� -   5         ��Name$ TxtBokLogWindow��5       �� 
IsReadOnly . $��2q�6F   $��5q�6R   " �� VerticalScrollBarVisibility$ Visible=�6]   
�������$��0��5   -   $	��360��6:    �� TextWrapping$
 Wrap=�6G   $��This is the Log Window...��6[   �� 5      $��1��6   	 �� Orientation$	 
Horizontal=�6*   $��Right=�6C   .+�5      �� -   5        
 ��Name$
 
BtnSaveLog��5      $	��100��61   $��30��6=   $��	10,0,5,10q�6I   $��Save Log��6\   5      �� 5      -   5        $
 BtnCloseLog��5      $	��100��62   $��30��6>   $��	10,0,5,10q�6J   $��Close��6]   5      5      5       5!      5#      5$      