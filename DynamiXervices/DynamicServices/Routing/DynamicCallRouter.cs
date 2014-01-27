using System;
using System.Linq;
using Taga.DynamicServices.Client;
using Taga.DynamicServices.Invocation;
using Taga.DynamicServices.Routing.Mapping;

namespace Taga.DynamicServices.Routing
{
    public class DynamicCallRouter : IDynamicCallRouter
    {
        private const char Dot = '.';

        private readonly RouteInfo _route;
        private readonly IDynamicMethod _targetMethod;

        public DynamicCallRouter(RouteInfo route, IDynamicMethod targetMethod)
        {
            _route = route;
            _targetMethod = targetMethod;
        }

        public object Call(IDynamicInvocationContext context)
        {
            MapInputParameters(context.InputParameters);
            
            var sourceOutput = _targetMethod.Invoke();

            var skipOutputMapping = _route.OutputMapping.Mappings.Count == 0 || sourceOutput == null || context.ReturnType == typeof(void);

            return skipOutputMapping 
                ? sourceOutput 
                : MapOutputParameters(sourceOutput, context.ReturnType);
        }

        private void MapInputParameters(Parameter[] parameters)
        {
            foreach (var mapping in _route.InputMapping.Mappings)
            {
                var sourceParamName = mapping.SourceParameterName;
                var sourceParam = parameters.First(p => p.Name == sourceParamName);

                var targetParamName = mapping.TargetParameterName;
                var targetParam = _targetMethod.GetParameter(targetParamName);

                SetTargetMethodParameterValue(sourceParam, targetParam, mapping);
            }
        }

        private static void SetTargetMethodParameterValue(Parameter sourceParam, IDynamicParameter targetParam, ParameterMapping inputMapping)
        {
            if (sourceParam.Value == null)
                return;

            var sourceHasProperty = inputMapping.Source.Contains(Dot);
            var targetHasProperty = inputMapping.Target.Contains(Dot);

            if (sourceHasProperty && targetHasProperty)
            {
                var dynamicSource = new DynamicObject(sourceParam.Value);
                var sourceValue = dynamicSource.GetProperty(inputMapping.SourcePropertyPath);
                SetTargetMethodParameterProperty(targetParam, inputMapping.TargetPropertyPath, sourceValue);
            }
            else if (sourceHasProperty)
            {
                var dynamicSource = new DynamicObject(sourceParam.Value);
                targetParam.Value = dynamicSource.GetProperty(inputMapping.SourcePropertyPath);
            }
            else if (targetHasProperty)
            {
                SetTargetMethodParameterProperty(targetParam, inputMapping.TargetPropertyPath, sourceParam.Value);
            }
            else
            {
                targetParam.Value = sourceParam.Value;
            }
        }

        private static void SetTargetMethodParameterProperty(IDynamicParameter targetParam, string propertyPath, object value)
        {
            DynamicObject dynamicTarget;
            if (targetParam.Value == null)
            {
                dynamicTarget = new DynamicObject(targetParam.Type);
                dynamicTarget.CallConstructor();
            }
            else
            {
                dynamicTarget = new DynamicObject(targetParam.Value);
            }

            dynamicTarget.SetProperty(propertyPath, value, true);

            targetParam.Value = dynamicTarget.ObjectInstance;
        }

        private object MapOutputParameters(object sourceOutput, Type returnType)
        {
            var dynamicSourceOutput = new DynamicObject(sourceOutput);

            var dynamicTargetOutput = new DynamicObject(returnType);
            dynamicTargetOutput.CallConstructor();

            if (_route.OutputMapping.Mappings.Count == 1)
            {
                var mapping = _route.OutputMapping.Mappings.First();

                if (String.IsNullOrWhiteSpace(mapping.Source) && String.IsNullOrWhiteSpace(mapping.Target))
                    return sourceOutput;
                
                if (String.IsNullOrWhiteSpace(mapping.Target))
                    return dynamicSourceOutput.GetProperty(mapping.SourcePropertyPath);

                if (String.IsNullOrWhiteSpace(mapping.Source))
                {
                    dynamicTargetOutput.SetProperty(mapping.TargetPropertyPath, sourceOutput);
                    return dynamicTargetOutput.ObjectInstance;
                }
            }
         
            foreach (var mapping in _route.OutputMapping.Mappings)
                dynamicTargetOutput.SetProperty(mapping.TargetPropertyPath, dynamicSourceOutput.GetProperty(mapping.SourcePropertyPath));
            
            return dynamicTargetOutput.ObjectInstance;
        }
    }
}
