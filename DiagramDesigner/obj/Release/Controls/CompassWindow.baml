   M S B A M L   `   `   `  ���� Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:DiagramDesigner.ControlsDiagramDesigner.Controls  ,    &DiagramDesigner.Controls.CompassWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     ;controls&clr-namespace:DiagramDesigner.Controls       � WindowStyle$  SingleBorderWindow=� � 
ResizeMode$ NoResize=�$	��700��$	��900�� � Title$! Maintain Compass Directions�� � WindowStartupLocation$ CenterOwner=�.���  � RowDefinitions �� $��* ��� $��50 �.+���  �� Orientation$ 
Horizontal=�$��1��$��0��$
��Left=�$��5q�.+��� -   $��90�� � ToolTip$% Clear all values on the screen.��$��21��   AppButtonStyle#��[  $��22��$��2q�.��Clear�� -   $��90��$&  Load default values into screen.��$��22��#��[  $��22��$��2q�.��Load Defaults�� $ 
Horizontal=�$��1��$��0��$��Right=�$��5q�.+��� -   $��90��$��23��$" Save the compass directions.��#��[  $��22��$��2q� �� 	IsDefault . .��OK�� -    ��Name$ CancelButton��$��90��$��24��$( "Dismiss the screen without saving.��#��[  $��22��$��2q�	 �� IsCancel	 . .��Cancel� $��0��$��0��
 � ColumnDefinitions
 �� $��* ��� $	��150 �.+�� $��0��$��0��$��	-10 0 0 0q�
 �� $��* ��� $
��4.5* ��� $��* � �� $��* ��� $
��4.5* ��� $��* �.+��� $
��Fill=�$��1��$��1��$#��/Resources/Images/compass.png��*   $DiagramDesigner.Controls.RadialPanel  -     Name$ MainText��$��0��$��Stretch=�$��Stretch=�.+��� $ Vertical=�.+�.   (DiagramDesigner.Controls.TextOnlyTextBox  -     Name$
 Key1��$��1��$��0 0 0 0q�$��50��  -   $ Value1��$��2��$��0 0 0 0q�$��50���� $ Vertical=�.+�  -   $
 Key2��$��3��$��0 0 0 0q�$��50��  -	   $ Value2��$��4��$��0 0 0 0q�$��50���� $ Vertical=�.+�  -
   $
 Key3��$��5��$��0 0 0 0q�$��50��  -   $ Value3��$��6��$��0 0 0 0q�$��50���� $ Vertical=�.+�  -   $
 Key4��$��7��$��0 0 0 0q�$��50��  -   $ Value4��$��8��$��0 0 0 0q�$��50���� $ Vertical=�.+�  -   $
 Key5��$��9��$��0 0 0 0q�$��50��  -   $ Value5��$��10��$��0 0 0 0q�$��50���� $ Vertical=�.+�  -   $
 Key6��$��11��$��0 0 0 0q�$��50��  -   $ Value6��$��12��$��0 0 0 0q�$��50���� $ Vertical=�.+�  -   $
 Key7��$��13��$��0 0 0 0q�$��50��  -   $ Value7��$��14��$��0 0 0 0q�$��50���� $ Vertical=�.+�  -   $
 Key8��$��15��$��0 0 0 0q�$��50��  -   $ Value8��$��16��$��0 0 0 0q�$��50���� $ Vertical=�$��1��$��Center=�.+��� $ Vertical=�.+�  -   $
 Key9��$��17��$��0 0 0 0q�$	��100��  -   $ Value9��$��18��$��0 0 0 0q�$	��100���� $"��/Resources/Images/ladder.jpg���� $ Vertical=�.+�  -   $ Key10��$��19��$��0 0 0 0q�$	��100��  -   $ Value10��$��20��$��0 0 0 0q�$	��100��