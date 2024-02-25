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
        /// 显示名称
        /// </summary>
        [XmlAttribute("displayName")]
        public string displayName;

        public enum ActionType
        {
            none,
            createNode
        }

        /// <summary>
        /// nodeName在创建完成后就是只读的，如果是文件夹，则显示文件夹名称，如果是文件，就显示文件名称
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
    /// 添加节点时弹窗显示规则
    /// </summary>
    public class ResourceAdditionDisplayRules
    {
        public DisplayRule btnNewNode;
        public DisplayRule btnNewFolder;
        public DisplayRule btnNewFile;
    }

    /// <summary>
    /// 每个节点的显示规则
    /// </summary>
    public class NodeItemDisplayRules
    {
        /// <summary>
        /// 名字输入框
        /// </summary>
        public DisplayRule inputName;
        /// <summary>
        /// 参数输入框
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
        /// 应用按键显示规则
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
        /// 应用按键显示规则
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
        /// 默认添加节点面板显示规则
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
        /// 默认根节点显示规则
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
        /// 默认文件节点显示规则
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
        /// 默认文件节点显示规则
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
        /// 默认文件夹节点显示规则
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