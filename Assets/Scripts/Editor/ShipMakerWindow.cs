using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using ShipMaker.EditorData;
using ShipMaker.Data;
using System.Collections.Generic;
using System.Collections;
using UnityEditorInternal;

namespace ShipMaker.CEditor
{
    public class ShipMakerWindow : EditorWindow
    {
        public BundleData curData;

        private ReorderableList matList;
        private ReorderableList texturList;
        private ReorderableList meshList;
        private ReorderableList turList;

        private GUIStyle titleStyle;
        private string path;

        private static bool init = false;

        private Vector2 scrollPos;

        [OnOpenAsset(1)]
        public static bool ShowWindow(int instanceId, int line)
        {
            init = false;
            UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceId);

            if (item is BundleData)
            {
                ShipMakerWindow window = (ShipMakerWindow)GetWindow<ShipMakerWindow>(typeof(ShipMakerWindow));
                window.titleContent = new GUIContent("Ship Builder");
                window.minSize = new Vector2(500, 525);

                window.curData = (BundleData)item;

                window.position = new Rect(Screen.width / 2, Screen.height / 2, 500, 250);
            }

            return true;
        }

        private void Init()
        {
            matList = new ReorderableList(curData.Materials, typeof(UnityEngine.Object), true, true, true, true);
            matList.drawElementCallback = MaterialsDrawCallback;
            matList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Materials"), EditorStyles.boldLabel); };

            texturList = new ReorderableList(curData.Textures, typeof(UnityEngine.Object), true, true, true, true);
            texturList.drawElementCallback = TexturesDrawCallback;
            texturList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Textures"), EditorStyles.boldLabel); };

            meshList = new ReorderableList(curData.Meshes, typeof(UnityEngine.Object), true, true, true, true);
            meshList.drawElementCallback = MeshesDrawCallback;
            meshList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Meshes"), EditorStyles.boldLabel); };

            turList = new ReorderableList(curData.Turrets, typeof(UnityEngine.Object), true, true, true, true);
            turList.drawElementCallback = TurretsDrawCallback;
            turList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Turrets"), EditorStyles.boldLabel); };

            init = true;
        }

        private void TurretsDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            curData.Turrets[index] = EditorGUI.ObjectField(
                                    new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                                    GUIContent.none,
                                    curData.Turrets[index],
                                    typeof(TextAsset),
                                    false);
        }

        private void MeshesDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            curData.Meshes[index] = EditorGUI.ObjectField(
                                    new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                                    GUIContent.none,
                                    curData.Meshes[index],
                                    typeof(Mesh),
                                    false);
        }

        private void TexturesDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            curData.Textures[index] = EditorGUI.ObjectField(
                                    new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                                    GUIContent.none,
                                    curData.Textures[index],
                                    typeof(Texture),
                                    false);
        }

        private void MaterialsDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            curData.Materials[index] = EditorGUI.ObjectField(
                                    new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                                    GUIContent.none,
                                    curData.Materials[index],
                                    typeof(Material),
                                    false);
        }

        private void OnGUI()
        {
            if (!init)
                Init();

            titleStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                alignment = TextAnchor.MiddleCenter
            };

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.LabelField("Ship Builder", titleStyle);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    curData.Name = EditorGUILayout.TextField("Name", curData.Name);
                    curData.Icon = (Sprite)EditorGUILayout.ObjectField("Icon", curData.Icon, typeof(Sprite), false);
                    curData.ShipBlank = (GameObject)EditorGUILayout.ObjectField("Ship Prefab", curData.ShipBlank, typeof(GameObject), false);
                    curData.ShipData = (TextAsset)EditorGUILayout.ObjectField("ShipData", curData.ShipData, typeof(TextAsset), false);

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            turList.DoLayoutList();
            matList.DoLayoutList();
            texturList.DoLayoutList();
            meshList.DoLayoutList();

            EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

            if (GUILayout.Button("Create Ship Data"))
            {
                SerializeData();
            }

            if (GUILayout.Button("Create Ship Asset"))
            {
                CreateAsset();

                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndScrollView();

            EditorUtility.SetDirty(curData);
        }

        private void CheckPath()
        {
            if (curData.Name == "")
            {
                Debug.LogError("Enter a name.");
            }
            else 
            {
                path = Application.dataPath + "/ShipsResources/" + curData.Name;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
        }

        private void CreateAsset()
        {
            CheckPath();

            List<string> assetNames = new List<string>();

            assetNames.Add(AssetDatabase.GetAssetPath(curData.Icon));
            assetNames.Add(AssetDatabase.GetAssetPath(curData.ShipBlank));
            assetNames.Add(AssetDatabase.GetAssetPath(curData.ShipData));

            foreach(TextAsset turret in curData.Turrets)
            {
                assetNames.Add(AssetDatabase.GetAssetPath(turret));
            }

            foreach(Material mat in curData.Materials)
            {
                assetNames.Add(AssetDatabase.GetAssetPath(mat));
            }

            foreach (Texture t in curData.Textures)
            {
                assetNames.Add(AssetDatabase.GetAssetPath(t));
            }

            foreach (Mesh mesh in curData.Meshes)
            {
                assetNames.Add(AssetDatabase.GetAssetPath(mesh));
            }

            AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

            buildMap[0].assetBundleName = curData.Name;
            buildMap[0].assetNames = assetNames.ToArray();

            BuildPipeline.BuildAssetBundles("Assets/Ships",
                                            buildMap,
                                            BuildAssetBundleOptions.None,
                                            BuildTarget.StandaloneWindows);
        }

        private void SerializeData()
        {
            if (curData.ShipBlank == null)
            {
                Debug.LogError("Set Ship Prefab First!");
            }
            else
            {

                CheckPath();

                string jsonString;

                MShipModelData modelData = curData.ShipBlank.GetComponent<MShipModelData>();

                jsonString = JsonUtility.ToJson(modelData, true);

                File.WriteAllText(path + "/ShipData.json", jsonString);

                foreach (WeaponTurretData w in modelData.transform.GetComponentsInChildren<WeaponTurretData>())
                {
                    jsonString = JsonUtility.ToJson(w, true);
                    File.WriteAllText(path + "/Turret_" + w.turretIndex + ".json", jsonString);
                }
            }
        }
    }
}