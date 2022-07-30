using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Hsiang
{
    public class SlotManager : MonoBehaviour
    {
        enum StateEnum
        {
            None,           //�L
            Roll,           //��ʤ�
            Roll_StopOK,    //��ʤ�(�w�]�w���G)
            PlayWin,        //�����ʵe
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
                SpinSlot sObj = _slot[i].GetComponent<SpinSlot>();
                switch (sObj.GetSlotState() )
                {
                    case SlotState.None:       //�L
                        break;
                    case SlotState.Roll:       //���                     
                        state = SlotState.Roll;
                        break;
                    case SlotState.Roll_StopOK://��� �w�]�w���G(�i����)
                        if( state == SlotState.None ||  state == SlotState.Roll )
                            state = SlotState.Roll_StopOK;
                        break;
                    case SlotState.Stop:       //����
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

        bool CheckSpinEnd()
        {
            return true;
        }

        bool CheckStartSpin()
        {
            return true;
        }

        bool BetClick(int v)
        {
            if (CheckStartSpin() == false)
                return false;

            _bet += v;
            if (_bet > _maxBet)
                _bet = _maxBet;

            return true;
        }

        public async void SpinClick()
        {
            if(_state == StateEnum.None)
            {
                _resultValue.Clear();
                for (int i = 0; i < _slot.Length; i++)
                {
                    int randV = (int)Random.Range(0, 9);
                    _resultValue.Add(randV);

                    SpinSlot sObj = _slot[i].GetComponent<SpinSlot>();
                    sObj.Roll(1.5f);
                    sObj.SetRollResult(randV);
                }
                _state = StateEnum.Roll;
                StartCoroutine(ThreadRollProc());
            }
            else if (_state == StateEnum.Roll_StopOK)
            {
                for (int i = 0; i < _slot.Length; i++)
                {
                    SpinSlot sObj = _slot[i].GetComponent<SpinSlot>();
                    sObj.RollStop();
                }
            }
//            _winPnl.GetComponent<SacleOutEfffect>().ShowUI(1);

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

            //����Slot ���ʵe

            for( int i = 0; i <5; i++)
            {
                _slot[i * 3 + 1].GetComponent<SpinSlot>().WinPlay();
            }           
            
            yield return new WaitForSeconds(1f);

            //��ܳӧQ���B
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