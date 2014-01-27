using System;
using Taga.DynamicServices;

namespace TestConsole.Routing
{
    public class SourceService
    {
        public SourceResponse SourceMethod(SourceRequest request, string transactionId, int x)
        {
            var handler = new MockDynamicInvocationHandler();
            return handler.Handle(new MockDynamicInvocationContext
            {
                RouteKey = "Target",
                MethodName = "SourceMethod",
                ServiceName = GetType().FullName,
                ReturnType = typeof(SourceResponse),
                InputParameters = new[]
                {
                    new Parameter
                    {
                        Name = "request", 
                        Value = request
                    },
                    new Parameter
                    {
                        Name = "transactionId",
                        Value = transactionId
                    },
                    new Parameter
                    {
                        Name = "x",
                        Value = x
                    }
                }
            }) as SourceResponse;
        }

        public void Test()
        {
            var handler = new MockDynamicInvocationHandler();
            handler.Handle(new MockDynamicInvocationContext
            {
                RouteKey = "Target",
                MethodName = "Test",
                ServiceName = GetType().FullName,
                ReturnType = typeof(void),
                InputParameters = new Parameter[0]
            });
        }

        public int Sum(int x, int y)
        {
            var handler = new MockDynamicInvocationHandler();
            return (int)handler.Handle(new MockDynamicInvocationContext
            {
                RouteKey = "Target",
                MethodName = "Sum",
                ServiceName = GetType().FullName,
                ReturnType = typeof(int),
                InputParameters = new[]
                {
                    new Parameter
                    {
                        Name = "x", 
                        Value = x
                    },
                    new Parameter
                    {
                        Name = "y",
                        Value = y
                    } 
                }
            });
        }

        public double Mult(int x, int y)
        {
            var handler = new MockDynamicInvocationHandler();
            return (double)handler.Handle(new MockDynamicInvocationContext
            {
                RouteKey = "Target",
                MethodName = "Mult",
                ServiceName = GetType().FullName,
                ReturnType = typeof(int),
                InputParameters = new[]
                {
                    new Parameter
                    {
                        Name = "x", 
                        Value = x
                    },
                    new Parameter
                    {
                        Name = "y",
                        Value = y
                    } 
                }
            });
        }

        public SubtractionResult Subs(int x, int y)
        {
            var handler = new MockDynamicInvocationHandler();
            return handler.Handle(new MockDynamicInvocationContext
            {
                RouteKey = "Target",
                MethodName = "Subs",
                ServiceName = GetType().FullName,
                ReturnType = typeof(SubtractionResult),
                InputParameters = new[]
                {
                    new Parameter
                    {
                        Name = "x", 
                        Value = x
                    },
                    new Parameter
                    {
                        Name = "y",
                        Value = y
                    } 
                }
            }) as SubtractionResult;
        }
    }

    class TargetService
    {
        public TargetResponse TargetMethod(TargeteRequest req, string usr, string pwd, int a)
        {
            return new TargetResponse
            {
                DateTime = req.DateTime,
                Info = usr + " : " + pwd,
                IsSuccessful = true,
                Status = 12
            };
        }

        public void Deneme()
        {
            Console.WriteLine("TargetService.Deneme");
        }

        public int Topla(int a, int b)
        {
            return a + b;
        }

        public CarpmaSonucu Carp(int a, int b)
        {
            return new CarpmaSonucu { Sonuc = a * b };
        }

        public int Cikar(int a, int b)
        {
            return a - b;
        }
    }

    public class SubtractionResult
    {
        public int Result { get; set; }
    }

    public class CarpmaSonucu
    {
        public double Sonuc { get; set; }
    }

    public class SourceRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Time { get; set; }
    }

    public class SourceResponse
    {
        public bool Successful { get; set; }
        public int ResultCode { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
    }

    public class TargeteRequest
    {
        public string TransId { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class TargetResponse
    {
        public bool IsSuccessful { get; set; }
        public int Status { get; set; }
        public DateTime DateTime { get; set; }
        public string Info { get; set; }
    }
}
