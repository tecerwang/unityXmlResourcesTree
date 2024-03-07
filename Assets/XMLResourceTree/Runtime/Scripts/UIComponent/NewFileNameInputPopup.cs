using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace XMLResourceTree
{
    public class NewFileNameInputPopup : UIPopupBase
    {
        public InputField nameInput;

        public Button btnCreate;
        public Button btnCancel;

        private TaskCompletionSource<string> completeSource = new TaskCompletionSource<string>();

        public override string popupName => "NewFileNameInputPopup";

        protected override async Task OnPopupGoingLeave()
        {
            await Task.CompletedTask;
        }

        protected override async Task OnPopupGoingShow()
        {
            btnCreate.onClick.AddListener(BtnCreate);
            btnCancel.onClick.AddListener(BtnCancel);
            await Task.CompletedTask;
        }

        protected override async Task OnPopupHidden()
        {
            await Task.CompletedTask;
        }

        protected override async Task OnPopupShown()
        {
            await Task.CompletedTask;
        }

        public async Task<string> WaitingForResult()
        {
            return await completeSource.Task;
        }

        private void BtnCreate()
        {
            if (!string.IsNullOrEmpty(nameInput.text))
            {
                completeSource.SetResult(nameInput.text);
            }
        }

        private void BtnCancel()
        {
            completeSource.SetResult(string.Empty);
        }
    }
}
