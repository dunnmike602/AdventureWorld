   M S B A M L   `   `   `  ���� Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:DiagramDesigner.AdventureWorld.Domain%DiagramDesigner.AdventureWorld.Domain  1clr-namespace:DiagramDesignerDiagramDesigner  C&clr-namespace:DiagramDesigner.ControlsDiagramDesigner.Controls  A%clr-namespace:DiagramDesigner.SymbolsDiagramDesigner.Symbols  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R8clr-namespace:System.ComponentModel;assembly=WindowsBaseSystem.ComponentModel '    !DiagramDesigner.AdventureDesigner   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Elocal3clr-namespace:DiagramDesigner.AdventureWorld.Domain     .convclr-namespace:DiagramDesigner     X TXceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  5controls&clr-namespace:DiagramDesigner.Controls  3symbols%clr-namespace:DiagramDesigner.Symbols  McomponentModel8clr-namespace:System.ComponentModel;assembly=WindowsBase  ` \Syncfusion.Tools.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a	 ]Syncfusion.Shared.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c894
syncfusion!http://schemas.syncfusion.com/wpf  	 -     �Name$  
MainWindow�� � WindowStartupLocation$ CenterScreen=�$	��800��$
��1400�� � Icon$ Resources/Images/maze.png�� � 	Resources    WaitStoryboard �� 
TargetName �� TargetProperty W� From	 W� To g� Duration	 g� RepeatBehavior  DesignerCanvasContextMenu.  (System.Windows.Input.ApplicationCommands
   Paste m� Command m� Icon��%  &	        &	 �     �� .��W� $ WaitSpinner��$9 3(TextBlock.RenderTransform).(RotateTransform.Angle)�$ 0X�$	 360X�$ 0:0:2D�$	 Forever���� .@�m� $��Paste��# Z
  �� $ ��Resources/Images/Paste.png��$��16��m� $��
Select All�� �� .��� -    �Name$ 
RootWindow��$��10q� � RowDefinitions �� $
��Auto ��� $	��140 ��� $��4* ��� $
��Auto �.+�o� -   $��MainMenu��$��0��  ToolbarBrush#��[ .@�m� $��_File��.@�m� $
��_New�� �� $*��$Resources/Images/GenericDocument.png��$��16��.@�m� $��Blank Project��
   New# Z  � ToolTip$  Create a new blank project�� m� CommandTarget ��  �� ElementName$ 
MainWindow�� �� $*��$Resources/Images/GenericDocument.png��$��16��m� $��_Game Wizard��$ Run the game wizard�� �� *WizardCommand+ �� $ 
MainWindow�� "  FontAwesome.WPF.ImageAwesome     Icon$ Magic=�$��16��$��16��m� $
��Open��   Open# Z $ Open Project�� �� $ 
MainWindow�� �� $%��Resources/Images/OpenFolder.png��$��16���� m� $
��Save��   Save# Z $ Save the current project�� �� $ 
MainWindow�� �� $��Resources/Images/Save.png��$��16��m� $��Save As��   SaveAs# Z $. (Save the current project with a new name�� �� $ 
MainWindow���� m� $��O_ptions��$ View Project Options�� �� *OptionsCommand+ �� $ 
MainWindow�� �� $"��Resources/Images/options.png��$��16���� m� $��_Print��$5 /Print the current project designers and details��   Print# Z  �� $ 
MainWindow�� �� $ ��Resources/Images/Print.png��$��16���� m� -   $��Recent Files������ *RecentFiles+�� m� $��_Exit��$ Exit the application�� �� *ExitCommand+ �� $ 
MainWindow��   $ SignOut=�   
Foreground
 �  �� J� Opacity$ .7Q�$��16��$��16��m� $��E_dit��.@�m� $	��Cut��
   Cut# Z  �� $ RoomDesigner�� �� $��Resources/Images/Cut.png��$��16��m� $��Paste��# Z
  �� $ RoomDesigner�� �� $#��Resources/Images/PasteBig.png��$��16��m� $
��Copy��   Copy# Z  �� $ RoomDesigner�� �� $��Resources/Images/Copy.png��$��16��m� $��Delete��   Delete# Z  �� $ RoomDesigner�� �� $!��Resources/Images/Delete.png��$��16�� select allm� $��_Tools��.@�m� $��Game Properties��$ Show Game Properties�� �� *PropertiesCommand+ �� $%��Resources/Images/Properties.png��$��16��m� $��Check Model��$" Check the current game model�� �� *CheckCommand+ �� $ ��Resources/Images/check.png��$��16��m� -   $
��_Run������ *	Clients+���� .R��� ���� *Label+ �� *	ToolTip+ �� $��Resources/Images/exec.png��$��16��m� $��_Format��.@�m� $��
_Alignment��.@�m� $��
Align Left��$ Align objects at the left��,   &DiagramDesigner.Symbols.DesignerCanvas   	AlignLeft# Z  �� $ RoomDesigner�� �� $+��%Resources/Images/AlignObjectsLeft.png��$��16��m� $ ��Align Centers Horizontally��$+ %Align Objects at centres horizontally��   AlignHorizontalCenters# Z  �� $ RoomDesigner�� �� $9��3Resources/Images/AlignObjectsCenteredHorizontal.png��$��16��m� $��Align Right��$ Align Objects Right��   
AlignRight# Z  �� $ RoomDesigner�� �� $,��&Resources/Images/AlignObjectsRight.png��$��16��m� $��	Align Top��$ Align objects at the top��    AlignTop# Z   �� $ RoomDesigner�� �� $*��$Resources/Images/AlignObjectsTop.png��$��16��m� $��Align Centers Vertically��$- 'Align objects at the centres vertically��# Z   �� $ RoomDesigner�� �� $7��1Resources/Images/AlignObjectsCenteredVertical.png��$��16��m� $��Align Bottom��$ Align objects at the top��!   AlignBottom# Z!  �� $ RoomDesigner�� �� $-��'Resources/Images/AlignObjectsBottom.png��$��16��m� $��Distr_ibute & Size��.@�m� $��Distribute Horizontally��$$ Distribute shapes Horizontally��"   DistributeHorizontal# Z"  �� $ RoomDesigner�� �� $6��0Resources/Images/DistributeObjectsHorizontal.png��$��16��m� $��Distribute Vertically��$" Distribute shapes Vertically��#   DistributeVertical# Z#  �� $ RoomDesigner�� �� $4��.Resources/Images/DistributeObjectsVertical.png��$��16���� m� $��Same Widths��$ Make Objects Same Widths��$   	SameWidth# Z$  �� $ RoomDesigner��   $ ArrowsH=�$��16��m� $��Same Heights��$ Make Objects Same Heights��%   
SameHeight# Z%  �� $ RoomDesigner��   $ ArrowsV=�$��16��m� $��	Same Size��$ Make Objects Same Size��&   SameSize# Z&  �� $ RoomDesigner��   $ 	ArrowsAlt=�$��16��m� $��Arran_ge��.@�m� $��Group��$ Group shapes together��'   Group# Z'  �� $ RoomDesigner�� �� $ ��Resources/Images/Group.png��$��16��m� $��Ungroup��$ Ungroup shapes��(   Ungroup# Z(  �� $ RoomDesigner�� �� $"��Resources/Images/Ungroup.png��$��16���� m� $��Bring Forward��$ Bring shapes forward��)   BringForward# Z)  �� $ RoomDesigner�� �� $'��!Resources/Images/BringForward.png��$��16��m� $��Send Backward��$ Send shapes backward��*   SendBackward# Z*  �� $ RoomDesigner�� �� $'��!Resources/Images/SendBackward.png��$��16��m� $��Bring To Front��$ Bring Shapes to Front��+   BringToFront# Z+  �� $ RoomDesigner�� �� $'��!Resources/Images/BringToFront.png��$��16��m� $��Send to Back��$ Send Shapes to Back��,   
SendToBack# Z,  �� $ RoomDesigner�� �� $%��Resources/Images/SendToBack.png��$��16��m� $��_Help��.@�m� $��About Adventure Designer�� �� *AboutCommand+�� $��1��.��c� $��0,5,0,0q�.@��� $
��File��  ToolbarGroup#��[ $	��0,3q�$	��221��.���� $��1��- �� Orientation$- 
Horizontal=�.+��� $- Vertical=�$��0,2,0,2q�.+��� $��1q�$��2q�$
��Left=�$  Create a new blank project��$��27��  ToolBarButtonBaseStyle#��[ #��Z $	��100������ $ 
MainWindow����� . � ColumnDefinitions. �� �� .+��� $*��$Resources/Images/GenericDocument.png��$��16���� $��3,0,3,0q�$	��New��$��Center=�$��1��6 	 0Syncfusion.Windows.Tools.Controls.SplitButtonAdv  -   $��FileOpenButton��$��1q�$��2q�$
��Left=�$��27��/   Command#/ Z 9 	 3Syncfusion.Windows.Tools.Controls.DropDownButtonAdv0   Label$
0 Open��1   	SmallIcon$%1 Resources/Images/OpenFolder.png��$ Open Project��$	��100��2   CommandTarget2 �� $ 
MainWindow��.��9 	 3Syncfusion.Windows.Tools.Controls.DropDownMenuGroup  ���� *RecentFiles+���� .R�8 	 2Syncfusion.Windows.Tools.Controls.DropDownMenuItem  -   ���� �� $��1q�$��2q�$
��Left=�$ Save the current project��#��[ #��Z $��27��$	��100������ $ 
MainWindow����� . �� �� .+��� $��Resources/Images/Save.png��$��16���� $��3,0,3,0q�$
��Save��$��Center=�$��1���� $- Vertical=�$��0,2,0,2q�$	��108��.+��� $��1q�$��2q�$
��Left=�$	��100��$��27��#��[ $ Print Project Information��#��Z ���� $ 
MainWindow����� . �� �� .+��� $ ��Resources/Images/Print.png��$��16���� $��3,0,3,0q�$��Print��$��Center=�$��1���� $��1q�$��2q�$
��Left=�$	��100��$��27��$ View Project Options��#��[ ���� *OptionsCommand+.��� . �� �� .+��� $"��Resources/Images/options.png��$��16���� $��3,0,-9,0q�$��Options��$��Center=�$��1���� $��1q�$��2q�$
��Left=�$	��100��$ Exit Application��3   Stop#��Z3 #��[ $��27������ $ 
MainWindow����� . �� �� .+�  $ SignOut=�
 �  ��$ .7Q�$��16��$��16��$ Exit the application���� $��3,0,3,0q�$
��Exit��$��Center=�$��1���� $
��Edit��#��[ $��3q�.���� $��1��$- 
Horizontal=�.+��� $- Vertical=�$��0,2,0,2q�.+��� -   $��CmdCut��$��1 1 1 4q�$��2q�$
��Left=�#��[ #��Z ���� $ RoomDesigner����� . �� �� .+��� $��Resources/Images/Cut.png��$��16���� $��3,0,3,0q�$	��Cut��$��Center=�$��1���� -	   $��CmdCopy��$��1 1 1 4q�$��2q�$
��Left=�#��[ #��Z ���� $ RoomDesigner����� . �� �� .+��� $��Resources/Images/Copy.png��$��16���� $��3,0,3,0q�$
��Copy��$��Center=�$��1���� -
   $��CmdPaste��$��1 1 1 4q�$��2q�$
��Left=�#��[ #��Z
 ���� $ RoomDesigner����� . �� �� .+��� $ ��Resources/Images/Paste.png��$��16���� $��3,0,3,0q�$��Paste��$��Center=�$��1���� $- Vertical=�$��0,2,0,2q�.+��� -   $��CmdSelectAll��$��1 1 1 4q�$��2q�$
��Left=�#��[ 4   	SelectAll#��Z4 ���� $ RoomDesigner����� . �� �� .+�  $ HandPointerOutline=�$��14��$, &Select all objects in current designer���� $��3,0,3,0q�$��
Select All��$��Center=�$��1���� -   $��	CmdDelete��$��1 1 1 4q�$��2q�$
��Left=�#��[ #��Z ���� $ RoomDesigner����� . �� �� .+��� $!��Resources/Images/Delete.png��$��16���� $��3,0,3,0q�$��Delete��$��Center=�$��1���� $��Tool Box��#��[ $	��0,3q�  FlowChartStencils#��[ $	��255���� $��Tools��#��[ $	��0,3q�$	��200��.���� $��1��$- 
Horizontal=�$��	0,0,-10,0q�.+��� $��3,3,3,2q�$��60��#��[ $ Show Game Properties������ *PropertiesCommand+���  �� $��* ��� $��* �.+��� $%��Resources/Images/properties.png��$��32��$��32���� $��1��$��Center=�$��Center=��� .�Game�� �� .�
Properties�� $- Vertical=�$��0,2,0,2q�.+��� -   5 ��Name$5 CheckModelButton��$��1q�$��2q�$
��Left=�$" Check the current game model��#��[ ���� *CheckCommand+��� . �� �� .+��� $ ��Resources/Images/check.png��$��16���� $��3,0,3,0q�$��Check Model��$��Center=�$��1��  $��1q�$��2q�$
��Left=�$��28��$ 1 /Resources/Images/exec.png��$	��123�� �� *SelectedClient.ToolTip+0 �� *SelectedClient.Label+2 �� $ 
MainWindow��/ �� *
RunCommand+.��  ���� *	Clients+���� .R�  -   ���� *Label+6   	IsChecked6 �� *	IsChecked+ �� *	ToolTip+�� $��	Alignment��#��[ $��3q�.���� $��0��$- Vertical=�.+���   ToolbarBorderBrush#��[ $��1q�7 �� CornerRadius$7 2��$��3q�._��� $- 
Horizontal=�.+��� -   $��CmdAlignHorizontalCenters��$��5q�#��[ #��Z ���� $ RoomDesigner������ $+��%Resources/Images/AlignObjectsLeft.png��$��16��$ Align objects at the left���� -   $'��!CmdAlignObjectsCenteredHorizontal��$��5q�#��[ $+ %Align Objects at centres horizontally��#��Z ���� $ RoomDesigner������ $9��3Resources/Images/AlignObjectsCenteredHorizontal.png��$��16���� -   $��CmdAlignObjectsRight��$��5q�#��[ $ Align Objects Right��#��Z ���� $ RoomDesigner������ $,��&Resources/Images/AlignObjectsRight.png��$��16���� #��[ $��1q�$7 2��$��3q�._��� $- 
Horizontal=�.+��� -   $��CmdAlignObjectsTop��$��5q�#��[ #��Z  ���� $ RoomDesigner������ $*��$Resources/Images/AlignObjectsTop.png��$��16��$ Align objects at the top���� -   $%��CmdAlignObjectsCenteredVertical��$��5q�#��[ 8   AlignVerticalCenters#��Z8 ���� $ RoomDesigner������ $7��1Resources/Images/AlignObjectsCenteredVertical.png��$- 'Align objects at the centres vertically��$��16���� -   $��CmdAlignObjectsBottom��$��5q�#��[ #��Z! ���� $ RoomDesigner������ $-��'Resources/Images/AlignObjectsBottom.png��$! Align objects at the bottom��$��16���� $��Distribute & Size��#��[ $��3q�.��� . �� $��2* ��� $��* �.+��� $��0��$- Vertical=�$��5,0,0,0q�.+��� -   $��CmdDistributeHorizontal��$��3q�$��5q�$
��Left=�#��[ #��Z" $$ Distribute shapes Horizontally������ $ RoomDesigner����� . �� �� .+��� $6��0Resources/Images/DistributeObjectsHorizontal.png��$��16���� $��3q�$��
Horizontal��$��Center=�$��1���� -   $��CmdDistributeVertical��$��3q�$��3q�$
��Left=�#��[ #��Z# $# Distribute shapes Vertically.������ $ RoomDesigner����� . �� �� .+��� $4��.Resources/Images/DistributeObjectsVertical.png��$��16���� $��3q�$��Vertical��$��Center=�$��1���� $��1��$- Vertical=�$��5 0 0 0q�.+��� -   $��CmdMakeSameWidths��$��2q�$��0q�#��[ #��Z$ ���� $ RoomDesigner����  $ ArrowsH=�$��18��$��18��$ Make Objects Same Widths���� -   $��CmdMakeSameHeight��$��2q�#��[ #��Z% ���� $ RoomDesigner����  $ ArrowsV=�$��18��$��18��$ Make Objects Same Heights���� -   $��CmdMakeSameSize��$��2q�#��[ #��Z& ���� $ RoomDesigner����  $ 	ArrowsAlt=�$��18��$��18��$ Make Objects Same Size���� $��Arrange��#��[ $��3q�.��� . �� $
��Auto ��� �� .+��� .+��� -   $��CmdGroup��$��0,3,0,3q�$��3q�$
��Left=�#��[ #��Z' ���� $ RoomDesigner����� . �� �� .+��� $ ��Resources/Images/Group.png��$��16���� $��3q�$��Group��$��Center=�$��1���� -   $��
CmdUnGroup��$��0,3,0,3q�$��3q�$
��Left=�#��[ #��Z( ���� $ RoomDesigner����� . �� �� .+��� $"��Resources/Images/Ungroup.png��$��16���� $��3q�$��Ungroup��$��Center=�$��1���� $��1��$��7,0,0,0q�.+��� -   $��
CmdForward��$��0,3,0,3q�$��3q�$
��Left=�#��[ #��Z) ���� $ RoomDesigner����� . �� �� .+��� $'��!Resources/Images/BringForward.png��$��16���� $��3q�$��Forward��$��Center=�$��1���� -   $��
CmdToFront��$��0,3,0,3q�$��3q�$
��Left=�#��[ #��Z+ ���� $ RoomDesigner����� . �� �� .+��� $'��!Resources/Images/BringToFront.png��$��16���� $��3q�$��To Front��$��Center=�$��1���� #��[ $��1��	9 �� X1$9 0��	: �� Y1$: 0��	; �� X2$; 0��	< �� Y2$< 20��= �� Stretch$
= Fill=�$
��Left=�$��1��$��3,0,0,0q��� $��2��.+��� -   $��CmdBackward��$��0,3,0,3q�$��3q�$
��Left=�#��[ #��Z* ���� $ RoomDesigner����� . �� �� .+��� $'��!Resources/Images/SendBackward.png��$��16���� $��3q�$��Backward��$��Center=�$��1���� -   $��	CmdToBack��$��0,3,0,3q�$��3q�$
��Left=�#��[ #��Z, ���� $ RoomDesigner����� . �� �� .+��� $%��Resources/Images/SendToBack.png��$��16���� $��3q�$��To Back��$��Center=�$��1��� $��2��$��0,10,0,0q�. �� $��* ��� $
��Auto ��� $
��2.8* �.+��� $��1��$��5��$��Stretch=�
������ � $��0��$��0,0,5,1q�$��Stretch=�$��2��.+�5  /Syncfusion.Windows.Tools.Controls.TabControlExt  -    >  Name$> ProjectExplorerTab��? J� 	AllowDrop? .  $��0 0 0 0q�$��Stretch=�$��Stretch=�@   IsNewButtonEnabled@ .  A   EnableLabelEditA .  B   CloseButtonType$
B Hide=�
������ .@�2	  ,Syncfusion.Windows.Tools.Controls.TabItemExt	  $��Explorer��.��� $��Stretch=�$��Stretch=� �� $��* ��� $
��Auto ��� $��* �.+�R� -!   C R�Name$C Explorer��$��0q�$��0��$��0��
���   �$��Stretch=�$��Stretch=�.@�Q� -"   D Q�Name$D GameExplorer��
������ ���� E �� Path$E GameName�.@�Q� ���� *Rooms+ F �� DataType0
   *DiagramDesigner.AdventureWorld.Domain.RoomG �� ItemsSource0   *DiagramDesigner.AdventureWorld.Domain.Exit��%�   (��       *,
 +)(�� x     *, +)�� "F 
 G �� *Exits+.R��� $- 
Horizontal=�.+��� ���� $E BaseName� �� $E CurrentDescription��� "F  .R��� ���� $E Description����� $- 
Horizontal=�.+��� $��0 3 0 3q�$��18��$��18��$$��/Resources/Images/New-Room.png���� $��5 3 0 3q�$��Rooms (and Exits)��Q� ���� *Rooms+ ;   5DiagramDesigner.AdventureWorld.Domain.PlaceableObject��%�   (��       *,
 +)(�� �     *, +)�� "F 
 G �� *PlaceableObjects+.R��� $- 
Horizontal=�.+��� ���� $E BaseName� �� $E CurrentDescription��� "F  .R��� ���� $E BaseName� �� $E CurrentDescription����� $- 
Horizontal=�.+��� $��0 3 0 3q�$��18��$��18��$$��/Resources/Images/New-Room.png���� $��5 3 0 3q�$��Rooms (and Objects)��Q� ���� *PlaceableObjects+ ��%�   (��       *, +)�� "F  G �� *PlaceableObjects+.R��� $- 
Horizontal=�.+��� ���� $E BaseName� �� $E CurrentDescription����� $- 
Horizontal=�.+��� $��0 3 0 3q�$��18��$��18��$$��/Resources/Images/Key-icon.png���� $��5 3 0 3q�$��Objects��Q� ���� *Exits+ ��%^   (��       *, +)�� "F  .R��� $- 
Horizontal=�.+��� ���� $E FriendlyDescription����� $- 
Horizontal=�.+��� $��0 3 0 3q�$��18��$��18��$ ��/Resources/Images/exit.png���� $��5 3 0 3q�$��Exits���� $��0��$��0��$- 
Horizontal=�$��Right=�$	��Top=�.+��� -#   $��	CmdExpand��$��2q�#��[ $ 
Expand All����  $ 
PlusSquare=�$��Center=�$��16��$��16��$��Center=��� -$   $��CmdCollapse��$��2q�#��[ $ Collapse All����  $ MinusSquare=�$��Center=�$��16��$��16��$��Center=��� $��1��$��5��$��Stretch=�
������ � $��2��$��0��$��Stretch=�$��Stretch=� �� $
��Auto ��� $��* �.+��� $��0��$��CharacterEllipsis=�$
��Bold"�$��12(����� *PropertyTitle+ �� *PropertyTitle+1  +Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid  -%   H  Name$H GameProperties��$��1��I   ShowSummaryI . J   ShowSortOptionsJ .  K   ShowAdvancedOptionsK .  L   	ShowTitleL .  M   ShowSearchBoxM . N   IsCategorizedN . O   ShowPreviewO .  
���   �P   AutoGeneratePropertiesP . $��Stretch=�$��Stretch=�Q   FilterWatermark$Q  ��	  $��Options��.��  -&   $H OptionsProperties��I . J .  K .  M . N . O .  
������ P . $��Stretch=�$��Stretch=�$Q  ��	  $��Commands��.���    CommandsSourceR �� SourceS �� SortDescriptions+  %System.ComponentModel.SortDescriptionT   PropertyNameU   	Direction��%�   &	       �� R �� *AdventureCommandMappings+S   $T 	IsEnabled��$U 
Descending=�  $T VerbName��$U 	Ascending=� �� $
��Auto ��� $��* �.+�� V � Orientation$V 
Horizontal=�.+�3 	 -Syncfusion.Windows.Tools.Controls.ComboBoxAdv  -'   $��AdventureCommandList��$
��Left=�$	��175��
������ $��2 2 2 10q����� W �� Source#W [ X �� SelectedItemX �� *SelectedCommand+Y �� Mode$Y TwoWay=����� .R��� $- 
Horizontal=�.+��� $��16��$��16��$��0 0 5 0q�$��Resources/Images/maze.png��$  This is a built-in command��y��� *IsBuiltInCommand+  VisibilityHiddenConverterZ �� 	Converter#Z [ [ �� FallbackValue$[ Hidden���� $��Strikethrough{����� *
VerbName+y��� *	IsEnabled+ 	 BooleanVisibilityConverter#Z [	 \ �� ConverterParameter$\ INVERT���� ���� *
VerbName+y��� *	IsEnabled+#Z [	 �� -(   $5 CmdAddCommand�� 
 AppButtonStyle#��[
 $��Center=�$��5 2 2 10q�$��45��$��0��$	��Add��$��20���� -)   $5 CmdDeleteCommand��$��Center=�#��[
 $��5 2 2 10q�$��45��$��0��$��Delete��$��20��{��� *" SelectedCommand.IsBuiltInCommand+  ReverseBooleanConverter#Z [ $[ false���� -*   $5 CmdResetCommand��#��[
 $��Center=�$��5 2 2 10q�$��45��$��0��$��Reset��$��20��{��� *" SelectedCommand.IsBuiltInCommand+$[ false��  -+   $ ��AdventureCommandProperties��$��1��I . J .  K .  M .  N . O .  
������ P . $��Stretch=�$��Stretch=�$Q  ��]   SelectedObject] �� *SelectedCommand+$Y TwoWay=�	  $��Errors��.��&   System.Windows.Controls.DataGrid  -,   ^  Name$^ 	ErrorList��_   AutoGenerateColumns_ .  `   CanUserAddRows` .  a   CanUserDeleteRowsa .  
����  �$
��Left=�b   CanUserSortColumnsb . c   RowHeaderWidth$c 0Q�d   CanUserReorderColumnsd .  e   CanUserResizeRowse .  $f   HorizontalScrollBarVisibility$
f Auto=�"g   VerticalScrollBarVisibility$
g Auto=�$��Stretch=�$��Stretch=�h   SelectionMode$h Single=�i   CanUserResizeColumnsi .  ���� *ValidationItems+$Y OneWay=�j   RowStylej �� k �� 
TargetType)  #System.Windows.Controls.DataGridRow"k  .�--   l   Columnsl 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumnm   Width5  /System.Windows.Controls.DataGridLengthConverter$m 30 n   
IsReadOnlyn . o   CanUserSorto . p   SortMemberPath$p Severity��q   CellTemplateq �� .R��� $��18�� �� *
Severity+���� *SeverityImage+r �� UpdateSourceTrigger$r PropertyChanged=�  $	m 240 n . o . $p Description��s   Header$s Description��q �� .R���  �� *Description+���� *Description+$r PropertyChanged=�  $m * n . o . $p ValidationCategory��$
s Type��q �� .R���  �� *ValidationCategory+���� *ValidationCategory+$r PropertyChanged=�  $m * n . o . $p PlaceableObjectName��$
s Name��q �� .R���  �� *PlaceableObjectName+���� *PlaceableObjectName+$r PropertyChanged=�  -.   $> MainScreenTabs��$��2��$��0 0 0 0q�$��Stretch=�
������ $��Stretch=�A .  @ .  $
B Hide=�.@�	  $��Game Designer��
������ .��� $��0,0,0,0q�. �� $��1.75* ��� $
��Auto ��� $��* �.+��� $��1��$��5��$��Stretch=�
������ � $��0�� �� $��* ��� $
��Auto �.+��� -/   t ��Name$t MapGroup��$	��Map��$��0,0,0,0q�$��0��
������ .���� -0   u ��Name$u ScrollRooms��$
��Auto=�
������ $
��Auto=�.��  -1   v  Name$v RoomDesigner��|�. w   CanHaveConnectorsw .   SnappingGridBrush_Ponits#��[ $��10q�x � ContextMenu#x [ y � FocusVisualStyley Z� z � LayoutTransformz �� { �� ScaleX{ �� $ 
MainWindow��$E 	RoomScale�| �� ScaleY| �� $ 
MainWindow��$E 	RoomScale�� $��1��
����������� *ActualWidth+$ MapGroup��. �� $��* ��� $��* �.+��� $��0��$- 
Horizontal=�$��5q�$��2��.+��� -2   } � Cursor$
} Hand��$
��Left=�$ Zoom In��
������ $��Center=�~ �� 	ClickMode$~ Press=�.��  $ 
SearchPlus=�$��Center=�$��16��$��Center=��� -3   $
} Hand��$��5 0 0 0q�$��Center=�
������ $
��Left=�$ Zoom Out��.��  $ SearchMinus=�$��Center=�$��16��$��Center=��� -4   $5 CmdZoomRoomsDefault��$
} Hand��#��[
 $��Center=�$��5 0 0 0q�$��50��$��0��$
