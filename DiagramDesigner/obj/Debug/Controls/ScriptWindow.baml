   M S B A M L   `   `   `  ����Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:DiagramDesigner.AdventureWorld.Domain%DiagramDesigner.AdventureWorld.Domain  5   	   Y1clr-namespace:DiagramDesigner.Controls.Behaviours#DiagramDesigner.Controls.Behaviours  5	      +    %DiagramDesigner.Controls.ScriptWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ] YICSharpCode.AvalonEdit, Version=5.0.3.0, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonedit.http://icsharpcode.net/sharpdevelop/avalonedit  5   	   ` \Syncfusion.Tools.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a ]Syncfusion.SfGrid.WPF, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a	 ]Syncfusion.Shared.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c896
syncfusion!http://schemas.syncfusion.com/wpf   	 5   	   Clocal3clr-namespace:DiagramDesigner.AdventureWorld.Domain   	 5   	   Z
 VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/ 
 5   	   :d2http://schemas.microsoft.com/expression/blend/2008 
 5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006 
 6B   B
behaviours1clr-namespace:DiagramDesigner.Controls.Behaviours 
 5	      -   5        .��5      �   � ColumnDefinitions  5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��3.5* �6   .+�5   
   �� $��1��6   $��3��6'   $��Stretch=�61   
������ 6P   � 5   
   $��0��6   $��0��6    � RowDefinitions 5      �� 5      $��* �6    �� 5      $��40 �6    .+�5      �� $��Script Explorer��6   $��5,5,5,1q�61      AltGroupBox#��[  6C   
