using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsiang
{
    public class SpinMulSlot : MonoBehaviour
    {
        //老虎機Icon圖
        [SerializeField] Sprite[] _spSlotImg;
        [SerializeField] GameObject _prefab;
        [SerializeField] float _maxSpeed;
        [SerializeField] float _rollDelayTime;

        List<GameObject> _slotCell = new List<GameObject>();
        List<int> slotCellIDs = new List<int>();
        List<int> slotEndIDs = new List<int>();
        SlotState _state = SlotState.None;
        int _slotCount = 3;

        void Start()
        {
            _prefab.SetActive(false);
            for( int i = 0; i <_slotCount; i++)
            {                
                CreateSlotImg();
            }
        }

        public SlotState GetSlotState()        
        {
            return _state;
        }

        bool CreateSlotImg()
        {
            int id = 0;

            switch( _state)
            {
                case SlotState.None:
                    id = Random.Range(0, _spSlotImg.Length);
                    break;
                case SlotState.Roll:       //轉動
                case SlotState.Roll_StopOK://轉動 已設定結果(可停止)
                    id = Random.Range(0, _spSlotImg.Length);
                    break;
                case SlotState.Stop:       //停止
                    if (slotCellIDs.Count == 0)
                        return false;

                    id = slotCellIDs[0];
                    slotCellIDs.RemoveAt(0);

                    break;
            }
                     
            {
                float prefabSizeY = _prefab.GetComponent<RectTransform>().sizeDelta.y;
                float lastCellPosY = _prefab.transform.localPosition.y + prefabSizeY;

                if (_slotCell.Count > 0 )
                {
                    lastCellPosY = _slotCell[_slotCell.Count - 1].transform.localPosition.y;
                }

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
        public void RollStop()
        {
            if (_state != SlotState.Roll_StopOK)
                return;
            _state = SlotState.Stop;
        }

        //設定轉動結果
        public void SetRollResult(List<int> retIDs)
        {
            if (_state != SlotState.Roll)
                return;

            slotEndIDs = retIDs;
            _state = SlotState.Roll_StopOK;

            int addSlots = Random.Range(5, 10);
            slotCellIDs.Clear();
            for(int i= 0; i < addSlots; i++)
            {
                int id = Random.Range(0, _spSlotImg.Length);
                slotCellIDs.Add(id);
            }

            slotCellIDs.AddRange(retIDs);
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
            StartCoroutine(ThreadRoll(delayTime, stopTime));
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
                if (_state == SlotState.Stop && (slotCellIDs.Count ) < 3 )
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

                if (_slotCell.Count > 0 && _slotCell[0].transform.localPosition.y > prefabSizeY + _prefab.transform.localPosition.y )
                {
                    Destroy(_slotCell[0]);
                    _slotCell.RemoveAt(0);
                }

                //最少要有三個確保顯示OK
                for( int i = _slotCell.Count ; i < _slotCount+2; i++)
                    CreateSlotImg();

                float firstCellPosY = _slotCell[0].transform.localPosition.y;
                if (_slotCell.Count == _slotCount && firstCellPosY + nowSpeed * dt > _prefab.transform.localPosition.y)
                {
                    for( int i = 0; i < _slotCount;i++)
                        _slotCell[i].transform.localPosition = new Vector3(0, _prefab.transform.localPosition.y - prefabSizeY * i, 0);
                    _state = SlotState.None;
                    yield break;
                }

                for (int i = 0; i < _slotCell.Count; i++)
                {
                    Vector3 pos = _slotCell[i].transform.localPosition;
                    pos.y += nowSpeed * dt * prefabSizeY;
                    _slotCell[i].transform.localPosition = pos;
                }
                yield return null;
            }
        }

        public void WinPlay( int pos )
        {
            StartCoroutine(ThreadWinPlay(pos));
        }

        IEnumerator ThreadWinPlay(int pos)
        {           
            GameObject img = _slotCell[pos];
            GetComponent<RectMask2D>().enabled = false;
            img.GetComponent<EazeUIEffect>().PlayShrink(3, 3);            
            yield return new WaitForSeconds(1);
            img.transform.localScale = new Vector3(1f, 1f, 1f);
            GetComponent<RectMask2D>().enabled = true;
        }
    }

}
