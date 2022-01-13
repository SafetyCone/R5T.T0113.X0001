using System;
using System.Threading.Tasks;

using R5T.D0078;
using R5T.D0079;
using R5T.T0106;
using R5T.T0113;
using R5T.T0114;

using Instances = R5T.T0113.X0001.Instances;


namespace System
{
    public static class IProjectGeneratorExtensions
    {
        public static async Task CreateServiceImplementationLibraryProject(this IProjectGenerator _,
            SolutionFileContext solutionFileContext,
            IProjectFileSpecification implementationProjectFileSpecification,
            IServiceImplementationSpecification serviceImplementationSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            // Create the default service implementation project.
            await _.CreateLibraryProject(
                solutionFileContext,
                implementationProjectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardLibraryProject(
                    implementationProjectFileSpecification.Description,
                    implementationProjectFileSpecification.Documentation,
                    implementationProjectFileSpecification.DefaultNamespaceName,
                    (implementationProjectFileContext) =>
                    {
                        Instances.ProjectOperator.CreateServiceImplementation(
                            implementationProjectFileContext,
                            serviceImplementationSpecification.ImplementationNamespacedTypeName,
                            serviceImplementationSpecification.DefinitionNamespacedTypeName);

                        Instances.ProjectOperator.CreateIServiceCollectionExtensionsStub(
                            implementationProjectFileContext,
                            Instances.ProjectNameOperator.GetDefaultNamespaceNameFromProjectName(implementationProjectFileContext.Name));

                        return Task.CompletedTask;
                    }));
        }

        public static async Task CreateBasicConstructionConsoleProject(this IProjectGenerator _,
            SolutionFileContext solutionFileContext,
            IProjectFileSpecification basicConstructionConsoleProjectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<ProjectFileWithSolutionFileContext, Task> projectFileWithSolutionFileContextAction = default)
        {
            await _.CreateConsoleProject(
                solutionFileContext,
                basicConstructionConsoleProjectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardConsoleProject(
                    basicConstructionConsoleProjectFileSpecification.Description,
                    projectFileWithSolutionFileContextAction));
        }

        public static async Task CreateExtensionsLibraryProject(this IProjectGenerator _,
            SolutionFileContext solutionFileContext,
            IProjectFileSpecification extensionsProjectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            await _.CreateLibraryProject(
                solutionFileContext,
                extensionsProjectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardLibraryProject(
                    extensionsProjectFileSpecification.Description,
                    extensionsProjectFileSpecification.Documentation,
                    extensionsProjectFileSpecification.DefaultNamespaceName,
                    (implementationProjectFileContext) =>
                    {
                        // Create /Extensions, /Bases/Extensions, and /Services/Extensions
                        var extensionsDirectoryRelativePaths = new[]
                        {
                            Instances.ProjectPathsOperator.GetExtensionsDirectoryRelativePath(),
                            Instances.ProjectPathsOperator.GetBasesExtensionsDirectoryRelativePath(),
                            Instances.ProjectPathsOperator.GetServiceExtensionsDirectoryRelativePath(),
                        };

                        foreach (var extensionsDirectoryRelativePath in extensionsDirectoryRelativePaths)
                        {
                            implementationProjectFileContext.InProjectSubDirectoryPathContextSynchronous(
                            extensionsDirectoryRelativePath,
                            (extensionsDirectoryPathContext) =>
                            {
                                // Create place-holder code file to ensure the directory appears when opening Visual Studio.
                                extensionsDirectoryPathContext.InProjectSubFilePathContextSynchronous(
                                    Instances.CodeFileName.Class1(),
                                    (class1CodeFileContext) =>
                                    {
                                        Instances.CodeFileGenerator.CreateDefaultClass1(
                                            extensionsProjectFileSpecification.DefaultNamespaceName,
                                            class1CodeFileContext.FilePath);
                                    });
                            });
                        }

                        return Task.CompletedTask;
                    }));
        }

