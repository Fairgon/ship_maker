using System.Collections;
using UnityEngine;

namespace ShipMaker.Data
{
    [System.Serializable]
    public class CraftMaterial
    {
        public int itemID;
        public int quantity;

        public CraftMaterial()
        {
        }

        public CraftMaterial(int newItemID, int newQuantity)
        {
            this.itemID = newItemID;
            this.quantity = newQuantity;
        }
    }
}