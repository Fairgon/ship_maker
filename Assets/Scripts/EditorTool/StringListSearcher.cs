using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR 
using UnityEditor.Experimental.GraphView;

namespace ShipMaker.EditorData
{
    public class StringListSearcher : ScriptableObject, ISearchWindowProvider
    {
        private static BonusesData bonusesData;
        private Action<string> onSetIndexCallback;

        public StringListSearcher(Action<string> callback)
        {
            onSetIndexCallback = callback;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (bonusesData == null)
                bonusesData = JsonUtility.FromJson<BonusesData>(Resources.Load<TextAsset>("ShipBonuses").text);

            List<SearchTreeEntry> searchList = new List<SearchTreeEntry>();
            searchList.Add(new SearchTreeGroupEntry(new GUIContent("List"), 0));

            for(int i = 0; i < bonusesData.Names.Count; ++i)
            {
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(bonusesData.Names[i]));
                entry.level = 1;
                entry.userData = bonusesData.Names[i];

                searchList.Add(entry);
            }

            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            onSetIndexCallback?.Invoke((string)SearchTreeEntry.userData);

            return true;
        }
    }
}
#endif