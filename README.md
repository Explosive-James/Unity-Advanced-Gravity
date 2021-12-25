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

Download the zip under Code >> Downlod ZIP and extract the files, then inside of the Unity Editor open up the package manager under Window >> Package Manager and click on + >> Add package from disk and locate the **package.json** file extracted from the ZIP.  
You should note that these files won't be moved into your project directory and deleting the files after importing them will break the package, you should put them in a safe directory before importing them into Unity.

## Basic Usager Guide:

#### Adding a Gravity Feild to a Scene:  

To add a new gravity field, you simply need to add a script to an gameObject, ideally an empty gameObject or one without a collider, and adjust the settings in the inspector. The included gravity fields are named: "Spherical Gravity", "Cublic Gravity", "Planar Gravity", "Cylindrical Gravity" and "Capsule Gravity", all can be found by simply searching for the workd "gravity".

#### Adjusting Preset Values:

To adjust a value you go inside the project settings under Edit >> Project Settings >> Advanced Gravity and adjust the values from their, you can adjust both the names and the strengths of the values as well as adding new values.  
The save file for these settings are located in the project settings folder of your project's main directory as a file called "GravitySettings.asset" and you can copy that over to other projects, deleting this file will restore the default settings as well as creating a new file under the same name.

## Creating Custom Gravity Fields:

To create your own custom gravity field shapes, create a new script in Unity with the appropriate name and include both the **UnityEngine** and **AdvancedGravity.Internal** namespaces and instead of **MonoBehaviour** inherit **GravityField** instead:
	
	using AdvancedGravity.Internal;
	using UnityEngine;
	
	public class CustomGravity : GravityField

For the gravity field to function correctly it requires two things, the first is a Unity collider as a trigger and the easiest way to do this is to use **gameObject.AddComponent** inside of **Awake** and change it's **isTrigger** property to **true**:

	private void Awake() {
		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		
		collider.isTrigger = true;
		collider.radius = MaximumRange;
	}
You should also take this opportunity to adjust the size of the collider, the most important variable to help with this is the **MaximumRange** property. 
    
The second requirement is the overriding the **GetFieldGravity** method, this is where both the direction and strength of your gravity field should be calculate. To help calculate the gravity fading effect you have access to the **CalculateFadeMultiplier** method that will take a float representing distance and compare them to the fade distances. This returns a value between 0 to 1 that you can multiply with the **Strength** property or **_strength** variable and direction:  
	
	public override Vector3 GetGravityField(Vector3 rigidbodyPosition) {
	
		Vector3 difference = rigidbodyPosition - transform.position;
		float distance = difference.magnitude;
		
		return difference.normalized * CalculateFadeMultiplier(distance) * -Strength;
	}
  
Displaying the gravity field is done using **Gizmos** inside of the **OnDrawGizmos** method and for the colour you have the **GetGameObjectColour** method:

	private void OnDrawGizmos() {
			
		Gizmos.color = GetGameObjectColour();
		
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(Vector3.zero, MaximumRange);
	}

**Important Notes:**  
* Adjusting the **_priority** variable during runtime might not update rigidbodies that are already in the gravity field.  
* Adjusting size variables such as **_maximumRange** during runtime should continue to work correctly for internal functions, they won't automatically adjust the size of the collider that checks if a rigidbody should have gravity applied to it.  
* The **CalculateFadeMultipler** method doesn't consider object scale when comparing distances to the minimum and maximum ranges. You must convert the distance to local space if you do want it to consider the object's scale.
