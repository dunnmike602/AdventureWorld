   M S B A M L   `   `   `  ����Q  MAdventureWorldDesigner, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:DiagramDesigner.SymbolsDiagramDesigner.Symbols  5   	   C&clr-namespace:DiagramDesigner.ControlsDiagramDesigner.Controls  5	   	   ?    9DiagramDesigner.ConversationDesigner.ConversationDesigner   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   X TXceed.Wpf.Toolkit, Version=3.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  5   	   3symbols%clr-namespace:DiagramDesigner.Symbols  5   	   ` \Syncfusion.Tools.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a	 ]Syncfusion.SfGrid.WPF, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89a
 ]Syncfusion.Shared.Wpf, Version=15.4460.0.17, Culture=neutral, PublicKeyToken=3d67ed1f87d44c896
syncfusion!http://schemas.syncfusion.com/wpf  	 
 5   	   9controls&clr-namespace:DiagramDesigner.Controls  	 
 5	   	   -   5          � WindowStartupLocation$  CenterScreen=�5   	   $	��800��5
   	   $
��1400��6    � Icon$( "/Resources/Images/Conversation.png��6#    � 	Resources 5         WaitStoryboard �� 
TargetName �� TargetProperty W� From	 W� To g� Duration g� RepeatBehavior  DesignerCanvasContextMenu.  (System.Windows.Input.ApplicationCommands	   Paste
 m� Command m� Icon��%�  &	        &	 �     �� 5   
   .��5      W� $ WaitSpinner��5      $9 3(TextBlock.RenderTransform).(RotateTransform.Angle)�5      $ 0X�5      $	 360X�5      $ 0:0:2D�5      $ Forever��5      5      �� 5   
   .@�5      m� $��Paste��6   #
 Z	 6&    5      �� 5      $!��/Resources/Images/Paste.png��6   $��16��6A   5      m� 5      $��
Select All��6   
 6+   �� 5      5      .��5      � -   5         �Name$ 
RootWindow��5      $��10q�6    � RowDefinitions 5    
   �� 5!      $
��Auto �6   �� 5"      $	��142 �6   �� 5#      $��4* �6   �� 5$      $
��Auto �6   �� 5%      $
��Auto �6   .+�5(   
   � $��4��6   .+�5)      ��  �� Orientation$ 
Horizontal=�6   $��Right=�64   $��0 10 0 0q�6P   .+�5*      ��  � ToolTip$R LSave the conversation. This also cause the main project to be saved as well.��6   $
��Save��6q     AppButtonStyle#��[ 5+      $	��100��6n   ��6A   �� *SaveConversationCommand+5,      �� 5-      -   5        $��3q�5-      $% Close the Conversation Designer��6%   $	��100��6P   $��Close��6\   #��[ 5.      $��10 0 0 0q�6P   5/      50      51      o� 53   
   -   5        $��MainMenu��53   G   $��0��6     ToolbarBrush#��[ 6   $��0 5 0 5q�6W   .@�54      m� $��E_dit��6   .@�55      m� $	��Cut��6   
   Cut#
 Z 6)    m� CommandTarget 6V   ��  �� ElementName$ Designer�� 56      �� 57      $��/Resources/Images/Cut.png��6    $��16��6C   59      m� 5:      $��Paste��6   #
 Z	 6+    6Z   �� $ Designer�� 5;      �� 5<      $$��/Resources/Images/PasteBig.png��6    $��16��6H   5>      m� 5?      $
��Copy��6      Copy#
 Z 6*    6X   �� $ Designer�� 5@      �� 5A      $ ��/Resources/Images/Copy.png��6    $��16��6D   5C      m� 5D      $��Delete��6      Delete#
 Z 6,    6\   �� $ Designer�� 5E      �� 5F      $"��/Resources/Images/Delete.png��6    $��16��6F   5H      5I      m� 5J      $��_Format��6   .@�5K      m� $��
_Alignment��6   .@�5L      m� $��
Align Left��6   $ Align objects at the left��63   ,   &DiagramDesigner.Symbols.DesignerCanvas   	AlignLeft#
 Z 6W    6�   �� $ Designer�� 5M      �� 5N      $,��&/Resources/Images/AlignObjectsLeft.png��6$   $��16��6T   5P      m� 5Q      $ ��Align Centers Horizontally��6   $+ %Align Objects at centres horizontally��6C      AlignHorizontalCenters#
 Z 6s    6�   �� $ Designer�� 5R      �� 5S      $:��4/Resources/Images/AlignObjectsCenteredHorizontal.png��6$   $��16��6b   5U      m� 5V      $��Align Right��6   $ Align Objects Right��64      
AlignRight#
 Z 6R    6�   �� $ Designer�� 5W      �� 5X      $-��'/Resources/Images/AlignObjectsRight.png��6$   $��16��6U   5Z      m� 5[      $��	Align Top��6   $ Align objects at the top��62      AlignTop#
 Z 6U    6�   �� $ Designer�� 5\      �� 5]      $+��%/Resources/Images/AlignObjectsTop.png��6$   $��16��6S   5_      m� 5`      $��Align Centers Vertically��6   $- 'Align objects at the centres vertically��6A   #
 Z 6s    6�   �� $ Designer�� 5a      �� 5b      $8��2/Resources/Images/AlignObjectsCenteredVertical.png��6$   $��16��6`   5d      m� 5e      $��Align Bottom��6   $ Align objects at the top��65      AlignBottom#
 Z 6X    6�   �� $ Designer�� 5f      �� 5g      $.��(/Resources/Images/AlignObjectsBottom.png��6$   $��16��6V   5i      5j      m� 5k      $��Distr_ibute & Size��6   .@�5l      m� $��Distribute Horizontally��6   $$ Distribute shapes Horizontally��6@      DistributeHorizontal#
 Z 6i    6�   �� $ Designer�� 5m      �� 5n      $7��1/Resources/Images/DistributeObjectsHorizontal.png��6$   $��16��6_   5p      m� 5q      $��Distribute Vertically��6   $" Distribute shapes Vertically��6>      DistributeVertical#
 Z 6e    6�   �� $ Designer�� 5r      �� 5s      $5��//Resources/Images/DistributeObjectsVertical.png��6$   $��16��6]   5u      �� 5v      m� 5w      $��Same Widths��6   $ Make Objects Same Widths��64      	SameWidth#
 Z 6W    6�   �� $ Designer�� 5x      "  FontAwesome.WPF.ImageAwesome  5y         Icon$ ArrowsH=�6.   $��16��6>   5{      m� 5|      $��Same Heights��6   $ Make Objects Same Heights��65      
SameHeight#
 Z 6Y    6�   �� $ Designer�� 5}        5~      $ ArrowsV=�6.   $��16��6>   5�      m� 5�      $��	Same Size��6   $ Make Objects Same Size��62      SameSize#
 Z 6S    6�   �� $ Designer�� 5�        5�      $ 	ArrowsAlt=�6.   $��16��6@   5�      5�      m� 5�      $��Arran_ge��6   .@�5�      m� $��Group��6   $ Group shapes together��6.       Group#
 Z  6N    6�   �� $ Designer�� 5�      �� 5�      $!��/Resources/Images/Group.png��6$   $��16��6I   5�      m� 5�      $��Ungroup��6   $ Ungroup shapes��60   !   Ungroup#
 Z! 6I    6}   �� $ Designer�� 5�      �� 5�      $#��/Resources/Images/Ungroup.png��6$   $��16��6K   5�      �� 5�      m� 5�      $��Bring Forward��6   $ Bring shapes forward��66   "   BringForward#
 Z" 6U    6�   �� $ Designer�� 5�      �� 5�      $(��"/Resources/Images/BringForward.png��6$   $��16��6P   5�      m� 5�      $��Send Backward��6   $ Send shapes backward��66   #   SendBackward#
 Z# 6U    6�   �� $ Designer�� 5�      �� 5�      $(��"/Resources/Images/SendBackward.png��6$   $��16��6P   5�      m� 5�      $��Bring To Front��6   $ Bring Shapes to Front��67   $   BringToFront#
 Z$ 6W    6�   �� $ Designer�� 5�      �� 5�      $(��"/Resources/Images/BringToFront.png��6$   $��16��6P   5�      m� 5�      $��Send to Back��6   $ Send Shapes to Back��65   %   
SendToBack#
 Z% 6S    6�   �� $ Designer�� 5�      �� 5�      $&�� /Resources/Images/SendToBack.png��6$   $��16��6N   5�      5�      5�      5�      �� 5�   
   $��1��6   .��5�      c� $��0,5,0,0q�6   .@�5�      �� $
��Edit��6     ToolbarGroup#��[ 6)   $��3q�6O   .��5�      �� $��1��6!   $ 
Horizontal=�6.   .+�5�      �� $ Vertical=�6%   $��0,2,0,2q�6<   .+�5�      �� -   5        $��CmdCut��5�   e   $��1 1 1 4q�6%   $��2q�66   $
��Left=�6B     ToolBarButtonBaseStyle#��[ 5�      #��Z 5�      ��5�      �� $ Designer����5�   "   � 5�   &   & � ColumnDefinitions& 5�   *   �� 5�   .   �� 5�   .   .+�5�   *   �� $��/Resources/Images/Cut.png��60   $��16��6S   �� 5�   *   $��3,0,3,0q�64   $	��Cut��6E   $��Center=�6P   $��1��6k   5�   '   5�      �� 5�      -   5        $��CmdCopy��5�   M   $��1 1 1 4q�5�   %   $��2q�66   $
��Left=�6B   #��[ 5�      #��Z 5�      ��5�      �� $ Designer����5�   "   � 5�   &   & 5�   *   �� 5�   .   �� 5�   .   .+�5�   *   �� $ ��/Resources/Images/Copy.png��60   $��16��6T   �� 5�   *   $��3,0,3,0q�64   $
��Copy��6E   $��Center=�6Q   $��1��6l   5�   '   5�      �� 5�      -   5        $��CmdPaste��5�   U   $��1 1 1 4q�5�   %   $��2q�66   $
��Left=�6B   #��[ 5�   %   #��Z	 5�   %   ��5�   %   �� $ Designer����5�   "   � 5�   &   & 5�   *   �� 5�   .   �� 5�   .   .+�5�   *   �� $!��/Resources/Images/Paste.png��60   $��16��6U   �� 5�   *   $��3,0,3,0q�64   $��Paste��6E   $��Center=�6R   $��1��6m   5�   '   5�      5�      �� 5�      $ Vertical=�6%   $��0,2,0,2q�6<   .+�5�      �� -   5        $��CmdSelectAll��5�   d   $��1 1 1 4q�6%   $��2q�66   $
��Left=�6B   #��[ 5�   %   '   	SelectAll#��Z' 5�   %   ��5�   %   �� $ Designer����5�   "   � 5�   &   & 5�   *   �� 5�   .   �� 5�   .   .+�5�   *     $ HandPointerOutline=�6:   $��14��6U   $, &Select all objects in current designer��6a   �� 5�   *   $��3,0,3,0q�64   $��
Select All��6E   $��Center=�6W   $��1��6r   5�   '   5�      �� 5�      -	   5        $��	CmdDelete��5�   d   $��1 1 1 4q�6%   $��2q�66   $
��Left=�6B   #��[ 5�   %   #��Z 5�   %   ��5�   %   �� $ Designer����5�   "   � 5�   &   & 5�   *   �� 5�   .   �� 5�   .   .+�5�   *   �� $"��/Resources/Images/Delete.png��60   $��16��6V   �� 5�   *   $��3,0,3,0q�64   $��Delete��6E   $��Center=�6S   $��1��6n   5�   '   5      5     5     5     �� 5     $��Tool Box��6   #��[ 6-   $	��0,3q�6T   $��Center=�6a     ConversationStencils#��[ 5     �� 5	     $��Tools��6   #��[ 6*   $	��0,3q�6P   .��5     �� $ Vertical=�6!   $��0,2,0,2q�68   .+�5     �� -
   5        ( ��Name$( CheckModelButton��5  M   $��1q�5  !   $��2q�6,   $
��Left=�68   $" Check the current game model��6[   #��[ 5     ��5     �� *CheckCommand+��5     � 5  "   & 5  &   �� 5  *   �� 5  *   .+�5  &   �� $!��/Resources/Images/check.png��6,   $��16��6Q   �� 5  &   $��3,0,3,0q�60   $��Validate Model��6A   $��Center=�6W   $��1��6s   5  #   5     5     5     �� 5     $��	Alignment��6   #��[ 6.   $��3q�6T   .��5!     �� $��0��6!   $ Vertical=�61   .+�5"     ��   ToolbarBorderBrush#��[ 6!   $��1q�6S   ) �� CornerRadius$) 2��6g   $��3q�6x   ._�5#     �� $ 
Horizontal=�6)   .+�5$  "   �� -   5        $��CmdAlignHorizontalCenters��5%  [   $��5q�5$  )   #��[ 65   #��Z 5%  %   ��5&  %   �� $ Designer����5'  &   �� 5(  *   $,��&/Resources/Images/AlignObjectsLeft.png��60   $��16��6`   $ Align objects at the left��6k   5*  #   �� 5+  "   -   5        $'��!CmdAlignObjectsCenteredHorizontal��5+  e   $��5q�6)   #��[ 65   $+ %Align Objects at centres horizontally��6�   #��Z 5,  %   ��5-  %   �� $ Designer����5.  &   �� 5/  *   $:��4/Resources/Images/AlignObjectsCenteredHorizontal.png��60   $��16��6o   51  #   �� 52  "   -   5        $��CmdAlignObjectsRight��52  e   $��5q�6)   #��[ 65   $ Align Objects Right��6�   #��Z 53  %   ��54  %   �� $ Designer����55  &   �� 56  *   $-��'/Resources/Images/AlignObjectsRight.png��60   $��16��6a   58  #   59     5:     �� 5;     #��[ 6!   $��1q�6S   $) 2��6g   $��3q�6x   ._�5<     �� $ 
Horizontal=�6)   .+�5=  "   �� -   5        $��CmdAlignObjectsTop��5?  %   $��5q�5=  )   #��[ 65   #��Z 5>  %   ��5@  %   �� $ Designer����5A  &   �� 5B  *   $+��%/Resources/Images/AlignObjectsTop.png��60   $��16��6_   $ Align objects at the top��6j   5D  #   �� 5E  "   -   5        $%��CmdAlignObjectsCenteredVertical��5G  U   $��5q�5E  )   #��[ 65   *   AlignVerticalCenters#��Z* 5F  %   ��5G  %   �� $ Designer����5H  &   �� 5I  *   $8��2/Resources/Images/AlignObjectsCenteredVertical.png��60   $- 'Align objects at the centres vertically��6l   $��16��6�   5K  #   �� 5L  "   -   5        $��CmdAlignObjectsBottom��5N  %   $��5q�5L  )   #��[ 65   #��Z 5M  %   ��5O  %   �� $ Designer����5P  &   �� 5Q  *   $.��(/Resources/Images/AlignObjectsBottom.png��60   $! Align objects at the bottom��6b   $��16��6�   5S  #   5T     5U     5V     5W     �� 5Z     $��Distribute & Size��6   #��[ 6:   $��3q�6`   .��5[     � & 5\     �� 5]     $��2* �6/   6<   �� 5^     $��* �6/   6;   .+�5`     �� $��0��6%   $ Vertical=�65   $��5,0,0,0q�6L   .+�5a     �� -   5        $��CmdDistributeHorizontal��5c     $��3q�5a  %   $��5q�60   $
��Left=�6<   #��[ 5b     #��Z 5d     $$ Distribute shapes Horizontally��6^   ��5e     �� $ Designer����5f  "   � 5g  &   & 5h  *   �� 5i  .   �� 5j  .   .+�5l  *   �� $7��1/Resources/Images/DistributeObjectsHorizontal.png��60   $��16��6k   �� 5m  *   $��3q�64   $��
Horizontal��6?   $��Center=�6Q   $��1��6l   5n  '   5p     �� 5q     -   5        $��CmdDistributeVertical��5t     $��3q�5q  %   $��3q�60   $
��Left=�6<   #��[ 5r     #��Z 5s     $# Distribute shapes Vertically.��5u  L   ��6   �� $ Designer����5v  "   � 5w  &   & 5x  *   �� 5y  .   �� 5z  .   .+�5|  *   �� $5��//Resources/Images/DistributeObjectsVertical.png��60   $��16��6i   �� 5}  *   $��3q�64   $��Vertical��6?   $��Center=�6O   $��1��6j   5~  '   5�     5�     �� 5�     $��1��6%   $ Vertical=�65   $��5 0 0 0q�6L   .+�5�     �� -   5        $��CmdMakeSameWidths��5�  m   $��2q�6%   $��0q�60   #��[ 6<   #��Z 5�  !   ��5�  !   �� $ Designer����5�  "     5�  &   $ ArrowsH=�66   $��18��6F   $��18��6Q   $ Make Objects Same Widths��6]   5�     �� 5�     -   5        $��CmdMakeSameHeight��5�  `   $��2q�6%   #��[ 60   #��Z 5�  !   ��5�  !   �� $ Designer����5�  "     5�  &   $ ArrowsV=�66   $��18��6F   $��18��6Q   $ Make Objects Same Heights��6]   5�     �� 5�     -   5        $��CmdMakeSameSize��5�  a   $��2q�6%   #��[ 61   #��Z 5�  !   ��5�  !   �� $ Designer����5�  "     5�  &   $ 	ArrowsAlt=�66   $��18��6H   $��18��6S   $ Make Objects Same Size��6_   5�     5�     5�     5�     �� 5�     $��Arrange��6   #��[ 6,   $��3q�6R   .��5�     � & 5�     �� 5�     $
��Auto �6/   �� 5�     �� 5�     .+�5�     �� .+�5�     �� -   5        $��CmdGroup��5�     $��0,3,0,3q�5�  %   $��3q�66   $
��Left=�6B   #��[ 5�     #��Z  5�     ��5�     �� $ Designer����5�  "   � 5�  &   & 5�  *   �� 5�  .   �� 5�  .   .+�5�  *   �� $!��/Resources/Images/Group.png��60   $��16��6U   �� 5�  *   $��3q�64   $��Group��6?   $��Center=�6L   $��1��6g   5�  '   5�     �� 5�     -   5        $��
CmdUnGroup��5�     $��0,3,0,3q�5�  %   $��3q�66   $
��Left=�6B   #��[ 5�     #��Z! 5�     ��5�     �� $ Designer����5�  "   � 5�  &   & 5�  *   �� 5�  .   �� 5�  .   .+�5�  *   �� $#��/Resources/Images/Ungroup.png��60   $��16��6W   �� 5�  *   $��3q�64   $��Ungroup��6?   $��Center=�6N   $��1��6i   5�  '   5�     5�     �� 5�     $��1��6%   $��7,0,0,0q�65   .+�5�     �� -   5        $��
CmdForward��5�     $��0,3,0,3q�5�  %   $��3q�66   $
��Left=�6B   #��[ 5�     #��Z" 5�     ��5�     �� $ Designer����5�  "   � 5�  &   & 5�  *   �� 5�  .   �� 5�  .   .+�5�  *   �� $(��"/Resources/Images/BringForward.png��60   $��16��6\   �� 5�  *   $��3q�64   $��Forward��6?   $��Center=�6N   $��1��6i   5�  '   5�     �� 5�     -   5        $��
CmdToFront��5�     $��0,3,0,3q�5�  %   $��3q�66   $
��Left=�6B   #��[ 5�     #��Z$ 5�     ��5�     �� $ Designer����5�  "   � 5�  &   & 5�  *   �� 5�  .   �� 5�  .   .+�5�  *   �� $(��"/Resources/Images/BringToFront.png��60   $��16��6\   �� 5�  *   $��3q�64   $��To Front��6?   $��Center=�6O   $��1��6j   5�  '   5�     5�     �� 5�     #��[ 6   $��1��5�     	+ �� X1$+ 0��6+   	, �� Y1$, 0��62   	- �� X2$- 0��69   	. �� Y2$. 20��6@   / �� Stretch$
/ Fill=�5�     $
��Left=�6&   $��1��5�     $��3,0,0,0q�6'   �� 5�     $��2��6%   .+�5�     �� -   5        $��CmdBackward��5�     $��0,3,0,3q�5�  %   $��3q�66   $
��Left=�6B   #��[ 5�     #��Z# 5�     ��5�     �� $ Designer����5�  "   � 5�  &   & 5�  *   �� 5�  .   �� 5�  .   .+�5�  *   �� $(��"/Resources/Images/SendBackward.png��60   $��16��6\   �� 5�  *   $��3q�64   $��Backward��6?   $��Center=�6O   $��1��6k   5�  '   5�     �� 5�     -   5        $��	CmdToBack��5�     $��0,3,0,3q�5�  %   $��3q�66   $
��Left=�6B   #��[ 5�     #��Z% 5�     ��5      �� $ Designer����5  "   � 5  &   & 5  *   �� 5  .   �� 5  .   .+�5  *   �� $&�� /Resources/Images/SendToBack.png��60   $��16��6Z   �� 5  *   $��3q�64   $��To Back��6?   $��Center=�6N   $��1��6i   5	  '   5     5     5     5     5     5     � 5  
   $��2��6   $��0,10,0,0q�6   & 5     �� 5     $��* �6#   �� 5     $
