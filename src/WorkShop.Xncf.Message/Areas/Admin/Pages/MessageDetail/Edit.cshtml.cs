using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Senparc.Ncf.Service;
using Senparc.CO2NET.Trace;
using Senparc.CO2NET.Extensions;
using WorkShop.Xncf.Message.Models.DatabaseModel.Dto;
using WorkShop.Xncf.Message.Services;

namespace WorkShop.Xncf.Message.Areas.Admin.Pages.MessageDetail
{
    public class EditModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        private readonly MessageDetailService _messageDetailService;
        public EditModel(MessageDetailService messageDetailService,Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
        {
            CurrentMenu = "MessageDetail";
            _messageDetailService = messageDetailService;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public MessageDetailDto MessageDetailDto { get; set; }

        /// <summary>
        /// Handler=Save
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSaveAsync([FromBody] MessageDetailDto messageDetailDto)
        {
            if (messageDetailDto == null)
            {
                return Ok(false);
            }
            await _messageDetailService.CreateOrUpdateAsync(messageDetailDto);
            return Ok(true);
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] string[] ids)
        {
            var entity = await _messageDetailService.GetFullListAsync(_ => ids.Contains(_.Id));
            await _messageDetailService.DeleteAllAsync(entity);
            IEnumerable<string> unDeleteIds = ids.Except(entity.Select(_ => _.Id));
            return Ok(unDeleteIds);
        }
    }
}
