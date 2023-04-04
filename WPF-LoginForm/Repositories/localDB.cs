using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Repositories
{
    public class localDB
    {
        public localDB() { 
            Init();
        }

        private List<Sutdent> StudentsLs;

        private void Init()
        {
            StudentsLs = new List<Sutdent>();
            for(int i = 0; i < 10; i++)
            {
                StudentsLs.Add(new Sutdent()
                {
                    Id = i,
                    Name = $"Sample{i}"
                });
            }
        }

        public List<Sutdent> GetSutdents()
        {
            return StudentsLs;
        }

        public void AddStudnet(Sutdent stu)
        {
            StudentsLs.Add(stu);
        }

        public void DelStudent(int id)
        {
            var model = StudentsLs.FirstOrDefault(t => t.Id == id);
            if (model != null)
            {
                StudentsLs.Remove(model);
            }
        }

        public List<Sutdent> GetSutdentsByName(string name)
        {
            return StudentsLs.Where(q =>  q.Name.Contains(name)).ToList();
        }

        public Sutdent GetSutdentById(int id)
        {
            var model = StudentsLs.FirstOrDefault(t => t.Id == id);
            if(model != null)
            {
                return new Sutdent()
                {
                    Id = model.Id,
                    Name = model.Name,
                };
                
            }
            return null;

            // return StudentsLs.FirstOrDefault(q => q.Id == id); // 不修改时也会出现修改的动作
        }
    }
}
