using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface ITagRepository
    {
        void SaveTag(Tag tag);

        void DeleteTag(Tag tag);

        void UpdateTag(Tag tag);

        List<Tag> GetTags();

        Tag GetTagById(int id);
    }
}
