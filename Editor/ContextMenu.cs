using System;
using UnityEngine;
using UnityEditor;
using AdvancedGravity;

namespace AdvancedGravityEditor
{
    public class ContextMenu
    {
        [MenuItem("GameObject/GravityFields/Spherical Gravity")]
        static void CreateSphericalGravity(MenuCommand menuCommand) => CreateGravityField(menuCommand, typeof(SphericalGravity));

        [MenuItem("GameObject/GravityFields/Cubic Gravity")]
        static void CreateCubicGravity(MenuCommand menuCommand) => CreateGravityField(menuCommand, typeof(CubicGravity));

        [MenuItem("GameObject/GravityFields/Planar Gravity")]
        static void CreatePlanarGravity(MenuCommand menuCommand) => CreateGravityField(menuCommand, typeof(PlanarGravity));

        [MenuItem("GameObject/GravityFields/Cylindrical Gravity")]
        static void CreateCylindricalGravity(MenuCommand menuCommand) => CreateGravityField(menuCommand, typeof(CylindricalGravity));

        [MenuItem("GameObject/GravityFields/Capsule Gravity")]
        static void CreateCapsuleGravity(MenuCommand menuCommand) => CreateGravityField(menuCommand, typeof(CapsuleGravity));

        [MenuItem("GameObject/GravityFields/Donut Gravity")]
        static void CreateDonutGravity(MenuCommand menuCommand) => CreateGravityField(menuCommand, typeof(DonutGravity));

        static void CreateGravityField(MenuCommand command, Type gravityType)
        {
            GameObject go = new GameObject(gravityType.Name);
            GameObjectUtility.SetParentAndAlign(go, command.context as GameObject);

            go.AddComponent(gravityType);

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeGameObject = go;
        }
    }
}
