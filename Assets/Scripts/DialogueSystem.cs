using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    /// <summary>
    /// 对话系统
    /// </summary>
    public class DialogueSystem : MonoBehaviour
    {
        public bool isOpen;
        //public string content;
        public Image image;
        public Text text;
        public Sprite[] head;

        public void Dialoge(int id,string content)
        {
            image.sprite = head[id];
            text.text = content;
        }
        public void Open()
        {
            gameObject.SetActive(true);
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }

    }