��100%��$��20���� -5   $5 CmdZoomToFit��$
} Hand��#��[
 $��Center=�$��5 0 5 0q�$��50��$��0��$	��Fit��$��20��� $��2�� �� $��* ��� $
��Auto �.+��� -6   $t ObjectGroup��$��
Placeables��$��3,0,0,0q�.���� -7   $u ScrollObjects��$
��Auto=�$
��Auto=�.��  -8   $v ObjectDesigner��|�. #��[ $��10q�#x [ y Z� z �� { �� $ 
MainWindow��$E ObjectScale�| �� $ 
MainWindow��$E ObjectScale�� $��1��
����������� *ActualWidth+$ ObjectGroup��. �� $
��Auto ��� $��* �.+��� $��1��$- 
Horizontal=�$��20 5 5 5q�$��Right=�.+��� -9   $��3q�$> 8Arrange All Objects in a square to fill available space.��  ToolBarButtonBaseStyleAlt#��[ $��0 0 5 0q���  $ Th=�$��Center=�$��16��$��Center=��� -:   $��3q�#��[ $��0 0 5 0q�$U OArrange and Sort all Objects by name. Toggles between ascending and descending.����  -;   $��SortByNameImage��$ SortAlphaAsc=�$��Center=�$��16��$��Center=��� -<   $��3q�#��[ $� |Arrange and Sort all Objects by name of the room or container they are located in. Toggles between ascending and descending.����� .+�  -=   $��SortByRoomNameImage��$ SortAlphaAsc=�$��Center=�$��16��$��Center=�  $
 Home=�$	 .55Q�$��Center=�$��16��$��Center=��� $��0��$- 
