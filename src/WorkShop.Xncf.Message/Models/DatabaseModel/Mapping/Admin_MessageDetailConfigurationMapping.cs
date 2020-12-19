using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Ncf.Core.Models.DataBaseModel;
using Senparc.Ncf.XncfBase.Attributes;
using WorkShop.Xncf.Message.Models.DatabaseModel;

namespace WorkShop.Xncf.Message.Models
{
    [XncfAutoConfigurationMapping]
    public class Admin_MessageDetailConfigurationMapping : ConfigurationMappingWithIdBase<MessageDetail, string>
    {
        public override void Configure(EntityTypeBuilder<MessageDetail> builder)
        {
            
        }
    }
}