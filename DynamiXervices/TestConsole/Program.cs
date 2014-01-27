using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Taga.DynamicServices;
using Taga.DynamicServices.AsmxClient.Impl;
using Taga.DynamicServices.Routing.Mapping;
using Taga.DynamicServices.WCFClient.Impl;
using Taga.DynamicServices.Wsdl;
using TestConsole.Client;
using TestConsole.Routing;

namespace TestConsole
{
    class Program
    {
        static void Main()
        {
            try
            {
                for (var i = 0; i < 1; i++)
                    TestPayments();

                //ShowServiceInfos("TurkcellAxis");
                //TestRouting();
                //TestPropertyPath();
                //TestMappingSerialization();
                //TestPocoInit();

                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }

        private static void TestPayments(params string[] companies)
        {
            var wcfFactory = new DynamicWCFClientFactory();
            var asmxFactory = new DynamicWsClientFactory();

            foreach (var key in DynamicTestClient.ServiceWsdlUris.Keys.Where(key => !companies.Any() || companies.Contains(key)))
            {
                var client = DynamicTestClient.GetTestClient(key);
                client.TestPayment(asmxFactory);
                client.TestPayment(wcfFactory);
            }
        }

        private static void ShowServiceInfos(params string[] companies)
        {
            var wcfFactory = new DynamicWCFClientFactory();
            var asmxFactory = new DynamicWsClientFactory();

            foreach (var key in DynamicTestClient.ServiceWsdlUris.Keys.Where(key => !companies.Any() || companies.Contains(key)))
            {
                var client = DynamicTestClient.GetTestClient(key);
                client.ShowServiceInfo(asmxFactory);
                client.ShowServiceInfo(wcfFactory);
            }
        }

        //dynReq.SetProperty("SubscriberNumber", "subs1234");
        //dynReq.SetProperty("Amount", 12.45d);
        //dynReq.SetProperty("Currency", "TL");
        //dynReq.SetProperty("RequestId", "reqId1234");
        //dynReq.SetProperty("RequestDate", DateTime.Now);
        //dynReq.SetProperty("ReceiptNumber", "rec1234");
        //dynReq.SetProperty("BankCode", "bank1234");
        //dynReq.SetProperty("Message", "msg");
        //dynReq.SetProperty("InvoiceNumber", "invNum-123456");

        private static void TestPocoInit()
        {
            var init = new PocoInitializer<A>();
            var a = init.Init();
            
            var dyn = new DynamicObject(a);

            Console.WriteLine(dyn.GetProperty("B.C.X"));
            dyn.SetProperty("B.C.X", "Deneme");
            Console.WriteLine(dyn.GetProperty("B.C.X"));
        }

        private static void TestMappingSerialization()
        {
            var sb = new StringBuilder();

            var ser = new XmlSerializer<RouteMapping>();

            ser.Serialize(new RouteMapping
                {
                    Routes = new List<RouteInfo>
                        {
                            new RouteInfo
                                {
                                    Source = "src",
                                    Target = "target",
                                    InputMapping = new MappingList
                                        {
                                            Mappings = new List<ParameterMapping>
                                                {
                                                    new ParameterMapping
                                                        {
                                                            Source = "s",
                                                            Target = "t"
                                                        }
                                                }
                                        },
                                    OutputMapping = new MappingList
                                        { 
                                            Mappings = new List<ParameterMapping>
                                                {
                                                    new ParameterMapping
                                                        {
                                                            Source = "s",
                                                            Target = "t"
                                                        }
                                                }
                                        }
                                }
                        }
                }, sb);

            Console.WriteLine(sb.ToString());
            var rm = ser.Deserialize(@"C:\Mehmet\DynamicWebService\DynamicRouter\route-mapping.xml");
            sb.Clear();
            ser.Serialize(rm, sb);
            Console.WriteLine(sb.ToString());
        }

        private static void TestPropertyPath()
        {
            var o = new DynamicObject(typeof (A));
            o.CallConstructor();
            o.SetProperty("B1.A2.C1.B3.B2.C2.A3", new A(), true);
        }

        private static void TestRouting()
        {
            var srv = new SourceService();
            var response = srv.SourceMethod(new SourceRequest
                {
                    Password = "1234",
                    Time = DateTime.Now,
                    Username = "user"
                }, Guid.NewGuid().ToString(), 17);

            Console.WriteLine(response.Successful);
            Console.WriteLine(response.ResultCode);
            Console.WriteLine(response.Time);
            Console.WriteLine(response.Message);

            srv.Test();
            Console.WriteLine(srv.Sum(3, 5));
            Console.WriteLine(srv.Mult(3, 5));
            Console.WriteLine(srv.Subs(3, 5).Result);
        }

        private static void TestWsdl()
        {
            var wsdl = new SimpleWsdl
                {
                    Types = new WsdlTypes
                        {
                            Types = new List<WsdlType>
                                {
                                    new WsdlType
                                        {
                                            FullTypeName = "Taga.Test.User",
                                            Namespace = "http://dynamic.services.Taga.com.tr/",
                                            Properties = new List<WsdlProperty>
                                                {
                                                    new WsdlProperty {Name = "Id", FullTypeName = "System.Int32"},
                                                    new WsdlProperty {Name = "Name", FullTypeName = "System.String"},
                                                    new WsdlProperty {Name = "Surname", FullTypeName = "System.String"},
                                                    new WsdlProperty {Name = "DateOfBirth", FullTypeName = "System.DateTime"}
                                                }
                                        }
                                }
                        },
                    Services = new WsdlServices
                        {
                            Services = new List<WsdlService>
                                {
                                    new WsdlService
                                        {
                                            FullTypeName = "Taga.Test.UserService",
                                            Namespace = "http://dynamic.services.Taga.com.tr/",
                                            Methods = new List<WsdlMethod>
                                                {
                                                    new WsdlMethod
                                                        {
                                                            Name = "AddUser",
                                                            FullTypeName = "System.Int32",
                                                            Inputs = new List<WsdlInput>
                                                                {
                                                                    new WsdlInput
                                                                        {Name = "Name", FullTypeName = "System.String"},
                                                                    new WsdlInput
                                                                        {Name = "Surname", FullTypeName = "System.String"},
                                                                    new WsdlInput
                                                                        {Name = "DateOfBirth", FullTypeName = "System.DateTime"}
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };

            var serializer = new XmlSerializer<SimpleWsdl>();
            var sb = new StringBuilder();
            serializer.Serialize(wsdl, @"C:\Mehmet\DynamicWebService\DynamicWebService\Wsdl\simple_wsdl.xml");
            Console.WriteLine(sb);

            const string path = @"C:\Mehmet\DynamicWebService\DynamicWebService\Wsdl\wsdl.xml";
            wsdl = serializer.Deserialize(path);
            Console.WriteLine(wsdl.Services.Services.Count);

            WsdlTypeBuilder.BuildTypes(wsdl);
        }
    }

    class PocoInitializer<T> where T : class
    {
        private readonly PocoTree<T> _tree;

        public PocoInitializer()
        {
            _tree = new PocoTree<T>();
        }

        public T Init()
        {
            return Init(_tree.Root) as T;
        }

        private static object Init(PocoNode node)
        {
            var type = node.Type;

            if (IsSystemType(type))
                return type.GetDefaultValue();

            var obj = type.CreateInstance();

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.CanRead && pi.CanWrite);

            foreach (var propertyInfo in props)
            {
                var childNode = node.GetChild(propertyInfo.Name);
                type.SetPropertyValue(propertyInfo.Name, obj, childNode.IsCyclicNode
                                                                  ? childNode.Type.CreateInstance()
                                                                  : Init(childNode));
            }

            return obj;
        }

        private static bool IsSystemType(Type type)
        {
            return type.FullName != null && type.FullName.StartsWith("System.");
        }
    }

    class PocoTree<T>
    {
        private readonly PocoNode _root;

        public PocoTree()
        {
            _root = new PocoNode(typeof(T));
        }

        public PocoNode Root
        {
            get { return _root; }
        }

        public IEnumerable<PocoNode> Children
        {
            get { return _root.Children; }
        }
    }

    class PocoNode
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public PocoNode Parent { get; private set; }
        public bool IsCyclicNode { get; private set; }

        private List<PocoNode> _children;
        public IEnumerable<PocoNode> Children
        {
            get { return _children; }
        }

        public PocoNode(Type type)
        {
            Type = type;
            Name = type.Name;
            _children = new List<PocoNode>();

            AddChildren();
        }

        private PocoNode(PropertyInfo propertyInfo, PocoNode parent)
        {
            Type = propertyInfo.PropertyType;
            Name = propertyInfo.Name;
            _children = new List<PocoNode>();
            Parent = parent;
            CheckCycle();
        }

        public PocoNode GetChild(string name)
        {
            return _children.FirstOrDefault(child => child.Name == name);
        }

        private void AddChildren()
        {
            var pocoProps = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(pi => pi.CanRead && pi.CanWrite);

            foreach (var propertyInfo in pocoProps)
            {
                var childNode = new PocoNode(propertyInfo, this);

                if (!childNode.IsCyclicNode)
                    childNode.AddChildren();

                _children.Add(childNode);
            }
        }

        private void CheckCycle()
        {
            var tmpParent = Parent;

            while (tmpParent != null)
            {
                if (tmpParent.Type == Type)
                {
                    _children = tmpParent._children;
                    IsCyclicNode = true;
                    return;
                }
                tmpParent = tmpParent.Parent;
            }

            IsCyclicNode = false;
        }

        public override string ToString()
        {
            return Name + " : " + Type.Name + " [Cyclic: " + IsCyclicNode + "]";
        }
    }

    internal class A
    {
        public A A1 { get; set; }
        public B B1 { get; set; }
        public C C1 { get; set; }
    }

    class B
    {
        public A A2 { get; set; }
        public B B2 { get; set; }
        public C C2 { get; set; }
    }

    class C
    {
        public A A3 { get; set; }
        public B B3 { get; set; }
        public C C3 { get; set; }
    }
}
