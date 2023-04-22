using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using ShipMaker.Data;
using ShipMaker.EditorData;
using UnityEditor.Experimental.GraphView;

namespace ShipMaker.CEditor
{
    [CustomEditor(typeof(MShipModelData), true)]
    public class ShipModelDataEditor : Editor
    {
        private SerializedProperty id;
        private SerializedProperty shipModelName;
        private SerializedProperty manufacturer;
        private SerializedProperty shipClass;
        private SerializedProperty shipRole;
        private SerializedProperty sellChance;
        private SerializedProperty level;
        private SerializedProperty hullPoints;

        public SerializedProperty modelBonus;

        public SerializedProperty weaponSpace;
        public SerializedProperty equipSpace;
        public SerializedProperty cargoSpace;
        public SerializedProperty passengers;

        public SerializedProperty hangarDroneSpace;
        public SerializedProperty hangarShipSpace;

        public SerializedProperty crewSpace;

        public SerializedProperty speed;
        public SerializedProperty agility;
        public SerializedProperty mass;
        public SerializedProperty sortPower;
        public SerializedProperty rarity;
        public SerializedProperty sizeScale;
        public SerializedProperty drawScale;

        public SerializedProperty repReq;

        public SerializedProperty factions;
        public SerializedProperty craftingMaterials;
        public SerializedProperty extraSurFXScale;

        private ReorderableList bonusList;
        private string str = "";
        private int bonusLastIndex = 0;

        private ReorderableList crewList;
        private ReorderableList fractionList;
        private ReorderableList craftingMaterialsList;

        void OnEnable()
        {
            id = serializedObject.FindProperty("id");
            shipModelName = serializedObject.FindProperty("shipModelName");
            manufacturer = serializedObject.FindProperty("manufacturer");
            shipClass = serializedObject.FindProperty("shipClass");
            shipRole = serializedObject.FindProperty("shipRole");

            sellChance = serializedObject.FindProperty("sellChance");
            level = serializedObject.FindProperty("level");
            hullPoints = serializedObject.FindProperty("hullPoints");

            modelBonus = serializedObject.FindProperty("modelBonus");

            weaponSpace = serializedObject.FindProperty("weaponSpace");
            equipSpace = serializedObject.FindProperty("equipSpace");
            cargoSpace = serializedObject.FindProperty("cargoSpace");
            passengers = serializedObject.FindProperty("passengers");

            hangarDroneSpace = serializedObject.FindProperty("hangarDroneSpace");
            hangarShipSpace = serializedObject.FindProperty("hangarShipSpace");

            crewSpace = serializedObject.FindProperty("crewSpace");

            speed = serializedObject.FindProperty("speed");
            agility = serializedObject.FindProperty("agility");
            mass = serializedObject.FindProperty("mass");
            sortPower = serializedObject.FindProperty("sortPower");
            rarity = serializedObject.FindProperty("rarity");
            sizeScale = serializedObject.FindProperty("sizeScale");
            drawScale = serializedObject.FindProperty("drawScale");
            repReq = serializedObject.FindProperty("repReq");

            factions = serializedObject.FindProperty("factions");
            craftingMaterials = serializedObject.FindProperty("craftingMaterials");
            extraSurFXScale = serializedObject.FindProperty("extraSurFXScale");

            bonusList = new ReorderableList(serializedObject, modelBonus, true, true, true, true);
            bonusList.drawElementCallback = BonusDrawElemtCallback;
            bonusList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Bonuses"), EditorStyles.boldLabel); };

            crewList = new ReorderableList(serializedObject, crewSpace, true, true, true, true);
            crewList.drawElementCallback = CrewDrawCallback;
            crewList.elementHeight = 86;
            crewList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Crew"), EditorStyles.boldLabel); };

            fractionList = new ReorderableList(serializedObject, factions, true, true, true, true);
            fractionList.drawElementCallback = FractionDrawCallback;
            fractionList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Factions"), EditorStyles.boldLabel); };

            craftingMaterialsList = new ReorderableList(serializedObject, craftingMaterials, true, true, true, true);
            craftingMaterialsList.drawElementCallback = CraftingDrawCallback;
            craftingMaterialsList.elementHeight = 64;
            craftingMaterialsList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Crafting Materials"), EditorStyles.boldLabel); };
        }

        private void CraftingDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = craftingMaterialsList.serializedProperty.GetArrayElementAtIndex(index);

            var id = element.FindPropertyRelative("itemID");
            var quantity = element.FindPropertyRelative("quantity");

            EditorGUI.LabelField(
                new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                new GUIContent("item " + (index + 1)));

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 22, rect.width, EditorGUIUtility.singleLineHeight),
                id, new GUIContent("itemID"));

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 42, rect.width, EditorGUIUtility.singleLineHeight),
                quantity, new GUIContent("Quantity"));
        }

        private void FractionDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = fractionList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                element, GUIContent.none);
        }

        private void CrewDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = crewList.serializedProperty.GetArrayElementAtIndex(index);

            var pos = element.FindPropertyRelative("position");
            var minRequired = element.FindPropertyRelative("minRequired");
            var space = element.FindPropertyRelative("space");

            EditorGUI.LabelField(
                new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                new GUIContent("Crew Seat " + (index + 1)));

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 22, rect.width, EditorGUIUtility.singleLineHeight),
                pos, new GUIContent("Position"));

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 44, rect.width, EditorGUIUtility.singleLineHeight),
                minRequired, new GUIContent("MinRequired"));

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 66, rect.width, EditorGUIUtility.singleLineHeight),
                space, new GUIContent("Space"));
        }

        private void BonusDrawElemtCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = bonusList.serializedProperty.GetArrayElementAtIndex(index);

            if (GUI.Button(new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight), element.stringValue, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), new StringListSearcher(x => { str = x; bonusLastIndex = index; }));
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawGeneral();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGeneral()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Main", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);
            
                    EditorGUILayout.PropertyField(id);
                    EditorGUILayout.PropertyField(shipModelName);
                    EditorGUILayout.PropertyField(manufacturer);
                    EditorGUILayout.PropertyField(shipClass);
                    EditorGUILayout.PropertyField(shipRole);
                    
                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(sellChance);
                    EditorGUILayout.PropertyField(level);
                    EditorGUILayout.PropertyField(hullPoints);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    bonusList.DoLayoutList();

                    if(str != "")
                    {
                        modelBonus.GetArrayElementAtIndex(bonusLastIndex).stringValue = str;
                        str = "";
                    }

            
                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Gear Space", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(weaponSpace);
                    EditorGUILayout.PropertyField(equipSpace);
                    EditorGUILayout.PropertyField(cargoSpace);
                    EditorGUILayout.PropertyField(passengers);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Gear Space", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(hangarDroneSpace);
                    EditorGUILayout.PropertyField(hangarShipSpace);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    crewList.DoLayoutList();

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Parameters", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(speed);
                    EditorGUILayout.PropertyField(agility);
                    EditorGUILayout.PropertyField(mass);
                    EditorGUILayout.PropertyField(sortPower);
                    EditorGUILayout.PropertyField(rarity);
                    EditorGUILayout.PropertyField(drawScale);
                    EditorGUILayout.PropertyField(sizeScale);
            
                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Label("Reputation Requirements", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(repReq.FindPropertyRelative("factionIndex"));
                    EditorGUILayout.PropertyField(repReq.FindPropertyRelative("repNeeded"));

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Factions that use this ship", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    fractionList.DoLayoutList();

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    craftingMaterialsList.DoLayoutList();

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Other", EditorStyles.boldLabel);
                
                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(extraSurFXScale);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }
    }
}