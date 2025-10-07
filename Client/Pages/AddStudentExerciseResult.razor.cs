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
    public partial class AddStudentExerciseResult
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
            studentExerciseResult = new LearningApp.Server.Models.SvgDB.StudentExerciseResult();
        }
        protected bool errorVisible;
        protected LearningApp.Server.Models.SvgDB.StudentExerciseResult studentExerciseResult;

        protected IEnumerable<LearningApp.Server.Models.SvgDB.StudentExercise> studentExercisesForExerciseID;


        protected int studentExercisesForExerciseIDCount;
        protected LearningApp.Server.Models.SvgDB.StudentExercise studentExercisesForExerciseIDValue;
        protected async Task studentExercisesForExerciseIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await SvgDBService.GetStudentExercises(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(ExerciseName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                studentExercisesForExerciseID = result.Value.AsODataEnumerable();
                studentExercisesForExerciseIDCount = result.Count;

                if (!object.Equals(studentExerciseResult.ExerciseID, null))
                {
                    var valueResult = await SvgDBService.GetStudentExercises(filter: $"ExerciseID eq {studentExerciseResult.ExerciseID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        studentExercisesForExerciseIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load StudentExercise" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await SvgDBService.CreateStudentExerciseResult(studentExerciseResult);
                DialogService.Close(studentExerciseResult);
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