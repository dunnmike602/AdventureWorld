   M S B A M L   `   `   `  ����  AdventureWorldDesigner6    0DiagramDesigner.Controls.AboutBox.AboutBoxSimple   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$#  About Adventure Game Designer��5      
idAboutBox�� � Icon$  /Resources/Images/maze.png��6>      AboutDialogStyle#���   5       � WindowStartupLocation$ CenterOwner=�60    � 	Resources 5        	AboutLogo �� 
TargetType �� Property �� Value  DisplayAreaStyle  BackgroundStyle �� EndPoint � Offset	 � Color  ParagraphSeparator  LinkLabelStyle  ReadOnlyDescStyle  OkButtonStyle��%�  &	       &	  H     &	 �    &	 �    '��     &	 `    &	 �    &	     &	 @    �� 5   
   $ ��/Resources/Images/maze.png��6"   $��0 0 20 0q�6F   �� 5   
   " �6)   .�5	      ��  � ��$
 Auto��6'   �� 5
       � ��$	 500��6&   �� 5       �@�ShowInTaskbar .  6.   �� 5       �@�
ResizeMode$ NoResize=�6+   �� 5       �@�WindowStyle$ SingleBorderWindow=�6,   �� 5       �@�SizeToContent$ Height=�6.   5      �� 5   
   " ��6)   .�5      ��  � ��$ 
10,10,10,5q�6'   5      �� 5   
   " ��6(   .�5      ��  � �� 5      �� 5      $	 0,1&�6*   .��5      � $ 0Q�6'   $	 	LightBlue��62   � 5      $
 0.55Q�6'   $	 White��65   � 5      $
 0.85Q�6'   $	 White��65   � 5      $ 1Q�6'   $	 	LightBlue��62   5      5      5      �� 5   
   " ��6   .�5       ��  � ��$ 0q�6(   5!      �� 5"   
   " ��6+   .�5#      ��  � ��$ 0,10,0,0q�6(   5$      �� 5%   
   .�5&      ��  � ��$ 0q�60   �� 5'       � ��$ Center=�6:   5(      �� 5)   
   " ��6*   .�5*      ��  �@��MinLines$ 6��6)   �� 5+       �@��MaxLines$ 6��6)   �� 5,       �@��
IsReadOnly . 6+   �� 5-       �@��TextWrapping$ WrapWithOverflow=�6-   �� 5.      # �@��VerticalScrollBarVisibility$ Visible=�6<   5/      �� 50   
   " ��6&   .�51      ��  � ��$ 75��6)   �� 52       � ��$	 0,5q�6'   �� 53       � ��$ Right=�6/   �� 54       �@��	IsDefault . 6*   �� 55       �@��IsCancel . 6)   56      57      .��58      �� 
clientArea��#��[ 6$   .+�5:   
   �� 
 �� Orientation$
 
Horizontal=�6   $��10q�6.   .+�5;      ��  �� ResourceKey$ 	AboutLogo��6   �� 5<      $��Adventure Game Designer��6   $
��Bold"�67   $��20(�6I   5=      �� 5>   
   displayArea��#��[ 6)     AboutDataProvider � DataContext# [ 5?      .+�5@      �� versionArea��$
 
Horizontal=�6-   .+�5A      �� -   5         ��Name$ versionLabel��5A      VersionLabel��$��
Version - ��6C   �� 5B      -   5        $ version��5B      version����69   ��  �� Mode$ OneTime=� �� Path$ Version�5C      �� 5D      -   5        $ 	copyright��5D      	copyright����69   �� $ OneTime=�$ 	Copyright�]� 5G      
buttonArea��.+�5H      �� -   5         ��Name$ okButton��5H      okButton��#��[ 6<   $��OK��5I      �� 5J      -   5        $ productLink��5J      productLink��#��[ 6A   .��5K      �� -   5         ��Name$ 	hyperlink��5K       	hyperlink��#��[ 6y   ��6E   �� $ OneTime=�$ LinkUri�.�5L       .�5M      �� ��6$   �� $ OneTime=�$ LinkText�5N      5O      5P      5Q      5R      5S      