   M S B A M L   `   `   `  ���� T  PSoundSourceCoverApplyTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:SoundSourceCoverApplyToolSoundSourceCoverApplyTool  T PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls fCclr-namespace:MahApps.Metro.Controls.Dialogs;assembly=MahApps.MetroMahApps.Metro.Controls.Dialogs *    $SoundSourceCoverApplyTool.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     9local'clr-namespace:SoundSourceCoverApplyTool     PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     VDialogCclr-namespace:MahApps.Metro.Controls.Dialogs;assembly=MahApps.Metro     -     � 
ResizeMode$  NoResize=�(  "MahApps.Metro.Controls.MetroWindow   ShowIconOnTitleBar .    AccentColorBrush   	GlowBrush# �      NonActiveBorderBrush �	#CDFF0000 � Icon$ LE_Symbol.ico�� � Title$# Sound Source Cover Apply Tool��$��637.029��$��899.697�� � 	Resources   ListTemplateItem �� Orientation1  +MahApps.Metro.Controls.Dialogs.CustomDialog  Name  CustomDialogTest4  .MahApps.Metro.Controls.Dialogs.BaseMetroDialog	   Title
 ��Name ��Name ��Name��%�  &	       &	 �    �� .R��� $ 
Horizontal=�$��	10,5,10,5q�.+��� $ 
Horizontal=�.+��� $��60��$��60��$
��Fill=����� *ImageSource+�� $ Vertical=�$��10,0,0,0q�.+��� $
��Auto��$
��Bold"�$
��11pt(�$��0,0,0,0q�$��0,0,0,0q����� *
FileName+�� ���#C2C2C2$
��Auto��$��0,0,0,0q�$��0,0,0,0q����� *Artist+�� ���#C2C2C2$
��Auto��$��0,0,0,0q�$��0,0,0,0q����� *Title+  $ CustomTestDialog��$	  ��.��� .+��� $
 imgCover��$
��Fill=�$	��300��$	��300���� $ MessageTextBlock��$	��200��#���   ���� *Artists[0].Name+�� $ btnOk��$	��100��$��30��$��OK��.��� $��0,0,-8,0q� � RowDefinitions �� $��* ��� $
��Auto �.+�� $��0��.+��� -    ��Name$ listBox��#��[ $
��Left=�$	��506�� �� SelectionMode$ Extended=� J� 	AllowDrop . $	��Top=�$	��577�����	#FF008A00$��3q�$��	27,54,0,0q��� -    ��Name$ 	lblArtist��$
��Left=�$��630,310,0,0q�$	��Top=�$
��Bold"�$
��14pt(�.���� -   $��	txtArtist��$	��230��$��WrapWithOverflow=��� -   $ lblSongTitle��$
��Left=�$��630,360,0,0q�$	��Top=�.���� -   $��txtSongTitle��$	��230��$��WrapWithOverflow=��� -   $ lblFileSize��$
��Left=�$��630,390,0,0q�$	��Top=��� -   $ lblType��$
��Left=�$��630,405,0,0q�$	��Top=��� $��2q����	#FF008A00$��631,54,32,289q�._��� -	   $
 imgFrontCover��$	��221��$��8,8,8,0q�$	��Top=��� -
   $ btnRemoveCoverImg��$��Remove Cover Image (R)��$
��Left=�$��631,438,0,0q�$	��Top=�$	��237��$��38��  AccentedSquareButtonStyle#��[ �� -   $ btnApplyInFolderImg��$��Apply In Folder Image (F)��$
��Left=�$��631,480,0,0q�$	��Top=�$	��237��$��38��#��[ �� -   $ btnApplyCoverImg��$��Apply With Gracenote (C)��$
��Left=�$��631,522,0,0q�$	��Top=�$	��237��$��38��#��[ �� -   $ 	lblSsList��$)��#Sound Sources(Ctrl+A is select all)��$
��16pt(�$
��Left=�$��	27,11,0,0q�$	��Top=��� $��1��.@��� -    ��Name$ sbiMajor��.��!Drag N Drop your file or folder��   MetroStatusBarSeparator#��[ �� -   $ sbiMinor��.��
List Empty � TaskbarItemInfo *  $System.Windows.Shell.TaskbarItemInfo     Description$ Total Sound Sources :��   ThumbButtonInfos *  $System.Windows.Shell.ThumbButtonInfo     ImageSource$ LE_Symbol.ico��   Description$ Description #1��  $ LE_Symbol.ico��$ Description #2��