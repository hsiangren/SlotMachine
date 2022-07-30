using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsiang
{
    //ИsВе CheckBox 
    public class ImgCheckBoxGroup : MonoBehaviour
    {
        [SerializeField] int _checkID;
        List<ImgCheckBox> _imgCheckBoxs = new List<ImgCheckBox>();
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                ImgCheckBox cb = child.GetComponent<ImgCheckBox>();
                if (cb)
                {
                    cb.InitCheckBoxGroup();
                    _imgCheckBoxs.Add(cb);
                }
            }

            ResetInfo();

        }

        void ResetInfo()
        {
            for (int i = 0; i < _imgCheckBoxs.Count; i++)
            {
                if (i == _checkID)
                    _imgCheckBoxs[i].Check = true;
                else
                    _imgCheckBoxs[i].Check = false;

            }

            Debug.Log($"checid={_checkID}");
        }

        public void ImgButtonClick(ImgCheckBox obj)
        {
            for (int i = 0; i < _imgCheckBoxs.Count; i++)
            {
                if (_imgCheckBoxs[i] == obj)
                {
                    if (i == _checkID)
                        return;
                    CheckID = i;
                }
            }
        }

        public int CheckID
        {
            get { return _checkID; }
            set
            {
                _checkID = value;
                ResetInfo();
            }
        }
    }

}
