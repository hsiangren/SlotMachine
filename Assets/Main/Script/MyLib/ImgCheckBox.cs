using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hsiang
{
    //¹Ï§Î CheckBox
    public class ImgCheckBox : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] bool _check;
        [SerializeField] Image _img;
        [SerializeField] Sprite _spCheck, _spUnCheck;
        [SerializeField] AudioClip _clipDn;
        [SerializeField] AudioSource _audio;
        [SerializeField] GameObject[] _showObj;
        [SerializeField] GameObject[] _hideObj;
        ImgCheckBoxGroup _imgCheckBoxGroup = null;

        public bool Check
        {
            get { return _check; }
            set
            {
                _check = value;
                ResetInfo();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_imgCheckBoxGroup)
            {
                if (Check)
                    return;
                _imgCheckBoxGroup.ImgButtonClick(this);
            }
            else
            {
                Check = !Check;
            }
            _audio.PlayOneShot(_clipDn);

        }

        void ResetInfo()
        {
            if (_check)
            {
                _img.sprite = _spCheck;
                for (int i = 0; i < _showObj.Length; i++)
                    if (_showObj[i])
                        _showObj[i].SetActive(true);
                for (int i = 0; i < _hideObj.Length; i++)
                    if (_hideObj[i])
                        _hideObj[i].SetActive(false);
            }
            else
            {
                _img.sprite = _spUnCheck;
                for (int i = 0; i < _showObj.Length; i++)
                    if (_showObj[i])
                        _showObj[i].SetActive(false);
                for (int i = 0; i < _hideObj.Length; i++)
                    if (_hideObj[i])
                        _hideObj[i].SetActive(true);
            }


        }

        // Start is called before the first frame update
        void Start()
        {
            ResetInfo();
        }
        public void InitCheckBoxGroup()
        {
            _imgCheckBoxGroup = transform.parent.gameObject.GetComponent<ImgCheckBoxGroup>();
        }
    }

}
