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
    public class UserService : ServiceBase<User>
    {
        public UserService(IRepositoryBase<User> repo, IServiceProvider serviceProvider) : base(repo, serviceProvider)
        {
        }

        //TODO: 更多业务方法可以写到这里
        public async Task<IEnumerable<UserDto>> GetUserList(int PageIndex, int PageSize)
        {
            List<UserDto> selectListItems = null;
            List<User> user = (await GetFullListAsync(_ => true).ConfigureAwait(false)).OrderByDescending(_ => _.AddTime).ToList();
            selectListItems = this.Mapper.Map<List<UserDto>>(user);
            return selectListItems;
        }

        public async Task CreateOrUpdateAsync(UserDto dto)
        {
            User user;
            if (String.IsNullOrEmpty(dto.Id))
            {
                user = new User(dto);
            }
            else
            {
                user = await GetObjectAsync(_ => _.Id == dto.Id);
                user.Update(dto);
            }
            await SaveObjectAsync(user);
        }

    }

}
