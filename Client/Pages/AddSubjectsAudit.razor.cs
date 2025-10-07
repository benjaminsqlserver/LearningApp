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
    public partial class AddSubjectsAudit
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
            subjectsAudit = new LearningApp.Server.Models.SvgDB.SubjectsAudit();
        }
        protected bool errorVisible;
        protected LearningApp.Server.Models.SvgDB.SubjectsAudit subjectsAudit;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await SvgDBService.CreateSubjectsAudit(subjectsAudit);
                DialogService.Close(subjectsAudit);
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