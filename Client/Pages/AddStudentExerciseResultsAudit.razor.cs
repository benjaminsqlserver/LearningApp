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
    public partial class AddStudentExerciseResultsAudit
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

        protected override async Task OnInitializedAsync()
        {
            studentExerciseResultsAudit = new LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit();
        }
        protected bool errorVisible;
        protected LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit studentExerciseResultsAudit;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await SvgDBService.CreateStudentExerciseResultsAudit(studentExerciseResultsAudit);
                DialogService.Close(studentExerciseResultsAudit);
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
    }
}