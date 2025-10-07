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
    public partial class EditStudentExercise
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
        public long ExerciseID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            studentExercise = await SvgDBService.GetStudentExerciseByExerciseId(exerciseId:ExerciseID);
        }
        protected bool errorVisible;
        protected LearningApp.Server.Models.SvgDB.StudentExercise studentExercise;

        protected IEnumerable<LearningApp.Server.Models.SvgDB.Subject> subjectsForSubjectID;


        protected int subjectsForSubjectIDCount;
        protected LearningApp.Server.Models.SvgDB.Subject subjectsForSubjectIDValue;
        protected async Task subjectsForSubjectIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await SvgDBService.GetSubjects(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(SubjectName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                subjectsForSubjectID = result.Value.AsODataEnumerable();
                subjectsForSubjectIDCount = result.Count;

                if (!object.Equals(studentExercise.SubjectID, null))
                {
                    var valueResult = await SvgDBService.GetSubjects(filter: $"SubjectID eq {studentExercise.SubjectID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        subjectsForSubjectIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Subject" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await SvgDBService.UpdateStudentExercise(exerciseId:ExerciseID, studentExercise);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(studentExercise);
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

            studentExercise = await SvgDBService.GetStudentExerciseByExerciseId(exerciseId:ExerciseID);
        }
    }
}