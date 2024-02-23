using SFB;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XMLResourceTree;
using UIFramework;

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
        private void CreateFile()
        {
            menuFileOptions.ShrinkAfterOperationSuccess();
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
    }
}