        public static async Task CreateBasicTypesLibraryProject(this IProjectGenerator _,
            SolutionFileContext solutionFileContext,
            IProjectFileSpecification basicTypesProjectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            await _.CreateLibraryProject(
                solutionFileContext,
                basicTypesProjectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardLibraryProject(
                    basicTypesProjectFileSpecification.Description,
                    basicTypesProjectFileSpecification.Documentation,
                    basicTypesProjectFileSpecification.DefaultNamespaceName,
                    (implementationProjectFileContext) =>
                    {
                        // Create /Classes and /Interfaces directories.
                        implementationProjectFileContext.InCodeDirectoryPathContextSynchronous(
                            (codeDirectoryPathContext) =>
                            {
                                // Create /Code/Classes/Class1.cs.
                                codeDirectoryPathContext.InProjectSubDirectoryPathContextSynchronous(
                                    Instances.CodeDirectoryName.Classes(),
                                    (classesDirectoryPathContext) =>
                                    {
                                        classesDirectoryPathContext.InProjectSubFilePathContextSynchronous(
                                            Instances.CodeFileName.Class1(),
                                            (class1CodeFileContext) =>
                                            {
                                                Instances.CodeFileGenerator.CreateDefaultClass1(
                                                    basicTypesProjectFileSpecification.DefaultNamespaceName,
                                                    class1CodeFileContext.FilePath);
                                            });
                                    });

                                // Create /Code/Interfaces/Interface1.cs.
                                codeDirectoryPathContext.InProjectSubDirectoryPathContextSynchronous(
                                    Instances.CodeDirectoryName.Interfaces(),
                                    (interfacesDirectoryPathContext) =>
                                    {
                                        interfacesDirectoryPathContext.InProjectSubFilePathContextSynchronous(
                                            Instances.CodeFileName.Interface1(),
                                            (interface1CodeFileContext) =>
                                            {
                                                Instances.CodeFileGenerator.CreateDefaultInterface1(
                                                    basicTypesProjectFileSpecification.DefaultNamespaceName,
                                                    interface1CodeFileContext.FilePath);
                                            });
                                    });
                            });

                        return Task.CompletedTask;
                    }));
        }

        public static async Task CreateExtensionMethodBaseTypesLibraryProject(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification extensionMethodBaseTypesProjectFileSpecification,
            IExtensionMethodBaseSpecification extensionMethodBaseSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            // Create the default service implementation project.
            await _.CreateLibraryProject(
                solutionFileContext,
                extensionMethodBaseTypesProjectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardLibraryProject(
                    extensionMethodBaseTypesProjectFileSpecification.Description,
                    extensionMethodBaseTypesProjectFileSpecification.Documentation,
                    extensionMethodBaseTypesProjectFileSpecification.DefaultNamespaceName,
                    (implementationProjectFileContext) =>
                    {
                        Instances.ProjectOperator.CreateExtensionMethodBase(
                            implementationProjectFileContext,
                            extensionMethodBaseSpecification);

                        return Task.CompletedTask;
                    }));
        }

        public static Task CreateProgramAsAServiceConstructionConsoleProject(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification projectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            return _.CreateProgramAsAServiceConsoleProject(
                solutionFileContext,
                projectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator);
        }

        public static async Task CreateProgramAsAServiceConsoleProject(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification projectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<ProjectFileWithSolutionFileContext, Task> modifier = default)
        {
            await _.CreateConsoleProject(
                solutionFileContext,
                projectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardConsoleProject(
                    projectFileSpecification.Description,
                    (projectFileContext) =>
                    {
                        projectFileContext.InCodeDirectoryPathContextSynchronous(
                            (codeDirectoryPathContext) =>
                            {
                                // Create the special Program.cs C# code file. (Overwriting the already created default Program.cs file.)
                                codeDirectoryPathContext.InProjectSubFilePathContextSynchronous(
                                    Instances.CodeFileName.Program(),
                                    programFileContext =>
                                    {
                                        Instances.CodeFileGenerator.CreateProgramAsAService_Old(
                                            programFileContext.FilePath,
                                            projectFileSpecification.DefaultNamespaceName);
                                    });

                                // Create the Startup.cs C# code file.
                                codeDirectoryPathContext.InProjectSubFilePathContextSynchronous(
                                    Instances.CodeFileName.Startup(),
                                    startupFilePathContext =>
                                    {
                                        Instances.CodeFileGenerator.CreateT0027_T009Startup(
                                            startupFilePathContext.FilePath,
                                            projectFileSpecification.DefaultNamespaceName);
                                    });
                            });

                        return FunctionHelper.Run(modifier, projectFileContext);
                    })
                );
        }

