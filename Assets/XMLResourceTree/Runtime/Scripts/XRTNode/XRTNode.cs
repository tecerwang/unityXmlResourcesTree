using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace XMLResourceTree
{
    /// <summary>
    /// xml resource node, xml 热加载资源
    /// </summary>
    public class XRTNode
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        [XmlAttribute("name")]
        public string name;
       
        /// <summary>
        /// 运行参数
        /// </summary>
        [XmlAttribute("paramaters")]
        public string paramaters;

        /// <summary>
        /// 节点类型
        /// </summary>
        [XmlAttribute("type")]
        public string type;

        /// <summary>
        /// 资源工程相对路径 StreamingAssets/xxx
        /// </summary>
        [XmlAttribute("path")]
        public string path;

        [XmlElement("XRTNode")]
        public List<XRTNode> ChildrenNode;

        [XmlIgnore]
        public XRTNode parent { get; set; }

        public string AbsolutePath()
        {
            return GetAbsolutePath(Application.streamingAssetsPath, path);
        }

        public static string GetRelativePath(string rootPath, string fullPath)
        {
            Uri rootUri = new Uri(rootPath);
            Uri fullUri = new Uri(fullPath);

            Uri relativeUri = rootUri.MakeRelativeUri(fullUri);

            // Uri has forward slashes, so we replace them with backslashes
            return Uri.UnescapeDataString(relativeUri.ToString()).Replace('/', '\\');
        }

        public static string GetAbsolutePath(string rootPath, string refPath)
        {
            // Convert the root path to a URI
            Uri rootUri = new Uri(rootPath);

            // Combine the root URI with the reference path
            Uri fullUri = new Uri(rootUri, refPath);

            // Get the absolute path from the combined URI
            string absolutePath = fullUri.LocalPath;

            return absolutePath;
        }

        public static XRTNode LoadNodeTreeByPath(string path)
        {
            XRTNode node = null;
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XRTNode));
                    node = (XRTNode)serializer.Deserialize(reader);
                    reader.Close();
                }
            }
            SetNodeParentValue(node);
            return node;
        }

        /// <summary>
        /// 递归赋值 parentNode
        /// </summary>
        /// <param name="node"></param>
        private static void SetNodeParentValue(XRTNode node)
        {
            if (node != null && node.ChildrenNode != null && node.ChildrenNode.Count > 0)
            {
                foreach (var cnode in node.ChildrenNode)
                {
                    cnode.parent = node;
                    SetNodeParentValue(cnode);
                }
            }
        }

        /// <summary>
        /// 保存文件为同步方法，会阻塞 Unity 线程
        /// </summary>
        public void Save2File(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("File path is null or empty. Save operation aborted.");
                return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XRTNode));
                    // 从根节点保存
                    var rootNode = this;
                    while (rootNode.parent != null)
                    {
                        rootNode = rootNode.parent;
                    }
                    serializer.Serialize(writer, rootNode);
                }

                Debug.Log("XRTNode saved successfully to: " + filePath);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error saving XRTNode to file: " + ex.Message);
            }
        }
    }

    ///// <summary>
    ///// 文件夹内全部是内容文件，this folder is sealed node
    ///// </summary>
    //public class XRTContentsForlder : XRTNode
    //{
    //    /// <summary>
    //    /// 文件后缀内容过滤，例如 "*.png|*.mp4"
    //    /// </summary>
    //    public string extFiliter;
    //}

    /// <summary>
    /// Node 类型
    /// </summary>
    public static class XRTNodeTypes
    {
        /// <summary>
        /// 文件
        /// </summary>
        public const string node = "node";

        /// <summary>
        /// 内容文件夹
        /// </summary>        
        public const string folder = "folder";

        /// <summary>
        /// 文件
        /// </summary>
        public const string file = "file";

        public static string ParseTypeByPath(string path)
        {
            var dotIndex = path.LastIndexOf(".");
            if (dotIndex > 0 && dotIndex < path.Length)
            {
                return path.Substring(dotIndex + 1, path.Length - dotIndex);
            }
            return string.Empty;
        }
    } 
}
