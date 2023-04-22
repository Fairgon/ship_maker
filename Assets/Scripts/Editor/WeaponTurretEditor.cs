using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using ShipMaker.Data;
using System.Collections.Generic;

namespace ShipMaker.CEditor
{
    [CustomEditor(typeof(WeaponTurretData), true)]
    public class WeaponTurretEditor : Editor
    {
        private readonly List<string> sprites = new List<string>() { "None", "cannon_core", "dual_gun", "tri_gun", "quad_gun", "five_gun", "spinal_gun" };

        public SerializedProperty turretName;
        public SerializedProperty turretIndex;
        public SerializedProperty turretMode;
        public SerializedProperty spinalMount;

        public SerializedProperty type;
        public SerializedProperty degreesLimit;
        public SerializedProperty turnSpeed;

        public SerializedProperty spriteName;
        public SerializedProperty hasSpecialStats;
        
        public SerializedProperty totalSpace;
        public SerializedProperty maxInstalledWeapons;

        public SerializedProperty extraGunsParent;
        public SerializedProperty extraGunsParentTransform;
        public SerializedProperty extraGunsTransforms;
        public SerializedProperty extraGuns;

        //[Tooltip("Alternate extra fire, when available?")]
        public SerializedProperty alternateFire;
        public SerializedProperty iconTranslate;

        public SerializedProperty baseWeaponMods;
        public SerializedProperty tempDmgBonus;

        private ReorderableList gunsList;
        private ReorderableList dmgBonusList;

        private int spriteIndex = 0;

        void OnEnable()
        {
            turretName = serializedObject.FindProperty("turretName");
            turretIndex = serializedObject.FindProperty("turretIndex");
            turretMode = serializedObject.FindProperty("turretMode");
            spinalMount = serializedObject.FindProperty("spinalMount");

            type = serializedObject.FindProperty("type");
            degreesLimit = serializedObject.FindProperty("degreesLimit");
            turnSpeed = serializedObject.FindProperty("turnSpeed");

            spriteName = serializedObject.FindProperty("spriteName");
            hasSpecialStats = serializedObject.FindProperty("hasSpecialStats");

            totalSpace = serializedObject.FindProperty("totalSpace");
            maxInstalledWeapons = serializedObject.FindProperty("maxInstalledWeapons");

            extraGuns = serializedObject.FindProperty("extraGuns");
            extraGunsTransforms = serializedObject.FindProperty("extraGunsTransforms");
            extraGunsParent = serializedObject.FindProperty("extraGunsParent");
            extraGunsParentTransform = serializedObject.FindProperty("extraGunsParentTransform");

            alternateFire = serializedObject.FindProperty("alternateFire");
            iconTranslate = serializedObject.FindProperty("iconTranslate");

            baseWeaponMods = serializedObject.FindProperty("baseWeaponMods");
            tempDmgBonus = baseWeaponMods.FindPropertyRelative("tempDmgBonus");

            gunsList = new ReorderableList(serializedObject, extraGunsTransforms, false, true, true, true);
            gunsList.drawElementCallback = GunsDrawTCallback;
            gunsList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Extra Guns Transforms"), EditorStyles.boldLabel); };

            dmgBonusList = new ReorderableList(serializedObject, tempDmgBonus, true, true, true, true);
            dmgBonusList.drawElementCallback = DamageDrawCallback;
            dmgBonusList.elementHeight = 64;
            dmgBonusList.drawHeaderCallback = (Rect rect) => { EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), new GUIContent("Damage Bonuses"), EditorStyles.boldLabel); };

            spriteIndex = sprites.FindIndex(x => x == spriteName.stringValue);

