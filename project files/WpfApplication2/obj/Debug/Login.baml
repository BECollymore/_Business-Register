   M S B A M L   `   `   `  ����J  FWpfApplication2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:StatsBRStatsBR  5   	       StatsBR.Login   5   	   q mTelerik.Windows.Controls.ImageEditor, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Charting, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.GridView, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7s oTelerik.Windows.Controls.RichTextBoxUI, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Chart, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Spreadsheet, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Diagrams, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j	 fTelerik.Windows.Controls.Data, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7m
 iTelerik.Windows.Controls.Docking, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7q mTelerik.Windows.Controls.Expressions, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2016.2.606.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7f bTelerik.Windows.Documents, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7u qTelerik.Windows.Controls.EntityFramework, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7y uTelerik.Windows.Controls.Diagrams.Extensions, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.RibbonView, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7{ wTelerik.Windows.Documents.Proofing.RadGridView, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2016.2.613.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7gtelerik1http://schemas.telerik.com/2008/xaml/presentation         	 
            W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:StatsBR     5   	   -   5          �Name$  frmLogin��5   L    � Title$ Login��5   	   $	��300��6   $	��300��6$    � WindowStartupLocation$ CenterScreen=�60   .��5	      � .+�5
   
   �� -   5         ��Name$ labelLoginName��5
      $��Login Name ��6(   $
��Left=�6>   $��	10,41,0,0q�6Y   $	��Top=�6l   $��81��6�   �� 5   
   -   5        $ labelPassword��5      $��	Password ��6'   $
��Left=�6;   $��	10,72,0,0q�6V   $	��Top=�6i   $��81��6�   �� 5   
   -   5         ��Name$ txtboxLoginName��5      $
��Left=�6+   $��23��6F   $��
103,43,0,0q�6R    �� TextWrapping$
 Wrap=�6f   $	��Top=�6z   $	��134��6�   �� 5   
   -   5         ��Name$ btnLogin��5      $��Submit��6#   $
��Left=�64   $��125,112,0,0q�6O   $	��Top=�6d   $��90��6|   $��26��6�   $
��Bold"�6�   6�   �� 5   
   -   5        $ AuthenticateUser��5      $ ��Use Windows authentication��6+   $
��Left=�6P   $��
91,188,0,0q�6k   $	��Top=�6   $	��170��6�   $��26��6�   $
��Bold"�6�   6�   �� 5   
   -   5        $ labelLoginSuccess��5      $�� ��6+   $
��Left=�66   $��
18,232,0,0q�6Q   $	��Top=�6e   $��25��6}   $	��264��6�   $��10(�6�   ���	#FFF01E1E6�   G� 5   
   -   5         G�Name$ passwordBox��5      $
��Left=�6+   $��
103,75,0,0q�6F   $	��Top=�6Z   $	��134��6r   $��23��6~   �� 5   
   $
��Left=�6   $��18��6/   $��
48,157,0,0q�6;   $	��Top=�6O   $	��222��6g   5      5      