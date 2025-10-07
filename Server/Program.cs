using Radzen;
using LearningApp.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using LearningApp.Server.Data;
using Microsoft.AspNetCore.Identity;
using LearningApp.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "LearningAppTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<LearningApp.Server.SvgDBService>();
builder.Services.AddDbContext<LearningApp.Server.Data.SvgDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SvgDBConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderSvgDB = new ODataConventionModelBuilder();
    oDataBuilderSvgDB.EntitySet<LearningApp.Server.Models.SvgDB.StudentExerciseResult>("StudentExerciseResults");
    oDataBuilderSvgDB.EntitySet<LearningApp.Server.Models.SvgDB.StudentExerciseResultsAudit>("StudentExerciseResultsAudits");
    oDataBuilderSvgDB.EntitySet<LearningApp.Server.Models.SvgDB.StudentExercise>("StudentExercises");
    oDataBuilderSvgDB.EntitySet<LearningApp.Server.Models.SvgDB.Student>("Students");
    oDataBuilderSvgDB.EntitySet<LearningApp.Server.Models.SvgDB.StudentsAudit>("StudentsAudits");
    oDataBuilderSvgDB.EntitySet<LearningApp.Server.Models.SvgDB.Subject>("Subjects");
    oDataBuilderSvgDB.EntitySet<LearningApp.Server.Models.SvgDB.SubjectsAudit>("SubjectsAudits");
    var svgDBGetStudentByAdmissionNumber = oDataBuilderSvgDB.Function("GetStudentByAdmissionNumbersFunc");
    svgDBGetStudentByAdmissionNumber.Parameter<string>("StudentAdmissionNumber");
    svgDBGetStudentByAdmissionNumber.Returns<LearningApp.Server.Models.SvgDB.GetStudentByAdmissionNumber>();
    var svgDBGetStudentExerciseResult = oDataBuilderSvgDB.Function("GetStudentExerciseResultsFunc");
    svgDBGetStudentExerciseResult.Parameter<long?>("StudentID");
    svgDBGetStudentExerciseResult.Parameter<long?>("ExerciseID");
    svgDBGetStudentExerciseResult.Returns<LearningApp.Server.Models.SvgDB.GetStudentExerciseResult>();
    var svgDBLastStudentExerciseResultAuditRecord = oDataBuilderSvgDB.Function("LastStudentExerciseResultAuditRecordsFunc");
    svgDBLastStudentExerciseResultAuditRecord.Parameter<long?>("ResultID");
    svgDBLastStudentExerciseResultAuditRecord.Returns<LearningApp.Server.Models.SvgDB.LastStudentExerciseResultAuditRecord>();
    var svgDBDeleteStudentExerciseResultWithAudit = oDataBuilderSvgDB.Function("DeleteStudentExerciseResultWithAuditsFunc");
    svgDBDeleteStudentExerciseResultWithAudit.Parameter<long?>("ResultID");
    svgDBDeleteStudentExerciseResultWithAudit.Parameter<string>("ChangedBy");
    svgDBDeleteStudentExerciseResultWithAudit.Returns(typeof(int));
    var svgDBInsertStudentExerciseResultWithAudit = oDataBuilderSvgDB.Function("InsertStudentExerciseResultWithAuditsFunc");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<long?>("StudentID");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<long?>("ExerciseID");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<string>("DateTaken");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<TimeSpan?>("TimeTaken");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<int?>("MarkObtainable");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<int?>("MarkObtained");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<string>("UserID");
    svgDBInsertStudentExerciseResultWithAudit.Parameter<string>("ChangedBy");
    svgDBInsertStudentExerciseResultWithAudit.Returns(typeof(int));
    var svgDBUpdateStudentExerciseResultWithAudit = oDataBuilderSvgDB.Function("UpdateStudentExerciseResultWithAuditsFunc");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<long?>("ResultID");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<long?>("OldStudentID");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<long?>("OldExerciseID");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<string>("OldDateTaken");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<TimeSpan?>("OldTimeTaken");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<int?>("OldMarkObtainable");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<int?>("OldMarkObtained");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<string>("OldUserID");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<string>("OldChangedBy");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<long?>("NewStudentID");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<long?>("NewExerciseID");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<string>("NewDateTaken");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<TimeSpan?>("NewTimeTaken");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<int?>("NewMarkObtainable");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<int?>("NewMarkObtained");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<string>("NewUserID");
    svgDBUpdateStudentExerciseResultWithAudit.Parameter<string>("NewChangedBy");
    svgDBUpdateStudentExerciseResultWithAudit.Returns(typeof(int));
    opt.AddRouteComponents("odata/SvgDB", oDataBuilderSvgDB.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<LearningApp.Client.SvgDBService>();
builder.Services.AddHttpClient("LearningApp.Server").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false }).AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<LearningApp.Client.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SvgDBConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<AuthenticationStateProvider, LearningApp.Client.ApplicationAuthenticationStateProvider>();
var app = builder.Build();
var forwardingOptions = new ForwardedHeadersOptions()
{
    ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
};
forwardingOptions.KnownNetworks.Clear();
forwardingOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardingOptions);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseHeaderPropagation();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(LearningApp.Client._Imports).Assembly);
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Run();