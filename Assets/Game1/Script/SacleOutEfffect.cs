using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsiang
{
    public class SacleOutEfffect: MonoBehaviour
    {
        [SerializeField] GameObject _img;
        bool _isShow;
        float _closeTime;
        void Star()
        {
        }
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

        public void ShowUI( float closeTime = 0)
        {
            if (_isShow)
                return;

            _isShow = true;
            gameObject.SetActive(true);
            _img.transform.localScale = new Vector3(0, 0, 0);
            _closeTime = closeTime;

            StartCoroutine(OnShow());
        }

        IEnumerator OnHide()
        {
            Vector3 orgScale = new Vector3(1, 1, 1);
            for (int i = 1; i <= 20; i++ )
            {
                float dt = i * 0.05f;
                dt = EasingFunction.EaseInElastic(0, 1, dt);                
                _img.transform.localScale = orgScale * (1 - dt);
                yield return new WaitForSeconds(0.05f /1.5f);
            }

            gameObject.SetActive(false);

        }

        IEnumerator OnShow()
        {
            Vector3 orgScale = new Vector3(1, 1, 1);
            for (int i = 1; i <= 20; i++)
            {
                float dt = i * 0.05f;
                //dt = EasingFunction.EaseInElastic(0, 1, dt);
                dt = EasingFunction.EaseOutElastic(0, 1, dt);
                _img.transform.localScale = orgScale * dt;
                yield return new WaitForSeconds(0.05f);
            }

            if( _closeTime > 0 )
            {
                yield return new WaitForSeconds(_closeTime);
                HideUI();
            }

        }
    }
}
