using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Ncf.Core.Models.DataBaseModel;
using Senparc.Ncf.XncfBase.Attributes;
using WorkShop.Xncf.Message.Models.DatabaseModel;

namespace WorkShop.Xncf.Message.Models
{
    [XncfAutoConfigurationMapping]
    public class Admin_MessagesConfigurationMapping : ConfigurationMappingWithIdBase<Messages, string>
    {
        public override void Configure(EntityTypeBuilder<Messages> builder)
        {
            
        }
    }
}