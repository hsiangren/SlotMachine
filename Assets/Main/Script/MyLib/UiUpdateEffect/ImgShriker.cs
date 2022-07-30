using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hsiang
{
    //���I���ϧκu�ʯS��
    public class ImgShriker : MonoBehaviour
    {
        [SerializeField] float _maxScale;
        [SerializeField] float _minScale;
        [SerializeField] float _speed;

        float _scale;
        bool _shrinkFlag;
        Vector3 _orgScale;

        private void Start()
        {
            _scale = 1;
            _shrinkFlag = false;
            _orgScale = transform.localScale;
        }


        void Update()
        {
        
            if (_shrinkFlag)
            {
                _scale -= Time.deltaTime * _speed; 
            }
            else
            {
                _scale += Time.deltaTime * _speed;
            }

            if(_scale >= _maxScale)
            {
                _scale = _maxScale;
                _shrinkFlag = true;
            }
            else if(_scale <= _minScale)
            {
                _scale = _minScale;
                _shrinkFlag = false;
            }

            transform.localScale = _orgScale*_scale;            
        }
    }
}
