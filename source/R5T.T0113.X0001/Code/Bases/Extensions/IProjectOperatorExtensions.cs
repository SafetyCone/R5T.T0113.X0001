using System;
using System.Linq;
using System.Threading.Tasks;

using R5T.D0079;
using R5T.T0106;
using R5T.T0113;
using R5T.T0114;

using Instances = R5T.T0113.X0001.Instances;


namespace System
{
    public static class IProjectOperatorExtensions
    {
        public static async Task AddProjectReferencesOkIfAlreadyAdded(this IProjectOperator _,
            IProjectFileSpecification projectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator)
        {
            // Perform actions for the project's specified project references and dependency project references.
            var projectReferences = projectFileSpecification.DependencyProjectReferenceFilePaths
                .Concat(projectFileSpecification.ProjectReferenceFilePaths);

            foreach (var projectReference in projectReferences)
            {
                await visualStudioProjectFileOperator.AddProjectReferenceOkIfAlreadyAdded(
                    projectFileSpecification.FilePath,
                    projectReference);
            }
        }

        public static async Task AddProjectReferences(this IProjectOperator _,
            IProjectFileSpecification projectFileSpecification,
            IVisualStudioProjectFileOperator visualStudioProjectFileOperator)
        {
            // Perform actions for the project's specified project references and dependency project references.
            var projectReferences = projectFileSpecification.DependencyProjectReferenceFilePaths
                .Concat(projectFileSpecification.ProjectReferenceFilePaths);

            foreach (var projectReference in projectReferences)
            {
                await visualStudioProjectFileOperator.AddProjectReference(
                    projectFileSpecification.FilePath,
                    projectReference);
            }
        }

        public static void CreateServiceImplementation(this IProjectOperator _,
            ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext,
            string serviceImplementationNamespacedTypeName,
            string serviceDefinitionNamespacedTypeName)
        {
            var serviceImplementationTypeName = Instances.NamespacedTypeName.GetTypeName(serviceImplementationNamespacedTypeName);

            var serviceImplementationFileName = Instances.TypeName.GetCSharpCodeFileName(serviceImplementationTypeName);

            projectFileWithSolutionFileContext.InProjectSubDirectoryPathContextSynchronous(
                Instances.ProjectPathsOperator.GetServiceImplementationsDirectoryRelativePath(),
                (serviceImplementationsDirectoryContext) =>
                {
                    serviceImplementationsDirectoryContext.InProjectSubFilePathContextSynchronous(
                        serviceImplementationFileName,
                        (serviceImplementationFileContext) =>
                        {
                            Instances.CodeFileGenerator.CreateServiceImplementation(
                                serviceImplementationFileContext.FilePath,
                                serviceImplementationNamespacedTypeName,
                                serviceDefinitionNamespacedTypeName);
                        });
                });
        }

        public static void CreateServiceImplementation(this IProjectOperator _,
            ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext,
            IServiceImplementationSpecification serviceImplementationSpecification)
        {
            var serviceImplementationTypeName = Instances.NamespacedTypeName.GetTypeName(serviceImplementationSpecification.ImplementationNamespacedTypeName);

            var serviceImplementationFileName = Instances.TypeName.GetCSharpCodeFileName(serviceImplementationTypeName);

            projectFileWithSolutionFileContext.InProjectSubDirectoryPathContextSynchronous(
                Instances.ProjectPathsOperator.GetServiceImplementationsDirectoryRelativePath(),
                (serviceImplementationsDirectoryContext) =>
                {
                    serviceImplementationsDirectoryContext.InProjectSubFilePathContextSynchronous(
                        serviceImplementationFileName,
                        (serviceImplementationFileContext) =>
                        {
                            Instances.CodeFileGenerator.CreateServiceImplementation(
                                serviceImplementationFileContext.FilePath,
                                serviceImplementationSpecification.ImplementationNamespacedTypeName,
                                serviceImplementationSpecification.DefinitionNamespacedTypeName);
                        });
                });
        }

        public static void CreateIServiceCollectionExtensionsStub(this IProjectOperator _,
            ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext,
            string serviceImplementationsNamespaceName)
        {
            projectFileWithSolutionFileContext.InProjectSubDirectoryPathContextSynchronous(
                Instances.ProjectPathsOperator.GetExtensionsDirectoryRelativePath(),
                (serviceImplementationsDirectoryContext) =>
                {
                    serviceImplementationsDirectoryContext.InProjectSubFilePathContextSynchronous(
                        Instances.CodeFileName.IServiceCollectionExtensions(),
                        (serviceImplementationFileContext) =>
                        {
                            Instances.CodeFileGenerator.CreateIServiceCollectionExtensionsStub(
                                serviceImplementationFileContext.FilePath,
                                serviceImplementationsNamespaceName);
                        });
                });
        }

