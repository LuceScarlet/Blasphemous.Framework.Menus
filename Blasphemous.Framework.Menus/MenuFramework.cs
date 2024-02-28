﻿using Blasphemous.ModdingAPI;
using Blasphemous.ModdingAPI.Input;
using Gameplay.UI.Others.MenuLogic;
using UnityEngine;

namespace Blasphemous.Framework.Menus;

/// <summary>
/// Handles adding and displaying in-game menus
/// </summary>
public class MenuFramework : BlasMod
{
    internal MenuFramework() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    private MenuCollection _newGameMenus;
    private MenuCollection _loadGameMenus;
    private MenuCollection CurrentMenuCollection => _isContinue ? _loadGameMenus : _newGameMenus;
    private bool IsMenuActive => CurrentMenuCollection.IsActive;

    private bool _isContinue = false;
    private int _currentSlot = 0;

    protected override void OnAllInitialized()
    {
        _newGameMenus = new MenuCollection(MenuRegister.NewGameMenus, OnFinishMenu, OnCancelMenu);
        _loadGameMenus = new MenuCollection(MenuRegister.LoadGameMenus, OnFinishMenu, OnCancelMenu);
    }

    protected override void OnUpdate()
    {
        if (!IsMenuActive)
            return;

        if (InputHandler.GetButtonDown(ButtonCode.UISubmit))
            CurrentMenuCollection.ShowNextMenu();
        else if (InputHandler.GetButtonDown(ButtonCode.UICancel))
            CurrentMenuCollection.ShowPreviousMenu();
    }

    public bool TryStartGame(int slot, bool isContinue)
    {
        _currentSlot = slot;
        _isContinue = isContinue;

        if (CurrentMenuCollection.IsEmpty)
            return true;

        StartMenu();
        return false;
    }

    /// <summary>
    /// Whenever new game or continue is pressed, open the menus
    /// </summary>
    private void StartMenu()
    {
        SlotsMenu.gameObject.SetActive(false);
        CurrentMenuCollection.StartMenu();
    }

    /// <summary>
    /// Whenever 'A' is pressed on the final menu, actually start the game
    /// </summary>
    private void OnFinishMenu()
    {
        Menu_Play_Patch.StartGameFlag = true;
        SlotsMenu.gameObject.SetActive(true);
        SlotsMenu.OnAcceptSlots(_currentSlot);
        Menu_Play_Patch.StartGameFlag = false;
    }

    /// <summary>
    /// Whenever 'B' is pressed on the first menu, return to slots screen
    /// </summary>
    private void OnCancelMenu()
    {
        //_mainMenuCache.Value.OpenSlotMenu();
        //_mainMenuCache.Value.slotsList.SelectElement(_currentSlot);
        SlotsMenu.gameObject.SetActive(true);
    }

#if DEBUG
    /// <summary>
    /// Register test menus
    /// </summary>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        provider.RegisterNewGameMenu(new TestMenu("Testing number 1", 10));
        provider.RegisterNewGameMenu(new TestMenu("Testing number 2", 21));

        provider.RegisterLoadGameMenu(new TestMenu("Loading game...", 50));
    }
#endif

    private SelectSaveSlots x_slotsMenu;
    private SelectSaveSlots SlotsMenu
    {
        get
        {
            if (x_slotsMenu == null)
                x_slotsMenu = Object.FindObjectOfType<SelectSaveSlots>();
            return x_slotsMenu;
        }
    }
}
