using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hsiang
{
    //���I���ϧκu�ʯS��
    public class ImgScroller : MonoBehaviour
    {
//        [SerializeField] private RawImage _img;
        [SerializeField] private float _x, _y;

        void Update()
        {
            RawImage _img = GetComponent<RawImage>();
            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
        }
    }
}
