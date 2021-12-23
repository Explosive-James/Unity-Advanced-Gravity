using UnityEngine;
using System.IO;
using System;

namespace AdvancedGravity.UnityEditor.Settings
{
    internal class GravitySettings : ScriptableObject
    {
        #region Structs
        [Serializable]
        internal struct Setting
        {
            public string name;
            public float strength;

            public Setting(string name, float strength)
            {
                this.name = name;
                this.strength = strength;
            }
        }
        [Serializable]
        internal struct Payload
        {
            public Setting[] strengths;

            public Payload(Setting[] strengths)
            {
                this.strengths = strengths;
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// Editor directory for the scriptable object asset.
        /// </summary>
        public const string directory = "\\ProjectSettings\\GravitySettings.asset";

        private static GravitySettings _instance;
        /// <summary>
        /// The different strengths for gravity, controlled by the user.
        /// </summary>
        public Setting[] strengths = new Setting[]{
            new Setting("Low", 4.5f),
            new Setting("Normal", 9.81f),
            new Setting("High", 19.62f),
        };
        #endregion

        #region Properties
        public static GravitySettings Instance {
            get {
                if (_instance == null)
                    _instance = LoadOrCreate();
                return _instance;
            }
        }
        #endregion

        #region Public Functions
        public static void SaveOrCreate()
        {
            string fullDirectory = Environment.CurrentDirectory + directory;
            string jsonText = JsonUtility.ToJson(new Payload(Instance.strengths));

            File.WriteAllText(fullDirectory, jsonText);
        }
        #endregion

        #region Private Functions
        private static GravitySettings LoadOrCreate()
        {
            string fullDirectory = Environment.CurrentDirectory + directory;
            GravitySettings settings = CreateInstance<GravitySettings>();

            if (File.Exists(fullDirectory)) {

                string jsonText = File.ReadAllText(fullDirectory);
                Payload payload = JsonUtility.FromJson<Payload>(jsonText);

                settings.strengths = payload.strengths;
            }

            return settings;
        }
        #endregion
    }
}
