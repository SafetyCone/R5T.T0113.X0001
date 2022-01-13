using System;

using R5T.T0037;

using Instances = R5T.T0113.X0001.Instances;


namespace System
{
    public static class ICodeFileNameExtensions
    {
        public static string GetCSharpFileNameForTypeName(this ICodeFileName _,
            string typeName)
        {
            var output = Instances.TypeName.GetCSharpCodeFileName(typeName);
            return output;
        }

        public static string IServiceActionExtensions(this ICodeFileName _)
        {
            var output = Instances.CodeFileName.GetCSharpFileNameForTypeName(
                Instances.TypeName.IServiceActionExtensions());

            return output;
        }

        public static string IServiceCollectionExtensions(this ICodeFileName _)
        {
            var output = Instances.CodeFileName.GetCSharpFileNameForTypeName(
                Instances.TypeName.IServiceCollectionExtensions());

            return output;
        }
    }
}
