using System;

using R5T.T0040;
using R5T.T0106;

using Instances = R5T.T0113.X0001.Instances;


namespace System
{
    public static class IProjectPathsOperatorExtensions
    {
        public static string GetInitialProgramCodeFileRelativePath(this IProjectPathsOperator _)
        {
            var output = Instances.CodeFileName.Program();
            return output;
        }

        public static string GetInitialProgramCodeFilePath(this IProjectPathsOperator _,
            string projectDirectoryPath)
        {
            var output = Instances.PathOperator.GetFilePath(
                projectDirectoryPath,
                _.GetInitialProgramCodeFileRelativePath());

            return output;
        }

        public static ProjectFileWithSolutionFileContext GetProjectFileWithSolutionFileContext(this IProjectPathsOperator _,
            string projectFilePath,
            ISolutionFileContext solutionFileContext)
        {
            var projectFileContext = _.GetProjectFileContext(projectFilePath);

            var output = new ProjectFileWithSolutionFileContext
            {
                Name = projectFileContext.Name,
                DirectoryPath = projectFileContext.DirectoryPath,
                FilePath = projectFileContext.FilePath,
                SolutionFileContext = solutionFileContext,
            };

            return output;
        }

        public static ProjectFileContext GetProjectFileContext(this IProjectPathsOperator _,
            string projectFilePath)
        {
            var projectDirectoryPath = Instances.PathOperator.GetDirectoryPathOfFilePath(projectFilePath);

            var projectFileName = Instances.PathOperator.GetFileNameForFilePath(projectFilePath);

            var projectName = Instances.ProjectFileNameOperator.GetProjectNameFromProjectFileName(projectFileName);

            var output = new ProjectFileContext
            {
                Name = projectName,
                DirectoryPath = projectDirectoryPath,
                FilePath = projectFilePath,
            };

            return output;
        }
    }
}
