# Advanced Gravity Extension Guide

## Creating Custom Gravity Fields:

To create your own custom gravity field shapes, create a new script in Unity with the appropriate name and include both the **UnityEngine** and **AdvancedGravity.Internal** namespaces and instead of **MonoBehaviour** inherit **GravityField** instead:
	
	using AdvancedGravity.Internal;
	using UnityEngine;
	
	public class CustomGravity : GravityField

For the gravity field to function correctly it requires two things, the first is a Unity collider as a trigger and the easiest way to do this is to use **gameObject.AddComponent** inside of **Start** and change it's **isTrigger** property to **true**:

	private void Start() {
		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		
		collider.isTrigger = true;
		collider.radius = MaximumRange;
	}
You should also take this opportunity to adjust the size of the collider, the most important variable to help with this is the **MaximumRange** property.<br><br>
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

## Creating Custom Gravity Inspector:

To create a custom inspector for your gravity field you'll need an **Editor** folder in your project, these are special folders in Unity that create an editor onl assembly that won't be built.<br>
Inside this folder you'll want to create a new script and make sure to include the **UnityEditor**, **AdvancedGravity** and **AdvancedGravityEditor.Inspectors** namespaces, inherit from **Editor** and give it the **CustomEditor** attribute:  

	using UnityEditor;
	using AdvancedGravity;
	using AdvancedGravityEditor.Inspectors;

	[CustomEditor(typeof(CustomGravity))]
	class CustomGravityInspector : Editor

Next you'll need to override the **OnInspectorGUI** method, inside you'll need to serialize the **target** into a **SerializedObject**:  

	public override void OnInspectorGUI() {

		SerializedObject serializedGravity = new SerializedObject(target);
	}

then you can call the helper methods inside of the **GravityInspectorHelper** class to drawn all the main properties, **DrawPriorityInspector**, **DrawGravityInspector**, **DrawMaximumRangeInspector** and **DrawMinimumRangeInspector**:  

	public override void OnInspectorGUI() {

		SerializedObject serializedGravity = new SerializedObject(target);

		EditorGUILayout.Separator();
		GravityInspectorHelper.DrawPriorityInspector(serializedGravity);

		EditorGUILayout.Separator();
		GravityInspectorHelper.DrawGravityInspector(serializedGravity);

		EditorGUILayout.Separator();
		GravityInspectorHelper.DrawMaximumRangeInspector(seralizedGravity);

		EditorGUILayout.Separator();
		GravityInspectorHelper.DrawMinimumRangeInspector(seralizedGravity);
	}

The last step will be to draw any additional variables you put inside your custom gravity field class.