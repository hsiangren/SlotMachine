using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//¹Ï§Î«ö¶s
namespace Hsiang
{
   public class ImgButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] Image _img;
        [SerializeField] Sprite _spDn, _spUp;
        [SerializeField] AudioClip _clipDn, _clipUp;
        [SerializeField] AudioSource _audio;
        public void OnPointerDown(PointerEventData eventData)
        {
            _img.sprite = _spDn;
            if (_audio && _clipDn)
                _audio.PlayOneShot(_clipDn);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _img.sprite = _spUp;
            if (_audio && _clipUp)
                _audio.PlayOneShot(_clipUp);
        }

    }

}

