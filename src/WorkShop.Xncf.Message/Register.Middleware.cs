using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Senparc.Ncf.XncfBase;
using System;
using System.IO;
using Senparc.Xncf.Admin.Utils;

namespace WorkShop.Xncf.Message
{
    public partial class Register : IXncfMiddleware
    {
        public IApplicationBuilder UseMiddleware(IApplicationBuilder app)
        {
            var staticResourceSetting = app.ApplicationServices.GetService<IOptionsMonitor<StaticResourceSetting>>();
            //静态资源允许跨域
            var path = Path.Combine(Directory.GetCurrentDirectory(), staticResourceSetting.CurrentValue.RootDir);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var fileOptions = new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = staticResourceSetting.CurrentValue.RequestPath,
            };
            app.UseStaticFiles(fileOptions);
            app.UseCors();

            //添加MVC模式支持
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            return app;
        }
    }
}
