using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameMain
{
    //象限的枚举
    public enum Quadrant
    {
        FirstQuadrant,//第一象限
        SecondQuadrant,//第二象限
        ThirdQuadrant,//第三象限
        FourthQuadrant//第四象限
    }
    
    // 用于处理文本显示不全时，鼠标悬浮显示全部文本
    [RequireComponent(typeof(Text))]
    public class EllipsisText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public bool autoLimitLength = false;  //是否自动计算长度
        [SerializeField] public int limitLength = 10;  //在Inspector中调整你需要的最大长度
        
        private Text textComponent;
        //是否需要添加省略号
        private bool needEllipsis = false;
        private string fullText;
        [SerializeField] public RectTransform tipsTransform;//提示框的Transform

        private void Awake()
        {
            textComponent = GetComponent<Text>();
        }

        void Start()
        {
            RefreshInfo();
        }
        
        //刷新数据,用于外部调用
        public void RefreshInfo()
        {
            //如果设置了自动计算长度，获取 Text 的宽度，然后计算出最大长度
            if (autoLimitLength)
            {
                limitLength = (int)(textComponent.rectTransform.sizeDelta.x / textComponent.fontSize) - 2;//-2是为了留出省略号的位置
                Debug.Log("autoLimitLength:" + limitLength);
            }
            //获取要显示的全部文本
            fullText = textComponent.text;
            //如果文本长度超过预设限制，进行截断并添加省略号
            if (fullText.Length > limitLength)
            {
                needEllipsis = true;
                textComponent.text = fullText.Substring(0, limitLength) + "...";
            }
            else
            {
                needEllipsis = false;
            }
        }
    
        //当鼠标进入Text时触发的事件
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (needEllipsis)
            {
                ShowFullText();
            }
        }
    
        //当鼠标离开Text时触发的事件
        public void OnPointerExit(PointerEventData eventData)
        {
            if (needEllipsis)
            {
                //隐藏提示框
                tipsTransform.gameObject.SetActive(false);
            }
        }
    
        private void ShowFullText()
        {
            SetTipsPosition(GetQuadrant());
            //显示全部文本
            var text = tipsTransform.GetComponentInChildren<Text>();
            text.text = fullText;
        }
        
        
        //判断目标点在第几象限
        private Quadrant GetQuadrant()
        {
            //屏幕坐标的00点
            Vector2 zeroPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        
            var textPosition = transform.position;
            //根据x,y坐标是否大于屏幕中心点来判断象限
            switch (textPosition.x >= zeroPoint.x, textPosition.y >= zeroPoint.y)
            {
                case (true, true): return Quadrant.FirstQuadrant;
                case (false, true): return Quadrant.SecondQuadrant;
                case (false, false): return Quadrant.ThirdQuadrant;
                case (true, false): return Quadrant.FourthQuadrant;
            }
        }
        
        //根据象限设置提示框的位置
        private void SetTipsPosition(Quadrant quadrant)
        {
            switch (quadrant)
            {
                case Quadrant.FirstQuadrant:
                    tipsTransform.pivot = new Vector2(1, 1);
                    break;
                case Quadrant.SecondQuadrant:
                    tipsTransform.pivot = new Vector2(0, 1);
                    break;
                case Quadrant.ThirdQuadrant:
                    tipsTransform.pivot = new Vector2(0, 0);
                    break;
                case Quadrant.FourthQuadrant:
                    tipsTransform.pivot = new Vector2(1, 0);
                    break; 
            }
            tipsTransform.position = transform.position;
            tipsTransform.gameObject.SetActive(true);
        }
    }
}
