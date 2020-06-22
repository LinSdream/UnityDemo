using LS.Helper.Timer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LS.Common;

namespace LS.DialogFrameWork
{
    /// 对话框系统，基类
    /// Time:  2020.3.27
    /// Update Time:2020.4.7
    /// 
    /// Log：使用方法
    /// 1.将脚本挂载到任意GameManager下，脚本属于单例模式，切换场景会删除
    /// 2.添加文本的资源键值对 name-path
    /// 3.调用GetTextContent方法获取要读取的文本。或者通过ImportText获取外部的文本内容
    /// 4.调用TriggerDialog方法显示对话框

    public enum TextShowState
    {
        /// <summary> 默认 </summary>
        None,
        /// <summary> 使用中 </summary>
        Read,
        /// <summary> 行读取完成 </summary>
        LineEnd,
        /// <summary> 读取结束 </summary>
        End

    }

    /// <summary> 简单的对话框系统 </summary>
    public class DialogSystem : MonoSingletonBasis<DialogSystem>
    {
        public enum TextShowType
        {
            /// <summary> 自定义 </summary>
            Manual,
            /// <summary> 行输出，逐字显示 </summary>
            ShowLineByWord,
            /// <summary> 行输出，逐行显示 </summary>
            ShowLineByLine,
            /// <summary> 全文输出，逐字显示 </summary>
            ShowContentByWord,
            /// <summary> 全文输出，逐行显示 </summary>
            ShowContentByLine
        }

        #region Public Fields

        [Header("UI组件")]
        [Tooltip("文本内容")] public Text DialogText;
        [Tooltip("对话框的背景板Image或者Panel，必须是Canvas下的gameobject")] public GameObject Dialog;
        [Tooltip("文本速度")] public float ShowTextSpeed = 0.1f;

        public TextShowType ShowType = TextShowType.ShowContentByWord;

        [HideInInspector] public TextShowState DialogState { get; protected set; }
        #endregion

        #region Protected Fields
        /// <summary> 单个文件的文本内容 </summary>
        protected WaitForSeconds _showSpeed;
        /// <summary> 当前使用的文本引用 </summary>
        protected List<string> _currentText = null;
        /// <summary> 文本资源的键值对 文本名+文本路径 </summary>
        protected Dictionary<string, string> _textAssets = new Dictionary<string, string>();
        /// <summary> 单文本内容中的当前行数 </summary>
        protected int _lineIndex;
        #endregion

        #region MonoBehaviour Callbacks and Overrider Methods

        protected override void Init()
        {
            _showSpeed = new WaitForSeconds(ShowTextSpeed);
            //初始设为true,即可以打开一个新的对话框
            DialogState = TextShowState.None;
        }

        #endregion

        #region Public Methods

        /// <summary> 添加文本资源 </summary>
        /// <param name="name">key值 文本名</param>
        /// <param name="path">value值 路径名</param>
        public void AddTextAsset(string name, string path)
        {
            if (!_textAssets.ContainsKey(name))
                _textAssets.Add(name, path);
        }

        /// <summary> 创建文本资源 </summary>
        /// <param name="dic">文本资源字典  name-path </param>
        public void CreateTextAssets(Dictionary<string, string> dic)
        {
            _textAssets = dic;
        }

        /// <summary> 获取文本 </summary>
        public bool GetTextContent(string name)
        {
            if (!_textAssets.ContainsKey(name))
                return false;
            return IOHelper.Stream_FileReadByLine(_textAssets[name], out _currentText, System.Text.Encoding.UTF8);
        }

        /// <summary> 外部带入文本内容 </summary>
        public void ImportText(List<string> data)
        {
            _currentText = data;
        }

        /// <summary> 更改文本显示速度 </summary>
        public void ChangeShowTime(float value)
        {
            _showSpeed = new WaitForSeconds(value);
        }
        #endregion

        #region Public Virtual Methods

        /// <summary> 打开文本框 </summary>
        public virtual void DialogOpen()
        {
            //可以自定义通过开关Image组件来实现，这里简单粗暴处理
            Dialog.SetActive(true);
            DialogText.text = string.Empty;
            //只有当当前文本读取完全是，才会把行数置0
            if (DialogState == TextShowState.End || DialogState == TextShowState.None)
                _lineIndex = 0;//reset vaild
            if (_currentText == null)//做一个检查，希望的是打开对话框的时候，已经是存在文本的情况下
                Debug.LogWarning("DialogSystem/DialogOpen Warning : _currentText is null");
        }

