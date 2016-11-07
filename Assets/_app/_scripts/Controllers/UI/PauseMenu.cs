﻿using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace EA4S
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu I;

        [Header("Buttons")]
        public MenuButton BtPause;
        public MenuButton BtExit, BtRestart, BtMusic, BtFx, BtResume;
        [Header("Other")]
        public GameObject PauseMenuContainer;
        public Image MenuBg;
        public RectTransform SubButtonsContainer;
        public RectTransform Logo;

        public bool IsMenuOpen { get; private set; }

        MenuButton[] menuBts;
        float timeScaleAtMenuOpen = 1;
        Sequence openMenuTween;
        Tween logoBobTween;

        void Awake()
        {
            I = this;
        }

        void Start()
        {
            menuBts = PauseMenuContainer.GetComponentsInChildren<MenuButton>(true);

            // Tweens - Logo bobbing
            logoBobTween = Logo.DOAnchorPosY(16, 0.6f).SetRelative().SetUpdate(true).SetAutoKill(false).Pause()
                .SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
            logoBobTween.OnRewind(() => logoBobTween.SetEase(Ease.OutQuad))
                .OnStepComplete(() => logoBobTween.SetEase(Ease.InOutQuad));
            logoBobTween.ForceInit();
            // Tweens - menu
            CanvasGroup[] cgButtons = new CanvasGroup[menuBts.Length];
            for (int i = 0; i < menuBts.Length; i++)
                cgButtons[i] = menuBts[i].GetComponent<CanvasGroup>();
            openMenuTween = DOTween.Sequence().SetUpdate(true).SetAutoKill(false).Pause()
                .OnPlay(() => PauseMenuContainer.SetActive(true))
                .OnRewind(() => {
                    PauseMenuContainer.SetActive(false);
                    logoBobTween.Rewind();
                });
            openMenuTween.Append(MenuBg.DOFade(0, 0.5f).From())
                .Join(Logo.DOAnchorPosY(750f, 0.4f).From().SetEase(Ease.OutQuad).OnComplete(()=> logoBobTween.Play()))
                .Join(SubButtonsContainer.DORotate(new Vector3(0, 0, 180), 0.4f).From());
            const float btDuration = 0.3f;
            for (int i = 0; i < menuBts.Length; ++i) {
                CanvasGroup cgButton = cgButtons[i];
                RectTransform rtButton = cgButton.GetComponent<RectTransform>();
                openMenuTween.Insert(i * 0.05f, rtButton.DOScale(0.0001f, btDuration).From().SetEase(Ease.OutBack));
            }

            // Deactivate pause menu
            PauseMenuContainer.SetActive(false);

            // Listeners
            BtPause.Bt.onClick.AddListener(() => OnClick(BtPause));
            foreach (MenuButton bt in menuBts) {
                MenuButton b = bt; // Redeclare to fix Unity's foreach issue with delegates
                b.Bt.onClick.AddListener(() => OnClick(b));
            }
        }

        void OnDestroy()
        {
            openMenuTween.Kill();
            logoBobTween.Kill();
            BtPause.Bt.onClick.RemoveAllListeners();
            foreach (MenuButton bt in menuBts)
                bt.Bt.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// Opens or closes the pause menu
        /// </summary>
        /// <param name="_open">If TRUE opens, otherwise closes</param>
        public void OpenMenu(bool _open)
        {
            IsMenuOpen = _open;

            // Set toggles
            BtMusic.Toggle(AudioManager.I.MusicEnabled);
            BtFx.Toggle(AppManager.Instance.GameSettings.HighQualityGfx);

            if (_open) {
                timeScaleAtMenuOpen = Time.timeScale;
                Time.timeScale = 0;
                if (AppManager.Instance.CurrentGameManagerGO != null)
                    AppManager.Instance.CurrentGameManagerGO.SendMessage("DoPause", true, SendMessageOptions.DontRequireReceiver);
                openMenuTween.timeScale = 1;
                openMenuTween.PlayForward();
                AudioManager.I.PlaySfx(Sfx.UIPauseIn);
            } else {
                Time.timeScale = timeScaleAtMenuOpen;
                logoBobTween.Pause();
                openMenuTween.timeScale = 2; // Speed up tween when going backwards
                if (AppManager.Instance.CurrentGameManagerGO != null)
                    AppManager.Instance.CurrentGameManagerGO.SendMessage("DoPause", false, SendMessageOptions.DontRequireReceiver);
                openMenuTween.PlayBackwards();
                AudioManager.I.PlaySfx(Sfx.UIPauseOut);
            }
        }

        /// <summary>
        /// Callback for button clicks
        /// </summary>
        void OnClick(MenuButton _bt)
        {
            _bt.AnimateClick();
            if (_bt == BtPause) {
                OpenMenu(!IsMenuOpen);
            } else if (!openMenuTween.IsPlaying()) { // Ignores pause menu clicks when opening/closing menu
                switch (_bt.Type) {
                    case MenuButtonType.Back: // Exit
                        OpenMenu(false);
                        AppManager.Instance.Modules.SceneModule.LoadSceneWithTransition("_Start");
                        break;
                    case MenuButtonType.MusicToggle: // Music on/off
                        AudioManager.I.ToggleMusic();
                        BtMusic.Toggle(AudioManager.I.MusicEnabled);
                        break;
                    case MenuButtonType.FxToggle: // FX on/off
                        AppManager.Instance.ToggleQualitygfx();
                        BtFx.Toggle(AppManager.Instance.GameSettings.HighQualityGfx);
                        break;
                    case MenuButtonType.Restart: // Restart
                        OpenMenu(false);
                        break;
                    case MenuButtonType.Continue: // Resume
                        OpenMenu(false);
                        break;
                }
            }
        }
    }
}
