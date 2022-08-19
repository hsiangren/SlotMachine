using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsiang
{
    public interface MyMenuEffect
    {
        void ShowUI();
        void HideUI();
    }
    public class FadeEffect : MonoBehaviour, MyMenuEffect
    {
        bool _isShow;

        private void Awake()
        {
            _isShow = gameObject.activeSelf;
        }
        public void HideUI()
        {
            if (_isShow == false)
                return;
            _isShow = false;
            StartCoroutine(OnHide());
        }

        public void ShowUI()
        {
            if (_isShow)
                return;

            _isShow = true;
            gameObject.SetActive(true);
            GetComponent<CanvasGroup>().alpha = 0;
            StartCoroutine(OnShow());
        }

        IEnumerator OnHide()
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            //for (float alpha = 1f; alpha >= 0; alpha -= 0.05f)
            while (canvasGroup.alpha > 0)
            {
                if (_isShow)
                    yield break;

                canvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);

        }

        IEnumerator OnShow()
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            //for (float alpha = 0f; alpha <= 1; alpha += 0.05f)
            while (canvasGroup.alpha < 1)
            {
                if (_isShow == false)
                    yield break;

                canvasGroup.alpha += Time.deltaTime;
                yield return null;
            }

        }
    }

}
