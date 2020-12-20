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
using Microsoft.EntityFrameworkCore;

namespace WorkShop.Xncf.Message.Areas.Admin.Pages.Messages
{
    public class EditModel : Senparc.Ncf.AreaBase.Admin.AdminXncfModulePageModelBase
    {
        private readonly MessagesService _messagesService;
        private readonly MessageDetailService messageDetailService;

        public EditModel(MessagesService messagesService,Lazy<XncfModuleService> xncfModuleService,MessageDetailService messageDetailService) : base(xncfModuleService)
        {
            CurrentMenu = "Messages";
            _messagesService = messagesService;
            this.messageDetailService = messageDetailService;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public MessagesDto MessagesDto { get; set; }

        /// <summary>
        /// Handler=Save
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSaveAsync([FromBody] MessagesExtendDto messagesExtendDto)
        {
            SenparcTrace.Log($"messagesExtendDto----{messagesExtendDto.ToJson()}");
            if (messagesExtendDto == null)
            {
                return Ok(false);
            }
            MessagesDto messagesDto = new MessagesDto
            {
                Id = messagesExtendDto.Id,
                Title = messagesExtendDto.Title,
                Content = messagesExtendDto.Content,
                Method = messagesExtendDto.Method,
                Type = messagesExtendDto.Type,
                Status = messagesExtendDto.Status
            };
            var strategy = _messagesService.BaseData.BaseDB.BaseDataContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                await _messagesService.BeginTransactionAsync(async () =>
                {
                    var messages = await _messagesService.CreateOrUpdateAsync(messagesDto);
                    await messageDetailService.InsertMessageUserAsync(messages.Id, messagesExtendDto.RelationUser);
                });
            });
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
