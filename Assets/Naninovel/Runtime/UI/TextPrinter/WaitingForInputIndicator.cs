// Copyright 2017-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityCommon;
using UnityEngine;
using UnityEngine.UI;

namespace Naninovel.UI
{
    public class WaitingForInputIndicator : ScriptableUIComponent<RawImage>
    {
        [SerializeField] private Color pingColor = ColorUtils.ClearWhite;
        [SerializeField] private Color pongColor = Color.white;
        [SerializeField] private float pingPongTime = 1.5f;
        [SerializeField] private float revealTime = 0.5f;
        [SerializeField] private float hideTime = 0.05f;

        private float showTime;

        /// <summary>
        /// Reveales and sets position in world space.
        /// </summary>
        public void Show (Vector2 position)
        {
            showTime = Time.time;
            RectTransform.position = position; //Used for World space

           // RectTransform.localPosition = new  Vector3(position.x, position.y, RectTransform.localPosition.z);
           // RectTransform.anchoredPosition.Set(.5f, .5f);
            //Debug.Log("ended up here after set " + RectTransform.localPosition);
            SetIsVisibleAsync(true, revealTime).WrapAsync();
        }

        public override void Hide () => SetIsVisibleAsync(false, hideTime).WrapAsync();

        private void Update ()
        {
            if (IsVisible)
                UIComponent.color = Color.Lerp(pingColor, pongColor, Mathf.PingPong(Time.time - showTime, pingPongTime));
        }


    } 
}
