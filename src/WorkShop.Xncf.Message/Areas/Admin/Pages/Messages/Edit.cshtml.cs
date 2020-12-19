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

namespace WorkShop.Xncf.Message.Areas.Admin.Pages.Messages
{
    public class EditModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        private readonly MessagesService _messagesService;
        public EditModel(MessagesService messagesService,Lazy<XncfModuleService> xncfModuleService) : base(xncfModuleService)
        {
            CurrentMenu = "Messages";
            _messagesService = messagesService;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public MessagesDto MessagesDto { get; set; }

        /// <summary>
        /// Handler=Save
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSaveAsync([FromBody] MessagesDto messagesDto)
        {
            if (messagesDto == null)
            {
                return Ok(false);
            }
            await _messagesService.CreateOrUpdateAsync(messagesDto);
            return Ok(true);
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] string[] ids)
        {
            var entity = await _messagesService.GetFullListAsync(_ => ids.Contains(_.Id));
            await _messagesService.DeleteAllAsync(entity);
            IEnumerable<string> unDeleteIds = ids.Except(entity.Select(_ => _.Id));
            return Ok(unDeleteIds);
        }
    }
}
