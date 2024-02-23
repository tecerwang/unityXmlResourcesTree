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

            private string _configFilePath = string.Empty;

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
                else if (xrtNode.type != XRTNodeTypes.node)
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
                if (xrtNode != null && !string.IsNullOrEmpty(_configFilePath))
                {
                    xrtNode.Save2File(_configFilePath);
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
                var result = await panel.InvokePanel();
                await PopupScreen.singleton.DestoryPopup(panel.gameObject);

                if (result == ResourceAdditionOptionPanel.Result.cancel)
                {
                    return;
                }
                else
                {
                    var node = new XRTNode();
                    if (result == ResourceAdditionOptionPanel.Result.folder)
                    {
                        node.type = XRTNodeTypes.folder;
                    }
                    else if (result == ResourceAdditionOptionPanel.Result.node)
                    {
                        node.type = XRTNodeTypes.node;
                    }
                    else if (result == ResourceAdditionOptionPanel.Result.file)
                    { 
                        node.type = XRTNodeTypes.file;
                    }
                    if (xrtNode.ChildrenNode == null)
                    {
                        xrtNode.ChildrenNode = new List<XRTNode>();
                    }
                    xrtNode.ChildrenNode.Add(node);
                    node.parent = xrtNode;
                    var cinstance = GameObject.Instantiate(_nodePrefab, childrenDisplay).GetComponent<XRTNodeDisplay>();
                    cinstance.SetXRTNode(node, _nodePrefab, _configFilePath);
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

            public void SetXRTNode(XRTNode node, GameObject prefab, string configFilePath)
            {
                _nodePrefab = prefab;
                if (node != null)
                {
                    this.xrtNode = node;
                    nameInput.text = node.name;
                    paramatersInput.text = node.paramaters;
                    btnOpenFile.gameObject.SetActive(node.type != XRTNodeTypes.node);
                    // root 节点可以进行预览，点击后是定位xml文件
                    btnPreview.gameObject.SetActive(node.type != XRTNodeTypes.node || node.parent == null);
                    btnOpenFile.GetComponentInChildren<Text>().text = node.type == XRTNodeTypes.folder ? "选择文件夹" : "选择文件";
                    btnOpenFile.GetComponentInChildren<Text>().color = node.type == XRTNodeTypes.folder ? new Color(0.11f, 0.61f, 0) : new Color(0.47f, 0.61f, 1);
                    this._configFilePath = configFilePath;

                    if (node.ChildrenNode != null && node.ChildrenNode.Count > 0)
                    {
                        foreach (var cnode in node.ChildrenNode)
                        {
                            var cinstance = GameObject.Instantiate(_nodePrefab, childrenDisplay);
                            cinstance.GetComponent<XRTNodeDisplay>().SetXRTNode(cnode, _nodePrefab, configFilePath);
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
                    ResetDisplayStyle();
                }
            }

            private void ResetDisplayStyle()
            {
                btnSave.gameObject.SetActive(xrtNode.parent == null);
                btnAdd.gameObject.SetActive(xrtNode.type != XRTNodeTypes.file);
                btnDropdown.gameObject.SetActive(xrtNode.type != XRTNodeTypes.file
                    && xrtNode.ChildrenNode != null
                    && xrtNode.ChildrenNode.Count > 0);
            }
        }
    }
}
