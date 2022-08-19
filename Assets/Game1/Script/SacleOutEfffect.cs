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
            float speed = 1.5f;
            Vector3 orgScale = new Vector3(1, 1, 1);
            for (float dt = 0; dt < 1;)
            {
                dt += Time.deltaTime * speed;
                float easeDt = EasingFunction.EaseInElastic(0, 1, dt);                
                _img.transform.localScale = orgScale * (1 - easeDt);
                yield return null;
            }

            gameObject.SetActive(false);

        }

        IEnumerator OnShow()
        {
            Vector3 orgScale = new Vector3(1, 1, 1);

            for (float dt = 0; dt < 1 ;  )
            {
                dt += Time.deltaTime;
                float easeDt = EasingFunction.EaseOutElastic(0, 1, dt);
                _img.transform.localScale = orgScale * easeDt;
                yield return null;
            }

            if( _closeTime > 0 )
            {
                yield return new WaitForSeconds(_closeTime);
                HideUI();
            }

        }
    }
}
