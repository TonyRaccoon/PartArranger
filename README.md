PartArranger
============
A custom part sorting plugin for Kerbal Space Program. Only shows up in the SPH and VAB. Attaches to either the stock toolbar or Blizzy's toolbar, depending on the value of **UseBlizzyToolbar** in config.txt (see below).


Installation
============
Place the PartArranger folder inside of Kerbal Space Program/GameData. Load up the editor (VAB or SPH) once to generate a default configuration file at Kerbal Space Program/GameData/PartArranger/config.txt. See below for possible configuration options.


Usage
=====
This plugin allows you to rearrange the parts in any order you want (it does not allow you to change which category a part is in, however.

When you are in either the VAB or the SPH, click the toolbar icon (two arrows, one facing up and one facing down) to show or hide the window. Select a category, then select a part in the list to move. Then click one of the buttons on the right to move it up, down, or to the top or bottom of the category. If you hold the up/down button for a second, it'll start quickly moving the part up or down.

You can resize the window by dragging the bottom edge of the window up or down. You can lock the window in place and disable resizing by clicking on the padlock icon in the top-right corner.

Removing a part mod will not break anything and won't alter the order of other parts. Newly added parts will appear at the end of their categories.

Config File Values
==================

*Note: you must load up the VAB or SPH to generate config.txt for the first time*

* **WindowX**: saved horizontal window position
* **WindowY**: saved vertical window position
* **WindowHeight**: saved window height
* **WindowLocked**: whether the window is currently locked
* **WindowShown**: whether the window is currently shown
* **UseBlizzyToolbar**: if true, adds a button to Blizzy's Toolbar instead of KSP's default toolbar if available
* **Everything else**: a sorted list of every part