��Auto �6#   62   �� 5     $
��2.8* �6#   .+�5     �� $��1��6   $��5��6+   $��Stretch=�65   
������ 6T   � 5     $��0��6   $��0,0,5,1q�6#   $��Stretch=�64   $��2��6P   .+�5     5  /Syncfusion.Windows.Tools.Controls.TabControlExt  -   5        0  Name$ 0 ConversationObjectExplorer��5  >   1 J� 	AllowDrop1 .  $��0 0 0 0q�6b   $��Stretch=�6s   $��Stretch=�5  ,   2   IsNewButtonEnabled2 .  3   EnableLabelEdit3 .  4   CloseButtonType$
4 Hide=�5  D   
������ .@�5     2  ,Syncfusion.Windows.Tools.Controls.TabItemExt  $��Property Explorer��6,   .��5      1  +Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid  -   5        5  Name$5 ObjectProperties��5   -   6   ShowSummary6 . 7   ShowSortOptions7 .  8   ShowAdvancedOptions8 .  9   	ShowTitle9 . :   ShowSearchBox: . ;   IsCategorized; . <   ShowPreview< .  
���   �=   AutoGenerateProperties= . $��Stretch=�5"  C   $��Stretch=�6a   >   FilterWatermark$>  ��6}   5$     5%       5&     $��Errors��6,   .��5'     &   System.Windows.Controls.DataGrid  -   5        ?  Name$? 	ErrorList��5'  #   @   AutoGenerateColumns@ .  A   CanUserAddRowsA .  B   CanUserDeleteRowsB .  
����  �$
��Left=�6�   C   CanUserSortColumnsC . D   RowHeaderWidth$D 0Q�5(  5   E   CanUserReorderColumnsE .  F   CanUserResizeRowsF .  $G   HorizontalScrollBarVisibility$
G Auto=�6�   "H   VerticalScrollBarVisibility$
H Auto=�5)  #   $��Stretch=�6F   $��Stretch=�5*  a   I   SelectionMode$I Single=�6~   J   CanUserResizeColumnsJ .  ��6   �� *" ConversationTree.ValidationItems+K �� Mode$K OneWay=�L   RowStyleL 5+     �� 5,  "   M �� 
TargetType)  #System.Windows.Controls.DataGridRow"M  6(   .�5-  &   -   5        5.  #   N   ColumnsN 50     4	  .System.Windows.Controls.DataGridTemplateColumn	  51  "   ,
  &System.Windows.Controls.DataGridColumnO 
  Width5  /System.Windows.Controls.DataGridLengthConverter$O 30 69   P 
  
