   M S B A M L   `   `   `  ���� n  jTelerik.Windows.Controls.Docking, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7mPclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.DockingTelerik.Windows.Controls  q mTelerik.Windows.Controls.RibbonView, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7�iclr-namespace:Telerik.Windows.Controls.RibbonView.Primitives;assembly=Telerik.Windows.Controls.RibbonView.Telerik.Windows.Controls.RibbonView.Primitives R NSystem.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089>.clr-namespace:System.Data;assembly=System.DataSystem.Data  WpfApplication2   StatsBR.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ktelerikDockingPclr-namespace:Telerik.Windows.Controls;assembly=Telerik.Windows.Controls.Docking     k gTelerik.Windows.Controls.Data, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7z	 vTelerik.Windows.Controls.Diagrams.Extensions, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o
 kTelerik.Windows.Controls.GridView, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.Diagrams, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Data, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7l hTelerik.Windows.Controls.Input, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Controls, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7o kTelerik.Windows.Controls.Charting, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Navigation, Version=2013.3.1016.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Stelerik1http://schemas.telerik.com/2008/xaml/presentation     	 
       �
Primitivesiclr-namespace:Telerik.Windows.Controls.RibbonView.Primitives;assembly=Telerik.Windows.Controls.RibbonView     	 
       MData.clr-namespace:System.Data;assembly=System.Data     	 
         � Title$  
MainWindow��$	��839��$
��1421�� � WindowState$ 	Maximized=�����  �� ImageSource ��Images/background_final.png��.��� .+�]� ��.  $��	0,0,10,10q�.+�)   #Telerik.Windows.Controls.RadDocking  $��
10,49,0,32q�$	��268�����	#FF4E6476.@�0   *Telerik.Windows.Controls.RadSplitContainer  .@�+   %Telerik.Windows.Controls.RadPaneGroup  -   $��0,0,-4,0q�.@�&    Telerik.Windows.Controls.RadPane  $��Pane 1��.��-  'Telerik.Windows.Controls.RadContextMenu  $��0,29,0,0q�$��14(�.@�*  $Telerik.Windows.Controls.RadMenuItem  $��BR��$
��Bold"�����  �� Color$ 	#FFFEFEFE�� �� Opacity$	 0.5Q�.@�  $��
Sub Item 1��  $��
Sub Item 2��  $	��NIC��$
��Bold"�.@�  $��
Sub Item 3��  $��
Sub Item 4��  $��
Sub Item 5��3  -Telerik.Windows.Controls.RadMenuSeparatorItem  $��RadMenuItem��.@�  $��
Sub Item 1��  $��
Sub Item 2��  $��
Sub Item 2��  $	��IRD��$
��Bold"�.@�  $��
Sub Item 1��  $��
Sub Item 2��  $	��VAT��$
��Bold"�.@�  $��
Sub Item 3��  $��
Sub Item 4��  $��
Sub Item 5��  $��RadMenuItem��$
��Left=�$	��238��$��0,0,-2,0q�$
��Bold"�.@�  $��
Sub Item 1��  $��
Sub Item 2��  $��
Sub Item 2��  $��Pane 2��.��*  $Telerik.Windows.Controls.RadPanelBar  $
��Left=�$	��250��$	��Top=�$	��238��$��0,25,0,0q�$��16(�.@�.	  (Telerik.Windows.Controls.RadPanelBarItem	  -    	 Name$ MainBrPanel��.
  (Telerik.Windows.Controls.RadTreeViewItem 
  DropPosition$ Inside=�$��Main BR��	  -   $ NicPanel��$ Inside=�$	��NIC��	  -   $ IrdPanel��$ Inside=�$	��IRD��	  -   $ VatPanel��$ Inside=�$	��VAT��	  -   $ RegistrarPanel��$ Inside=�$��	Registrar��,  &Telerik.Windows.Controls.RadTabControl  $��	0,49,0,32q�$��stretch=�$	��803��;  5Telerik.Windows.Controls.TabControl.RadTabControlBase   HeaderBackground �	#FF4E6476.@�)  #Telerik.Windows.Controls.RadTabItem  -     Name$ 
resultstab��$��	View Data�����	#FF7495EE$��50��$	��103��$
��Bold"�$��14(����	#FF688CAF.��� 
������ $��
0,-27,0,27q�	 � RowDefinitions	 �� .+�'  !Telerik.Windows.Controls.GroupBox  -   
  Name$
 chartGroupBox��$
��Left=�$	��271��$��
10,338,0,0q�$	��Top=�$	��781�����	#FF688CAF$��
View Graph��.��'  !Telerik.Windows.Controls.RadChart  $��RadChart��$
��Left=�$	��241��$	��Top=�$	��769��$��0,10,0,0q�*  $Telerik.Windows.Controls.RadComboBox  -	     Name$ cbxGraphType��$
��Left=�$��
10,609,0,0q�$	��Top=�$	��250��.@�.  (Telerik.Windows.Controls.RadComboBoxItem  $��Item 1��  $��Item 2��  $��Item 3��* 
 $Telerik.Windows.Controls.RadGridView  -
   $��radGridView1��$
��Left=�$��	10,31,0,0q�$	��Top=�$	��291��$	��781��+  %Telerik.Windows.Controls.RadDataPager  -   $��xRadDataPager��   PageSize$ 5��$��10,294,10,341q�   Source �� *Items+ �� ElementName$ radGridView1��  -   $ 	updateTab��$��Update Source�����	#FF7495EE$	��120��$
��Bold"�$��14(�.��� 
������   -   $ editTab��$��Edit Source�����	#FF7495EE$	��106��$
��Bold"�$��14(�.��� 
������ .+�*  $Telerik.Windows.Controls.RadDataForm  $
��Left=�$��	10,10,0,0q�$	��Top=�$	��527��$	��781��.  (Telerik.Windows.Controls.RadPropertyGrid  -     Name$ PropertyGrid1��$	��747��   LabelColumnWidth$	 130 �&   AutoGeneratePropertyDefinitions . $	��Top=�$	��289��$��5,49,0,0q�   Item �� *SelectedItem+$ radGridView1�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ 	#FF4E6476��   ShadowDepth$ 10Q�   
BlurRadius$ 15Q� � DataContext   System.Data.DataSet  �� 
���   �$��1q�$	��799��$	��Top=�$	��100���� $��Stretch=�$��36��$��Bottom=����	#FFFBF8F8.@��� $��
status bar��$��26��$��75��&   Telerik.Windows.Controls.RadMenu  $��0,10,0,0q�$	��Top=����	#FFF9FAFD   $ White��$ 10Q�.@�  -     Name$ fileMenuItem��$
��File��.@�  $��Configure DB Connection��  -   $��Input text file��  $��Item 2��.@�  $��
Sub Item 3��  $��
Sub Item 4��  $��
Sub Item 5��