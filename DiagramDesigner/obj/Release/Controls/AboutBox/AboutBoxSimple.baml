   M S B A M L   `   `   `  ����   AdventureWorldDesigner6    0DiagramDesigner.Controls.AboutBox.AboutBoxSimple   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$#  About Adventure Game Designer��
idAboutBox�� � Icon$  /Resources/Images/maze.png��   AboutDialogStyle#���    � WindowStartupLocation$ CenterOwner=� � 	Resources   	AboutLogo �� 
TargetType �� Property �� Value  DisplayAreaStyle  BackgroundStyle �� EndPoint � Offset	 � Color  ParagraphSeparator  LinkLabelStyle  ReadOnlyDescStyle  OkButtonStyle��%  &	       &	  5     &	     &	 ?    '�� �    &	     &	 ,    &	 c    &	 3    �� $ ��/Resources/Images/maze.png��$��0 0 20 0q��� " �.���  � ��$
 Auto����  � ��$	 500����  �@�ShowInTaskbar .  ��  �@�
ResizeMode$ NoResize=���  �@�WindowStyle$ SingleBorderWindow=���  �@�SizeToContent$ Height=��� " ��.���  � ��$ 
10,10,10,5q��� " ��.���  � �� �� $	 0,1&�.��� $ 0Q�$	 	LightBlue��� $
 0.55Q�$	 White��� $
 0.85Q�$	 White��� $ 1Q�$	 	LightBlue���� " ��.���  � ��$ 0q��� " ��.���  � ��$ 0,10,0,0q��� .���  � ��$ 0q���  � ��$ Center=��� " ��.���  �@��MinLines$ 6����  �@��MaxLines$ 6����  �@��
IsReadOnly . ��  �@��TextWrapping$ WrapWithOverflow=��� # �@��VerticalScrollBarVisibility$ Visible=��� " ��.���  � ��$ 75����  � ��$	 0,5q���  � ��$ Right=���  �@��	IsDefault . ��  �@��IsCancel . .���� 
clientArea��#��[ .+��� 
 �� Orientation$
 
Horizontal=�$��10q�.+���  �� ResourceKey$ 	AboutLogo���� $��Adventure Game Designer��$
��Bold"�$��20(��� displayArea��#��[   AboutDataProvider � DataContext# [ .+��� versionArea��$
 
Horizontal=�.+��� -    ��Name$ versionLabel��VersionLabel��$��
Version - ���� -   $ version��version������  �� Mode$ OneTime=� �� Path$ Version��� -   $ 	copyright��	copyright������ $ OneTime=�$ 	Copyright�]� 
buttonArea��.+��� -    ��Name$ okButton��okButton��#��[ $��OK���� -   $ productLink��productLink��#��[ .���� -    ��Name$ 	hyperlink��	hyperlink��#��[ ���� $ OneTime=�$ LinkUri�.�.��� ���� $ OneTime=�$ LinkText