   M S B A M L   `   `   `  ����Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:DiagramDesigner.SymbolsDiagramDesigner.Symbols  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   ConnectionContextMenu.   (System.Windows.Input.ApplicationCommands     Delete m� Command m� Icon,   &DiagramDesigner.Symbols.DesignerCanvas   BringForward   BringToFront   SendBackward   
SendToBack(   "DiagramDesigner.Symbols.Connection �� 
TargetType �� 	Resources  Key	 �� Property
 �� Value T� X T� Y �� 
TargetType �� RadiusX �� RadiusY  SolidBorderBrush  ArrowSymbolBaseStyle �� BasedOn � ContextMenu � DataContext �� RelativeSource �� StrokeStartLineCap �� StrokeEndLineCap �� StrokeLineJoin J� SnapsToDevicePixels  ArrowGridStyle �� Left
 �� Top � LayoutTransform �� Angle �Name �� Triggers �� Value �� Binding  �� Path! �� 
TargetName 	 Arrow  Diamond�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      1symbols%clr-namespace:DiagramDesigner.Symbols  5      %  &	        '  �    7  7  7  7   7  7  7  7  7  7  7  �� 5      .@�5   
   m� $��Delete��6   # Z  6#    5      �� 5      $��Images/Delete.png��6   $��16��63   5
      �� 5   
   m� 5   
   $��Order��6   .@�5      m� $��Bring Forward��6   # Z 6.    5      �� 5      $��Images/BringForward.png��6   $��16��6=   5      m� 5      $��Bring To Front��6   # Z 6/    5      �� 5      $��Images/BringToFront.png��6   $��16��6=   5      m� 5      $��Send Backward��6   # Z 6.    5      �� 5      $��Images/SendBackward.png��6   $��16��6=   5      m� 5      $��Send To Back��6   # Z 6-    5      �� 5      $��Images/SendToBack.png��6   $��16��6;   5       5!      5"      �� 5$      "  6    5%   
   ���� 5'      ConnectionAdornerThumbStyle " n�68   .�5(      �� 	 � ��$
 12��6*   �� 5)      	 � ��$
 12��6+   �� 5*      	 �@J�SnapsToDevicePixels
 . 68   �� 5+      	 � z�
 5,      T� 5-      $ -6Q�6-   $ -6Q�64   5/      �� 50      	 � ��
 51      �� 52      " n�6*   .R�53      �� ���	#AADCDCDC
������$��1��6M   $ 0��6a   $ 0��6m   54      56      57      �� 59      ArrowGridStyle " �6+   .�5:      �� 	 � ��$
 15��6*   �� 5;      	 � ��$
 15��6+   �� 5<      	 � z�
 5=      T� 5>      $
 -7.5Q�6-   $
 -7.5Q�66   5@      5A      �� 5C      ArrowSymbolBaseStyle " F�61   .�5D      �� 	 � ��8
   6)   �� 5E      	 �@��Stretch$

 Fill=�6,   5F      �� 5H      	Arrow " F�6"   8  .�5I      �� 	 � ��"
 ��        � z  	= �     z 	6)   5J      �� 5L      Diamond " F�6$   8  .�5M      �� 	 � ��+
 ������    �    �����@KL     �    @KL 	6)   5N      5O      .�5P   
   �� 	 �@J�SnapsToDevicePixels
 . 60   �� 5Q   
   	 � ��
 5R      �� 5S      "  6"   .R�5T      �� 8   6   ��  �� *TemplatedParent+.+�5U      F� $��PART_ConnectionPath��6   $��2��5V      8�� $ Round=�5X      $ Round=�5Y      $ Round=�5Z       . ��5\      �� *PathGeometry+5]      � 5^      8��  5_      �� *AnchorPositionSource.X+ 5`      �� *AnchorPositionSource.Y+.+�5a      F� $��PART_SourceAnchorPath��6#    5b      �� 5c   "    62   �� *AnchorAngleSource+5e      � 5f      $ DirectionText��5i      8��  5g      �� *AnchorPositionSink.X+ 5h      �� *AnchorPositionSink.Y+.+�5j      F� $��PART_SinkAnchorPath��6#    5k      �� 5l   "    62   �� *AnchorAngleSink+5n      �� 5o      $��DirectionIndicator��6%   $��8(�6?   $
��Bold"�6L   
����  � 5p   *   �� *LabelPosition.X+ 5q   *   �� *LabelPosition.Y+5r      5s       5t      �� 5u      $ Arrow��6&    64   ��  �� *Self+$  SourceArrowSymbol�.a�5v      �� $! PART_SourceAnchorPath��6%   8
  	 � ��6Y   5w      �� 5x      $ Diamond��6&    66   ��  �� *Self+$  SourceArrowSymbol�.a�5y      �� $! PART_SourceAnchorPath��6%   8
  	 � ��6Y   5z      �� 5{      $ Arrow��6&    64   ��  �� *Self+$  SinkArrowSymbol�.a�5|      �� $! PART_SinkAnchorPath��6%   8
 	 	 � ��6W   5}      �� 5~      $ Diamond��6&    66   ��  �� *Self+$  SinkArrowSymbol�.a�5      �� $! PART_SinkAnchorPath��6%   8
 
 	 � ��6W   5�      5�      5�      5�      5�      