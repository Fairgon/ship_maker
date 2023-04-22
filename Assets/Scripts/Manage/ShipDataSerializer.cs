using UnityEngine;
using System.IO;
using ShipMaker.Data;

namespace ShipMaker.Core
{
    public class ShipDataSerializer : MonoBehaviour
    {
        public Transform targetShip = null;

        private void Awake()
        {
            string settingsData = "";
            string path = Application.dataPath + "/ShipData.json";
            string pathTurret = Application.dataPath;

            MShipModelData data = targetShip.GetComponent<MShipModelData>();

            string turretdata = "";

            foreach (WeaponTurretData w in targetShip.GetComponentsInChildren<WeaponTurretData>())
            {
                turretdata = JsonUtility.ToJson(w, true);
                File.WriteAllText(pathTurret + "/Turret_" + w.turretIndex + ".json", turretdata);
            }     

            settingsData = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, settingsData);
        }
    }
}
