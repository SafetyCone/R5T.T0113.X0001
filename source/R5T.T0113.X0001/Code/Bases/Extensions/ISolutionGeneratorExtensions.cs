using System;
using System.Threading.Tasks;

using R5T.D0078;
using R5T.T0106;
using R5T.T0113;
using R5T.T0114;

using Instances = R5T.T0113.X0001.Instances;


namespace System
{
    public static class ISolutionGeneratorExtensions
    {
        public static async Task CreateSolution(this ISolutionGenerator _,
            string repositoryDirectoryPath,
            string solutionName,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<SolutionFileContext, Task> solutionFileContextAction = default)
        {
            var solutionFilePath = Instances.SolutionPathsOperator.GetSolutionFilePath(
                repositoryDirectoryPath,
                solutionName);

            await _.CreateSolution(
                solutionFilePath,
                visualStudioSolutionFileOperator,
                solutionFileContextAction);
        }

        public static async Task CreateSolution(this ISolutionGenerator _,
            string solutionFilePath,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<SolutionFileContext, Task> solutionFileContextAction = default)
        {
            // Create the solution file.
            await visualStudioSolutionFileOperator.Create(solutionFilePath);

            // Now modify the solution.
            var solutionFileContext = Instances.SolutionPathsOperator.GetSolutionFileContext(
                solutionFilePath);

            await FunctionHelper.Run(solutionFileContextAction, solutionFileContext);
        }

        public static async Task CreateSolutionOnlyIfNotExistsButAlwaysModify(this ISolutionGenerator _,
            string solutionDirectoryPath,
            string solutionName,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<SolutionFileContext, Task> solutionFileContextAction = default)
        {
            var solutionFilePath = Instances.SolutionPathsOperator.GetSolutionFilePath(
                solutionDirectoryPath,
                solutionName);

            await _.CreateSolutionOnlyIfNotExistsButAlwaysModify(
                solutionFilePath,
                visualStudioSolutionFileOperator,
                solutionFileContextAction);
        }

        public static async Task CreateSolutionOnlyIfNotExistsButAlwaysModify(this ISolutionGenerator _,
            string solutionFilePath,
            IVisualStudioSolutionFileOperator visualStudioSolutionFileOperator,
            Func<SolutionFileContext, Task> solutionFileContextAction = default)
        {
            var solutionFileExists = Instances.FileSystemOperator.FileExists(solutionFilePath);
            if(!solutionFileExists)
            {
                // Create the solution file.
                await visualStudioSolutionFileOperator.Create(solutionFilePath);
            }

            // Now modify the solution.
            var solutionFileContext = Instances.SolutionPathsOperator.GetSolutionFileContext(
                solutionFilePath);

            await FunctionHelper.Run(solutionFileContextAction, solutionFileContext);
        }
    }
}