        /// <summary> 关闭文本框,同时清空当前文本引用</summary>
        public virtual void DialogClose()
        {
            Dialog.SetActive(false);
            //只有读完或者为默认状态下重置_currenterText减少io流操作
            if (DialogState == TextShowState.None || DialogState == TextShowState.End)
                _currentText = null;
        }

        /// <summary>重置</summary>
        public virtual void Restart()
        {
            _currentText = null;
            _lineIndex = 0;
            DialogState = TextShowState.None;
        }

        #endregion

        #region Virtual Trigger Dialog OverLoad Methods
        /// <summary>自定义的对话框触发，自带有DialogOpen方法，但不具有DialogClose方法</summary>
        /// <param name="trigger">触发对话条件</param>
        public virtual void TriggerDialogManual(bool trigger, TextShowType type, Action callback = null)
        {

            if (trigger && (DialogState != TextShowState.Read))
            {
                DialogOpen();
                switch (type)
                {
                    case TextShowType.ShowContentByWord:
                        StartCoroutine(WaitForTextShowAllContent(true, callback));
                        break;
                    case TextShowType.ShowLineByWord:
                        StartCoroutine(WaitForTextShowSingleLine(true, callback));
                        break;
                    case TextShowType.ShowLineByLine:
                        StartCoroutine(WaitForTextShowSingleLine(false, callback));
                        break;
                    case TextShowType.ShowContentByLine:
                        StartCoroutine(WaitForTextShowAllContent(false, callback));
                        break;
                    case TextShowType.Manual:
                        break;
                }
            }
        }

        ///<summary>自定义的对话框触发 TextShowType.Content会有DialogOpen方法，但不具有DialogClose方法</summary>
        public virtual void TriggerDialogManual(string name, bool trigger, TextShowType type, Action cb = null)
        {
            if (!GetTextContent(name))
                return;
            TriggerDialogManual(trigger, type, cb);
        }

        public virtual void TriggerDialogManual(List<string> data, bool trigger, TextShowType type, Action cb = null)
        {
            _currentText = data;
            TriggerDialogManual(trigger, type, cb);
        }

        public virtual void TriggerDialog(string name, bool trigger, Action cb = null)
        {
            TriggerDialogManual(name, trigger, ShowType, cb);
        }

        public virtual void TriggerDialog(bool trigger, Action callback = null)
        {
            TriggerDialogManual(trigger, ShowType, callback);
        }

        public virtual void TriggerDialog(List<string> data, bool trigger, Action cb = null)
        {
            TriggerDialogManual(data, trigger, ShowType, cb);
        }

        #endregion

        #region Protected Methods

        //some bugs
        /// <summary>单行文本显示 逐字显示</summary>
        protected IEnumerator WaitForTextShowSingleLine(bool word, Action cb)
        {
            DialogText.text = string.Empty;
            if (_currentText == null || _lineIndex >= _currentText.Count)
            {
                DialogState = TextShowState.None;
                yield return null;
            }
            else
            {
                DialogState = TextShowState.Read;
                if (word)
                {

                    for (int i = 0; i < _currentText[_lineIndex].Length; i++)
                    {
                        DialogText.text += _currentText[_lineIndex][i];
                        yield return _showSpeed;
                    }
                }
                else
                {
                    DialogText.text += _currentText[_lineIndex];
                    yield return _showSpeed;
                }
                _lineIndex += 1;
                if (_lineIndex >= _currentText.Count)
                    DialogState = TextShowState.End;
                else
                    DialogState = TextShowState.LineEnd;
            }
            cb?.Invoke();
        }

        /// <summary> 全文本显示 </summary>
        protected IEnumerator WaitForTextShowAllContent(bool word, Action cb)
        {
            DialogState = TextShowState.Read;
            DialogText.text = string.Empty;
            while (_lineIndex < _currentText.Count)
            {
                DialogText.text = string.Empty;
                if (word)
                {

                    for (int i = 0; i < _currentText[_lineIndex].Length; i++)
                    {
                        DialogText.text += _currentText[_lineIndex][i];
                        yield return _showSpeed;
                    }
                }
                else
                {
                    DialogText.text += _currentText[_lineIndex];
                    yield return _showSpeed;
                }
                _lineIndex += 1;
            }
            DialogState = TextShowState.End;
            _lineIndex = 0;
            cb?.Invoke();
        }
        #endregion
    }

