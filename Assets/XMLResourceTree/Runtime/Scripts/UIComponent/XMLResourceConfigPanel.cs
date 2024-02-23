using SFB;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XMLResourceTree;
using UIFramework;
using System.Xml;
using System.Threading.Tasks;
using System;

namespace XMLResourceTree
{
    namespace XMLResourceTree
    {
        public class XMLResourceConfigPanel : MonoBehaviour
        {
            public Button btnOpenFile;
            public Button btnSaveFile;
            public Button btnCreateFile;

            public GameObject nodePrefab;

            public RectTransform contentRoot;

            public Text configPathName;

            public DropdownBtn menuFileOptions;

            private XRTNode _curNode;

            private string _curXmlPath;

            private void Awake()
            {
                contentRoot.DestroyChildren();
            }

            private void Start()
            {
                btnOpenFile.onClick.AddListener(OpenFile);
                btnSaveFile.onClick.AddListener(SaveFile);
                btnCreateFile.onClick.AddListener(CreateFile);
                configPathName.text = string.Empty;
            }

            private void Update()
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(contentRoot);
            }

            private void OpenFile()
            {
                // Open file async
                StandaloneFileBrowser.OpenFilePanelAsync("Open File", Application.streamingAssetsPath, "xml", false, (string[] paths) =>
                {
                    if (paths != null && paths.Length >= 1)
                    {
                        HandleXMLConfigFile(paths.FirstOrDefault());
                    }
                });
            }
            private void SaveFile()
            {
                if (_curNode != null && !string.IsNullOrEmpty(_curXmlPath))
                {
                    _curNode.Save2File(_curXmlPath);
                }
                menuFileOptions.ShrinkAfterOperationSuccess();
            }
            private async void CreateFile()
            {
                var loader = new AssetLoaderResource<GameObject>("NewFileNameInputPopup");
                var popup = await PopupScreen.singleton.CreatePopup(loader) as NewFileNameInputPopup;
                if (popup != null)
                {
                    var fileName = await popup.WaitingForResult();
                    await PopupScreen.singleton.DestoryPopup(popup.gameObject);
                    if (string.IsNullOrEmpty(fileName))
                    {
                        menuFileOptions.ShrinkAfterOperationSuccess();
                    }
                    else
                    {
                        XRTNode node = new XRTNode()
                        {
                            type = XRTNodeTypes.rootFile,
                            name = fileName
                        };
                        var filePath = Application.streamingAssetsPath + "/" + fileName;
                        if (!filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            filePath += ".xml";
                        }
                        node.Save2File(filePath);
                        HandleXMLConfigFile(filePath);
                    }
                }
            }

            private void HandleXMLConfigFile(string xmlPath)
            {
                _curXmlPath = xmlPath;
                _curNode = XRTNode.LoadNodeTreeByPath(xmlPath);
                if (_curNode != null)
                {
                    contentRoot.DestroyChildren();
                    GameObject instanceObject = GameObject.Instantiate(nodePrefab, contentRoot);
                    instanceObject.GetComponent<XRTNodeDisplay>().SetXRTNode(_curNode, nodePrefab, xmlPath);
                    configPathName.text = xmlPath;
                }
                menuFileOptions.ShrinkAfterOperationSuccess();
            }

            private static void CreateXMLFile(string fileName)
            {

            }
        }
    }
}