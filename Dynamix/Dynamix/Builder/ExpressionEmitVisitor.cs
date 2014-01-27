using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Dynamix.Metadata;
using Dynamix.Expressions;
using Dynamix.Utils;
using Method = Dynamix.Metadata.Method;

namespace Dynamix.Builder
{
    public sealed class ExpressionEmitVisitor
    {
        private readonly IDictionary<string, LocalBuilder> _localBuilders = new Dictionary<string, LocalBuilder>();

        private readonly ILGenerator _il;
        private readonly Method _method;

        internal ExpressionEmitVisitor(ILGenerator il, Method method)
        {
            _il = il;
            _method = method;
        }

        internal void Visit(EmptyMethodBodyExpression expression)
        {
            var returnType = expression.ReturnType.ResolveType();

            if (returnType != StaticType.Void.Type)
            {
                var returnValue = _il.DeclareLocal(returnType);
                _il.Emit(OpCodes.Ldloc, returnValue);
            }
            _il.Emit(OpCodes.Ret);
        }

        internal void Visit(EmptyMethodBodyDefaultReturnValue expression)
        {
            if (expression.ReturnType is TypeBase)
                (expression.ReturnType as TypeBase).Builder.CreateType();

            var returnType = expression.ReturnType.ResolveType();

            if (returnType != StaticType.Void.Type)
            {
                if (returnType.IsValueType)
                {
                    var res = _il.DeclareLocal(returnType);
                    _il.Emit(OpCodes.Ldloc, res);
                }
                else
                {
                    _il.Emit(OpCodes.Newobj, returnType.GetDefaultConstructor());
                }
            }
            _il.Emit(OpCodes.Ret);
        }

        internal void Visit(LoadArgExpression loadArgExpression)
        {
            _il.Emit(OpCodes.Ldarg, loadArgExpression.ArgIndex);
        }

        internal void Visit(AddExpression addExpression)
        {
            _il.Emit(OpCodes.Add);
        }

        internal void Visit(ReturnExpression returnExpression)
        {
            _il.Emit(OpCodes.Ret);
        }

        internal void Visit(DeclareLocalExpression declareExpression)
        {
            var localType = declareExpression.Type.ResolveType();
            var local = _il.DeclareLocal(localType);
            
            _localBuilders.Add(declareExpression.Name, local);
        }

        internal void Visit(SetLocalExpression setLocalExpression)
        {
            _il.Emit(OpCodes.Stloc, _localBuilders[setLocalExpression.LocalVariableName]);
        }

        internal void Visit(LoadLocalExpression loadLocalExpression)
        {
            _il.Emit(OpCodes.Ldloc, _localBuilders[loadLocalExpression.LocalVariableName]);
        }

        internal void Visit(LoadArgByNameExpression loadArgByNameExpression)
        {
            var parameter = _method.Parameters.First(p => p.Name == loadArgByNameExpression.ArgName);
            var parameterIndex = _method.Parameters.IndexOf(parameter);
            if (_method.Attribute != PolymorphicMemberAttribute.Static)
                parameterIndex += 1;
            _il.Emit(OpCodes.Ldarg, parameterIndex);
        }

        internal void Visit(LoadConstExpression loadConstExpression)
        {
            if (loadConstExpression.Type == typeof(int))
                _il.Emit(OpCodes.Ldc_I4, (int)loadConstExpression.Value);
            else if (loadConstExpression.Type == typeof(long))
                _il.Emit(OpCodes.Ldc_I8, (long)loadConstExpression.Value);
            else if (loadConstExpression.Type == typeof(float))
                _il.Emit(OpCodes.Ldc_R4, (float)loadConstExpression.Value);
            else if (loadConstExpression.Type == typeof(double))
                _il.Emit(OpCodes.Ldc_R8, (double)loadConstExpression.Value);
            else if (loadConstExpression.Type == typeof(string))
                _il.Emit(OpCodes.Ldstr, (string)loadConstExpression.Value);
        }

        internal void Visit(CallExpression callExpression)
        {
            var local = _localBuilders[callExpression.LocalVariableName];
            var methodInfo = local.LocalType.GetMethod(callExpression.MethodName);
            _il.Emit(OpCodes.Call, methodInfo);
        }
        
        internal void Visit(InstantiateExpression instantiateExpression)
        {
            var type = instantiateExpression.Type.ResolveType();
            if (type.HasDefaultConstrutor())
                _il.Emit(OpCodes.Newobj, type.GetDefaultConstructor());
        }
    }
}
