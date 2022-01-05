using System;

using R5T.T0029.Dotnet.T001;
using R5T.T0034;
using R5T.T0037;
using R5T.T0040;
using R5T.T0045;
using R5T.T0041;
using R5T.T0044;
using R5T.T0110;
using R5T.T0112;
using R5T.T0115;
using R5T.T0116;
using R5T.T0117;


namespace R5T.T0113.X0001
{
    public static class Instances
    {
        public static ICodeDirectoryName CodeDirectoryName { get; } = T0037.CodeDirectoryName.Instance;
        public static ICodeFileGenerator CodeFileGenerator { get; } = T0045.CodeFileGenerator.Instance;
        public static ICodeFileName CodeFileName { get; } = T0037.CodeFileName.Instance;
        public static IFileGenerator FileGenerator { get; } = T0117.FileGenerator.Instance;
        public static IFileSystemOperator FileSystemOperator { get; } = T0044.FileSystemOperator.Instance;
        public static ILibraryNameOperator LibraryNameOperator { get; } = T0110.LibraryNameOperator.Instance;
        public static INamespacedTypeName NamespacedTypeName { get; } = T0034.NamespacedTypeName.Instance;
        public static IPathOperator PathOperator { get; } = T0041.PathOperator.Instance;
        public static IProjectDescriptionGenerator ProjectDescriptionGenerator { get; } = T0115.ProjectDescriptionGenerator.Instance;
        public static IProjectFileNameOperator ProjectFileNameOperator { get; } = T0040.ProjectFileNameOperator.Instance;
        public static IProjectNameOperator ProjectNameOperator { get; } = T0112.ProjectNameOperator.Instance;
        public static IProjectPathsOperator ProjectPathsOperator { get; } = T0040.ProjectPathsOperator.Instance;
        public static IProjectOperator ProjectOperator { get; } = T0113.ProjectOperator.Instance;
        public static IProjectType ProjectType { get; } = T0029.Dotnet.T001.ProjectType.Instance;
        public static ISolutionFileNameOperator SolutionFileNameOperator { get; } = T0040.SolutionFileNameOperator.Instance;
        public static ISolutionFolder SolutionFolder { get; } = T0116.SolutionFolder.Instance;
        public static ISolutionOperator SolutionOperator { get; } = T0113.SolutionOperator.Instance;
        public static ISolutionPathsOperator SolutionPathsOperator { get; } = T0040.SolutionPathsOperator.Instance;
        public static ITypeName TypeName { get; } = T0034.TypeName.Instance;
    }
}
