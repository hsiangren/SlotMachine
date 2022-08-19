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
                for( float dt = 0; dt < 1; )
                {
                    dt += Time.deltaTime * speed;

                    if(_OnShrinkThreadProc == State.OnStop)
                    {
                        _OnShrinkThreadProc = State.None;
                        transform.localScale = orgScale;
                        yield break;
                    }

                    float exDt;
                    if (dt < 0.5f)
                        exDt = dt;
                    else
                        exDt = (1 - dt);

                    Vector3 scale = orgScale *(1+ exDt * 0.4f );
                    transform.localScale = scale;

                    yield return null;
                }
            }
            _OnShrinkThreadProc = State.None;
        }

        IEnumerator ThreadStrike(Vector3 maxPos , float speed)
        {
            Vector3 orgPos = transform.localPosition;

            for (float dt = 0; dt < 1;)
            {
                dt += Time.deltaTime * speed;

                float angle = Mathf.PI * dt * 5;// i / 4;

                float dpos = Mathf.Sin(angle)*(1-dt);
                transform.localPosition = orgPos + maxPos * dpos;
                yield return null;
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

//            speed = 360.0f / speed;

            for (float totalTime = 0; totalTime < times; )
            {
                if (_OnRotateThreadProc == State.OnStop)
                    break;

                float dt = Time.deltaTime * speed;
                totalTime += dt;

                transform.Rotate(0, 0, 360* dt);
                yield return null;
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

                    canvasGroup.alpha += Time.deltaTime*speed;
                    yield return null;
                }
                while (canvasGroup.alpha > 0)
                {
                    if (_OnFlikerThreadProc == State.OnStop)
                    {
                        _OnFlikerThreadProc = State.None;
                        canvasGroup.alpha = 1;
                        yield break;
                    }

                    canvasGroup.alpha -= Time.deltaTime * speed;
                    yield return null;
                }
            }
            _OnFlikerThreadProc = State.None;
        }
    }
}
