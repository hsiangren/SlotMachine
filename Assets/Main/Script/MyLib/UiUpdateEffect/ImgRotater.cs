using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hsiang
{
    //���I���ϧκu�ʯS��
    public class ImgRotater : MonoBehaviour
    {
        //        [SerializeField] private RawImage _img;
        [SerializeField] private float _speed;

        void Update()
        {
            transform.Rotate(0, 0, _speed * Time.deltaTime);
            //            RawImage _img = GetComponent<RawImage>();
            //            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
        }
    }
}