    //#region 以后扩写
    ///// <summary> 文本块 </summary> PS:本来采用struct，但是存在判断是否为空，而值类型不能为空，所以为了方便直接采用引用类型
    //public class TextBlock
    //{
    //    /// <summary> 文本ID </summary>
    //    public int Id;
    //    /// <summary> 标签，可以为空 </summary>
    //    public string Tag;
    //    /// <summary> 文本路径 </summary>
    //    public string TextPath;
    //    /// <summary> 文本是否加载 </summary>
    //    public bool IsLoad = false;

    //    // 重写hash值，以ID作为是否相等的判断
    //    public override int GetHashCode()
    //    {
    //        return Id;
    //    }
    //}

    //public class DialogSystem : ASingletonBasis<DialogSystem>
    //{

    //    [Tooltip("对话框")] public Text TextDialog;
    //    [Tooltip("文本显示速度")] public float ShowSpeed;

    //    protected Dictionary<TextBlock, List<string>> _text;
    //    protected WaitForSeconds _showSpeed;

    //    /// <summary> 当前对话框的文本 </summary>
    //    protected List<string> _currentText = null;

    //    protected override void Init()
    //    {
    //        _showSpeed = new WaitForSeconds(ShowSpeed);
    //    }


    //    /// <summary> 添加文本块，暂时不对文本进行内容获取 </summary>
    //    public void AddText(TextBlock block)
    //    {
    //        if (!_text.ContainsKey(block))
    //        {
    //            _text.Add(block, new List<string>());
    //        }
    //    }

    //    public void LoadText(TextBlock block)
    //    {
    //        if (!_text.ContainsKey(block))
    //            return;
    //        List<string> temp;
    //        IOHelper.Stream_FileReadByLine(block.TextPath, out temp, System.Text.Encoding.UTF8);
    //        _text[block] = temp;
    //        block.IsLoad = true;
    //    }

    //    public virtual void DialogShow(int id,Action cb=null)
    //    {
    //        var textBlock = FindTextBlockByID(id);
    //        if (textBlock == null)
    //        {
    //            Debug.LogWarning("DialogSystem/DialogShow Warning : can't find the textBlock whitch id is " + id);
    //            return;
    //        }
    //        if(!_text.ContainsKey(textBlock))
    //        {
    //            Debug.LogError("DialogSystem/DialogShow Error : can't find the textBlock whitch id is " + id);
    //            return;
    //        }
    //        if (!textBlock.IsLoad)
    //        {
    //            IOHelper.Stream_FileReadByLine(textBlock.TextPath, out _currentText, System.Text.Encoding.UTF8);
    //            textBlock.IsLoad = true;
    //            _text[textBlock] = _currentText;
    //        }
    //        else
    //            _currentText = _text[textBlock];
    //        StartCoroutine(WaitForTextShowAllContent(cb));
    //    }

    //    /// <summary>
    //    /// 根据ID查找对应的文本块
    //    /// </summary>
    //    protected TextBlock FindTextBlockByID(int id)
    //    {
    //        foreach (var cell in _text)
    //        {
    //            if (cell.Key.Id == id)
    //                return cell.Key;
    //        }
    //        return null;
    //    }

    //    /// <summary> 全文本显示 </summary>
    //    IEnumerator WaitForTextShowAllContent(Action cb)
    //    {
    //        TextDialog.text = string.Empty;
    //        int lineIndex = 0;
    //        while (lineIndex < _currentText.Count)
    //        {
    //            TextDialog.text = string.Empty;
    //            for (int i = 0; i < _currentText[lineIndex].Length; i++)
    //            {
    //                TextDialog.text += _currentText[lineIndex][i];
    //                yield return _showSpeed;
    //            }
    //            lineIndex += 1;
    //        }

    //        lineIndex = 0;
    //        cb?.Invoke();
    //    }
    //}

    //#endregion

}