   M S B A M L   `   `   `  ����  SharedControls$    SharedControls.MultiTextEditor   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ` \Syncfusion.Tools.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a ]Syncfusion.Shared.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c894
syncfusion!http://schemas.syncfusion.com/wpf   5   	   2controls!http://schemas.syncfusion.com/wpf   5   	   -   5        $	��500��5	   7   $	��550��6D     � Title$  
Edit Items��5
   	    � WindowStartupLocation$ CenterOwner=�6    � 	Resources 5      2  ,Syncfusion.Windows.Tools.Controls.TabItemExt �� 
TargetType �� Property �� Value �� Path �� Mode�� 5   
   %o   '        �� 5      "  6   .�5      ��  � �� 6+   �� $ 	LabelText�$ OneWay=�5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   �� 	 �� Orientation$	 
Horizontal=�6   $��1��6.   $
��Left=�6;   $��5q�6V   .+�5      �� -   5        
 ��Name$
 	AddButton��5   n   $	��Add��6      AppButtonStyle#��[  6$   $��2q�6L   $��60��6W   $��22��6b   $
��Left=�6�   5      �� 5      -   5        $
 DeleteButton��5   q   $��Delete��6   #��[  6'   $��2q�6O   $��60��6Z   $��22��6e   $
��Left=�6�   5      5      �� 5   
   $	 
Horizontal=�6   $��1��6.   $��Right=�6;   $��5q�6W   .+�5      �� -   5        $��60��5      #��[  6!   $��22��6I   $��2q�6U    �� 	IsDefault . .��6�   OK6�   �� 5      -   5        $
 CancelButton��5   p   $��60��6   #��[  6!   $��22��6I   $��2q�6U    �� IsCancel . .��6�   Cancel6�   5       5  /Syncfusion.Windows.Tools.Controls.TabControlExt  5!   
   -   5          Name$ TextTabControl��5!   #   $��0��6;   $��Stretch=�6H   $��Stretch=�5"      
������    EnableLabelEdit .     IsNewButtonEnabled .     CloseButtonType$ Common=�6w   ��5!   f   �� 5$      5%      5&      