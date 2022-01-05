using System;

using R5T.T0040;
using R5T.T0106;

using Instances = R5T.T0113.X0001.Instances;


namespace System
{
    public static class ISolutionPathsOperatorExtensions
    {
        public static SolutionFileContext GetSolutionFileContext(this ISolutionPathsOperator _,
            string solutionFilePath)
        {
            var solutionDirectoryPath = Instances.PathOperator.GetDirectoryPathOfFilePath(solutionFilePath);

            var solutionFileName = Instances.PathOperator.GetFileNameForFilePath(solutionFilePath);

            var solutionName = Instances.SolutionFileNameOperator.GetSolutionNameFromSolutionFileName(solutionFileName);

            var output = new SolutionFileContext
            {
                Name = solutionName,
                DirectoryPath = solutionDirectoryPath,
                FilePath = solutionFilePath,
            };

            return output;
        }
    }
}
