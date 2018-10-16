using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ChakraCore.NET.Hosting;
using ChakraCore.NET;
using ChakraCore.NET.API;
using ChakraCore.NET.Promise;

namespace ChakraDemo
{
    public class Startup
    {
        public class EvConsole
        {
            public void Log(String s)
            {
                Console.WriteLine(s);
            }
        }

        public struct EvContext
        {
            public string Title { get; set; }
        }

        private Tuple<ChakraContext, JSValue> CreateChakraContext()
        {
            JavaScriptHosting hosting = JavaScriptHosting.Default; //get default host instantce
            JavaScriptHostingConfig config = new JavaScriptHostingConfig();
            config.AddModuleFolder("wwwroot/dist/js"); //set script folder

            var chakraContext = hosting.CreateContext(config);

            // Function Converter
            var valueServices = chakraContext.ServiceNode.GetService<IJSValueConverterService>();
            valueServices.RegisterFunctionConverter<string>();
            valueServices.RegisterProxyConverter<EvConsole>((binding, instance, serviceNode) =>
            {
                binding.SetMethod<string>("log", instance.Log);
            });

            // EvContext Converter
            valueServices.RegisterStructConverter<EvContext>((to, from) => { to.WriteProperty("title", from.Title); },
                (from) => new EvContext
                {
                    Title = from.ReadProperty<string>("title")
                });
            
            var console = new EvConsole();
            chakraContext.GlobalObject.WriteProperty("console", console);
            
            chakraContext.Enter();
            // https://ssr.vuejs.org/guide/non-node.html
            var obj = JavaScriptValue.CreateObject();
            obj.SetProperty(JavaScriptPropertyId.FromString("VUE_ENV"), JavaScriptValue.FromString("server"), false);
            obj.SetProperty(JavaScriptPropertyId.FromString("NODE_ENV"), JavaScriptValue.FromString("production"),
                false);

            var process = JavaScriptValue.CreateObject();
            process.SetProperty(JavaScriptPropertyId.FromString("env"), obj, false);

            chakraContext.GlobalObject.WriteProperty(JavaScriptPropertyId.FromString("process"), process);
            chakraContext.Leave();

            var appClass = chakraContext.ProjectModuleClass("entrypoint", "App", config.LoadModule);

            return Tuple.Create(chakraContext, appClass);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var (chakraContext, appClass) = CreateChakraContext();

            app.Run(async (context) =>
            {
                try
                {
                    var evCtx = new EvContext
                    {
                        Title = "hase"
                    };

                    var renderString = await appClass.CallFunctionAsync<EvContext, string>("render", evCtx);
                    await context.Response.WriteAsync(renderString);
                    return;
                }
                catch (PromiseRejectedException ex)
                {
                    Console.WriteLine($"Error {ex}");
                }

                await context.Response.WriteAsync("Failure!");
            });
        }
    }
}