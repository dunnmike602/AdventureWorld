   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      Q MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:DiagramDesigner.SymbolsDiagramDesigner.Symbols 5      C&clr-namespace:DiagramDesigner.ControlsDiagramDesigner.Controls 5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   DesignerItemContextMenu.   (System.Windows.Input.ApplicationCommands
     Cut m� Command m� Icon    Copy    Paste    Delete,  &DiagramDesigner.Symbols.DesignerCanvas   Group   Ungroup   BringForward	   BringToFront
   SendBackward   
SendToBack   ShowInExplorer $  MakeVisibleWhenDesignerConverter �� 	Converter!   ShowInConversationExplorer ( $MakeVisibleWhenConversationConverter'  !DiagramDesigner.Symbols.Connector �� 
TargetType �� Property �� Value �� 
TargetType  ConnectorDecoratorTemplate  Name   Orientation �� RelativeSource �� AncestorType*  $DiagramDesigner.Symbols.DesignerItem �� Path  TopNegativeConverter  TopPositiveConverter  LeftNegativeConverter  LeftPositiveConverter  BottomNegativeConverter 	 BottomPositiveConverter 
 RightNegativeConverter  RightPositiveConverter  ResizeDecoratorTemplate J� Opacity J� SnapsToDevicePixels*  $DiagramDesigner.Controls.ResizeThumb � Cursor(  "DiagramDesigner.Controls.DragThumb � ContextMenu � DataContext  Name ��Name ��Name  �� Triggers! �� Value" �� Binding# �� 
TargetName$ ^� 
Conditions% �� Binding& �� Value' �� Empty( P� Property) P� Value�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      .sys&clr-namespace:System;assembly=mscorlib  5      1symbols%clr-namespace:DiagramDesigner.Symbols  5      3controls&clr-namespace:DiagramDesigner.Controls  5      %�  &	        7  7  '  Y    &	 �    7  7  7  7  7  7 	 7 
 7  &	     '  �    '  a    7   7  7  �� 5      .@�5   
   m� $	��Cut��6   # Z  6     5      �� 5	      $��Images/Cut.png��6   $��16��60   5      m� 5   
   $
��Copy��6   # Z 6!    5      �� 5      $��Images/Copy.png��6   $��16��61   5      m� 5   
   $��Paste��6   # Z 6"    5      �� 5      $��Images/Paste.png��6   $��16��62   5      m� 5   
   $��Delete��6   # Z 6#    5      �� 5      $��Images/Delete.png��6   $��16��63   5      �� 5   
   m� 5   
   $��Grouping��6   .@�5      m� $��Group��6   # Z 6&    5      �� 5      $��Images/Group.png��6   $��16��66   5!      m� 5"      $��Ungroup��6   # Z 6(    5#      �� 5$      $��Images/Ungroup.png��6   $��16��68   5&      5'      m� 5(   
   $��Order��6   .@�5)      m� $��Bring Forward��6   # Z 6.    5*      �� 5+      $��Images/BringForward.png��6   $��16��6=   5-      m� 5.      $��Bring To Front��6   # Z	 6/    5/      �� 50      $��Images/BringToFront.png��6   $��16��6=   52      m� 53      $��Send Backward��6   # Z
 6.    54      �� 55      $��Images/SendBackward.png��6   $��16��6=   57      m� 58      $��Send To Back��6   # Z 6-    59      �� 5:      $��Images/SendToBack.png��6   $��16��6;   5<      5=      �� 5>   
   m� 5?   
   $��Show in Room Explorer��6   # Z 5@      y�5?   3   �� *Parent.Name+8   5@      m� 5A   
   $#��Show in Conversation Explorer��6   # Z 5B      y�5A   ;   �� 8  5B      5C      �� 5F      "  6   .�5G   
   ��  � ��$ 8��6"   �� 5H   
    � ��$ 8��6#   �� 5I   
    �@�Cursor$ Cross��6#   �� 5J   
    �@J�SnapsToDevicePixels . 60   �� 5K   
    � �� 5L      �� 5M      "  6"   .R�5N      � .+�5P      �� 
������ $��-2q�67   �� 5Q      
�������$��1��64   ���	#AA0000806H   5R      5S      5U      5V      �� 5Y      " ��69   .R�5Z   
   � $!��ConnectorDecoratorContainer��6   $��-5q�6   .+�5[        $
 Left��6    $
 Left=�6.   $��Center=�6A   $
��Left=�6\     5\      $ Left1��6    $
 Left=�6/   $��Center=�6B   $
��Left=�6]   ��5]       ��  �� "  $ ActualHeight�8     5_      $ Left2��6    $
 Left=�6/   $��Center=�6B   $
��Left=�6]   ��5`       ��  �� "  $ ActualHeight�8    5c      $	 Top��6    $	 Top=�6-   $	��Top=�6?   $��Center=�6W     5d      $
 Top1��6    $	 Top=�6.   $	��Top=�6@   $��Center=�6X   ��5e       ��  �� "  $ ActualHeight�8    5g      $
 Top2��6    $	 Top=�6.   $	��Top=�6@   $��Center=�6X   ��5h       ��  �� "  $ ActualHeight�8    5k      $ Right��6    $ Right=�6/   $��Center=�6C   $��Right=�6^     5l      $ Right1��6    $ Right=�60   $��Center=�6D   $��Right=�6_   ��5m       ��  �� "  $ ActualHeight�8    5o      $ Right2��6    $ Right=�60   $��Center=�6D   $��Right=�6_   ��5p       ��  �� "  $ ActualHeight�8    5s      $ Bottom��6    $ Bottom=�60   $��Bottom=�6E   $��Center=�6`     5u      $ Bottom1��6    $ Bottom=�61   $��Bottom=�6F   $��Center=�6a   ��5v       ��  �� "  $ ActualHeight�8    5x      $ Bottom2��6    $ Bottom=�61   $��Bottom=�6F   $��Center=�6a   ��5y       ��  �� "  $ ActualHeight�8  5{      5|      �� 5      " ��66   .R�5�   
   � $	 0.7Q�6    . .+�5�        $��3��6#   $ SizeNS��6.   $��0 -4 0 0q�6>   $	��Top=�5�      $��Stretch=�6.     5�      $��3��6#   $ SizeWE��6-   $��-4 0 0 0q�6=   $��Stretch=�5�      $
��Left=�62     5�      $��3��6#   $ SizeWE��6-   $��0 0 -4 0q�6=   $��Stretch=�5�      $��Right=�62     5�      $��3��6#   $ SizeNS��6.   $��0 0 0 -4q�6>   $��Bottom=�5�      $��Stretch=�61     5�      $��7��6#   $��7��6-   $ SizeNWSE��68   $��	-6 -6 0 0q�6J   $	��Top=�5�      $
��Left=�6.     5�      $��7��6#   $��7��6-   $ SizeNESW��68   $��	0 -6 -6 0q�6J   $	��Top=�5�      $��Right=�6.     5�      $��7��6#   $��7��6-   $ SizeNESW��68   $��	-6 0 0 -6q�6J   $��Bottom=�5�      $
��Left=�61     5�      $��7��6#   $��7��6-   $ SizeNWSE��68   $��	0 0 -6 -6q�6J   $��Bottom=�5�      $��Right=�61   5�      5�      �� 5�      "  6   .�5�   
   ��  � �� 5�      �� 5�      "  6"   .R�5�      �� 
������ 6    5�      5�      5�      �� 5�      "  6   .�5�   
   ��  � ��$ 10��6%   �� 5�   
    � ��$ 10��6&   �� 5�   
    �@J�SnapsToDevicePixels . 60   �� 5�   
    � �� 5�      �� 5�      "  6"   .R�5�      � 8    6   ��  �� *TemplatedParent+.+�5�        $ PART_DragThumb��6-   $ SizeAll��6E   �� 5�      $ PART_ResizeDecorator��6"   $y�	Collapsed=�5�   "   8�� 5�   "   �� 5�      $ PART_ContentPresenter��6+   $��Stretch=�5�   +   $��Stretch=�5�   +   #��z��5�   +   #��z��5�   +   �� 5�      $ PART_ConnectorDecorator��6"   $y�Hidden=�5�   "   8�� 5�   "   5�        5�      �� 5�      $
! True��6&   " 63   ��  �� *Self+$ IsGroup�.a�5�      �� $# PART_DragThumb��6%    � y�$ 	Collapsed=�6W   5�      ^� 5�      $ 5�      �� 5�   "   % 69   ��  �� *Self+$ 
IsSelected�& True6,   69   �� 5�   "   #& Z' 6,   % 6N   ��  �� *Self+$ ParentId�.0�5�      �� $# PART_ResizeDecorator��6%    � y�$ Visible=�6]   5�      P� 5�      ( �@J�IsMouseOver) . . �5�      �� $# PART_ConnectorDecorator��6%    � y�$ Visible=�6`   5�      �� 5�      $
! True��6&   " 63   ��  �� *Self+$ IsDragConnectionOver�.a�5�      �� $# PART_ConnectorDecorator��6%    � y�$ Visible=�6`   5�      �� 5�      $
! True��6&   " 63   ��  �� *Self+$ IsGroup�.a�5�      �� $# PART_ConnectorDecorator��6%    � y�$ Hidden=�6`   5�      5�      5�      5�      5�      