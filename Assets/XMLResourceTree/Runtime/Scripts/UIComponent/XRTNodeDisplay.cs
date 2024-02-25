using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;
using XMLResourceTree;

namespace XMLResourceTree
{
    namespace XMLResourceTree
    {
        [ExecuteAlways]
        public class XRTNodeDisplay : MonoBehaviour
        {
            public XRTNode xrtNode { get; private set; }

            private GameObject _nodePrefab;

            public InputField nameInput;
            public InputField paramatersInput;
            public Button btnOpenFile;
            public RectTransform childrenDisplay;

            public Button btnDropdown;

            public Button btnPreview;
            public Button btnSave;
            public Button btnAdd;
            public Button btnMinus;

            private RectTransform _selfRect;
            private bool _childrenExpend = false;


            // Start is called before the first frame update
            void Start()
            {
                btnOpenFile.onClick.AddListener(OnSelect);
                nameInput.onValueChanged.AddListener(OnNameInputChanged);
                paramatersInput.onValueChanged.AddListener(OnParamtersInputChanged);

                btnDropdown.onClick.AddListener(OnBtnDropdown);
                btnSave.onClick.AddListener(OnBtnSave);
                btnAdd.onClick.AddListener(() => { _ = OnBtnAdd(); });
                btnMinus.onClick.AddListener(OnBtnMinus);
                btnPreview.onClick.AddListener(OnBtnPreivew);

                childrenDisplay.gameObject.SetActive(_childrenExpend);
            }

            private void OnSelect()
            {
                if (xrtNode == null)
                {
                    return;
                }
                // Open file async
                var path = string.IsNullOrEmpty(xrtNode.path) ? string.Empty : "/" + xrtNode.path;
                if (xrtNode.type == XRTNodeTypes.folder)
                {
                    StandaloneFileBrowser.OpenFolderPanelAsync("Open Folder", path, false, (string[] paths) =>
                    {
                        if (paths != null && paths.Length >= 1)
                        {
                            xrtNode.path = XRTNode.GetRelativePath(Application.streamingAssetsPath, paths.FirstOrDefault());
                        }
                    });
                }
                else if (xrtNode.type == XRTNodeTypes.file)
                {
                    StandaloneFileBrowser.OpenFilePanelAsync("Open File", path, "", false, (string[] paths) =>
                    {
                        if (paths != null && paths.Length >= 1)
                        {
                            xrtNode.path = paths.FirstOrDefault();
                            xrtNode.path = XRTNode.GetRelativePath(Application.streamingAssetsPath, xrtNode.path);
                        }
                    });
                }
            }

            private void OnBtnDropdown()
            {
                _childrenExpend = !_childrenExpend;
                childrenDisplay.gameObject.SetActive(_childrenExpend);
            }

            private void OnBtnSave()
            {
                if (xrtNode != null)
                {
                    xrtNode.xmlRoot.Save2File();
                }
            }

