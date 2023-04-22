using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ShipMaker.EditorData;
using System.Collections.Generic;
using System;
using ShipMaker.Data;
using System.Linq;

namespace ShipMaker.CEditor
{
    public class TurretFixerWindow : EditorWindow
    {
        public static Sprite icon;
        public static GameObject ship;

        private static List<Sprite> sprites;

        [MenuItem("Window/Turrets Preview")]
        public static bool ShowWindow()
        {
            TurretFixerWindow window = GetWindow<TurretFixerWindow>(typeof(TurretFixerWindow));
            window.titleContent = new GUIContent("Turrets Position Preview");
            window.minSize = new Vector2(500, 250);

            window.position = new Rect(Screen.width / 2, Screen.height / 2, 350, 450);
            window.maxSize = new Vector2(320, 420);
            window.minSize = new Vector2(320, 420);

            return true;
        }
        public void OnEnable()
        {
            if(sprites == null)
                sprites = new List<Sprite>(Resources.LoadAll<Sprite>("Cannons"));
        }

        public void OnInspectorUpdate()
        {
            Repaint();
        }

        void OnGUI()
        {
            GUIStyle title = new GUIStyle(EditorStyles.largeLabel)
            {
                alignment = TextAnchor.MiddleCenter
            };

            GUIStyle styleBack = new GUIStyle(EditorStyles.helpBox)
            {
                fixedWidth = 320
            };

            EditorGUILayout.BeginVertical(styleBack);

            GUILayout.Label("Turrets Position Preview", title);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            icon = (Sprite)EditorGUILayout.ObjectField(icon, typeof(Sprite), false);
            ship = (GameObject)EditorGUILayout.ObjectField(ship, typeof(GameObject), true);

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.FlexibleSpace();

            if (icon != null)
               Graphics.DrawTexture(new Rect(32, 100, 256, 256), icon.texture);

            DrawGuns();
            
            EditorGUILayout.EndVertical();


            EditorGUILayout.EndVertical();
        }

        private void DrawGuns()
        {
            if (ship == null)
                return;

            MShipModelData modelData = ship.GetComponent<MShipModelData>();
            Transform transform = ship.transform.Find("WeaponSlots");
            
            for (int index = 0; index < transform.childCount; ++index)
            {

                WeaponTurretData component = transform.GetChild(index).GetComponent<WeaponTurretData>();
                Vector3 localPosition = transform.GetChild(index).transform.localPosition;
                Vector2 vector2 = component != null ? component.iconTranslate : Vector2.zero;

                float x = (float)((localPosition.x + vector2.x) * modelData.drawScale) + 160f - 9;
                float y = (float)((-localPosition.z - vector2.y) * modelData.drawScale) + 228f - 16;

                int count = component.transform.GetComponentsInChildren<Transform>().Where(t => t.name.Contains("GunTip")).ToList().Count;
                count = Mathf.Max(count, component.extraGuns.Length + 1);
                count = Mathf.Clamp(count, 1, 4);

                Rect rect = new Rect(new Rect(x, y, 18, 32));
                Vector2 pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);

                Matrix4x4 matrixBackup = GUI.matrix;
                GUIUtility.RotateAroundPivot(component.transform.localEulerAngles.y, pivot);
                Graphics.DrawTexture(new Rect(x, y, 18, 32), sprites.Find(s => s.name == "cannon_slot_" + count).texture);
                GUI.matrix = matrixBackup;

                if(component.hasSpecialStats)
                {
                    matrixBackup = GUI.matrix;
                    GUIUtility.RotateAroundPivot(component.transform.localEulerAngles.y, pivot);
                    Graphics.DrawTexture(new Rect(x + 1, y + 13, 16, 16), sprites.Find(s => s.name == "star").texture);
                    GUI.matrix = matrixBackup;
                }
                    
            }
        }
    }
}