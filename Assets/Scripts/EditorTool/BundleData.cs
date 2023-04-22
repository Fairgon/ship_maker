using System.Collections.Generic;
using UnityEngine;

namespace ShipMaker.EditorData
{
    [CreateAssetMenu(menuName = "ShipMaker/New ShipBundleData")]
    public class BundleData : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public List<Object> Materials = new List<Object>();
        public List<Object> Textures = new List<Object>();
        public List<Object> Meshes = new List<Object>();
        public GameObject ShipBlank;
        public List<Object> Turrets = new List<Object>();
        public TextAsset ShipData;
    }
}