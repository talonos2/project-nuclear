﻿// Copyright 2017-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityCommon;

namespace Naninovel.UI
{
    public class TitleTipsButton : ScriptableButton
    {
        private UIManager uiManager;

        protected override void Awake ()
        {
            base.Awake();

            uiManager = Engine.GetService<UIManager>();
        }

        protected override void Start ()
        {
            base.Start();

            var tipsUI = uiManager.GetUI<ITipsUI>();
            if (tipsUI is null || tipsUI.TipsCount == 0)
                gameObject.SetActive(false);
        }

        protected override void OnButtonClick () => uiManager.GetUI<ITipsUI>()?.Show();
    }
}
