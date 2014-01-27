using System.Text;

namespace Taga.DynamicServices.WCFClient
{
    delegate string ProxyCodeModifier(string proxyCode);

    class DynamicProxyFactoryOptions
    {
        public enum LanguageOptions { CS, VB };
        public enum FormatModeOptions { Auto, XmlSerializer, DataContractSerializer }

        public DynamicProxyFactoryOptions()
        {
            Language = LanguageOptions.CS;
            FormatMode = FormatModeOptions.Auto;
            CodeModifier = null;
        }

        public LanguageOptions Language { get; set; }

        public FormatModeOptions FormatMode { get; set; }

        // The code modifier allows the user of the dynamic proxy factory to modify 
        // the generated proxy code before it is compiled and used. This is useful in 
        // situations where the generated proxy has to be modified manually for interop 
        // reason.
        public ProxyCodeModifier CodeModifier { get; set; }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("DynamicProxyFactoryOptions[")
                .Append("Language=" + Language)
                .Append(",FormatMode=" + FormatMode)
                .Append(",CodeModifier=" + CodeModifier)
                .Append("]")
                .ToString();
        }
    }
}
