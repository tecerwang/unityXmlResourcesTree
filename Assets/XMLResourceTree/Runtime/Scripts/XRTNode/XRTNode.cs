using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace XMLResourceTree
{
    /// <summary>
    /// xml �ļ��ĸ��ڵ�
    /// </summary>
    public class XMLNodeRoot
    {
        [XmlArray("rules")]
        public XRTNodeDisplayRule[] rules;
        public XRTNode node;

        [XmlIgnore]
        public string filePath { get; private set; }

        /// <summary>
        /// �����ļ�Ϊͬ�������������� Unity �߳�
        /// </summary>
        public void Save2File()
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
                    XmlSerializer serializer = new XmlSerializer(typeof(XMLNodeRoot));
                    serializer.Serialize(writer, this);
                }

                Debug.Log("XMLNodeRoot saved successfully to: " + filePath);
            }
            catch (InvalidOperationException ex)
            {
                Debug.LogError("Error saving XMLNodeRoot to file: " + ex.Message);
            }
        }

        public void CreateFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("File path is null or empty. Create operation aborted.");
                return;
            }

            if (System.IO.File.Exists(filePath))
            {
                Debug.Log("File already exist at " + filePath);
                return;
            }

            this.filePath = filePath;
            Save2File();
        }

        public static XMLNodeRoot LoadFromFile(string filePath)
        {
            XMLNodeRoot root = null;
            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(XMLNodeRoot));
                    root = (XMLNodeRoot)serializer.Deserialize(reader);
                    reader.Close();
                }
                if (root != null && root.node != null)
                {
                    root.filePath = filePath;
                    // �ȸ��Լ���ֵ
                    root.node.parent = null;
                    root.node.xmlRoot = root;
                    root.node.rule = root.rules?.Where(p => p.ruleName == root.node.ruleName).FirstOrDefault();
                    // �ݹ鸳ֵ
                    SetNodeParentValue(root.node, root);
                }
            }
            return root;
        }

        /// <summary>
        /// �ݹ鸳ֵ parentNode
        /// </summary>
        /// <param name="node"></param>
        private static void SetNodeParentValue(XRTNode node, XMLNodeRoot xmlRoot)
        {
            if (node != null && node.ChildrenNode != null && node.ChildrenNode.Count > 0)
            {
                foreach (var cnode in node.ChildrenNode)
                {
                    cnode.parent = node;
                    cnode.xmlRoot = xmlRoot;
                    cnode.rule = xmlRoot.rules?.Where(p => p.ruleName == cnode.ruleName).FirstOrDefault();
                    SetNodeParentValue(cnode, xmlRoot);
                }
            }
        }
    }

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

        /// <summary>
        /// �ڵ���ʾ����
        /// </summary>
        [XmlAttribute("ruleName")]
        public string ruleName;

        [XmlElement("XRTNode")]
        public List<XRTNode> ChildrenNode;

        /// <summary>
        /// ����ڵ����ʾ����
        /// </summary>
        [XmlIgnore]
        public XRTNodeDisplayRule rule;

        /// <summary>
        /// xml �ļ����ڵ�
        /// </summary>
        [XmlIgnore]
        public XMLNodeRoot xmlRoot;

        [XmlIgnore]
        public XRTNode parent { get; set; }

        public string AbsolutePath()
        {
            return GetAbsolutePath(Application.streamingAssetsPath, path);
        }

        /// <summary>
        /// node �ļ�����
        /// </summary>
        public enum FileType
        {
            text,
            picture,
            video,
            audio,
            unknown
        }

        /// <summary>
        /// node �ϵ��ļ����ͣ�ֻ�� file ��ʽ�� node ���ܻ�ȡ���ļ����ͣ�ͨ���ļ���׺���
        /// </summary>
        /// <returns></returns>
        public FileType GetFileType()
        {
            if (string.IsNullOrEmpty(path))
            {
                return FileType.unknown;
            }
            string extension = Path.GetExtension(path);
            switch (extension.ToLower())
            {
                case ".txt":
                case ".rtf":
                    return FileType.text;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return FileType.picture;
                case ".mp4":
                case ".avi":
                case ".wav":
                    return FileType.video;
                case ".mp3":
                    return FileType.video;

                default:
                    return FileType.unknown;

            }
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
        /// �����ļ���
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
