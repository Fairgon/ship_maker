using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ShipMaker.EditorData;
using System.Collections.Generic;
using System;

namespace ShipMaker.CEditor
{
    public class ShipMakerWindow : EditorWindow
    {
        public static BundleData Data;

        [MenuItem("Window/Ship Maker")]
        public static bool ShowWindow()
        {
            if (Data == null)
                Data = new BundleData();

            ShipMakerWindow window = (ShipMakerWindow)GetWindow<ShipMakerWindow>(typeof(ShipMakerWindow));
            window.titleContent = new GUIContent("Ship maker");
            window.minSize = new Vector2(500, 250);

            window.position = new Rect(Screen.width / 2, Screen.height / 2, 500, 250);

            return true;
        }
        public void OnEnable()
        {
            // Create some list of data, here simply numbers in interval [1, 1000]
            const int itemCount = 1000;
            var items = new List<string>(itemCount);
            for (int i = 1; i <= itemCount; i++)
                items.Add(i.ToString());

            // The "makeItem" function will be called as needed
            // when the ListView needs more items to render
            Func<VisualElement> makeItem = () => new Label();

            // As the user scrolls through the list, the ListView object
            // will recycle elements created by the "makeItem"
            // and invoke the "bindItem" callback to associate
            // the element with the matching data item (specified as an index in the list)
            Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i];

            // Provide the list view with an explict height for every row
            // so it can calculate how many items to actually display
            const int itemHeight = 16;

            var listView = new ListView(items, itemHeight, makeItem, bindItem);

            listView.selectionType = SelectionType.Multiple;

            listView.onItemChosen += obj => Debug.Log(obj);
            listView.onSelectionChanged += objects => Debug.Log(objects);

            listView.style.flexGrow = 1.0f;

            rootVisualElement.Add(listView);

            GenerateToolbar();
        }

        // Generating toolbar
        private void GenerateToolbar()
        {
            Toolbar toolbar = new Toolbar();

            // Save button
            Button saveBtn = new Button()
            {
                text = "Save"
            };
            saveBtn.clicked += () =>
            {
                Debug.Log(Data.Icon);
                //Save();
            };
            toolbar.Add(saveBtn);

            //// Load button
            //Button loadBtn = new Button()
            //{
            //    text = "Load"
            //};
            //loadBtn.clicked += () =>
            //{
            //    Load();
            //};
            //toolbar.Add(loadBtn);

            rootVisualElement.Add(toolbar);
        }

        private void CreateGUI()
        {
            var root = rootVisualElement;

            ObjectField spriteField = new ObjectField("Sprite")
            {
                objectType = typeof(Sprite),
                value = Data.Icon
            };

            spriteField.RegisterValueChangedCallback(evt => Data.Icon = (Sprite)evt.newValue);

            root.Add(spriteField);

            //ListView materialsList = new ListView();

            //root.Add(materialsList);

            //materialsList.makeItem = () => new ObjectField() { objectType = typeof(Material) };
            //materialsList.bindItem = (item, index) => { (item as ObjectField).value = Data.Materials[index]; };
            //materialsList.itemsSource = Data.Materials;
            //materialsList.itemHeight = 10;

        }
    }
}