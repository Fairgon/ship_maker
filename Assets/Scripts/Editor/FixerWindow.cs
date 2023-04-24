using UnityEditor;
using UnityEngine;
using ShipMaker.Data;

namespace ShipMaker.CEditor
{
    public class FixerWindow : EditorWindow
    {
        public static GameObject ship;

        private GUIStyle titleStyle;

        [MenuItem("Window/Fixer Window")]
        public static bool ShowWindow()
        {
            FixerWindow window = GetWindow<FixerWindow>(typeof(FixerWindow));
            window.titleContent = new GUIContent("Fixer Window");
            window.minSize = new Vector2(320, 125);

            window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 450);
            window.maxSize = new Vector2(320, 125);
            window.minSize = new Vector2(320, 125);

            return true;
        }
        
        void OnGUI()
        {
            titleStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                alignment = TextAnchor.MiddleCenter
            };

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Fixer", titleStyle);

            EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

            ship = (GameObject)EditorGUILayout.ObjectField("Ship", ship, typeof(GameObject), true);
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

            EditorGUILayout.LabelField("This will save all prefab changes. Are you sure?");

            if (GUILayout.Button("Fix GunTips Positions"))
            {
                FixGunTipsPositions();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }

        private void FixGunTipsPositions()
        {
            if(ship == null)
            {
                Debug.LogError("Set Ship First!");

                return;
            }

            float higherPos = Mathf.NegativeInfinity;
            
            foreach (WeaponTurretData t in ship.transform.GetComponentsInChildren<WeaponTurretData>())
            {
                higherPos = Mathf.Max(higherPos, t.transform.position.y);
            }

            foreach (WeaponTurretData t in ship.transform.GetComponentsInChildren<WeaponTurretData>())
            {
                Undo.RecordObject(t, t.name);
                t.transform.position -= Vector3.up * higherPos;
                EditorUtility.SetDirty(t);
            }

            Transform model = ship.transform.Find("Model");
            
            for(int index = 0; index < model.transform.childCount; ++index)
            {
                Transform t = model.transform.GetChild(index);

                Undo.RecordObject(t, t.name);
                t.position -= Vector3.up * higherPos;
                EditorUtility.SetDirty(t);
            }

            Transform thrusters = ship.transform.Find("Thrusters");

            for (int index = 0; index < thrusters.transform.childCount; ++index)
            {
                Transform t = thrusters.transform.GetChild(index);

                Undo.RecordObject(t, t.name);
                t.position -= Vector3.up * higherPos;
                EditorUtility.SetDirty(t);
            }

            foreach (Transform t in ship.transform.GetComponentsInChildren<Transform>())
            {
                if (!t.name.Contains("GunTip"))
                    continue;

                Undo.RecordObject(t, t.name);
                t.position = new Vector3(t.position.x, 0, t.position.z);
                EditorUtility.SetDirty(t);

                higherPos = Mathf.Max(higherPos, t.parent.position.y);
            }

            if(PrefabUtility.IsPartOfPrefabInstance(ship))
                PrefabUtility.ApplyPrefabInstance(ship, InteractionMode.UserAction);
        }
    }
}