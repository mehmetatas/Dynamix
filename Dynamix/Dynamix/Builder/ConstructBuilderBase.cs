using Dynamix.Metadata;

namespace Dynamix.Builder
{
    internal abstract class ConstructBuilderBase : TypeBuilder
    {
        protected readonly Construct Construct;

        protected ConstructBuilderBase(Construct dynamicConstruct)
        {
            Construct = dynamicConstruct;
        }

        protected override void BuildMembers()
        {
            BuildFields();
            BuildMethods();
            BuildProperties();
            BuildIndexers();
            BuildEvents();
        }

        private void BuildFields()
        {
            foreach (var field in Construct.Fields)
                field.Builder.Build();
        }

        private void BuildMethods()
        {
            foreach (var method in Construct.Methods)
                method.Builder.Build();
        }

        private void BuildProperties()
        {
            foreach (var prop in Construct.Properties)
                prop.Builder.Build();
        }

        private void BuildIndexers()
        {
            foreach (var indexer in Construct.Indexers)
                indexer.Builder.Build();
        }

        private void BuildEvents()
        {
            foreach (var evnt in Construct.Events)
                evnt.Builder.Build();
        }
    }
}
