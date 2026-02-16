using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class StudentUtility
    {
        Student student = new Student();
        private SortedDictionary<double, List<Student>> _data = new SortedDictionary<double, List<Student>>(Comparer<double>.Create((a,b) => b.CompareTo(a)));

        public void AddEntity(double gpa, Student entity)
        {
            if(gpa<0 || gpa > 10)
            {
                throw new InvalidGPAException("gpa must be greater than 0 and less than 10");
            }


            if (_data.Values.SelectMany(s => s).Any(s => s.Id == entity.Id))
            {
                throw new DublicateStudentException("student already exists");
            }

            if (!_data.ContainsKey(gpa))
            {
                _data[gpa] = new List<Student>();
            }

            _data[gpa].Add(entity);
        }

        public void UpdateEntity(string studentid,double newgpa)
        {
            if (newgpa < 0 || newgpa > 10) {
                throw new InvalidGPAException("gpa must be between 0 and 10");
            }

            Student studenttoupdate = null;
            double oldgpa = 0;


            foreach(var item in _data) {
                var student = item.Value.FirstOrDefault(s => s.Id == studentid);
                if (student != null) {

                    studenttoupdate = student;
                    oldgpa = item.Key;
                    break;
                }
            }


            if (studenttoupdate == null) {
                throw new StudentNotFoundException("Student not found");
            }
        }

       

        public void RemoveEntity(string studentId) 
        {
            // TODO: Remove entity logic
            foreach (var entry in _data)
            {
                var student = entry.Value.FirstOrDefault(s => s.Id == studentId);
                if (student != null)
                {
                    entry.Value.Remove(student);
                    if (entry.Value.Count == 0)
                        _data.Remove(entry.Key);
                    return;
                }
            }

            throw new StudentNotFoundException("Student not found.");
        }

        public IEnumerable<Student> GetAll()
        {
            // TODO: Return sorted entities
            return _data.Values.SelectMany(s => s);
        }
    }
}

