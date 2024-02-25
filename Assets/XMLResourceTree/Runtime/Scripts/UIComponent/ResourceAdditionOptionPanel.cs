using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramework;
using System.Threading.Tasks;

namespace XMLResourceTree
{
    public class ResourceAdditionOptionPanel : UIPopupBase
    {
        public enum Result
        {
            node,
            folder,
            file,
            cancel
        }

        public Button btnAddNodeRoot;
        public Button btnAddFolder;
        public Button btnAddFile;
        public Button btnCancel;
        private TaskCompletionSource<Result> _completeSource;

        public override string popupName => "ResourceAdditionOptionPanel";

        void OnBtnAddNode()
        {
            _completeSource?.SetResult(Result.node);
        }


        void OnBtnAddFolder()
        {
            _completeSource?.SetResult(Result.folder);
        }

        void OnBtnAddFile()
        {
            _completeSource?.SetResult(Result.file);
        }

        void OnBtnCancel()
        {
            _completeSource?.SetResult(Result.cancel);
        }

        /// <summary>
        /// 等待用户完成选择
        /// </summary>
        /// <returns></returns>
        public async Task<Result> AwaitForSelection()
        {
            _completeSource = new TaskCompletionSource<Result>();
            return await _completeSource.Task;
        }

        public void ApplyRule(ResourceAdditionDisplayRules rules)
        {
            if (rules == null)
            {
                return;
            }

            rules.btnNewNode.ApplyBtnRule(btnAddNodeRoot);
            rules.btnNewFolder.ApplyBtnRule(btnAddFolder);
            rules.btnNewFile.ApplyBtnRule(btnAddFile);
        }

        public override async Task OnPopupGoingShow()
        {
            btnAddNodeRoot.onClick.AddListener(OnBtnAddNode);
            btnAddFolder.onClick.AddListener(OnBtnAddFolder);
            btnAddFile.onClick.AddListener(OnBtnAddFile);
            btnCancel.onClick.AddListener(OnBtnCancel);

            await Task.CompletedTask;
        }

        public override async Task OnPopupShown()
        {
            await Task.CompletedTask;
        }

        public override async Task OnPopupGoingLeave()
        {
            await Task.CompletedTask;
        }

        public override async Task OnPopupHidden()
        {
            await Task.CompletedTask;
        }
    }
}
