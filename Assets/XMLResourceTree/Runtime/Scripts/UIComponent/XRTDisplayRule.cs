using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRule
{
    public DisplayRule()
    { }

    /// <summary>
    /// 显示名称
    /// </summary>
    [XmlAttribute("text")]
    public string text;
    /// <summary>
    /// 是否强制隐藏
    /// </summary>
    [XmlAttribute("force2Hide")]
    public bool force2Hide = false;

    public enum ActionType
    { 
        none,
        createNode
    }

    [XmlAttribute("actionType")]
    public ActionType actionType = ActionType.none;

    [XmlAttribute("actionParam")]
    public string actionParam;

    [XmlAttribute("placeHolder")]
    public string inputPlaceHolder;

    /// <summary>
    /// 应用按键显示规则
    /// </summary>
    public void ApplyBtnRule(Button btn)
    {
        if (btn == null)
        {
            return;
        }
        var textComp = btn.GetComponentInChildren<Text>();
        ApplyBtnRule(btn, textComp);
    }

    /// <summary>
    /// 应用按键显示规则
    /// </summary>
    public void ApplyBtnRule(Button btn, Text displayText)
    {
        if (btn == null)
        {
            return;
        }
        btn.gameObject.SetActive(btn.gameObject.activeSelf && !force2Hide);
        if (displayText != null)
        {
            displayText.text = text;
        }
    }

    public void ApplyInputField(InputField input)
    {
        if (input == null)
        {
            return;
        }
        input.gameObject.SetActive(input.gameObject.activeSelf && !force2Hide);
        if (!string.IsNullOrEmpty(inputPlaceHolder) && input.placeholder != null && input.placeholder is Text)
        {
            (input.placeholder as Text).text = inputPlaceHolder;
        }
    }
}

/// <summary>
/// style sheet of xml
/// </summary>
public class XRTNodeDisplayRule
{
    public string ruleName = "DefaultDisplayRule";
    public ResourceAdditionDisplayRules resourceAddition;
    public NodeItemDisplayRules nodeDisplay;

    private XRTNodeDisplayRule()
    { 
    
    }

    /// <summary>
    /// 默认添加节点面板显示规则
    /// </summary>
    private static ResourceAdditionDisplayRules DefaultAddtionDisplayRule = new ResourceAdditionDisplayRules() 
    {
        btnNewNode = new DisplayRule()
        {
            text = "Add new node",
            actionType = DisplayRule.ActionType.createNode,
            actionParam = "DefaultNodeRule"
        },
        btnNewFile = new DisplayRule()
        {
            text = "Add new file",
            actionType = DisplayRule.ActionType.createNode,
            actionParam = "DefaultFileRule"
        },
        btnNewFolder = new DisplayRule()
        {
            text = "Add new folder",
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
                inputPlaceHolder = "please input name"
            },
            inputParams = new DisplayRule()
            {
                inputPlaceHolder = "input paramters if it is need"
            },
            btnSelectFile = new DisplayRule()
            {
                force2Hide = true
            },
            btnPreview = new DisplayRule()
            {
                
            },
            btnSave = new DisplayRule()
            {
                
            },
            btnAddNode = new DisplayRule()
            {
                
            },
            btnRemoveNode = new DisplayRule()
            {
               force2Hide = true
            }
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
            btnSelectFile = new DisplayRule()
            {
                force2Hide = true
            },
            btnPreview = new DisplayRule()
            {
                force2Hide = true
            },
            btnSave = new DisplayRule()
            {
                force2Hide = true
            },
            btnAddNode = new DisplayRule()
            {

            },
            btnRemoveNode = new DisplayRule()
            {

            }
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
                inputPlaceHolder = "please input name"
            },
            inputParams = new DisplayRule()
            {
                inputPlaceHolder = "input paramters if it is need"
            },
            btnSelectFile = new DisplayRule()
            {
                text = "select file"
            },
            btnPreview = new DisplayRule()
            {

            },
            btnSave = new DisplayRule()
            {
                force2Hide = true
            },
            btnAddNode = new DisplayRule()
            {

            },
            btnRemoveNode = new DisplayRule()
            {

            }
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
                inputPlaceHolder = "please input name"
            },
            inputParams = new DisplayRule()
            {
                inputPlaceHolder = "input paramters if it is need"
            },
            btnSelectFile = new DisplayRule()
            {
                text = "select folder"
            },
            btnPreview = new DisplayRule()
            {

            },
            btnSave = new DisplayRule()
            {
                force2Hide = true
            },
            btnAddNode = new DisplayRule()
            {

            },
            btnRemoveNode = new DisplayRule()
            {

            }
        }
    };

    public void SaveToFile(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(XRTNodeDisplayRule));
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static XRTNodeDisplayRule ReadFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(XRTNodeDisplayRule));
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                return serializer.Deserialize(stream) as XRTNodeDisplayRule;
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
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