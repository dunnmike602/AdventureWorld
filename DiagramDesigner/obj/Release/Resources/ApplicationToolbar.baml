   M S B A M L   `   `   `  ���� Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:DiagramDesignerDiagramDesigner  A%clr-namespace:DiagramDesigner.SymbolsDiagramDesigner.Symbols  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   	MyToolbar  ToolbarGroup  �� Orientation  ToolBarButtonBaseStyle.   (System.Windows.Input.ApplicationCommands
    New �� ElementName � RowDefinitions    Open � ColumnDefinitions    Save    Print    Paste
	    Cut
    Copy    Delete  ToolbarBorderBrush �� CornerRadius,   &DiagramDesigner.Symbols.DesignerCanvas   	AlignLeft   AlignHorizontalCenters   
AlignRight   AlignTop   AlignVerticalCenters   AlignBottom   DistributeHorizontal   DistributeVertical	 �� X1	 �� Y1	 �� X2	 �� Y2 �� Stretch   Group   Ungroup   BringForward   BringToFront   SendBackward   
SendToBack�� F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     #sclr-namespace:DiagramDesigner  1symbols%clr-namespace:DiagramDesigner.Symbols  %]  &	        7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  7  c� $	��120��.@��� $
��File��8��  $��5,3,3,3q�.���� $��1��$  
Horizontal=�.+��� $��3q�$��55��8�� $��Center=�#��Z ���� $ 
RootWindow�����  �� $��4* ��� $��1* �.+��� $ ��Images/GenericDocument.png��$��45���� $��1��$	��New��$��Bottom=�$��Center=��� $  Vertical=�$��0,2,0,2q�.+��� $��1q�$��2q�$
��Left=�8�� #��Z ���� $ 
RootWindow�����  �� �� .+��� $��Images/OpenFolder.png��$��16���� $��3,0,3,0q�$
��Open��$��Center=�$��1���� $��1q�$��2q�$
��Left=�8�� #��Z ���� $ 
RootWindow�����  �� �� .+��� $��Images/Save.png��$��16���� $��3,0,3,0q�$
��Save��$��Center=�$��1���� $��1q�$��2q�$
��Left=�8�� #��Z ���� $ 
RootWindow�����  �� �� .+��� $��Images/Print.png��$��16���� $��3,0,3,0q�$��Print��$��Center=�$��1���� $
��Edit��8�� $��3q�.���� $��1��$  
Horizontal=�.+��� $��3q�$��55��8�� #��Z ���� $ RoomDesigner�����  �� $��4* ��� $��1* �.+��� $��Images/PasteBig.png��$��45���� $��1��$��Paste��$��Bottom=�$��Center=��� $  Vertical=�$��0,2,0,2q�.+��� $��1q�$��2q�$
��Left=�8�� #��Z	 ���� $ RoomDesigner�����  �� �� .+��� $��Images/Cut.png��$��16���� $��3,0,3,0q�$	��Cut��$��Center=�$��1���� $��1q�$��2q�$
��Left=�8�� #��Z
 ���� $ RoomDesigner�����  �� �� .+��� $��Images/Copy.png��$��16���� $��3,0,3,0q�$
��Copy��$��Center=�$��1���� $��1q�$��2q�$
��Left=�8��	 #��Z ���� $ RoomDesigner�����  �� �� .+��� $��Images/Delete.png��$��16���� $��3,0,3,0q�$��Delete��$��Center=�$��1���� $��	Alignment��8��
 $��3q�.���  �� $
��Auto ��� .+��� $��0��$  Vertical=�.+��� 8�� $��1q�$ 2��$��3q�._��� $  
Horizontal=�.+��� $��5q�8�� #��Z ���� $ RoomDesigner������ $!��Images/AlignObjectsLeft.png��$��16���� $��5q�8�� #��Z ���� $ RoomDesigner������ $/��)Images/AlignObjectsCenteredHorizontal.png��$��16���� $��5q�8�� #��Z ���� $ RoomDesigner������ $"��Images/AlignObjectsRight.png��$��16���� 8�� $��1q�$ 2��$��3q�._��� $  
Horizontal=�.+��� $��5q�8�� #��Z ���� $ RoomDesigner������ $ ��Images/AlignObjectsTop.png��$��16���� $��5q�8�� #��Z ���� $ RoomDesigner������ $-��'Images/AlignObjectsCenteredVertical.png��$��16���� $��5q�8�� #��Z ���� $ RoomDesigner������ $#��Images/AlignObjectsBottom.png��$��16���� $��1��$  Vertical=�$��5,0,0,0q�.+��� $��3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $,��&Images/DistributeObjectsHorizontal.png��$��16���� $��3q�$��
Horizontal��$��Center=�$��1���� $��3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $*��$Images/DistributeObjectsVertical.png��$��16���� $��3q�$��Vertical��$��Center=�$��1���� 8�� $��1��$ 0��$ 0��$ 0��$ 20��$
 Fill=�$
��Left=�$��3,0,0,0q�$��1���� $��Arrange��8�� $��3q�.���  �� $
��Auto ��� �� .+��� .+��� $��0,3,0,3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $��Images/Group.png��$��16���� $��3q�$��Group��$��Center=�$��1���� $��0,3,0,3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $��Images/Ungroup.png��$��16���� $��3q�$��Ungroup��$��Center=�$��1���� $��1��$��7,0,0,0q�.+��� $��0,3,0,3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $��Images/BringForward.png��$��16���� $��3q�$��Forward��$��Center=�$��1���� $��0,3,0,3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $��Images/BringToFront.png��$��16���� $��3q�$��To Front��$��Center=�$��1���� 8�� $��1��$ 0��$ 0��$ 0��$ 20��$
 Fill=�$
��Left=�$��1��$��3,0,0,0q��� $��2��.+��� $��0,3,0,3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $��Images/SendBackward.png��$��16���� $��3q�$��Backward��$��Center=�$��1���� $��0,3,0,3q�$��5q�$
��Left=�8�� #��Z ���� $ RoomDesigner�����  �� �� .+��� $��Images/SendToBack.png��$��16���� $��3q�$��To Back��$��Center=�$��1��