Horizontal=�$��5q�$
��Left=�.+��� ->   $
} Hand��$
��Left=�$ Zoom In��$��Center=�$~ Press=�
������ .��  $ 
SearchPlus=�$��Center=�$��16��$��Center=��� -?   $
} Hand��$��5 0 0 0q�$��Center=�$
��Left=�
������ $ Zoom Out��.��  $ SearchMinus=�$��Center=�$��16��$��Center=��� -@   $5 CmdZoomObjectDefault��$
} Hand��#��[
 $��Center=�$��5 0 0 0q�$��50��$��0��$
��100%��$��20���� -A   $5 CmdZoomObjectToFit��$
} Hand��#��[
 $��Center=�$��5 0 5 0q�$��50��$��0��$	��Fit��$��20��	  $��Script Designer��.��+   %DiagramDesigner.Controls.ScriptWindow  -B     Name$ ScriptExplorer��7  1Syncfusion.Windows.Tools.Controls.RibbonStatusBar  $��3��$��	0 10 10 0q�.@��� $- 
Horizontal=�$��Stretch=�$��10 0 0 0q�.+��� $��0 0 5 0q�$
��Bold"�$��Center=�	STATUS:�� -C   � ��Name$� Status��$��0 0 10 0q�$��Center=�  -D   �  Name$� WaitSpinner��$ Spinner=�$y�	Collapsed=�$��0 0 10 0q�$��Center=�$��16��$��Center=�� J� RenderTransformOrigin$� 0.5, 0.5&�z��� � �� Angle$� 0Q