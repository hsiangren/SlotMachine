using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsiang
{
    public class EazeUIEffect : MonoBehaviour
    {
        enum State
        {
            None,
            Run,
            OnStop,
        }
        State _OnShrinkThreadProc;
        State _OnRotateThreadProc;
        State _OnFlikerThreadProc;

        private void Start()
        {
            _OnShrinkThreadProc = State.None;
            _OnRotateThreadProc = State.None;
            _OnFlikerThreadProc = State.None;
        }

        public void PlayShrink(  int times , float speed )
        {
            StartCoroutine(ThreadShrink( times, speed));
        }

        public void PlayStrike( Vector3 maxPos, float speed)
        {
            StartCoroutine(ThreadStrike( maxPos, speed));
        }

        public void PlayRotate( int time , float speed)
        {
            StartCoroutine(ThreadRotate(time, speed));
        }

        public void PlayFliker(int time, float speed)
        {
            StartCoroutine(ThreadFliker(time, speed));
        }

        IEnumerator ThreadShrink( int times , float speed )
        {
            if(_OnShrinkThreadProc != State.None )
            {
                _OnShrinkThreadProc = State.OnStop;
                while (_OnShrinkThreadProc != State.None )
                {
                    yield return new WaitForSeconds(0.1f );
                }
            }
            _OnShrinkThreadProc = State.Run;

            Vector3 orgScale = transform.localScale;
            if (times < 0)
                times = int.MaxValue;

            for(int i = 0; i < times; i++ )
            {
                for( int j = 0; j < 10; j++ )
                {
                    if(_OnShrinkThreadProc == State.OnStop)
                    {
                        _OnShrinkThreadProc = State.None;
                        transform.localScale = orgScale;
                        yield break;
                    }

                    float dt;// = j * speed*0.1f;
                    if(j <= 5)
                        dt = j *  0.2f;
                    else
                        dt = (10-j) * 0.2f;

                    Vector3 scale = orgScale *(1+ dt * 0.2f );
                    transform.localScale = scale;

                    yield return new WaitForSeconds(0.1f/speed);
                }
            }
            _OnShrinkThreadProc = State.None;
        }

        IEnumerator ThreadStrike(Vector3 maxPos , float speed)
        {
            Vector3 orgPos = transform.localPosition;
            
            for( int i = 1; i <= 20; i++)
            {
                float angle = Mathf.PI * i / 4;
                float dt = (i+1) * 0.05f;

                float dpos = Mathf.Sin(angle)*(1-dt);
                transform.localPosition = orgPos + maxPos * dpos;
                yield return new WaitForSeconds(0.05f / speed);
            }
        }


        IEnumerator ThreadRotate(int times, float speed)
        {
            if (_OnRotateThreadProc != State.None)
            {
                _OnRotateThreadProc = State.OnStop;
                while (_OnRotateThreadProc != State.None)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            _OnRotateThreadProc = State.Run;

            Quaternion orgAngle = transform.localRotation;
            if (times < 0)
                times = int.MaxValue;

            speed = 360.0f / 20.0f / speed;

            for (int i = 0; i < times; i++)
            {
                if (_OnRotateThreadProc == State.OnStop)
                    break;

                transform.Rotate(0, 0, speed);
                yield return new WaitForSeconds(0.05f );                
            }
            transform.localRotation = orgAngle;
            _OnRotateThreadProc = State.None;
        }

        IEnumerator ThreadFliker(int times, float speed)
        {
            if (_OnFlikerThreadProc != State.None)
            {
                _OnFlikerThreadProc = State.OnStop;
                while (_OnFlikerThreadProc != State.None)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            _OnFlikerThreadProc = State.Run;
     
            if (times < 0)
                times = int.MaxValue;

            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            for (int i = 0; i < times; i++)
            {
                //for (float alpha = 0f; alpha <= 1; alpha += 0.05f)
                while (canvasGroup.alpha < 1)
                {
                    if (_OnFlikerThreadProc == State.OnStop)
                    {
                        _OnFlikerThreadProc = State.None;
                        canvasGroup.alpha = 1;
                        yield break;
                    }

                    canvasGroup.alpha += 0.05f*speed;
                    yield return new WaitForSeconds(0.05f);
                }
                while (canvasGroup.alpha > 0)
                {
                    if (_OnFlikerThreadProc == State.OnStop)
                    {
                        _OnFlikerThreadProc = State.None;
                        canvasGroup.alpha = 1;
                        yield break;
                    }

                    canvasGroup.alpha -= 0.05f*speed;
                    yield return new WaitForSeconds(0.05f);
                }
            }
            _OnFlikerThreadProc = State.None;
        }
    }
}
