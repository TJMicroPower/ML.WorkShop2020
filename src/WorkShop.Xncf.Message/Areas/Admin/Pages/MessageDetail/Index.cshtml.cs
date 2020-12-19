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

namespace WorkShop.Xncf.Message.Areas.Admin.Pages.MessageDetail
{
    public class IndexModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        private readonly MessageDetailService _messageDetailService;
        private readonly IServiceProvider _serviceProvider;
        public MessageDetailDto messageDetailDto { get; set; }
        public string Token { get; set; }
        public string UpFileUrl { get; set; }
        public string BaseUrl { get; set; }

        public IndexModel(Lazy<XncfModuleService> xncfModuleService, MessageDetailService messageDetailService, IServiceProvider serviceProvider) : base(xncfModuleService)
        {
            CurrentMenu = "MessageDetail";
            this._messageDetailService = messageDetailService;
            this._serviceProvider = serviceProvider;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        public PagedList<Models.DatabaseModel.MessageDetail> MessageDetail { get; set; }

        public Task OnGetAsync()
        {
            BaseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            UpFileUrl = $"{BaseUrl}/api/v1/common/upload";
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnGetMessageDetailAsync(string keyword, string orderField, int pageIndex, int pageSize)
        {
            var seh = new SenparcExpressionHelper<Models.DatabaseModel.MessageDetail>();
            //seh.ValueCompare.AndAlso(!string.IsNullOrEmpty(keyword), _ => _..Contains(keyword));
            var where = seh.BuildWhereExpression();
            var response = await _messageDetailService.GetObjectListAsync(pageIndex, pageSize, where, orderField);
            return Ok(new
                    {
                        response.TotalCount,
                        response.PageIndex,
                        List = response.Select(_ => new {
                            _.Id,
                            _.LastUpdateTime,
                            _.Remark,
                            _.MessageId,
                            _.UserId,
                            _.IsRead,
                            _.AddTime
                        })
                    });
        }
    }
}
