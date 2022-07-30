using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsiang
{
    public enum SlotState
    {
        None = 0,       //無
        Roll = 1,       //轉動
        Roll_StopOK = 2,//轉動 已設定結果(可停止)
        Stop = 3,       //停止
    };

    public class SpinSlot : MonoBehaviour
    {
        //老虎機Icon圖
        [SerializeField] Sprite[] _spSlotImg;
        [SerializeField] int _index;
        [SerializeField] GameObject _prefab;
        [SerializeField] float _maxSpeed;
        [SerializeField] float _rollDelayTime;

        List<GameObject> _slotCell = new List<GameObject>();
        List<int> slotCellIDs = new List<int>();
        int _slotCellIDPos = 0;
        int _orgIndex;
        SlotState _state = SlotState.None;


        void Start()
        {
            _prefab.SetActive(false);
            Index = (int)Random.Range(0, _spSlotImg.Length);
        }

        public SlotState GetSlotState()        
        {
            return _state;
        }

        bool CreateSlotImg()
        {
            if (slotCellIDs.Count == 0)
                return false;

            int id = 0;

            if (_state == SlotState.Stop)
            {
                if (_slotCellIDPos >= slotCellIDs.Count)
                    return false;
                id = slotCellIDs[_slotCellIDPos];
                _slotCellIDPos++;
            }
            else if (_state == SlotState.Roll || _state == SlotState.Roll_StopOK)
            {
                if (_slotCellIDPos >= slotCellIDs.Count)
                    _slotCellIDPos = 0;

                id = slotCellIDs[_slotCellIDPos];
                if (id == _orgIndex)
                    _slotCellIDPos = 0;
                else
                    _slotCellIDPos++;
            }

            {
                float lastCellPosY = _slotCell[_slotCell.Count - 1].transform.localPosition.y;
                float prefabSizeY = _prefab.GetComponent<RectTransform>().sizeDelta.y;
                {
                    Vector3 pos = new Vector3(0, lastCellPosY - prefabSizeY, 0);
                    GameObject newCell = Instantiate(_prefab, transform);
                    newCell.transform.localPosition = pos;
                    newCell.GetComponent<Image>().sprite = _spSlotImg[id];
                    newCell.SetActive(true);
                    _slotCell.Add(newCell);
                }
            }

            return true;
        }
        void ClearAllSlotCell()
        {
            for (int i = 0; i < _slotCell.Count; i++)
                Destroy(_slotCell[i]);
            _slotCell.Clear();
        }

        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                ClearAllSlotCell();

                GameObject newCell = Instantiate(_prefab, transform);
                newCell.GetComponent<Image>().sprite = _spSlotImg[_index];
                newCell.SetActive(true);
                _slotCell.Add(newCell);
            }
        }
        public void RollTest()
        {
            Roll(_rollDelayTime, 5);
            SetRollResult(Random.Range(0, _spSlotImg.Length));
        }
        public void RollStop()
        {
            if (_state != SlotState.Roll_StopOK)
                return;
            _state = SlotState.Stop;
        }

        //設定轉動結果
        public void SetRollResult(int endID)
        {
            if (_state != SlotState.Roll)
                return;

            _state = SlotState.Roll_StopOK;

            for (int i = 0; i < _spSlotImg.Length; i++)
            {
                int id = (i + _index + 1) % _spSlotImg.Length;
                slotCellIDs.Add(id);
                if (id == endID)
                    break;
            }
            _index = endID;
        }

        public bool Roll(float stopTime)
        {
            return Roll(_rollDelayTime, stopTime);
        }
        public bool Roll(float delayTime, float stopTime)
        {
            if (_state != SlotState.None)
                return false;

            _state = SlotState.Roll;
            slotCellIDs.Clear();
            _slotCellIDPos = 0;
            for (int i = 0; i < _spSlotImg.Length; i++)
            {
                slotCellIDs.Add((i + _index + 1) % _spSlotImg.Length);
            }

            StartCoroutine(ThreadRoll(delayTime, stopTime));
            _orgIndex = _index;
            // _index = slotCellIDs[slotCellIDs.Count - 1];
            return true;
        }

        IEnumerator ThreadRoll(float delayTime, float stopTime)
        {
            if (delayTime > 0)
                yield return new WaitForSeconds(delayTime);

            float nowSpeed = 0;
            float prefabSizeY = _prefab.GetComponent<RectTransform>().sizeDelta.y;
            while (true)
            {
                float dt = Time.deltaTime;
                float lastCellPosY = _slotCell[_slotCell.Count - 1].transform.localPosition.y;

                if (_state == SlotState.Stop && (slotCellIDs.Count - _slotCellIDPos) < 3)
                {
                    nowSpeed -= _maxSpeed * dt * 5;
                    if (nowSpeed < _maxSpeed / 10)
                        nowSpeed = _maxSpeed / 10;
                }
                else
                {
                    stopTime -= dt;
                    if (stopTime < 0 && _state == SlotState.Roll_StopOK)
                        _state = SlotState.Stop;

                    nowSpeed += _maxSpeed * dt;
                    if (nowSpeed > _maxSpeed)
                        nowSpeed = _maxSpeed;
                }

                if (_slotCell.Count > 0 && _slotCell[0].transform.localPosition.y > prefabSizeY)
                {
                    Destroy(_slotCell[0]);
                    _slotCell.RemoveAt(0);
                }

                //最少要有三個確保顯示OK
                for( int i = _slotCell.Count ; i < 3; i++)
                    CreateSlotImg();


                if (_slotCell.Count == 1 && lastCellPosY + nowSpeed * dt > 0)
                {
                    _slotCell[0].transform.localPosition = new Vector3(0, 0, 0);
                    _state = SlotState.None;
                    yield break;
                }
                /*
                if (_slotCell.Count == 0 || lastCellPosY + nowSpeed * dt > 0)
                {
                    if (CreateSlotImg() == false)
                    {
                        if (_slotCell.Count > 0)
                            _slotCell[_slotCell.Count - 1].transform.localPosition = new Vector3(0, 0, 0);

                        _state = SlotState.None;
                        yield break;
                    }
                }
                */

                for (int i = 0; i < _slotCell.Count; i++)
                {
                    Vector3 pos = _slotCell[i].transform.localPosition;
                    pos.y += nowSpeed * dt * prefabSizeY;
                    _slotCell[i].transform.localPosition = pos;
                }
                yield return null;
            }
        }

        public void WinPlay()
        {
            StartCoroutine(ThreadWinPlay());
        }

        IEnumerator ThreadWinPlay()
        {
            if (_slotCell.Count == 0)
                yield break;

            while(_slotCell.Count > 1 )
            {
                Destroy( _slotCell[0]);
                _slotCell.RemoveAt(0);
            }

            GameObject img = _slotCell[0];
            GetComponent<RectMask2D>().enabled = false;
            img.GetComponent<EazeUIEffect>().PlayShrink(3, 3);            
            yield return new WaitForSeconds(1);
            img.transform.localScale = new Vector3(1f, 1f, 1f);
            GetComponent<RectMask2D>().enabled = true;
        }
    }

}
