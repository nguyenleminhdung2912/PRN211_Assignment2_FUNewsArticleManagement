using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class TagRepository : ITagRepository
    {
        public void DeleteTag(Tag tag)
                => TagDAO.Delete(tag);

        public Tag GetTagById(int id)
                => TagDAO.GetTagById(id);

        public List<Tag> GetTags()
                => TagDAO.GetTags();

        public void SaveTag(Tag tag)
                => TagDAO.SaveTag(tag);

        public void UpdateTag(Tag tag)
                => TagDAO.UpdateTag(tag);

    }
}