        public static Task CreateConsoleProject(this IProjectGenerator _,
           string projectFilePath,
           ISolutionFileContext solutionFileContext,
           IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
           IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
           Func<ProjectFileWithSolutionFileContext, Task> projectFileContextAction = default)
        {
            return _.CreateProject(
                projectFilePath,
                Instances.ProjectType.Console(),
                solutionFileContext,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                projectFileContextAction);
        }

        public static Task CreateConsoleProject(this IProjectGenerator _,
           ISolutionFileContext solutionFileContext,
           IProjectFileSpecification projectFileSpecification,
           IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
           IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
           Func<ProjectFileWithSolutionFileContext, Task> projectFileContextAction = default)
        {
            return _.CreateProject(
                solutionFileContext,
                projectFileSpecification,
                Instances.ProjectType.Console(),
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                projectFileContextAction);
        }

        public static async Task CreateBasicConstructionConsoleProject(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification basicConstructionConsoleProjectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<ProjectFileWithSolutionFileContext, Task> projectFileWithSolutionFileContextAction = default)
        {
            await _.CreateConsoleProject(
                solutionFileContext,
                basicConstructionConsoleProjectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardConsoleProject(
                    basicConstructionConsoleProjectFileSpecification.Description,
                    projectFileWithSolutionFileContextAction));
        }

        public static async Task CreateProject(this IProjectGenerator _,
            string projectFilePath,
            string projectType,
            ISolutionFileContext solutionFileContext,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<ProjectFileWithSolutionFileContext, Task> projectFileContextAction = default)
        {
            // Create the project.
            await visualStudioProjectFileOperator.Create(
                projectType,
                projectFilePath);

            // Add project file to the solution file.
            await visualStudioSolutionFileOperator.AddProjectReference(
                solutionFileContext.FilePath,
                projectFilePath);

            // Now modify the project.
            var projectFileWithSolutionFileContext = Instances.ProjectPathsOperator.GetProjectFileWithSolutionFileContext(
                projectFilePath,
                solutionFileContext);

            await FunctionHelper.Run(
                projectFileContextAction,
                projectFileWithSolutionFileContext);
        }

        /// <summary>
        /// Creates the project file only if it does not exist, but adds project references (ok if already added), updates the project's solution (ok if already added), and runs the action to modify the project.
        /// </summary>
        public static async Task CreateProjectOnlyIfNotExistsButAlwaysModify(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification projectFileSpecification,
            string projectType,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<ProjectFileWithSolutionFileContext, Task> projectFileContextAction = default)
        {
            var projectFileExists = Instances.FileSystemOperator.FileExists(projectFileSpecification.FilePath);

            if(!projectFileExists)
            {
                await _.CreateProjectOnly(
                    projectFileSpecification,
                    projectType,
                    visualStudioProjectFileOperator);
            }

            await Instances.ProjectOperator.AddProjectReferencesOkIfAlreadyAdded(
                projectFileSpecification,
                visualStudioProjectFileOperator);

            await Instances.SolutionOperator.UpdateSolutionWithProjectOkIfAlreadyAdded(
                solutionFileContext,
                projectFileSpecification,
                visualStudioSolutionFileOperator);

            // Now modify the project.
            var projectFileWithSolutionFileContext = Instances.ProjectPathsOperator.GetProjectFileWithSolutionFileContext(
                projectFileSpecification.FilePath,
                solutionFileContext);

            await FunctionHelper.Run(
                projectFileContextAction,
                projectFileWithSolutionFileContext);
        }

        public static async Task CreateProjectOnly(this IProjectGenerator _,
            IProjectFileSpecification projectFileSpecification,
            string projectType,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator)
        {
            // Create the project.
            await visualStudioProjectFileOperator.Create(
                projectType,
                projectFileSpecification.FilePath);
        }

        public static async Task CreateProjectAndAddProjectReferences(this IProjectGenerator _,
            IProjectFileSpecification projectFileSpecification,
            string projectType,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator)
        {
            await _.CreateProjectOnly(
                projectFileSpecification,
                projectType,
                visualStudioProjectFileOperator);

            await Instances.ProjectOperator.AddProjectReferences(
                projectFileSpecification,
                visualStudioProjectFileOperator);
        }