            private async Task OnBtnAdd()
            {
                if (xrtNode == null)
                {
                    return;
                }
                var loader = new AssetLoaderResource<GameObject>("ResourceAdditionOptionPanel");
                var panel = await PopupScreen.singleton.CreatePopup(loader, null) as ResourceAdditionOptionPanel;
                // 如果这个 display 有显示规则更改，则应用显示规则
                if (xrtNode.rule != null)
                {
                    panel.ApplyRule(xrtNode.rule.resourceAddition);
                }
                var result = await panel.InvokePanel();
                await PopupScreen.singleton.DestoryPopup(panel.gameObject);

                if (result == ResourceAdditionOptionPanel.Result.cancel)
                {
                    return;
                }
                else
                {
                    var node = new XRTNode();
                    if (result == ResourceAdditionOptionPanel.Result.node)
                    {
                        // 按照规则创建节点规则名称
                        var rule = this.xrtNode?.rule?.resourceAddition?.btnNewNode;
                        if (rule != null && rule.actionType == DisplayRule.ActionType.createNode)
                        {
                            node.ruleName = rule.actionParam;
                        }
                        node.type = XRTNodeTypes.node;
                    }
                    else if (result == ResourceAdditionOptionPanel.Result.folder)
                    {
                        // 按照规则创建节点规则名称
                        var rule = this.xrtNode?.rule?.resourceAddition?.btnNewFolder;
                        if (rule != null && rule.actionType == DisplayRule.ActionType.createNode)
                        {
                            node.ruleName = rule.actionParam;
                        }
                        node.type = XRTNodeTypes.folder;
                    }                 
                    else if (result == ResourceAdditionOptionPanel.Result.file)
                    {
                        // 按照规则创建节点规则名称
                        var rule = this.xrtNode?.rule?.resourceAddition?.btnNewFile;
                        if (rule != null && rule.actionType == DisplayRule.ActionType.createNode)
                        {
                            node.ruleName = rule.actionParam;
                        }
                        node.type = XRTNodeTypes.file;
                    }
                    if (xrtNode.ChildrenNode == null)
                    {
                        xrtNode.ChildrenNode = new List<XRTNode>();
                    }
                    xrtNode.ChildrenNode.Add(node);
                    node.parent = xrtNode;
                    node.xmlRoot = xrtNode.xmlRoot;
                    // 添加时，规则设定
                    if (!string.IsNullOrEmpty(node.ruleName))
                    {
                        node.rule = node.xmlRoot?.rules?.Where(p => p.ruleName == node.ruleName).FirstOrDefault();
                    }
                    var cinstance = GameObject.Instantiate(_nodePrefab, childrenDisplay).GetComponent<XRTNodeDisplay>();
                    cinstance.SetXRTNode(node, _nodePrefab);
                }
            }

            private void OnBtnMinus()
            {
                if (xrtNode.parent != null)
                {
                    xrtNode.parent.ChildrenNode.Remove(xrtNode);
                    Destroy(gameObject);
                }
            }

            private void OnBtnPreivew()
            {
                var path = this.xrtNode.AbsolutePath();
                Utility.LogDebug("XRTNodeDisplay", $"Preview absolute path is {path}");
                Process.Start(path);
            }

            private void OnNameInputChanged(string value)
            {
                if (xrtNode == null)
                {
                    return;
                }
                xrtNode.name = value;
            }

            private void OnParamtersInputChanged(string value)
            {
                if (xrtNode == null)
                {
                    return;
                }
                xrtNode.paramaters = value;
            }

            public void SetXRTNode(XRTNode node, GameObject prefab)
            {
                _nodePrefab = prefab;
                if (node != null)
                {
                    this.xrtNode = node;
                    nameInput.text = node.name;
                    paramatersInput.text = node.paramaters;

                    var displayRule = node.rule?.nodeDisplay;
                    if (displayRule != null)
                    {
                        displayRule.inputName.ApplyInputField(nameInput);
                        displayRule.inputParams.ApplyInputField(paramatersInput);
                        displayRule.btnSelectFile.ApplyBtnRule(btnOpenFile);
                        displayRule.btnPreview.ApplyBtnRule(btnPreview);
                        displayRule.btnSave.ApplyBtnRule(btnSave);
                        displayRule.btnAddNode.ApplyBtnRule(btnAdd);
                        displayRule.btnRemoveNode.ApplyBtnRule(btnMinus);
                    }
                    if (node.ChildrenNode != null && node.ChildrenNode.Count > 0)
                    {
                        foreach (var cnode in node.ChildrenNode)
                        {
                            var cinstance = GameObject.Instantiate(_nodePrefab, childrenDisplay);
                            cinstance.GetComponent<XRTNodeDisplay>().SetXRTNode(cnode, _nodePrefab);
                        }
                    }
                }
            }

            private void Update()
            {
                if (_selfRect == null)
                {
                    _selfRect = GetComponent<RectTransform>();
                }
                var height = childrenDisplay.gameObject.activeSelf ? 68 + childrenDisplay.sizeDelta.y : 68;
                _selfRect.sizeDelta = new Vector2(_selfRect.sizeDelta.x, height);

                LayoutRebuilder.ForceRebuildLayoutImmediate(childrenDisplay);

                if (Application.isPlaying)
                {
                    btnDropdown.gameObject.SetActive(xrtNode.ChildrenNode != null && xrtNode.ChildrenNode.Count > 0);
                }
            }
        }
    }
}
