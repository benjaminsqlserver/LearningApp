using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace LearningApp.Client.Pages
{
    public partial class EditSubject
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public SvgDBService SvgDBService { get; set; }

        [Parameter]
        public long SubjectID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            subject = await SvgDBService.GetSubjectBySubjectId(subjectId:SubjectID);
        }
        protected bool errorVisible;
        protected LearningApp.Server.Models.SvgDB.Subject subject;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await SvgDBService.UpdateSubject(subjectId:SubjectID, subject);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(subject);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            subject = await SvgDBService.GetSubjectBySubjectId(subjectId:SubjectID);
        }
    }
}