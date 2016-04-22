using System.Collections.Generic;
using HarmonyWebApp.Entities;

namespace HarmonyWebApp.Abstract
{
    public interface IFieldOfStudyRepository
    {
        IEnumerable<FieldOfStudy> FieldsOfStudy { get; }
    }
}