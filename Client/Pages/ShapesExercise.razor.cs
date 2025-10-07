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
    /// <summary>
    /// ShapesExercise is a Blazor component that presents students with a visual
    /// shape-identification exercise. Users must identify each shape by name,
    /// and the system records and evaluates their answers, tracking scores over time.
    /// </summary>
    public partial class ShapesExercise
    {
        #region Dependency Injection

        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }
        [Inject] protected TooltipService TooltipService { get; set; }
        [Inject] protected ContextMenuService ContextMenuService { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected SecurityService Security { get; set; }
        [Inject] protected SvgDBService SchoolDBService { get; set; }

        #endregion

        #region Private Fields

        // Holds all the shapes used in this exercise
        private List<Shape> shapes = new();

        // Indicates whether the user has submitted their answers
        private bool isSubmitted = false;

        // The user’s current score for this session
        private int score = 0;

        // The highest score previously achieved by the current student
        private int highestScore = 0;

        // The total number of questions/shapes available in this exercise
        private int maxPossibleScore = 0;

        // The unique identifier for the currently logged-in student
        private long studentId = 0;

        #endregion

        #region Lifecycle Methods

        /// <summary>
        /// Invoked when the component is initialized.
        /// Loads shapes, retrieves the current student's details,
        /// and loads their previous highest score.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            InitializeShapes();
            maxPossibleScore = shapes.Count;

            // Retrieve the logged-in student's record using their username
            var result = await SchoolDBService.GetGetStudentByAdmissionNumbers(Security.User.UserName);
            var loggedInStudent = result.Value;

            if (loggedInStudent != null && loggedInStudent.Any())
            {
                studentId = loggedInStudent.First().StudentID;
                await LoadHighestScore();
            }
        }

        #endregion

        #region Data Loading and Initialization

        /// <summary>
        /// Retrieves the student's previous exercise attempts and determines
        /// their highest score for this specific exercise (ID = 1).
        /// </summary>
        private async Task LoadHighestScore()
        {
            try
            {
                var result = await SchoolDBService.GetGetStudentExerciseResults(studentId, 1);
                var previousResults = result.Value;

                if (previousResults != null && previousResults.Any())
                {
                    highestScore = previousResults.Max(r => r.MarkObtained);
                }
            }
            catch (Exception ex)
            {
                // Log or handle unexpected errors gracefully
                Console.WriteLine($"Error loading highest score: {ex.Message}");
            }
        }

        /// <summary>
        /// Initializes the list of shapes along with their corresponding SVG drawings
        /// and the correct shape names.
        /// </summary>
        private void InitializeShapes()
        {
            shapes = new List<Shape>
            {
                new Shape { CorrectAnswer = "Cuboid", SvgContent = CreateCuboidSvg() },
                new Shape { CorrectAnswer = "Cylinder", SvgContent = CreateCylinderSvg() },
                new Shape { CorrectAnswer = "Trapezoid", SvgContent = CreateTrapezoidSvg() },
                new Shape { CorrectAnswer = "Diamond", SvgContent = CreateDiamondSvg() },
                new Shape { CorrectAnswer = "Pyramid", SvgContent = CreatePyramidSvg() },
                new Shape { CorrectAnswer = "Semicircle", SvgContent = CreateSemicircleSvg() },
                new Shape { CorrectAnswer = "Oval", SvgContent = CreateOvalSvg() },
                new Shape { CorrectAnswer = "Sphere", SvgContent = CreateSphereSvg() },
                new Shape { CorrectAnswer = "Prism", SvgContent = CreatePrismSvg() },
                new Shape { CorrectAnswer = "Parallelogram", SvgContent = CreateParallelogramSvg() },
                new Shape { CorrectAnswer = "Rhombus", SvgContent = CreateRhombusSvg() },
                new Shape { CorrectAnswer = "Scalene Triangle", SvgContent = CreateScaleneTriangleSvg() },
                new Shape { CorrectAnswer = "Right Triangle", SvgContent = CreateRightTriangleSvg() },
                new Shape { CorrectAnswer = "Pentagon", SvgContent = CreatePentagonSvg() },
                new Shape { CorrectAnswer = "Circle", SvgContent = CreateCircleSvg() },
                new Shape { CorrectAnswer = "Sector", SvgContent = CreateSectorSvg() },
                new Shape { CorrectAnswer = "Hexagon", SvgContent = CreateHexagonSvg() },
                new Shape { CorrectAnswer = "Cylinder", SvgContent = CreateCylinderSvg2() }
            };
        }

        #endregion

        #region User Interaction Methods

        /// <summary>
        /// Called when the user submits their answers.
        /// Calculates the total score, updates the database, and displays notifications.
        /// </summary>
        private async Task SubmitAnswers()
        {
            score = 0;

            // Evaluate each shape answer
            foreach (var shape in shapes)
            {
                shape.IsCorrect = string.Equals(
                    shape.UserAnswer?.Trim(),
                    shape.CorrectAnswer,
                    StringComparison.OrdinalIgnoreCase
                );

                if (shape.IsCorrect)
                    score++;
            }

            // Save the current attempt into the student exercise results table
            await SchoolDBService.InsertStudentExerciseResultWithAudits(
                studentId,
                1, // Exercise ID
                DateTime.Today.Date.ToString(),
                DateTime.Now.TimeOfDay,
                maxPossibleScore,
                score,
                Security.User.Id,
                Security.User.Id
            );

            // Update highest score and notify the user if they achieved a new record
            if (score > highestScore)
            {
                highestScore = score;

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "New High Score!",
                    Detail = $"Congratulations! You achieved a new high score of {score} out of {maxPossibleScore}!",
                    Duration = 4000
                });
            }

            isSubmitted = true;
        }

        /// <summary>
        /// Resets the exercise state, clearing user answers and resetting the UI.
        /// </summary>
        private void ResetExercise()
        {
            isSubmitted = false;
            score = 0;

            foreach (var shape in shapes)
            {
                shape.UserAnswer = string.Empty;
                shape.IsCorrect = false;
            }
        }

        /// <summary>
        /// Determines the visual border color for each shape card
        /// based on whether the answer is correct or incorrect.
        /// </summary>
        private string GetResultCardStyle(Shape shape)
        {
            var borderColor = shape.IsCorrect ? "#4caf50" : "#f44336"; // Green or Red
            return $"text-align: center; min-height: 320px; border: 3px solid {borderColor};";
        }

        #endregion

        #region SVG Generation Methods

        // Each of the following helper methods generates SVG markup strings
        // representing geometric shapes. The SVGs are used to visually render
        // shapes on the UI without relying on external assets.

        private string CreateCuboidSvg() =>
             @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M20,40 L60,20 L100,40 L100,80 L60,100 L20,80 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
            <line x1='20' y1='40' x2='20' y2='80' stroke='#2196F3' stroke-width='2'/>
            <line x1='60' y1='20' x2='60' y2='60' stroke='#2196F3' stroke-width='2'/>
            <line x1='20' y1='40' x2='60' y2='60' stroke='#2196F3' stroke-width='2'/>
            <line x1='60' y1='60' x2='100' y2='40' stroke='#2196F3' stroke-width='2'/>
            <line x1='60' y1='60' x2='60' y2='100' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateCylinderSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <ellipse cx='60' cy='30' rx='30' ry='10' fill='none' stroke='#2196F3' stroke-width='2'/>
            <line x1='30' y1='30' x2='30' y2='90' stroke='#2196F3' stroke-width='2'/>
            <line x1='90' y1='30' x2='90' y2='90' stroke='#2196F3' stroke-width='2'/>
            <ellipse cx='60' cy='90' rx='30' ry='10' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateTrapezoidSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M30,80 L90,80 L70,40 L50,40 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateDiamondSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M60,20 L90,60 L60,100 L30,60 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreatePyramidSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M60,20 L100,90 L20,90 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
            <line x1='60' y1='20' x2='40' y2='90' stroke='#2196F3' stroke-width='2'/>
            <line x1='40' y1='90' x2='80' y2='90' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateSemicircleSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M30,70 A30,30 0 0,1 90,70 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
            <line x1='30' y1='70' x2='90' y2='70' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateOvalSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <ellipse cx='60' cy='60' rx='40' ry='25' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateSphereSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <circle cx='60' cy='60' r='35' fill='none' stroke='#2196F3' stroke-width='2'/>
            <ellipse cx='60' cy='60' rx='35' ry='10' fill='none' stroke='#2196F3' stroke-width='1' opacity='0.5'/>
        </svg>";

        private string CreatePrismSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M40,30 L60,15 L80,30 L80,85 L60,100 L40,85 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
            <line x1='40' y1='30' x2='40' y2='85' stroke='#2196F3' stroke-width='2'/>
            <line x1='60' y1='15' x2='60' y2='70' stroke='#2196F3' stroke-width='2'/>
            <line x1='40' y1='30' x2='60' y2='70' stroke='#2196F3' stroke-width='2'/>
            <path d='M40,85 L60,70 L80,85' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateParallelogramSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M30,70 L50,40 L90,40 L70,70 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateRhombusSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M60,30 L85,60 L60,90 L35,60 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateScaleneTriangleSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M30,85 L70,25 L95,80 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateRightTriangleSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M30,80 L30,30 L80,80 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
            <rect x='30' y='75' width='10' height='5' fill='none' stroke='#2196F3' stroke-width='1'/>
        </svg>";

        private string CreatePentagonSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M60,25 L90,45 L80,80 L40,80 L30,45 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateCircleSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <circle cx='60' cy='60' r='35' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateSectorSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M60,60 L60,25 A35,35 0 0,1 88,48 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateHexagonSvg() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <path d='M60,25 L85,40 L85,70 L60,85 L35,70 L35,40 Z' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        private string CreateCylinderSvg2() =>
            @"<svg width='120' height='120' viewBox='0 0 120 120'>
            <ellipse cx='60' cy='35' rx='25' ry='10' fill='none' stroke='#2196F3' stroke-width='2'/>
            <line x1='35' y1='35' x2='35' y2='85' stroke='#2196F3' stroke-width='2'/>
            <line x1='85' y1='35' x2='85' y2='85' stroke='#2196F3' stroke-width='2'/>
            <ellipse cx='60' cy='85' rx='25' ry='10' fill='none' stroke='#2196F3' stroke-width='2'/>
        </svg>";

        #endregion

        #region Helper Classes

        /// <summary>
        /// Represents a single geometric shape in the exercise.
        /// Contains the SVG content, correct name, user input, and evaluation result.
        /// </summary>
        private class Shape
        {
            /// <summary> The correct name of the shape. </summary>
            public string CorrectAnswer { get; set; } = string.Empty;

            /// <summary> The answer provided by the user. </summary>
            public string UserAnswer { get; set; } = string.Empty;

            /// <summary> The SVG markup used to render this shape visually. </summary>
            public string SvgContent { get; set; } = string.Empty;

            /// <summary> Indicates whether the user's answer is correct. </summary>
            public bool IsCorrect { get; set; }
        }

        #endregion
    }
}
