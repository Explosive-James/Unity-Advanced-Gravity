# Unity Advanced Gravity Package

## Summary:

The advanced gravity package includes a collection of gravity fields that apply different shaped gravities to Unity's 3d Rigidbodies.

**Features Include:**  
* **Priority System**: this allows for overlapping gravity fields to override the other's influence. When a rigidbody is inside of two gravity fields, the one with higher priority will have exclusive influence over it, if the priorities are the same then they both have influence over the rigidbody.
* **Customizable Preset Values**: instead of having to manually tune every gravity field's strength, you have the option of setting it to a preset strength that is controllable from inside the project settings, allowing you to add or remove values and customize their strength.

## How to Install:

#### Through Git:

To do this you must first have [Git](https://git-scm.com/) installed or other version control software using git. Simply copy the git URL under Code >> HTTPS then inside of the Unity Editor paste the code into the package manager under Window >> Package Manager >> + >> Add package from git URL.

#### Through Zip:

Download the zip under Code >> Downlod ZIP and extract the files, then inside of the Unity Editor open up the package manager under Window >> Package Manager and click on + >> Add package from disk and locate the **package.json** file extracted from the ZIP.<br>
*Note: The files won't be moved into your project directory and deleting the files after importing them will break the package, you should put them in a safe directory before importing them into Unity.*

## Basic User Guide:

#### Adding a Gravity Field to a Scene:  

To add a new gravity field, you simply need to add a script to an gameObject, ideally an empty gameObject or one without a collider, and adjust the settings in the inspector. The included gravity fields are named: **Spherical Gravity**, **Cublic Gravity**, **Planar Gravity**, **Cylindrical Gravity**, **Capsule Gravity** and **Donut Gravity**, all can be found by simply searching for the word "gravity".<br>
*Note: The global gravity will still be applied to rigidbodies inside of gravity fields, if you want the gravity fields to be the only influence to a rigidbody you need to change the gravity settings in the project settings.*

#### Finding Gravity through a Script:

To grab global gravity being applied at a position or to a rigidbody, you can access **AdvancedGravitty.Gravity** and use the **GetGlobalGravity** function that takes either a world position or a rigidbody.<br>
To get local gravity from a specific gravity field, you can access the **GetFieldGravity** from a gravity field instance. However this does not consider overlapping priorities or other gravity fields.

#### Adjusting Preset Values:

To adjust a value you go inside the project settings under Edit >> Project Settings >> Advanced Gravity and adjust the values from there, you can adjust both the names and the strengths of the values as well as adding new values.<br>
The save file for these settings are located in the project settings folder of your project's main directory as a file called "GravitySettings.asset", deleting this file will restore the default settings as well as creating a new file under the same name.  

## Creating a Custom Gravity Field:

For a guide on creating a custom gravity field using this package as well as a custom inspector for it, please see the [EXTENSION.md](https://github.com/Explosive-James/Unity-Advanced-Gravity/blob/main/EXTENSION.md).
