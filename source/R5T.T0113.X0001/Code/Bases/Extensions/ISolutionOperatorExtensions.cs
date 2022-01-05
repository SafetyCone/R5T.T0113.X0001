using System;
using System.Threading.Tasks;

using R5T.D0078;
using R5T.T0106;
using R5T.T0113;
using R5T.T0114;

using Instances = R5T.T0113.X0001.Instances;


namespace System
{
    public static class ISolutionOperatorExtensions
    {
        public static async Task InSolutionContext(this ISolutionOperator _,
            string solutionFilePath,
            Func<ISolutionFileContext, Task> solutionFileContextAction)
        {
            var solutionFileContext = Instances.SolutionPathsOperator.GetSolutionFileContext(
                solutionFilePath);

            await FunctionHelper.Run(solutionFileContextAction, solutionFileContext);
        }

        /// <summary>
        /// Just generates the solution file path required for the solution file specification.
        /// </summary>
        public static SolutionFileSpecification GetSolutionFileSpecificationInRepositorySourceDirectory(this ISolutionOperator _,
            string solutionName,
            string repositoryDirectoryPath)
        {
            var solutionDirectoryPath = Instances.SolutionPathsOperator.GetSourceSolutionDirectoryPath(repositoryDirectoryPath);

            var output = _.GetSolutionFileSpecification(
                solutionName,
                solutionDirectoryPath);

            return output;
        }

        public static async Task UpdateSolutionWithProjectOkIfAlreadyAdded(this ISolutionOperator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification projectFileSpecification,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            // Add project file to the solution file.
            await visualStudioSolutionFileOperator.AddProjectReferenceOkIfAlreadyAdded(
                solutionFileContext.FilePath,
                projectFileSpecification.FilePath);

            // Perform actions for the project's specified project references and dependency project references.
            foreach (var dependencyProjectReferenceFilePath in projectFileSpecification.DependencyProjectReferenceFilePaths)
            {
                await visualStudioSolutionFileOperator.AddDependencyProjectReferenceOkIfAlreadyAdded(
                    solutionFileContext.FilePath,
                    dependencyProjectReferenceFilePath);
            }

            foreach (var projectReferenceFilePath in projectFileSpecification.ProjectReferenceFilePaths)
            {
                await visualStudioSolutionFileOperator.AddProjectReferenceOkIfAlreadyAdded(
                    solutionFileContext.FilePath,
                    projectReferenceFilePath);
            }
        }

        public static async Task UpdateSolutionWithProject(this ISolutionOperator _,
            ISolutionFileContext solutionFileContext,
            IProjectFileSpecification projectFileSpecification,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator)
        {
            // Add project file to the solution file.
            await visualStudioSolutionFileOperator.AddProjectReference(
                solutionFileContext.FilePath,
                projectFileSpecification.FilePath);

            // Perform actions for the project's specified project references and dependency project references.
            foreach (var dependencyProjectReferenceFilePath in projectFileSpecification.DependencyProjectReferenceFilePaths)
            {
                await visualStudioSolutionFileOperator.AddDependencyProjectReference(
                    solutionFileContext.FilePath,
                    dependencyProjectReferenceFilePath);
            }

            foreach (var projectReferenceFilePath in projectFileSpecification.ProjectReferenceFilePaths)
            {
                await visualStudioSolutionFileOperator.AddProjectReference(
                    solutionFileContext.FilePath,
                    projectReferenceFilePath);
            }
        }
    }
}
