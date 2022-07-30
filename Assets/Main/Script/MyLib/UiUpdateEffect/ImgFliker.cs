using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hsiang
{
    //讓背景圖形滾動特效
    public class ImgFliker : MonoBehaviour
    {
        [SerializeField] float _maxAlpha;
        [SerializeField] float _minAlpha;
        [SerializeField] float _speed;

        bool _addFlag;


        private void Start()
        {
            _addFlag = true;
        }


        void Update()
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            float alpha = canvasGroup.alpha;
            if (_addFlag)
            {
                alpha += Time.deltaTime * _speed;
            }
            else
            {
                alpha -= Time.deltaTime * _speed;
            }

            if (alpha >= _maxAlpha)
            {
                alpha = _maxAlpha;
                _addFlag = false;
            }
            else if (alpha <= _minAlpha)
            {
                alpha = _minAlpha;
                _addFlag = true;
            }
            canvasGroup.alpha = alpha;
        }
    }
}
