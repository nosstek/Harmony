using System.Collections.Generic;
using HarmonyWebApp.Entities;

namespace HarmonyWebApp.Abstract
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> Departments { get; }
    }
}