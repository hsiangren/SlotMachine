using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hsiang
{
    public class Game2Manager : MonoBehaviour
    {
        enum StateEnum
        {
            None,           //無
            Roll,           //轉動中
            Roll_StopOK,    //轉動中(已設定結果)
            PlayWin,        //結束動畫
        }

        [SerializeField] TextMeshProUGUI _coinTxt;
        [SerializeField] TextMeshProUGUI _betTxt;
        [SerializeField] TextMeshProUGUI _winCoinTxt;
        [SerializeField] GameObject[] _slot;
        [SerializeField] GameObject _winPnl;

        [SerializeField] int _maxBet;
        [SerializeField] int _bet;
        [SerializeField] int _coin;
        [SerializeField] int _winCoin;
      
        List<int> _resultValue = new List<int>();
        StateEnum _state;
        // Start is called before the first frame update
        void Start()
        {
            _maxBet = 10;
            _bet = 1;
            _coin = 1000;
            _winCoin = 0;
            _state = StateEnum.None;
        }

        public SlotState GetSlotState()
        {
            SlotState state = SlotState.None;
            for (int i = 0; i < _slot.Length; i++)
            {
                SpinMulSlot sObj = _slot[i].GetComponent<SpinMulSlot>();
                switch (sObj.GetSlotState() )
                {
                    case SlotState.None:       //無
                        break;
                    case SlotState.Roll:       //轉動                     
                        state = SlotState.Roll;
                        break;
                    case SlotState.Roll_StopOK://轉動 已設定結果(可停止)
                        if( state == SlotState.None ||  state == SlotState.Roll )
                            state = SlotState.Roll_StopOK;
                        break;
                    case SlotState.Stop:       //停止
                        if (state == SlotState.None)
                            state = SlotState.Stop;
                        break;
                }                

            }

            return state;
        }

        // Update is called once per frame
        void Update()
        {
            //        _coinTxt.SetText(_coin.ToString());
            //        _betTxt.SetText(_bet.ToString());
        }

        bool BetClick(int v)
        {
            if (_state != StateEnum.None)
                return false;

            _bet += v;
            if (_bet > _maxBet)
                _bet = _maxBet;

            return true;
        }

        public void SpinClick()
        {
            if(_state == StateEnum.None)
            {
                _resultValue.Clear();
                for (int i = 0; i < _slot.Length; i++)
                {
                    List<int> retIds = new List<int>();
                    for(int j = 0; j <3; j++ )
                    {
                        retIds.Add(Random.Range(0, 9));
                    }

                    _resultValue.AddRange(retIds);

                    SpinMulSlot sObj = _slot[i].GetComponent<SpinMulSlot>();
                    sObj.Roll(1.5f);
                    sObj.SetRollResult(retIds);
                }
                _state = StateEnum.Roll;
                StartCoroutine(ThreadRollProc());
            }
            else if (_state == StateEnum.Roll_StopOK)
            {
                for (int i = 0; i < _slot.Length; i++)
                {
                    SpinMulSlot sObj = _slot[i].GetComponent<SpinMulSlot>();
                    sObj.RollStop();
                }
            }

        }

        public void BackClick()
        {
            if (_state != StateEnum.None)
                return;
            GameManager.instance.LoadScene((int)SceneIndexes.Main);
        }

        IEnumerator ThreadRollProc()
        {
            while(true)
            {
                if( GetSlotState() == SlotState.Roll_StopOK )
                {
                    _state = StateEnum.Roll_StopOK;
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            while (true)
            {
                if (GetSlotState() == SlotState.None)
                {
                    _state = StateEnum.PlayWin;
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            //中獎Slot 做動畫

            for( int i = 0; i < _slot.Length; i++)
            {
                _slot[i].GetComponent<SpinMulSlot>().WinPlay(1);
            }           
            
            yield return new WaitForSeconds(1f);

            //顯示勝利金額
            _winPnl.GetComponent<SacleOutEfffect>().ShowUI(0.1f);

            while (true)
            {
                if (_winPnl.activeSelf == false)
                    break;

                yield return new WaitForSeconds(0.1f);
            }

            _state = StateEnum.None;
        }
    }

}
