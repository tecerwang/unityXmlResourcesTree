using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace XMLResourceTree
{
    public class DisplayRule
    {
        public DisplayRule()
        { }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        [XmlAttribute("displayName")]
        public string displayName;

        public enum ActionType
        {
            none,
            createNode
        }

        /// <summary>
        /// nodeName�ڴ�����ɺ����ֻ���ģ�������ļ��У�����ʾ�ļ������ƣ�������ļ�������ʾ�ļ�����
        /// </summary>
        [XmlAttribute("IsDisplayNameReadOnly")]
        public bool IsDisplayNameReadOnly = false;

        [XmlAttribute("actionType")]
        public ActionType actionType = ActionType.none;

        [XmlAttribute("actionParam")]
        public string actionParam;

        [XmlAttribute("placeHolder")]
        public string inputPlaceHolder;            
    }

    /// <summary>
    /// style sheet of xml
    /// </summary>
    public class XRTNodeDisplayRule
    {
        public string ruleName = "DefaultDisplayRule";
        public ResourceAdditionDisplayRules resourceAddition;
        public NodeItemDisplayRules nodeDisplay;

        public XRTNodeDisplayRule()
        {

        }
    }

    /// <summary>
    /// ��ӽڵ�ʱ������ʾ����
    /// </summary>
    public class ResourceAdditionDisplayRules
    {
        public DisplayRule btnNewNode;
        public DisplayRule btnNewFolder;
        public DisplayRule btnNewFile;
    }

    /// <summary>
    /// ÿ���ڵ����ʾ����
    /// </summary>
    public class NodeItemDisplayRules
    {
        /// <summary>
        /// ���������
        /// </summary>
        public DisplayRule inputName;
        /// <summary>
        /// ���������
        /// </summary>
        public DisplayRule inputParams;

        public DisplayRule btnSelectFile;

        public DisplayRule btnPreview;

        public DisplayRule btnSave;

        public DisplayRule btnAddNode;

        public DisplayRule btnRemoveNode;
    }

    public static class DisplayRuleUtility
    {

        /// <summary>
        /// Ӧ�ð�����ʾ����
        /// </summary>
        public static void ApplyBtnRule(this DisplayRule rule, Button btn)
        {
            if (rule == null)
            {
                btn.gameObject.SetActive(false);
                return;
            }

            var textComp = btn.GetComponentInChildren<Text>();
            if (textComp != null)
            {
                textComp.text = rule.displayName;
            }
        }

        /// <summary>
        /// Ӧ�ð�����ʾ����
        /// </summary>
        public static void ApplyInputFieldRule(this DisplayRule rule, InputField input)
        {
            if (rule == null)
            {
                input.gameObject.SetActive(false);
                return;
            }
            if (input.placeholder != null && input.placeholder is Text)
            {
                (input.placeholder as Text).text = rule.inputPlaceHolder;
            }
            input.interactable = !rule.IsDisplayNameReadOnly;
        }

        /// <summary>
        /// Ĭ����ӽڵ������ʾ����
        /// </summary>
        public static ResourceAdditionDisplayRules DefaultAddtionDisplayRule = new ResourceAdditionDisplayRules()
        {
            btnNewNode = new DisplayRule()
            {
                displayName = "Add new node",
                actionType = DisplayRule.ActionType.createNode,
                actionParam = "DefaultNodeRule"
            },
            btnNewFile = new DisplayRule()
            {
                displayName = "Add new file",
                actionType = DisplayRule.ActionType.createNode,
                actionParam = "DefaultFileRule"
            },
            btnNewFolder = new DisplayRule()
            {
                displayName = "Add new folder",
                actionType = DisplayRule.ActionType.createNode,
                actionParam = "DefaultFolderRule"
            }
        };


        /// <summary>
        /// Ĭ�ϸ��ڵ���ʾ����
        /// </summary>
        public static XRTNodeDisplayRule DefaultRootRule = new XRTNodeDisplayRule()
        {
            ruleName = "DefaultRootRule",
            resourceAddition = DefaultAddtionDisplayRule,
            nodeDisplay = new NodeItemDisplayRules()
            {
                inputName = new DisplayRule()
                {
                    inputPlaceHolder = "please input name",
                    IsDisplayNameReadOnly = true
                },
                inputParams = new DisplayRule()
                {
                    inputPlaceHolder = "input paramters if it is need"
                },
                btnPreview = new DisplayRule(),
                btnSave = new DisplayRule(),
                btnAddNode = new DisplayRule()
            }
        };

        /// <summary>
        /// Ĭ���ļ��ڵ���ʾ����
        /// </summary>
        public static XRTNodeDisplayRule DefaultNodeRule = new XRTNodeDisplayRule()
        {
            ruleName = "DefaultNodeRule",
            resourceAddition = DefaultAddtionDisplayRule,
            nodeDisplay = new NodeItemDisplayRules()
            {
                inputName = new DisplayRule()
                {
                    inputPlaceHolder = "please input name"
                },
                inputParams = new DisplayRule()
                {
                    inputPlaceHolder = "input paramters if it is need"
                },
                btnAddNode = new DisplayRule(),
                btnRemoveNode = new DisplayRule()
            }
        };

        /// <summary>
        /// Ĭ���ļ��ڵ���ʾ����
        /// </summary>
        public static XRTNodeDisplayRule DefaultFileRule = new XRTNodeDisplayRule()
        {
            ruleName = "DefaultFileRule",
            resourceAddition = DefaultAddtionDisplayRule,
            nodeDisplay = new NodeItemDisplayRules()
            {
                inputName = new DisplayRule()
                {
                    IsDisplayNameReadOnly = true,
                    inputPlaceHolder = "Target file is empty, please select file"
                },
                inputParams = new DisplayRule()
                {
                    inputPlaceHolder = "input paramters if it is need"
                },
                btnSelectFile = new DisplayRule()
                {
                    displayName = "select file"
                },
                btnPreview = new DisplayRule(),
                btnAddNode = new DisplayRule(),
                btnRemoveNode = new DisplayRule()
            }
        };

        /// <summary>
        /// Ĭ���ļ��нڵ���ʾ����
        /// </summary>
        public static XRTNodeDisplayRule DefaultFolderRule = new XRTNodeDisplayRule()
        {
            ruleName = "DefaultFolderRule",
            resourceAddition = DefaultAddtionDisplayRule,
            nodeDisplay = new NodeItemDisplayRules()
            {
                inputName = new DisplayRule()
                {
                    IsDisplayNameReadOnly = true,
                    inputPlaceHolder = "Target folder is empty, please select folder"
                },
                inputParams = new DisplayRule()
                {
                    inputPlaceHolder = "input paramters if it is need"
                },
                btnSelectFile = new DisplayRule()
                {
                    displayName = "select folder"
                },
                btnPreview = new DisplayRule(),
                btnAddNode = new DisplayRule(),
                btnRemoveNode = new DisplayRule()
            }
        };
    }
}