using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;

namespace Hsiang
{
    public class OnObjActive : MonoBehaviour
    {
        // Start is called before the first frame update
        public float _startDelayTime;
        public float _startDurationTime;
        public Vector3 _startDPos;
        public Vector3 _startScale;
        //public EasingMode _easeMode;
        public EasingFunction.Ease _easeMode;

        float _delaytime;
        float _durationTime;
        Vector3 _orgPos;
        Vector3 _orgScale;

        void Start()
        {
            if (_startDurationTime == 0)
                _startDurationTime = 0.01f;
            _delaytime = _startDelayTime;
            _durationTime = _startDurationTime;
            _orgPos = transform.position;
            _orgScale = transform.localScale;

            transform.position = _orgPos + _startDPos;
            transform.localScale = _startScale;
        }


        void OnEnable()
        {
            _delaytime = _startDelayTime;
            _durationTime = 0;
        }
        void OnDisable()
        {
            _delaytime = _startDelayTime;
            _durationTime = _startDurationTime;
            transform.position = _orgPos + _startDPos;
            transform.localScale = _startScale;

        }

        // Update is called once per frame
        void Update()
        {
            if (_durationTime >= _startDurationTime)
                return;

            if (_delaytime > 0)
            {
                _delaytime -= Time.deltaTime;
                if (_delaytime > 0)
                {
                    transform.position = _orgPos + _startDPos;
                    transform.localScale = _startScale;
                    return;
                }
                _durationTime -= _delaytime;
                _delaytime = 0;
            }
            else
            {
                _durationTime += Time.deltaTime;
            }

            if (_durationTime > _startDurationTime)
                _durationTime = _startDurationTime;

            EasingFunction.Function func = EasingFunction.GetEasingFunction(_easeMode);

            float derivativeValue = func(0f, 1f, _durationTime / _startDurationTime);

            transform.position = _orgPos + _startDPos * (1 - derivativeValue);
            {
                Vector3 dScale = _orgScale - _startScale;
                transform.localScale = _startScale + dScale * derivativeValue;
            }
        }
    }

}
