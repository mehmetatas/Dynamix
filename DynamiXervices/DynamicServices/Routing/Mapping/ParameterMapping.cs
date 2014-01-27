
namespace Taga.DynamicServices.Routing.Mapping
{
    public class ParameterMapping : Mapping
    {
        public string SourceParameterName
        {
            get { return GetParameterName(Source); }
        }

        public string SourcePropertyPath
        {
            get { return GetPropertyPath(Source); }
        }
        public string TargetParameterName
        {
            get { return GetParameterName(Target); }
        }

        public string TargetPropertyPath
        {
            get { return GetPropertyPath(Target); }
        }

        private static string GetParameterName(string sourceOrTarget)
        {
            var indexOfDot = sourceOrTarget.IndexOf('.');
            return indexOfDot < 0
                ? sourceOrTarget
                : sourceOrTarget.Substring(0, indexOfDot);
        }

        private static string GetPropertyPath(string sourceOrTarget)
        {
            var indexOfDot = sourceOrTarget.IndexOf('.');
            return indexOfDot < 0
                ? sourceOrTarget
                : sourceOrTarget.Substring(indexOfDot + 1);
        }
    }
}