������  J� IsHitTestVisible . ��5      �� 5      .R�5      � $��Stretch=�6   $��Stretch=�6=     5      �� 5   "   $��* �63   �� 5   "   $��50 �63   .+�5!      �� $��Script Explorer��6(   $��0��6?   $
��Bold"�6O   $
��Left=�6a   $��Center=�6|    �� Padding$ 5 0 0 0q�6�    �� TextAlignment$
 Left=�6�   �� 5"      $��1��6)    �� Orientation$ 
Horizontal=�69   $��Right=�6R   .+�5#   "   ��  ��Name$ 	CmdExpand��6{   -   5        $��2q�5#   )     ToolBarButtonBaseStyle#��[ 65    � ToolTip$ 
Expand All��6e   .��5$   &   " 
 FontAwesome.WPF.ImageAwesome     Icon$ 
PlusSquare=�66   $��Center=�6I   $��16��6d   $��Center=�6o   5%   #   �� 5&   "   $ CmdCollapse��6}   -   5        $��2q�5&   )   #��[ 65   $ Collapse All��6e   .��5'   &     $ MinusSquare=�66   $��Center=�6J   $��16��6e   $��Center=�6p   5(   #   5)      5*      5+      .��5-      R� -   5        	 R�Name$	 ScriptExplorer��5-      
 J� 	AllowDrop
 .  $��Stretch=�6w   
������ $��Stretch=�5.      .@�5/      Q� -   5         Q�Name$ AllScriptsContainer��5/   $   $��All Scripts��6A   
���   �$
��Left=�50      
������ ��5/   i   �� *ScriptContainerHeaders+��51      �� 52       �� 
TargetType" Q�6$   .�53   "   ��  �� Property �@Q�
IsExpanded �� Value 6?   �� *
IsSelected+ �� Mode$ TwoWay=��� 54   "    �@Q�
IsSelected 6?   �� *
IsSelected+$ TwoWay=��� 55   "   >   8DiagramDesigner.Controls.Behaviours.TreeViewItemBehavior% �@ IsBroughtIntoViewWhenSelected . 6r   56       � 	Resources 58       �� DataTypeA   ;DiagramDesigner.AdventureWorld.Domain.ScriptContainerHeader �� ItemsSource;   5DiagramDesigner.AdventureWorld.Domain.ScriptContainer��%�  (��       59   7   *, +)(�� 6    5A   +   *, +)�� 59      "  67    6g   �� *ScriptContainers+.R�5:   "   �� $ 
Horizontal=�6-   .+�5;   &   �� $ 
Horizontal=�61   .+�5<   *   �� $��0 3 0 3q�60   $��20��6A   $��18��6M   ��6X   �� *Image+�� 5=   *   $��5 3 0 3q�64   ��6E   �� *Header+5>   '   5?   #   5@      �� 5A      "  6+   .R�5B   "   �� $ 
Horizontal=�6-   $��0 3 0 3q�6F   .+�5C   &   �� $��0 0 3 0q�6,   $��20��6=   $��18��6I    6T   �� *Script.ScriptStateText+��6�   �� *Script.ScriptStateImage+�� 5D   &   ��60   �� *Name+ 6F   �� *	Script.Id+5E   #   5F      5G      5H      5I      5J      ]� 5K      $��1��6   .+�5L      �� -   5        $ CmdValidateAll��5L   �   $��8,8,8,8q�6     AppButtonStyle#��[ 6,   $
��Left=�6U   $
��Left=�6p   $	��100��6�   $��Compile All��6�   6�   5M      5N      � 5O   
   $��2��6    5P      �� 5Q      $��* �6    �� 5R      $��40 �6    .+�5T      5  /Syncfusion.Windows.Tools.Controls.TabControlExt  -   5          Name$ 	ScriptTab��5T   4   $��0��6'   $��0q�6G   $��Stretch=�6R      IsNewButtonEnabled .  
������    EnableLabelEdit .     CloseButtonType$
 Hide=�5U   ;   .@�5V      2  ,Syncfusion.Windows.Tools.Controls.TabItemExt  $��Script��6(   .��5W      ]� .+�5X      c� $	��Top=�6"    5Y       �� Triggers �� Binding �� RelativeSource �� AncestorType �� AncestorLevel �� Path �� Value��%�   '��       �� 5Z   "   " ��6(    5[   &   �� 5\   *    66   ��  �� " ��$ 1��$ 	IsEnabled�	 False6�   .a�5]   .   ��  �@J�Opacity$
 0.30Q�6H   5^   +   5`   #   5a      .@�5b      �� -   5        $ Insert File��5b   ;   .��5c   "   �� $"��/Resources/Images/tbOpen.png��6(   $��20��6N   5d      �� 5e      -	   5        $ Save Project��5e   ;   .��5f   "   �� $"��/Resources/Images/tbSave.png��6(   $��20��6N   5g      �� 5h      �� 5i      $	��Cut��6%   $	 Cut��63   .��5j   "   �� $!��/Resources/Images/tbCut.png��6(   $��20��6M   5k      �� 5l      $
��Copy��6%   $
 Copy��64   .��5m   "   �� $"��/Resources/Images/tbCopy.png��6(   $��20��6N   5n      �� 5o      $��Paste��6%   $ Paste��65   .��5p   "   �� $#��/Resources/Images/tbPaste.png��6(   $��20��6O   5q      �� 5r      $��Delete��6%   $ Delete��66   .��5s   "   �� $$��/Resources/Images/tbDelete.png��6(   $��20��6P   5t      �� 5u      �� 5v      $
��Undo��6%   $
 Undo��64   .��5w   "   �� $"��/Resources/Images/tbUndo.png��6(   $��20��6N   5x      �� 5y      $
��Redo��6%   .��5z   "   �� $"��/Resources/Images/tbRedo.png��6(   $��20��6N   5{      �� 5|      �� 5}      $ 	Word Wrap��6b    d� 	IsChecked 6'   ��  �� ElementName$ 
textEditor��$ WordWrap�.��5~   "   �� $&�� /Resources/Images/tbWordWrap.png��6(   $��20��6R   5      �� 5�      $ Show Line Numbers��6m    6'   �� $ CodeTextEditor��$ ShowLineNumbers�.��5�   "   �� $��20��6,   $ Center=�67   #6Q   5�      �� 5�      $ Show End Of Line��6s    6'   �� $ CodeTextEditor��$ Options.ShowEndOfLine�.��5�   "   �� $��20��6,   $ Center=�67   ¶6Q   5�      �� 5�      �� 5�      -
   5        $ CmdObjectBrowser��5�   �   $��10 0 0 0q�6&   #��[ 69   $��20��6i   $��Right=�6u   $ Open Object Browser��6�   .��5�   "     $ Sitemap=�62   $��Center=�6B   $��16��6]   $��Center=�6h   5�      �� 5�      -   5        $��10 0 0 0q�5�   &   #��[ 69   $��20��6i   $��Right=�6u   $ Quick Search in Document��6�   .��5�   "     $ Search=�62   $��Center=�6A   $��12��6\   $��Center=�6g   5�      �� 5�      �� 5�      
���   �$��1q�69   $��10 0 0 0q�6M   ._�5�   "   �� $ 
Horizontal=�6-   $��2q�6F   .+�5�   &   �� $
��Bold"�60   $��Center=�6D   $��Script Name:��6`   �� 5�   &   -   5          ��Name$  
ScriptName��5�   B   $��5 0 0 0q�61   $��Center=�6V   5�   #   5�      5�      '  !ICSharpCode.AvalonEdit.TextEditor  5�      -   5        !  Name$! CodeTextEditor��5�   0   $"   HorizontalScrollBarVisibility$
" Auto=�6H   "#   VerticalScrollBarVisibility$
# Auto=�6m   $��Consolas)�6�   $
��10pt(�5�   W   $   ShowLineNumbers$ . %   Text.% 6~   5�      5�      5�        5�      $��Output��6(   
������ .��5�      �� -   5        & ��Name$& Errors��5�      $��0q�6/   
������ $��0q�6U   ' �� TextAlignment$
' Left=�5�   '   $��Normal$�6<   $
��Bold"�6O   ( �� AcceptsReturn( . ) �� 
IsReadOnly) . "* �� VerticalScrollBarVisibility$* Visible=�5�   O   + �� TextWrapping$
+ Wrap=�6u   $��Stretch=�5�   (   5�      5�      ]� 5�      $��1��6   .+�5�      �� -   5        $ CmdRun��5�   �   $��0,8,8,8q�6   #��[ 6,   $��Right=�6T   $��Right=�6p   $ Run the current script��6�   $	��100��6�   $	��Run��6�   6�   �� 5�      -   5        $ CmdValidate��5�   8   $��0,8,8,8q�5�      #��[ 6,   $��Right=�6T   $��Right=�6p   $@ :Compile but do not run the current script by compiling it.��6�   $	��100��5�      $��Compile��6&   6M   5�      5�      5�      5�      