            if(spriteName.stringValue == "")
                spriteIndex = 0;
        }

        private void GunsDrawTCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = gunsList.serializedProperty.GetArrayElementAtIndex(index);

            element.objectReferenceValue = (Transform)EditorGUI.ObjectField(
                                              new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                                              element.objectReferenceValue,
                                              typeof(Transform),
                                              true);
        }

        private void DamageDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = dmgBonusList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.LabelField(
                new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                "Bonus " + (index + 1));

            var type = element.FindPropertyRelative("type");

            type.intValue = EditorGUI.Popup(
                new Rect(rect.x, rect.y + 22, rect.width, EditorGUIUtility.singleLineHeight),
                "Type",
                type.intValue,
                typeof(DmgBonusTypes).GetEnumNames());

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 42, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("bonus"), new GUIContent("Bonus"));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawGeneral();

            serializedObject.ApplyModifiedProperties();

            iconTranslate.serializedObject.ApplyModifiedProperties();
        }

        private void DrawGeneral()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Main", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(turretIndex);
                    //EditorGUILayout.PropertyField(turretName);
                    EditorGUILayout.PropertyField(turretMode);
                    EditorGUILayout.PropertyField(spinalMount);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Rotation", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(type);
                    EditorGUILayout.PropertyField(degreesLimit);
                    EditorGUILayout.PropertyField(turnSpeed);
            
                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("View", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    spriteIndex = EditorGUILayout.Popup("Sprite Name", spriteIndex, sprites.ToArray());

                    spriteName.stringValue = spriteIndex == 0 ? "" : sprites[spriteIndex];

                    EditorGUILayout.PropertyField(hasSpecialStats);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Rotation", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(totalSpace);
                    EditorGUILayout.PropertyField(maxInstalledWeapons);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(extraGunsParentTransform, new GUIContent("Extra Guns Parent"));
                    gunsList.DoLayoutList();

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.Label("Other", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(alternateFire);
                    EditorGUILayout.PropertyField(iconTranslate);

                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                GUILayout.Label("Base Bonuses", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("dmgBonus"));

                    EditorGUILayout.Space(5f);

                    dmgBonusList.DoLayoutList();

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("criticalChanceBonus"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("criticalDamageBonus"));

                    EditorGUILayout.Space(5f);

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("armorPenBonus"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("massKiller"));

                    EditorGUILayout.Space(5f);

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("rangeBonus"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("rangeBonusPerc"));

                    EditorGUILayout.Space(5f);

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("proximityDmgMod"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("aoeBonus"));

                    EditorGUILayout.Space(5f);

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("projectileSpeedBonus"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("projectileSpeedPerc"));

                    EditorGUILayout.Space(5f);

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("autoTargeting"));
            
                    EditorGUILayout.Space(5f);

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("heatCapBonus"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("heatCapMod"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("weaponHeatMod"));
                    
                    EditorGUILayout.Space(5f);
                    EditorGUILayout.LabelField("Charge Settings");

                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("spinalMounted"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("chargeTime"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("chargedFireTime"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("chargedFireCooldown"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("fluxDamageMod"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("explodeBoostChance"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("explodeBoost"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("sizeMod"));
                    EditorGUILayout.PropertyField(baseWeaponMods.FindPropertyRelative("weaponChargedBaseDamageBoost"));
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            extraGuns.arraySize = extraGunsTransforms.arraySize;

            for (int i = 0; i < extraGuns.arraySize; ++i)
            {
                var stringElement = extraGuns.GetArrayElementAtIndex(i);
                var transformElement = extraGunsTransforms.GetArrayElementAtIndex(i);

                if (transformElement.objectReferenceValue != null)
                    stringElement.stringValue = (transformElement.objectReferenceValue as Transform).name;
                else
                    stringElement.stringValue = "";
            }

            if(extraGunsParentTransform.objectReferenceValue != null)
                extraGunsParent.stringValue = extraGunsParentTransform.objectReferenceValue.name;

            Fixer();
        }

        private void Fixer()
        {
            int index = turretIndex.intValue;

            turretName.stringValue = (target as WeaponTurretData).gameObject.name;

            (target as WeaponTurretData).transform.SetSiblingIndex(index);
        }
    }
}
