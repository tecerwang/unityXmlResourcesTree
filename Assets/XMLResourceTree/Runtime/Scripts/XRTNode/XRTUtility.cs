using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace XMLResourceTree
{
    public static class XRTUtility
    {
        private static bool ValidateNode(this XRTNode node)
        {
            if (node == null)
            {
                Debug.LogError("XRTNode is null");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ��ȡStreamingAssets�ļ����µ�xml�����ļ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XMLNodeRoot LoadNodeTreeByStreamingAssetsRefPath(string refPath)
        {
            return XMLNodeRoot.LoadFromFile(Application.streamingAssetsPath + "/" + refPath);
        }

        /// <summary>
        /// ͨ��Node��ȡ��Ƶ·��
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetVideoSourcePath(this XRTNode node)
        {
            if (node.ValidateNode())
            {
                return node.AbsolutePath();
            }
            return null;
        }

        /// <summary>
        /// ͨ��Node��ȡ��Ƶ·��
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetAudioSourcePath(this XRTNode node)
        {
            if (node.ValidateNode())
            {
                return node.AbsolutePath();
            }
            return null;
        }

        /// <summary>
        /// ͨ��Node��ȡ�ı�
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static async Task<string> GetTextContentAsync(this XRTNode node)
        {
            if (node.ValidateNode())
            {
                var filePath = node.AbsolutePath();
                if (File.Exists(filePath))
                {
                    return await File.ReadAllTextAsync(filePath);
                }
                else
                {
                    Debug.LogError("File not found: " + filePath);
                }
            }
            return null;
        }

        /// <summary>
        /// ͨ��Node��ȡͼƬ (Texture)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static async Task<Texture> GetTextTextureAsync(this XRTNode node)
        {
            if (node.ValidateNode())
            {
                var filePath = node.AbsolutePath();
                if (File.Exists(filePath))
                {
                    byte[] fileData = await File.ReadAllBytesAsync(filePath);
                    Texture2D texture = new Texture2D(2, 2); // Adjust the size as needed
                    texture.LoadImage(fileData); // Load the image data
                    return texture; // Apply texture to a material
                }
                else
                {
                    Debug.LogError("File not found: " + filePath);
                }
            }
            return null;
        }
    }
}
