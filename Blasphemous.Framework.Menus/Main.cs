﻿using BepInEx;

namespace Blasphemous.Framework.Menus;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "2.2.0")]
[BepInDependency("Blasphemous.Framework.UI", "0.1.0")]
internal class Main : BaseUnityPlugin
{
    public static MenuFramework MenuFramework { get; private set; }

    private void Start()
    {
        MenuFramework = new MenuFramework();
    }
}