        public static void CreateExtensionMethodBase(this IProjectOperator _,
            IProjectFileContext projectFileContext,
            IExtensionMethodBaseSpecification extensionMethodBaseSpecification)
        {
            // Create the the extension method base interface.
            projectFileContext.InProjectSubDirectoryPathContextSynchronous(
                Instances.ProjectPathsOperator.GetBasesInterfacesDirectoryRelativePath(),
                (baseInterfacesDirectoryContext) =>
                {
                    var baseInterfaceTypeName = Instances.NamespacedTypeName.GetTypeName(extensionMethodBaseSpecification.InterfaceNamespacedTypeName);

                    var baseInterfaceFileName = Instances.TypeName.GetCSharpCodeFileName(baseInterfaceTypeName);

                    baseInterfacesDirectoryContext.InProjectSubFilePathContextSynchronous(
                        baseInterfaceFileName,
                        (serviceImplementationFileContext) =>
                        {
                            Instances.CodeFileGenerator.CreateExtensionMethodBaseInterface(
                                serviceImplementationFileContext.FilePath,
                                extensionMethodBaseSpecification.InterfaceNamespacedTypeName);
                        });
                });

            // Create the the extension method base class.
            projectFileContext.InProjectSubDirectoryPathContextSynchronous(
                Instances.ProjectPathsOperator.GetBasesClassesDirectoryRelativePath(),
                (baseClassesDirectoryContext) =>
                {
                    var baseClassTypeName = Instances.NamespacedTypeName.GetTypeName(extensionMethodBaseSpecification.ClassNamespacedTypeName);

                    var baseClassFileName = Instances.TypeName.GetCSharpCodeFileName(baseClassTypeName);

                    baseClassesDirectoryContext.InProjectSubFilePathContextSynchronous(
                        baseClassFileName,
                        (serviceImplementationFileContext) =>
                        {
                            Instances.CodeFileGenerator.CreateExtensionMethodBaseClass(
                                serviceImplementationFileContext.FilePath,
                                extensionMethodBaseSpecification.ClassNamespacedTypeName,
                                extensionMethodBaseSpecification.InterfaceNamespacedTypeName);
                        });
                });
        }

        public static Func<ProjectFileWithSolutionFileContext, Task> SetupStandardProject(this IProjectOperator _,
            string projectDescription,
            Func<ProjectFileWithSolutionFileContext, Task> customProjectFileWithSolutionFileContextAction = default)
        {
            // If unspecified, just use the project description as its documentation.
            var projectDocumentation = projectDescription;

            Task StandardProjectSetup_Inner(ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext)
            {
                // If unspecified, the documentation file namespace name is the default
                var documentationFileNamespaceName = Instances.ProjectNameOperator.GetDefaultNamespaceNameFromProjectName(projectFileWithSolutionFileContext.Name);

                return _.SetupStandardProject(
                    projectDescription,
                    projectDocumentation,
                    documentationFileNamespaceName,
                    customProjectFileWithSolutionFileContextAction)
                    (projectFileWithSolutionFileContext);
            }

            return StandardProjectSetup_Inner;
        }

        public static Func<ProjectFileWithSolutionFileContext, Task> SetupStandardConsoleProject(this IProjectOperator _,
            string projectDescription,
            Func<ProjectFileWithSolutionFileContext, Task> customProjectFileWithSolutionFileContextAction = default)
        {
            Func<ProjectFileWithSolutionFileContext, Task> output = _.SetupStandardProject(
                projectDescription,
                (projectFileWithSolutionFileContext) =>
                {
                    var defaultNamespaceName = Instances.ProjectNameOperator.GetDefaultNamespaceNameFromProjectName(projectFileWithSolutionFileContext.Name);

                    // Delete the initial program file.
                    Instances.ProjectOperator.DeleteInitialProgramFile(
                        projectFileWithSolutionFileContext);

                    // Create the default program file.
                    Instances.ProjectOperator.CreateDefaultProgramFile(
                        projectFileWithSolutionFileContext,
                        defaultNamespaceName);

                    return FunctionHelper.Run(customProjectFileWithSolutionFileContextAction, projectFileWithSolutionFileContext);
                });

            return output;
        }

