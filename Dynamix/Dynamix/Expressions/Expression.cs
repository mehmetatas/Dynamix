using System;
using Dynamix.Metadata;

namespace Dynamix.Expressions
{
    public static class Expression
    {
        public static IExpression EmptyMethodBody(ITypeInfo type)
        {
            return new EmptyMethodBodyExpression(type);
        }

        public static IExpression EmptyMethodBodyDefaultReturnValue(ITypeInfo typeInfo)
        {
            return new EmptyMethodBodyDefaultReturnValue(typeInfo);
        }

        public static IExpression LoadArg(int index)
        {
            return new LoadArgExpression(index);
        }

        public static IExpression Return()
        {
            return new ReturnExpression();
        }

        public static IExpression Add()
        {
            return new AddExpression();
        }

        public static IExpression DeclareLocal(ITypeInfo type, string name)
        {
            return new DeclareLocalExpression(type, name);
        }

        public static IExpression NewLocal(ITypeInfo type, string name)
        {
            return new NewLocalExpression(type, name);
        }

        public static IExpression SetLocal(string name)
        {
            return new SetLocalExpression(name);
        }

        public static IExpression LoadLocal(string name)
        {
            return new LoadLocalExpression(name);
        }

        public static IExpression AddArgs(int argIndex1, int argIndex2)
        {
            return new AddArgsExpression(argIndex1, argIndex2);
        }

        public static IExpression SetNewLocal(ITypeInfo type, string name)
        {
            return new SetNewLocalExpression(type, name);
        }

        public static IExpression ReturnLocal(string name)
        {
            return new ReturnLocalExpression(name);
        }

        public static IExpression AddArgs(string arg1, string arg2)
        {
            return new AddArgsExpression(arg1, arg2);
        }

        public static IExpression LoadArg(string argName)
        {
            return new LoadArgByNameExpression(argName);
        }

        public static IExpression CallMethod(string callExpression, object[] args = null, Type[] argTypes = null)
        {
            var localAndMethod = callExpression.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            var local = localAndMethod[0];
            var method = localAndMethod[1];
            if (argTypes == null && args != null)
            {
                argTypes = new Type[args.Length];
                for (var i = 0; i < args.Length; i++)
                    argTypes[i] = args[i].GetType();
            }
            return new MethodCallExpression(local, method, args, argTypes);
        }

        public static IExpression LoadConst(object val, Type type)
        {
            return new LoadConstExpression(val, type);
        }

        public static IExpression Call(string local, string method)
        {
            return new CallExpression(local, method);
        }

        public static IExpression Instantiate(ITypeInfo type)
        {
            return new InstantiateExpression(type);
        }
    }
}
