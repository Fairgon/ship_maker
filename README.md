# Star Valor Ship Maker

For the project, you need to install Unity 2019.4. Then download the project, extract it to a convenient location, and add it to the list using the "ADD" button in the Unity Hub.

To create your own ship, you will need a model, materials and textures for it, and an icon.

Small video tutorial:
https://youtu.be/fknpVR-17xY

#### Main

Create a new folder for your ship in the "Assets/ShipResources" path. It's not necessary, but it's easier to work with. 
Then import your resources by simply dragging and dropping them into a folder.

There is a ShipBlank prefab in the root folder of the project. Move it to the scene, and give it coordinates (0, 0, 0) in Transform.
Right click on ShipBlank (in the scene hierarchy) and click Unpack Prefab from the context menu.

All meshes should be under the Model, in the ShipBlank hierarchy.

On ShipBlank itself, you need to hang the MShipModelData script. This is the ship's basic data.
You also need to add a collider to ShipBlank. It can be BoxCollide, CapsuleCollider, SphereCollider or other.

Once you've finished customizing the ship, drag the ShipBlank from the scene to your assets folder to create its prefab.

Important:
 - Do not rename the root ShipBlank Game Object.
 - The ship icon should be a Sprite and be called "Icon".

#### Turrets

In the WeaponSlots, you must add turrets.

Turrets must have an additional object as a child in the hierarchy called "GunTip". This is the main point from where the projectile or weapon beam will fly out.

If your weapon has more than one barrel, they should be named GunTip_ + barrel number. For example "GunTip_1", "GunTip_2" and so on. The main thing to remember is that the main one should be called simply "GunTip".

If you want to add additional guns. You need to create a new object in the Model and add them there. Their main trunk must have a number. For example "GunTip_0".

Attach the WeaponTurretData script to the turret. Additional guns in the Model tab do not need this script.

#### Fixer Window

Select the "Window" tab from the menu and the "Fixer Window" from the drop-down list.

A small window will open, add your ShipBlank from the scene to the "Ship" field.

And click the "Fix GunTips Postions" button. This will set the GunTips to zero y-coordinate. It will also lower the ship's meshes if necessary.

#### Turrets Preview Window 

Select the "Window" tab from the menu and the "Turrets Preview" from the drop-down list.

You need to drag the icon of your ship into the first field, and ShipBlank from the stage into the second one.

This window will help you adjust the position of the turrets. Just change the "IconTranslate" in the "WeaponTurretData" script and see what happens.

#### Build Asset

Right click on your resources folder. Select the "Create" tab from the context menu. Then "ShipMaker" and "New ShipBundleData".

Open the window by double-clicking on the created file.

Give a name to the future asset. It is desirable that it matches the name of your folder for the resources of this ship.

Add the icon and ShipBlank prefab from your folder. Click the "Create Ship Data" button. This will create configuration files for the ship.

Right click on your resources folder. Select the "Refresh" from the context menu.

Now add all your files (Textures, Materials, Meshes and ConfigFiles) to the Ship Builder window.

Click the "Create Ship Asset" button.

If everything was done correctly, your ship file should appear in the Assets/Ships folder. Don't forget to click "Refresh" from the context menu.

#### Mod installation

In order for your ship to load into the game, you need my plugin:
https://www.nexusmods.com/starvalor/mods/21

Then copy your ship file to the folder "Star Valor\BepInEx\plugins\Ships Data".

You can also add localization files. "...\plugins\Ships Data\Localization" just see how it works on finished files. You need to specify the language, your ship id, and the name. Language names: "english", "portuguese", "german", "spanish", "french", "russian", "chinese", "vietnamese", "korean", "korean", "polish", "italian".

#### Other 

If you want to build something more complex, you can use the standard asset build logic. By marking the necessary files in the inspector and build the ships through the menu item "Asset" - "Build AssetBundles".

