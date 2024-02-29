﻿using Blasphemous.Framework.UI;
using UnityEngine;

namespace Blasphemous.Framework.Menus;

internal class TestMenu(string title, int priority) : ModMenu(title, priority)
{
    public override void OnShow()
    {
        Main.MenuFramework.LogWarning($"Showing {Title} menu");
    }

    public override void OnHide()
    {
        Main.MenuFramework.LogWarning($"Hiding {Title} menu");
    }

    protected internal override void CreateUI(Transform ui)
    {
        // Create ui
        UIModder.Create(new RectCreationOptions()
        {
            Name = "test",
            Parent = ui,
            XRange = new Vector2(0, 1),
            YRange = new Vector2(0, 1),
            Size = Vector2.one,
        }).AddImage(new ImageCreationOptions()
        {
            Color = new Color(1, 1, 1, 0.5f)
        });
    }
}
