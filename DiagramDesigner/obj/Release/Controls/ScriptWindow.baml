   M S B A M L   `   `   `  ���� Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:DiagramDesigner.AdventureWorld.Domain%DiagramDesigner.AdventureWorld.Domain  Y1clr-namespace:DiagramDesigner.Controls.Behaviours#DiagramDesigner.Controls.Behaviours  +    %DiagramDesigner.Controls.ScriptWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ] YICSharpCode.AvalonEdit, Version=5.0.3.0, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonedit.http://icsharpcode.net/sharpdevelop/avalonedit  ` \Syncfusion.Tools.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a ]Syncfusion.Shared.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c894
syncfusion!http://schemas.syncfusion.com/wpf   Alocal3clr-namespace:DiagramDesigner.AdventureWorld.Domain   Z	 VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/ 	 :d2http://schemas.microsoft.com/expression/blend/2008 	 Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006 	 B
behaviours1clr-namespace:DiagramDesigner.Controls.Behaviours 	 .���   � ColumnDefinitions  �� $��* ��� $
��Auto ��� $
��3.5* �.+��� $��1��$��3��$��Stretch=�
������ � $��0��$��0�� � RowDefinitions �� $��* ��� $��40 �.+��� $��Script Explorer��$��5,5,5,1q�   AltGroupBox#��[  
������  J� IsHitTestVisible . ���� .R�� $��Stretch=�$��Stretch=�  �� $��* ��� $��50 �.+��� $��Script Explorer��$��0��$
��Bold"�$
��Left=�$��Center=� �� Padding$ 5 0 0 0q� �� TextAlignment$
 Left=��� $��1�� �� Orientation$ 
Horizontal=�$��Right=�.+���  ��Name$ 	CmdExpand��-   $��2q�  ToolBarButtonBaseStyle#��[  � ToolTip$ 
Expand All��.��" 	 FontAwesome.WPF.ImageAwesome     Icon$ 
PlusSquare=�$��Center=�$��16��$��Center=��� $ CmdCollapse��-   $��2q�#��[ $ Collapse All��.��  $ MinusSquare=�$��Center=�$��16��$��Center=�.��R� -   	 R�Name$	 ScriptExplorer��
 J� 	AllowDrop
 .  $��Stretch=�
������ $��Stretch=�.@�Q� -    Q�Name$ AllScriptsContainer��$��All Scripts��
���   �$
��Left=�
������ ���� *ScriptContainerHeaders+����  �� 
TargetType" Q�.���  �� Property �@Q�
IsExpanded �� Value �� *
IsSelected+ �� Mode$ TwoWay=���  �@Q�
IsSelected �� *
IsSelected+$ TwoWay=��� >   8DiagramDesigner.Controls.Behaviours.TreeViewItemBehavior% �@ IsBroughtIntoViewWhenSelected .  � 	Resources  �� DataTypeA   ;DiagramDesigner.AdventureWorld.Domain.ScriptContainerHeader �� ItemsSource;   5DiagramDesigner.AdventureWorld.Domain.ScriptContainer��%  (��       *, +)(�� �     *, +)�� "   �� *ScriptContainers+.R��� $ 
Horizontal=�.+��� $ 
Horizontal=�.+��� $��0 3 0 3q�$��20��$��18������ *Image+�� $��5 3 0 3q����� *Header+�� "  .R��� $ 
Horizontal=�$��0 3 0 3q�.+��� $��0 0 3 0q�$��20��$��18�� �� *Script.ScriptStateText+���� *Script.ScriptStateImage+�� $��3 0 0 0q�$��20��$��18�� �� *Script.LanguageName+���� *Script.ScriptImage+�� ���� *Name+ �� *	Script.Id+]� $��1��.+��� -   $ CmdValidateAll��$��8,8,8,8q�  AppButtonStyle#��[ $
��Left=�$
��Left=�$	��100��$��Compile All��� $��2�� �� $��* ��� $��40 �.+�5  /Syncfusion.Windows.Tools.Controls.TabControlExt  -     Name$ 	ScriptTab��$��0��$��0q�$��Stretch=�   IsNewButtonEnabled .  
������    EnableLabelEdit .     CloseButtonType$
 Hide=�.@�2  ,Syncfusion.Windows.Tools.Controls.TabItemExt  $��Script��.��]� .+�c� $	��Top=�  �� Triggers �� Binding �� RelativeSource �� AncestorType �� AncestorLevel �� Path �� Value��%y   '��       �� " �� ��  ��  �� " ��$ 1��$ 	IsEnabled�	 False.a���  �@J�Opacity$
 0.30Q�.@��� -   $ Insert File��.���� $"��/Resources/Images/tbOpen.png��$��20���� -   $ Save Project��.���� $"��/Resources/Images/tbSave.png��$��20���� �� $	��Cut��$	 Cut��.���� $!��/Resources/Images/tbCut.png��$��20���� $
��Copy��$
 Copy��.���� $"��/Resources/Images/tbCopy.png��$��20���� $��Paste��$ Paste��.���� $#��/Resources/Images/tbPaste.png��$��20���� $��Delete��$ Delete��.���� $$��/Resources/Images/tbDelete.png��$��20���� �� $
��Undo��$
 Undo��.���� $"��/Resources/Images/tbUndo.png��$��20���� $
��Redo��.���� $"��/Resources/Images/tbRedo.png��$��20���� �� $ 	Word Wrap�� d� 	IsChecked ��  �� ElementName$ 
textEditor��$ WordWrap�.���� $&�� /Resources/Images/tbWordWrap.png��$��20���� $ Show Line Numbers�� �� $ CodeTextEditor��$ ShowLineNumbers�.���� $��20��$ Center=�#�� $ Show End Of Line�� �� $ CodeTextEditor��$ Options.ShowEndOfLine�.���� $��20��$ Center=�¶�� �� -	   $ CmdObjectBrowser��$��10 0 0 0q�#��[ $��20��$��Right=�$ Open Object Browser��.��  $ Sitemap=�$��Center=�$��16��$��Center=��� -
   $��10 0 0 0q�#��[ $��20��$��Right=�$ Quick Search in Document��.��  $ Search=�$��Center=�$��12��$��Center=��� �� 
���   �$��1q�$��10 0 0 0q�._��� $ 
Horizontal=�$��2q�.+��� $
��Bold"�$��Center=�$��Script Name:���� -     ��Name$  
ScriptName��$��5 0 0 0q�$��Center=�'  !ICSharpCode.AvalonEdit.TextEditor  -   !  Name$! CodeTextEditor��$"   HorizontalScrollBarVisibility$
" Auto=�"#   VerticalScrollBarVisibility$
# Auto=�$��Consolas)�$
��10pt(�$   ShowLineNumbers$ . %   Text.%   $��Output��
������ .���� -   & ��Name$& Errors��$��0q�
������ $��0q�' �� TextAlignment$
' Left=�$��Normal$�$
��Bold"�( �� AcceptsReturn( . ) �� 
IsReadOnly) . "* �� VerticalScrollBarVisibility$* Visible=�+ �� TextWrapping$
+ Wrap=�$��Stretch=�]� $��1��.+��� -   $ CmdRun��$��0,8,8,8q�#��[ $��Right=�$��Right=�$ Run the current script��$	��100��$	��Run���� -   $ CmdValidate��$��0,8,8,8q�#��[ $��Right=�$��Right=�$@ :Compile but do not run the current script by compiling it.��$	��100��$��Compile��