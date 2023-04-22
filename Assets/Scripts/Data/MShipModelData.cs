using System.Collections.Generic;
using UnityEngine;

namespace ShipMaker.Data
{
    [System.Serializable]
    public class MShipModelData : MonoBehaviour
    {
        public int id;
        public string shipModelName;
        public TFaction manufacturer;
        public ShipClassLevel shipClass = ShipClassLevel.Yacht;
        public ShipRole shipRole;
        public List<string> modelBonus;
        public int sellChance = 100;
        [Tooltip("Ideal AI level (To have full HP on this ship)")]
        public int level = 5;
        public int hullPoints = 100;

        public int weaponSpace = 3;
        public int equipSpace = 15;
        public int cargoSpace = 25;
        public int passengers;

        [Tooltip("Space for drones, same as Equipment space")]
        public int hangarDroneSpace;
        [Tooltip("Space for ships, same as Fleet space")]
        public int hangarShipSpace;
        
        public CrewSeat[] crewSpace;
        
        public int speed = 10;
        public int agility = 10;
        public int mass = 70;
        [Tooltip("Overall power of the ship")]
        public int sortPower = 1;
        public int rarity = 1;
        [Tooltip("Ship size")]
        public float drawScale = 20f;
        public float sizeScale = 1f;

        public ReputationRequisite repReq;

        public TFaction[] factions;
        
        [Header("Blueprint Crafting:")]
        [Tooltip("Materials, if predefined. If not, will be auto generated.")]
        public List<CraftMaterial> craftingMaterials;
        
        public Vector3 extraSurFXScale = Vector3.one;
    }
}