   M S B A M L   `   `   `  ����K  GSynapse X Remake, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Synapse_X_RemakeSynapse_X_Remake  5   	   &     Synapse_X_Remake.ScriptHubWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Synapse_X_Remake     5   	   -   5          � Title$  ScriptHubWindow��5   	   $	��336��6!   $	��439��6.    � WindowStyle$
 None=�6:    � WindowStartupLocation$ CenterScreen=�6M    � AllowsTransparency . 
�������
������  � Topmost . ��6�   Z� .��5	      � .+�5
   
   �� -   5         ��Name$ ScriptHubBorder��5
      $��1q�6*   $
��Left=�6>   $	��336��6Y   $	��Top=�6f   $	��439��6~   ���	#FF333333._�5      � .+�5      � -   5         �Name$ TopBox��5      $
��Left=�6'   $��30��6B   $	��Top=�6N   $	��437��6f   ���	#FF3C3C3C.+�5      �� -   5         ��Name$ TitleBox��5      $��Synapse X - Script Hub��6.   $��Center=�6O   $��0,0,0,0q�6l   $��Center=�6}   
�������6�   �� 5      -   5         ��Name$ IconBox��5      $#��sxlogosmallwhite_OJJ_icon.ico��6-   $
��Left=�6T   $��2,0,0,0q�6o   $	��Top=�6�   �� 5      -   5        	 ��Name$	 
MiniButton��5      $��_��61   #��[" �6=   $��Right=�6x   $	��Top=�6�   $��22��6�   
�������6�   5      �� 5      -   5        
 ��Name$
 	ScriptBox��5      $��0q�6-   $
��Left=�6A   $	��286��6\   $��10,39.6,0,0q�6i   $	��Top=�6~   $	��115��6�   ���	#FF3C3C3C
�������6�   �� 5      -   5        $ ScriptPictureBox��5      $
��Left=�62   $	��170��6M   $��130,39.6,0,0q�6Z   $	��Top=�6p   $	��299��6�   �� 5      -	   5         ��Name$ DescriptionBox��5   ,    �� 
IsReadOnly . $��0q�6D   $
��Left=�6X   $��80��6s   $��130,214.6,0,0q�6    �� TextWrapping$
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��299��6�   ���	#FF1E1E1E
�������6�   �� 5      -
   5        $	 ExecuteButton��5      $��Execute��60   #��[" �6B   ���	#FF3C3C3C$
��Left=�6�   $��130,299.6,0,0q�6�   $	��Top=�6�   $	��149��6�   
�������$��26��6�   6	  �� 5      -   5        $	 CloseButton��5      $��Close��6.   #��[" �6>   ���	#FF3C3C3C$
��Left=�6�   $��284,299.6,0,0q�6�   $	��Top=�6�   $	��145��6�   
�������$��26��6�   6  5      5      5      5      