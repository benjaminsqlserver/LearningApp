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
    public partial class Students
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

        protected IEnumerable<LearningApp.Server.Models.SvgDB.Student> students;

        protected RadzenDataGrid<LearningApp.Server.Models.SvgDB.Student> grid0;
        protected int count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await SvgDBService.GetStudents(filter: $@"(contains(StudentAdmissionNumber,""{search}"") or contains(StudentSurname,""{search}"") or contains(StudentFirstName,""{search}"") or contains(StudentOtherNames,""{search}"") or contains(StudentTown,""{search}"") or contains(StudentResidentialAddress,""{search}"") or contains(StudentPhoto,""{search}"") or contains(FatherSurname,""{search}"") or contains(FatherFirstName,""{search}"") or contains(FatherOtherNames,""{search}"") or contains(FatherTelephone1,""{search}"") or contains(FatherTelephone2,""{search}"") or contains(FatherEmailAddress,""{search}"") or contains(FatherResidentialAddress,""{search}"") or contains(MotherSurname,""{search}"") or contains(MotherFirstName,""{search}"") or contains(MotherOtherNames,""{search}"") or contains(MotherTelephone1,""{search}"") or contains(MotherTelephone2,""{search}"") or contains(MotherEmailAddress,""{search}"") or contains(MotherResidentialAddress,""{search}"") or contains(GuardianSurname,""{search}"") or contains(GuardianFirstName,""{search}"") or contains(GuardianOtherNames,""{search}"") or contains(GuardianTelephone1,""{search}"") or contains(GuardianTelephone2,""{search}"") or contains(GuardianEmailAddress,""{search}"") or contains(GuardianResidentialAddress,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                students = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Students" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddStudent>("Add Student", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<LearningApp.Server.Models.SvgDB.Student> args)
        {
            await DialogService.OpenAsync<EditStudent>("Edit Student", new Dictionary<string, object> { {"StudentID", args.Data.StudentID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, LearningApp.Server.Models.SvgDB.Student student)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await SvgDBService.DeleteStudent(studentId:student.StudentID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Student"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await SvgDBService.ExportStudentsToCSV(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "Students");
            }

            if (args == null || args.Value == "xlsx")
            {
                await SvgDBService.ExportStudentsToExcel(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "Students");
            }
        }
    }
}