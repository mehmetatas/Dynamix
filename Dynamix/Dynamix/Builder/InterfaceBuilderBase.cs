using System;
using Dynamix.Metadata;

namespace Dynamix.Builder
{
    internal abstract class InterfaceBuilderBase : TypeBuilder
    {
        protected readonly InterfaceBase InterfaceBase;

        internal InterfaceBuilderBase(InterfaceBase dynamicInterface)
        {
            InterfaceBase = dynamicInterface;
        }

        protected override void BuildMembers()
        {
            BuildMethods();
            BuildProperties();
            BuildIndexers();
            BuildEvents();
        }

        private void BuildMethods()
        {
            foreach (var method in InterfaceBase.Methods)
                method.Builder.Build();
        }

        private void BuildProperties()
        {
            foreach (var prop in InterfaceBase.Properties)
                prop.Builder.Build();
        }

        private void BuildIndexers()
        {
            foreach (var indexer in InterfaceBase.Indexers)
                indexer.Builder.Build();
        }

        private void BuildEvents()
        {
            foreach (var evnt in InterfaceBase.Events)
                evnt.Builder.Build();
        }

        internal override Type BaseType
        {
            get { return null; }
        }
    }
}