PartArranger
Version: 1.1.1

PartArranger allows you to sort the VAB/SPH part list any way you want. By default, it attaches to Blizzy's toolbar if it's installed, or the stock toolbar otherwise. You can tell it which toolbar to use in config.txt (generated after you enter the VAB/SPH for the first time). Fully compatible with PartCatalog.

When you are in either the VAB or the SPH, click the toolbar icon (two arrows, one facing up and one facing down) to show or hide the window. Select a category, then select a part in the list to move. Then click one of the buttons on the right to move it up, down, or to the top or bottom of the category. If you hold the up/down button for a second, it'll start quickly moving the part up or down. You can also move parts up or down by dragging them with the cursor.

You can resize the window by dragging the bottom edge of the window up or down. You can lock the window in place and disable resizing by clicking on the padlock icon in the top-right corner.

Removing a part mod will not break anything and won't alter the order of other parts. Newly added parts will appear at the end of their categories.

Curse page: http://www.curse.com/ksp-mods/kerbal/223902-partarranger
KSP forum thread: http://forum.kerbalspaceprogram.com/threads/92511
KerbalStuff page: http://beta.kerbalstuff.com/mod/128/PartArranger
Source code: https://github.com/tony311/PartArranger

=== Installation ===

Place the PartArranger folder inside of Kerbal Space Program/GameData. Load up the VAB or SPH once to generate a default configuration file at Kerbal Space Program/GameData/PartArranger/config.txt. See below for possible configuration options.

=== Config File Values ===

Note: you must load up the VAB or SPH to generate config.txt for the first time

WindowX: saved horizontal window position
WindowY: saved vertical window position
WindowHeight: saved window height
WindowLocked: whether the window is currently locked
WindowShown: whether the window is currently shown
HidePlugin: if true, hides the plugin entirely and only applies sorting
UseBlizzyToolbar: if true, adds a button to Blizzy's Toolbar instead of KSP's default toolbar if available
Everything else: a sorted list of every part