IsReadOnlyP . Q 
  CanUserSortQ . R 
  SortMemberPath$R Severity��6i   S 	  CellTemplateS 52  &   �� 53  *   .R�54  .   �� $��18��64    6?   �� *
Severity+��6\   �� *SeverityImage+T �� UpdateSourceTrigger$T PropertyChanged=�55  +   57  #   	  58  "   $	O 240 69   P . Q . $R Description��6k   U 
  Header$U Description��6�   S 59  &   �� 5:  *   .R�5;  .   ��  68   �� *Description+��6X   �� *Description+$T PropertyChanged=�5<  +   5>  #   	  5?  "   $O * 69   P . Q . $R ValidationCategory��6i   $
U Type��6�   S 5@  &   �� 5A  *   .R�5B  .   ��  68   �� *ValidationCategory+��6_   �� *ValidationCategory+$T PropertyChanged=�5C  +   5E  #   	  5F  "   $O * 69   P . Q . $
R Name��6i   $
U Name��6   S 5G  &   �� 5H  *   .R�5I  .   ��  68   �� *Name+��6Q   �� *Name+$T PropertyChanged=�5J  +   5L  #   5O     5P     5Q     5R       5S     -    5        $0 MainScreenTabs��5S  7   $��2��6'   $��0 0 0 0q�6O   $��Stretch=�6`   
������ $��Stretch=�5T     3 .  2 .  $
4 Hide=�6h   .@�5U       $��Conversation Designer��6(   
������ .��5V     � $��0,0,0,0q�6   & 5W     �� 5X     $��1.75* �6/   �� 5Y     $
��Auto �6/   .+�5[     �� $��1��6'   $��5��67   $��Stretch=�6A   
������ 6`   � 5]     $��0��6    5^     �� 5_  "   $��* �60   6=   �� 5`  "   $
��Auto �60   6@   .+�5c     �� -!   5        V ��Name$V ScrollConversations��5c  P   $
��Auto=�6+   
������ $
��Auto=�6�   .��5d  "     -"   5        W  Name$W ConDesigner��5d  J   |�. X   CanHaveConnectorsX . 1 .   SnappingGridBrush_Ponits#��[ 5e  =   $��10q�6t   Y � ContextMenu#Y [ 5f     Z � FocusVisualStyleZ 5e  �   Z� [ � LayoutTransform[ 5g  &   �� 5h  *   \ �� ScaleX\ 6:   �� *Scale+] �� ScaleY] 6S   �� *Scale+5j  #   5k     � 5m     $��1��6Y   
���������6#   �� *ActualWidth+$ MapGroup��& 5n  "   �� 5o  &   $
��Auto �67   6F   �� 5p  &   $��* �67   6C   .+�5s  "   �� $��1��6-   $ 
Horizontal=�6=   $��20 5 5 5q�6V   $��Right=�6h   .+�5t  &   �� -#   5        $��3q�5t  -   $6 0Arrange all the objects to fill available space.��69    	 ToolBarButtonBaseStyleAlt#��[	 5u  -   $��0 0 5 0q�6   ��5v  *     5w  .   $ Th=�6>   $��Center=�6H   $��16��6c   $��Center=�6n   5y  '   5z  #   �� 5|  "   $��0��6-   $ 
Horizontal=�6=   $��5q�6V   .+�5}  &   �� -$   5        ^ � Cursor$
^ Hand��5}  3   $
��Left=�6B   $ Zoom In��6]   
������ $��Center=�6�   _ �� 	ClickMode$_ Press=�6�   .��5~  *     $ 
SearchPlus=�6:   $��Center=�6L   $��16��6g   $��Center=�6r   5  '   �� 5�  &   -%   5        $
^ Hand��5�  3   $��5 0 0 0q�6A   $��Center=�6R   
������ $
��Left=�6�   $ Zoom Out��6�   .��5�  *     $ SearchMinus=�6:   $��Center=�6M   $��16��6h   $��Center=�6s   5�  '   �� 5�  &   -&   5        $( CmdZoomRoomsDefault��5�  <   $
^ Hand��6-   #��[ 6Y   $��Center=�6�   $��5 0 0 0q�6�   $��50��6�   $��0��6�   $
��100%��6�   $��20��6�   6�   �� 5�  &   -'   5        $( CmdZoomToFit��5�  <   $
^ Hand��6-   #��[ 6R   $��Center=�6z   $��5 0 5 0q�6�   $��50��6�   $��0��6�   $	��Fit��6�   $��20��6�   6�   5�  #   5�     5�     5�     5�       5�     $��Script Explorer��6(   .��5�     +   %DiagramDesigner.Controls.ScriptWindow  -(   5        `  Name$` ScriptExplorer��5�  ,   a   UpdateContract$ a ConversationScriptContract��5�     b   SaveCommandb 5�  D   �� $ 
RootWindow��c �� Path$+c %DataContext.SaveCurrentProjectCommand�5�     5�       5�     $��Conversation Explorer��6(   .��5�     �  5�     �� 5�     $��3* �6,   6:   �� 5�     $��* �6,   69   .+�5�     �� $��0��6$   
���   �
�������$��Courier New)�6X   $
��Wrap=�6�   ��6q   �� *ConversationText+6�   �� 5�     $��1��6%   $ 
Horizontal=�62   .+�5�     3 
 -Syncfusion.Windows.Tools.Controls.ComboBoxAdv  $
