using Senparc.CO2NET.Trace;
using Senparc.Ncf.Core.Enums;
using Senparc.Ncf.Repository;
using Senparc.Ncf.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkShop.Xncf.Message.Models.DatabaseModel;
using WorkShop.Xncf.Message.Models.DatabaseModel.Dto;
namespace WorkShop.Xncf.Message.Services
{
    public class MessagesService : ServiceBase<Messages>
    {
        public MessagesService(IRepositoryBase<Messages> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
        }

        //TODO: 更多业务方法可以写到这里
        public async Task<IEnumerable<MessagesDto>> GetMessagesList(int PageIndex, int PageSize)
        {
            List<MessagesDto> selectListItems = null;
            List<Messages> messages = (await GetFullListAsync(_ => true).ConfigureAwait(false)).OrderByDescending(_ => _.AddTime).ToList();
            selectListItems = this.Mapper.Map<List<MessagesDto>>(messages);
            return selectListItems;
        }

        public async Task<Messages> CreateOrUpdateAsync(MessagesDto dto)
        {
            Messages messages;
            if (String.IsNullOrEmpty(dto.Id))
            {
                messages = new Messages(dto);
            }
            else
            {
                messages = await GetObjectAsync(_ => _.Id == dto.Id);
                messages.Update(dto);
            }
            await SaveObjectAsync(messages);
            return messages;
        }

    }

}
