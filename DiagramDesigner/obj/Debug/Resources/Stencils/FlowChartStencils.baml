   M S B A M L   `   `   `  ����Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:DiagramDesigner.ToolboxImplDiagramDesigner.ToolboxImpl  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089)    #DiagramDesigner.ToolboxImpl.Toolbox   FlowChartStencils     ItemSize J� SnapsToDevicePixels � ToolTip J� IsHitTestVisible � RowDefinitions ��Name ��Name �� FallbackValue �� Source	 �� Path
 J� Opacity �� 
TargetType �� Triggers �� Binding �� ElementName �� Value �� Property �� Value �� TextAlignment � Cursor�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      9toolboxImpl)clr-namespace:DiagramDesigner.ToolboxImpl  5      %�  &	           5      $  80,80��60    . $��	0 -10 0 0q�6\   $��Hidden=�6o   $��Hidden=�5      @�5   
   � 5	      -   5        $��RoomGrid��5	   "   $
 Room��6    .  .+�5
      �� $��1q�6   
���   �._�5      � 
������� 5      �� 5      $��15 �6,   6:   �� 5      $��* �6,   69   .+�5      �� -   5        $ RoomRectangle��5   A   $��0��6!   
����  �6A   �� 5      -   5        $ RoomText��5   r   $��1��6$   $
��Bold"�61   $��Center=�6�   $��Center=�5   $   $
��Wrap=�6?   
���   ���5   C   �� *
BaseName+$
 Room����5   $   �� $ RoomRectangle��$	 ActualWidth���5   $   �� $ RoomRectangle��$	 ActualHeight��� 5      -   5        $��	RoomImage��5   -   $��1��6    $
 .2Q�6>    .  6g   5      5      5      � 5      -   5        $��
ObjectGrid��5   -   $ PlaceableObject��6    .  .+�5      �� $��1q�6   
���   �._�5      � 
������� 5      �� 5      $��15 �6,   6:   �� 5      $��* �6,   69   �� 5       $
��Auto �6,   6<   .+�5"      �� -   5        $ ObjectRectangle��5"   @   $��0��6!   
���  ��6@   �� 5#      -   5        $ 
ObjectText��5#   t   $
��Bold"�6U   $��1��6g   $��Center=�6�   $��Center=�5$   $   
���   �$
��Wrap=�5'   $   ��5#   $   �� *
BaseName+$ Object����5%   $   �� $ ObjectRectangle��$	 ActualWidth���5&   $   �� $ ObjectRectangle��$	 ActualHeight�5'   $   �� 5(      -   5        $��ObjectImage��5(   -   $��1��6    $
 .2Q�6@    .  6i   �� 5*      -	   5        $  ObjectParentNameBackGround��5*   <   $��2��6!   $��18��60   $��Bottom=�6a   ._�5+      �� -
   5        $ ObjectParentName��5+   (   $
��Left=�6C   $��10(�6^   
�������$��Center=�5,   (   $��CharacterEllipsis=�6C   $
��Bold"�6d   $��NoWrap=�5-   (   5.      ��5/      �� 50   "   " ��6(    51   &   �� 52   *    66   �� $ ObjectParentName��$
	 Text�  6r   .a�53   .   ��  � y�$ 	Collapsed=�6K   54   +   �� 55   *    66   �� $ ObjectParentName��$
	 Text� 6r   Z� .a�56   .   ��  � y�$ 	Collapsed=�6K   57   +   59   #   5;      5<      5=      5>      � 5@      -   5        $��ContainerGrid��5@   '   $ 	Container��6    .  .+�5A      �� $��1q�6   
���   �._�5B      � 
������� 5C      �� 5D      $��15 �6,   6:   �� 5E      $��* �6,   69   �� 5F      $
��Auto �6,   6<   .+�5H      �� -   5        $ ContainerRectangle��5H   K   $��0��6!   
���/kU�6K   �� 5I      -   5        $ ContainerText��5I   e   $��1��6X   $ Center=�6|   $��Center=�6�   $��Center=�5J   $   $
��Bold"�6?   
���   �$
��Wrap=�5M   $   ��5I   $   �� *
BaseName+$ 	Container����5K   $   �� $ ContainerRectangle��$	 ActualWidth���5L   $   �� $ ContainerRectangle��$	 ActualHeight�5M   $   �� 5N      -   5        $��ContainerImage��5N   -   $��1��6    $
 .2Q�6C    .  6l   �� 5P      -   5        $# ContainerParentNameBackGround��5P   <   $��2��6!   $��18��60   $��Bottom=�6d   ._�5Q      �� -   5        $ ContainerParentName��5Q   (   $
��Left=�6F   $��10(�6a   
�������$
 Hand��6�   $��Center=�5R   (   $��CharacterEllipsis=�6C   $
��Bold"�6d   $��NoWrap=�5S   (   5T      ��5U      �� 5V   "   " ��6(    5W   &   �� 5X   *    66   �� $ ContainerParentName��$
	 Text�  6u   .a�5Y   .   ��  � y�$ 	Collapsed=�6K   5Z   +   �� 5[   *    66   �� $ ContainerParentName��$
	 Text� 6u   Z� .a�5\   .   ��  � y�$ 	Collapsed=�6K   5]   +   5_   #   5a      5b      5c      5d      � 5e      -   5        $��NpcGrid��5e   !   $	 Npc��6    .  .+�5f      �� $��1q�6   
���   �._�5g      � 
������� 5h      �� 5i      $��15 �6,   6:   �� 5j      $��* �6,   69   �� 5k      $
��Auto �6,   6<   .+�5m      �� -   5        $ NpcRectangle��5m   F   $��0��6!   
��� ���6F   �� 5n      -   5        $ NpcText��5n   _   $��1��6R   $ Center=�6p   $��Center=�6�   $��Center=�5o   $   
���   �$
��Bold"�6R   $
��Wrap=�5r   $   ��5n   $   �� *
BaseName+$	 NPC����5p   $   �� $ NpcRectangle��$	 ActualWidth���5q   $   �� $ NpcRectangle��$	 ActualHeight�5r   $   �� 5s      -   5        $��NpcImage��5s   -   $��1��6    $
 .2Q�6=    .  6f   �� 5t      -   5        $ NpcParentNameBackGround��5t   <   $��2��6!   $��18��60   $��Bottom=�6^   ._�5u      �� -   5        $ NpcParentName��5u   (   $
��Left=�6@   $��10(�6[   
�������$
 Hand��6|   $��Center=�5v   (   $��CharacterEllipsis=�6C   $
��Bold"�6d   $��NoWrap=�5w   (   5x      ��5y      �� 5z   "   " ��6(    5{   &   �� 5|   *    66   �� $ NpcParentName��$
	 Text�  6o   .a�5}   .   ��  � y�$ 	Collapsed=�6K   5~   +   �� 5   *    66   �� $ NpcParentName��$
	 Text� 6o   Z� .a�5�   .   ��  � y�$ 	Collapsed=�6K   5�   +   5�   #   5�      5�      5�      5�      5�      5�      