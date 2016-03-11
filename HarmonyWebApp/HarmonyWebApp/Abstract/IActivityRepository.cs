using HarmonyWebApp.Models.Database;
using System.Collections.Generic;

namespace HarmonyWebApp.Abstract
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> Activities { get; }
    }
}