using System;
using System.Collections.Generic;
using HarmonyWebApp.Abstract;
using HarmonyWebApp.Models.Database;

namespace HarmonyWebApp.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        private HarmonyData context = new HarmonyData();

        public IEnumerable<Activity> Activities
        {
            get
            {
                return context.Activity;
            }
        }
    }
}