        public static void CreateDefaultProgramFile(this IProjectOperator _,
            ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext,
            string defaultNamespaceName)
        {
            projectFileWithSolutionFileContext.InCodeDirectoryPathContextSynchronous(
                (projectCodeDirectoryPathContext) =>
                {
                    var programCodeFilePath = Instances.PathOperator.GetFilePath(
                                projectCodeDirectoryPathContext.DirectoryPath,
                                Instances.CodeFileName.Program());

                    Instances.CodeFileGenerator.CreateDefaultProgram(
                        defaultNamespaceName,
                        programCodeFilePath);
                });
        }

        public static void DeleteInitialProgramFile(this IProjectOperator _,
            IProjectFileContext projectFileContext)
        {
            var initialProgramCodeFilePath = Instances.ProjectPathsOperator.GetInitialProgramCodeFilePath(
                projectFileContext.DirectoryPath);

            Instances.FileSystemOperator.DeleteFile(initialProgramCodeFilePath);
        }

        public static void DeleteInitialClass1File(this IProjectOperator _,
            IProjectFileContext projectFileContext)
        {
            var initialProgramCodeFilePath = Instances.ProjectPathsOperator.GetInitialClass1CodeFilePath(
                projectFileContext.DirectoryPath);

            Instances.FileSystemOperator.DeleteFile(initialProgramCodeFilePath);
        }

        public static void CreateProjectPlanFile(this IProjectOperator _,
            ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext,
            string projectDescription)
        {
            projectFileWithSolutionFileContext.InProjectSubFilePathContextSynchronous(
                Instances.ProjectPathsOperator.GetStandardProjectPlanFileRelativePath(),
                (projectPlanFilePathContext) =>
                {
                    Instances.FileGenerator.CreateProjectPlanFile(
                        projectPlanFilePathContext.FilePath,
                        projectPlanFilePathContext.ProjectFileContext.Name,
                        projectDescription);
                });
        }

        public static void CreateCodeDirectory(this IProjectOperator _,
            ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext)
        {
            var codeDirectoryPath = Instances.ProjectPathsOperator.GetCodeDirectoryPath(
                projectFileWithSolutionFileContext.DirectoryPath);

            Instances.FileSystemOperator.CreateDirectory(codeDirectoryPath);
        }

        public static void CreateDocumentationFile(this IProjectOperator _,
            ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext,
            string defaultNamespaceName,
            string projectDescription)
        {
            projectFileWithSolutionFileContext.InProjectSubFilePathContextSynchronous(
                Instances.ProjectPathsOperator.GetStandardDocumentationFileRelativePath(),
                (documentationFilePathContext) =>
                {
                    Instances.CodeFileGenerator.CreateDocumentation(
                        documentationFilePathContext.FilePath,
                        defaultNamespaceName,
                        projectDescription);
                });
        }

        public static Func<ProjectFileWithSolutionFileContext, Task> SetupStandardProject(this IProjectOperator _,
            string projectDescription,
            string projectDocumentation,
            string documentationFileNamespaceName,
            Func<ProjectFileWithSolutionFileContext, Task> customProjectFileWithSolutionFileContextAction = default)
        {
            Task StandardProjectSetup_Inner(ProjectFileWithSolutionFileContext projectFileWithSolutionFileContext)
            {
                // Add the "Project Plan.txt" document.
                Instances.ProjectOperator.CreateProjectPlanFile(
                    projectFileWithSolutionFileContext,
                    projectDescription);

                // Create the /Code directory.
                Instances.ProjectOperator.CreateCodeDirectory(
                    projectFileWithSolutionFileContext);

                // Create the /Code/Documentation.cs file.
                Instances.ProjectOperator.CreateDocumentationFile(
                    projectFileWithSolutionFileContext,
                    documentationFileNamespaceName,
                    projectDocumentation);

                return FunctionHelper.Run(customProjectFileWithSolutionFileContextAction, projectFileWithSolutionFileContext);
            }

            return StandardProjectSetup_Inner;
        }

        public static Func<ProjectFileWithSolutionFileContext, Task> SetupStandardLibraryProject(this IProjectOperator _,
            string projectDescription,
            string projectDocumentation,
            string documentationFileNamespaceName,
            Func<ProjectFileWithSolutionFileContext, Task> customProjectFileWithSolutionFileContextAction = default)
        {
            Func<ProjectFileWithSolutionFileContext, Task> output = _.SetupStandardProject(
                projectDescription,
                projectDocumentation,
                documentationFileNamespaceName,
                (projectFileWithSolutionFileContext) =>
                {
                    var defaultNamespaceName = Instances.ProjectNameOperator.GetDefaultNamespaceNameFromProjectName(projectFileWithSolutionFileContext.Name);

                    // Delete the initial class1 file.
                    Instances.ProjectOperator.DeleteInitialClass1File(
                        projectFileWithSolutionFileContext);

                    return FunctionHelper.Run(customProjectFileWithSolutionFileContextAction, projectFileWithSolutionFileContext);
                });

            return output;
        }

