using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace XMLResourceTree
{
    /// <summary>
    /// xml resource node, xml �ȼ�����Դ
    /// </summary>
    public class XRTNode
    {
        /// <summary>
        /// �ڵ�����
        /// </summary>
        [XmlAttribute("name")]
        public string name;
       
        /// <summary>
        /// ���в���
        /// </summary>
        [XmlAttribute("paramaters")]
        public string paramaters;

        /// <summary>
        /// �ڵ�����
        /// </summary>
        [XmlAttribute("type")]
        public string type;

        /// <summary>
        /// ��Դ�������·�� StreamingAssets/xxx
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
        /// �ݹ鸳ֵ parentNode
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
        /// �����ļ�Ϊͬ�������������� Unity �߳�
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
                    // �Ӹ��ڵ㱣��
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
    ///// �ļ�����ȫ���������ļ���this folder is sealed node
    ///// </summary>
    //public class XRTContentsForlder : XRTNode
    //{
    //    /// <summary>
    //    /// �ļ���׺���ݹ��ˣ����� "*.png|*.mp4"
    //    /// </summary>
    //    public string extFiliter;
    //}

    /// <summary>
    /// Node ����
    /// </summary>
    public static class XRTNodeTypes
    {
        /// <summary>
        /// �ļ�
        /// </summary>
        public const string node = "node";

        /// <summary>
        /// �����ļ���
        /// </summary>        
        public const string folder = "folder";

        /// <summary>
        /// �ļ�
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