        public static async Task CreateProjectAddProjectReferencesAndUpdateSolution(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification projectFileSpecification,
            string projectType,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            await _.CreateProjectAndAddProjectReferences(
                projectFileSpecification,
                projectType,
                visualStudioProjectFileOperator);

            await Instances.SolutionOperator.UpdateSolutionWithProject(
                solutionFileContext,
                projectFileSpecification,
                visualStudioSolutionFileOperator);
        }

        public static async Task CreateProject(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification projectFileSpecification,
            string projectType,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<ProjectFileWithSolutionFileContext, Task> projectFileContextAction = default)
        {
            await _.CreateProjectAddProjectReferencesAndUpdateSolution(
                solutionFileContext,
                projectFileSpecification,
                projectType,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator);

            // Now modify the project.
            var projectFileWithSolutionFileContext = Instances.ProjectPathsOperator.GetProjectFileWithSolutionFileContext(
                projectFileSpecification.FilePath,
                solutionFileContext);

            await FunctionHelper.Run(
                projectFileContextAction,
                projectFileWithSolutionFileContext);
        }

        public static Task CreateLibraryProjectOnlyIfNotExistsButAlwaysModify(this IProjectGenerator _,
           ISolutionFileContext solutionFileContext,
           IProjectFileSpecification projectFileSpecification,
           IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
           IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
           Func<ProjectFileWithSolutionFileContext, Task> projectFileContextAction = default)
        {
            return _.CreateProjectOnlyIfNotExistsButAlwaysModify(
                solutionFileContext,
                projectFileSpecification,
                Instances.ProjectType.ClassLib(),
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                projectFileContextAction);
        }

        public static Task CreateLibraryProject(this IProjectGenerator _,
           ISolutionFileContext solutionFileContext,
           IProjectFileSpecification projectFileSpecification,
           IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
           IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
           Func<ProjectFileWithSolutionFileContext, Task> projectFileContextAction = default)
        {
            return _.CreateProject(
                solutionFileContext,
                projectFileSpecification,
                Instances.ProjectType.ClassLib(),
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                projectFileContextAction);
        }

        public static async Task CreateBasicTypesLibraryProject(this IProjectGenerator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification basicTypesProjectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            await _.CreateLibraryProject(
                solutionFileContext,
                basicTypesProjectFileSpecification,
                visualStudioProjectFileOperator,
                visualStudioSolutionFileOperator,
                Instances.ProjectOperator.SetupStandardLibraryProject(
                    basicTypesProjectFileSpecification.Description,
                    basicTypesProjectFileSpecification.Documentation,
                    basicTypesProjectFileSpecification.DefaultNamespaceName,
                    (implementationProjectFileContext) =>
                    {
                        // Create /Classes and /Interfaces directories.
                        implementationProjectFileContext.InCodeDirectoryPathContextSynchronous(
                            (codeDirectoryPathContext) =>
                            {
                                // Create /Code/Classes/Class1.cs.
                                codeDirectoryPathContext.InProjectSubDirectoryPathContextSynchronous(
                                    Instances.CodeDirectoryName.Classes(),
                                    (classesDirectoryPathContext) =>
                                    {
                                        classesDirectoryPathContext.InProjectSubFilePathContextSynchronous(
                                            Instances.CodeFileName.Class1(),
                                            (class1CodeFileContext) =>
                                            {
                                                Instances.CodeFileGenerator.CreateDefaultClass1(
                                                    basicTypesProjectFileSpecification.DefaultNamespaceName,
                                                    class1CodeFileContext.FilePath);
                                            });
                                    });

                                // Create /Code/Interfaces/Interface1.cs.
                                codeDirectoryPathContext.InProjectSubDirectoryPathContextSynchronous(
                                    Instances.CodeDirectoryName.Interfaces(),
                                    (interfacesDirectoryPathContext) =>
                                    {
                                        interfacesDirectoryPathContext.InProjectSubFilePathContextSynchronous(
                                            Instances.CodeFileName.Interface1(),
                                            (interface1CodeFileContext) =>
                                            {
                                                Instances.CodeFileGenerator.CreateDefaultInterface1(
                                                    basicTypesProjectFileSpecification.DefaultNamespaceName,
                                                    interface1CodeFileContext.FilePath);
                                            });
                                    });
                            });

                        return Task.CompletedTask;
                    }));
        }
    }
}