��Left=�66   $	��175��6Q   $��30��6]   $	��Top=�6i   $��5q�6�   
������ ��5�  O   �� * ConversationTree.Conversations+d �� SelectedItemd 6�   �� *SelectedConversation+��5�  "   �� 5�  &   .R�5�  *   �� ��65   �� *
BaseName+6R   5�  '   5�     �� 5�     $+ %Runs the conversation from the start.��6&   $	��100��6W   $	��Run��6c   $��30��6q   $	��Top=�6}   $��5q�6�   #��[ 5�     ��6E   �� *RunConversationCommand+5�     �� 5�     -)   5        $��ResponseList��5�  T   $��Stretch=�6i   $��Stretch=�6�   ��6+   �� *CurrentResponses+��5�  "   �� 5�  &   .R�5�  *   �� $( "Click here to select this reponse.��62   $��50��6_   $��50��6n   $��25��6�   $	��Top=�6�   $��5q�6�   #��[ 5�  1   ��5�  y   �� ��5�  Z   �� ��5�  1   �� $ ResponseList��$*c $DataContext.RunSelectResponseCommand�5�  '   ��5�  "   �� 5�  &   .R�5�  *   � e � Orientation$e 
Horizontal=�64   $��Stretch=�5�  4   $��Stretch=�5�  4   5�  +   5�  '   5�     5�     5�     5�     5�     5�     7  1Syncfusion.Windows.Tools.Controls.RibbonStatusBar  5�  
   $��3��6%   $��	0 10 10 0q�62   .@�5�     �� $ 
Horizontal=�6   $��Stretch=�63   $��10 0 0 0q�6O   .+�5�     �� $��0 0 5 0q�6   $
��Bold"�6-   $��Center=�6?   	STATUS:6c   �� 5�     -*   5        f ��Name$f Status��5�  .   $��0 0 10 0q�6   $��Center=�6>   6[     5�     -+   5        g  Name$g WaitSpinner��5�  Z   $ Spinner=�6"   $y�	Collapsed=�61   $��0 0 10 0q�6H   $��Center=�6o   $��16��6�   $��Center=�6�   h J� RenderTransformOrigin$h 0.5, 0.5&�6�   z�5�     �� 5�     i �� Angle$i 0Q�6*   5�     5�     5�     5�     5�     