using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XMLResourceTree
{
    [RequireComponent(typeof(Button))]
    public class DropdownBtn : MonoBehaviour
    {
        public GameObject content;
        public Transform arrow;
        private bool _isExpand = false;

        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
            content.SetActive(_isExpand);
        }

        private void OnClick()
        {
            _isExpand = !_isExpand;
            content.SetActive(_isExpand);
            arrow.localScale = new Vector3(1, _isExpand ? -1 : 1, 1);
        }

        /// <summary>
        /// 操作成功后收缩
        /// </summary>
        public void ShrinkAfterOperationSuccess()
        {
            _isExpand = false;
            content.SetActive(_isExpand);
        }
    }
}