        public static Func<ProjectFileWithSolutionFileContext, Task> SetupStandardLibraryProject(this IProjectOperator _,
            string projectDescription,
            Func<ProjectFileWithSolutionFileContext, Task> customProjectFileWithSolutionFileContextAction = default)
        {
            Func<ProjectFileWithSolutionFileContext, Task> output = _.SetupStandardProject(
                projectDescription,
                (projectFileWithSolutionFileContext) =>
                {
                    var defaultNamespaceName = Instances.ProjectNameOperator.GetDefaultNamespaceNameFromProjectName(projectFileWithSolutionFileContext.Name);

                    // Delete the initial class1 file.
                    Instances.ProjectOperator.DeleteInitialClass1File(
                        projectFileWithSolutionFileContext);

                    return FunctionHelper.Run(customProjectFileWithSolutionFileContextAction, projectFileWithSolutionFileContext);
                });

            return output;
        }

        public static ProjectFileSpecification GetBasicConstructionProjectFileSpecification(this IProjectOperator _,
            string libraryName,
            string libraryDescription,
            string parentDirectoryPath,
            string libraryProjectFilePath)
        {
            var projectNameStem = Instances.LibraryNameOperator.GetProjectNameStem(libraryName);
            var projectName = Instances.ProjectNameOperator.GetConstructionProjectName(projectNameStem);
            var projectDescription = Instances.ProjectDescriptionGenerator.GetConstructionProjectDescription(libraryDescription);
            var projectDocumentation = projectDescription; // Reuse the description.
            var projectDefaultNamespaceName = Instances.ProjectNameOperator.GetDefaultNamespaceNameFromProjectName(projectName);

            var projectDirectoryPath = Instances.ProjectPathsOperator.GetProjectDirectoryPath(parentDirectoryPath, projectName);
            var projectFilePath = Instances.ProjectPathsOperator.GetProjectFilePath(projectDirectoryPath, projectName);

            var projectReferenceFilePaths = new[]
            {
                libraryProjectFilePath,
            };

            var dependencyProjectReferenceFilePaths = Array.Empty<string>();

            var output = new ProjectFileSpecification
            {
                DefaultNamespaceName = projectDefaultNamespaceName,
                DependencyProjectReferenceFilePaths = dependencyProjectReferenceFilePaths,
                Description = projectDescription,
                Documentation = projectDocumentation,
                FilePath = projectFilePath,
                Name = projectName,
                ProjectReferenceFilePaths = projectReferenceFilePaths,
            };

            return output;
        }

        public static ProjectFileSpecification GetBasicTypesProjectFileSpecification(this IProjectOperator _,
            string libraryName,
            string libraryDescription,
            string parentDirectoryPath)
        {
            var projectName = Instances.LibraryNameOperator.GetProjectName(libraryName); // Directly use the library name.
            var projectDescription = Instances.ProjectDescriptionGenerator.GetProjectDescription(libraryDescription);
            var projectDocumentation = projectDescription; // Reuse the description.
            var projectDefaultNamespaceName = Instances.ProjectNameOperator.GetDefaultNamespaceNameFromProjectName(projectName);

            var projectDirectoryPath = Instances.ProjectPathsOperator.GetProjectDirectoryPath(parentDirectoryPath, projectName);
            var projectFilePath = Instances.ProjectPathsOperator.GetProjectFilePath(projectDirectoryPath, projectName);

            var projectReferenceFilePaths = Array.Empty<string>();

            //var dependencyProjectReferenceFilePaths = new[]
            //{
            //    //Instances.ProjectPath.R5T_Magyar(), // Always include Magyar.
            //};

            var dependencyProjectReferenceFilePaths = Array.Empty<string>(); //TODO

            var output = new ProjectFileSpecification
            {
                DefaultNamespaceName = projectDefaultNamespaceName,
                DependencyProjectReferenceFilePaths = dependencyProjectReferenceFilePaths,
                Description = projectDescription,
                Documentation = projectDocumentation,
                FilePath = projectFilePath,
                Name = projectName,
                ProjectReferenceFilePaths = projectReferenceFilePaths,
            };

            return output;
        }
    }
}
