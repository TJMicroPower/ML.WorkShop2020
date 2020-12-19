using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Ncf.Service;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Ncf.Core.Models;
using Senparc.CO2NET.Trace;
using Senparc.Ncf.Utility;
using WorkShop.Xncf.Message.Models.DatabaseModel.Dto;
using WorkShop.Xncf.Message.Services;

namespace WorkShop.Xncf.Message.Areas.Admin.Pages.Messages
{
    public class IndexModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        private readonly MessagesService _messagesService;
        private readonly IServiceProvider _serviceProvider;
        public MessagesDto messagesDto { get; set; }
        public string Token { get; set; }
        public string UpFileUrl { get; set; }
        public string BaseUrl { get; set; }

        public IndexModel(Lazy<XncfModuleService> xncfModuleService, MessagesService messagesService, IServiceProvider serviceProvider) : base(xncfModuleService)
        {
            CurrentMenu = "Messages";
            this._messagesService = messagesService;
            this._serviceProvider = serviceProvider;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public PagedList<Models.DatabaseModel.Messages> Messages { get; set; }

        public Task OnGetAsync()
        {
            BaseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            UpFileUrl = $"{BaseUrl}/api/v1/common/upload";
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnGetMessagesAsync(string keyword, string orderField, int pageIndex, int pageSize)
        {
            var seh = new SenparcExpressionHelper<Models.DatabaseModel.Messages>();
            seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(keyword), _ => _.Title.Contains(keyword));
            var where = seh.BuildWhereExpression();
            var response = await _messagesService.GetObjectListAsync(pageIndex, pageSize, where, orderField);
            return Ok(new
                    {
                        response.TotalCount,
                        response.PageIndex,
                        List = response.Select(_ => new {
                            _.Id,
                            _.LastUpdateTime,
                            _.Remark,
                            _.Title,
                            _.Content,
                            _.Method,
                            _.SendTime,
                            _.Type,
                            _.Status,
                            _.AddTime
                        })
                    });
        }
    }
}
