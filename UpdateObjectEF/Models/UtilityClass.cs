using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateObjectEF.Models
{
    public class UtilityClass
    {
        private readonly DemoDBContext context;

        public UtilityClass(DemoDBContext context)
        {
            this.context = context;
        }

        public  void UpdateIfModified<T>(T existingEntity, T modifiedExistingEntity)
        {
           
            foreach (var property in existingEntity.GetType().GetProperties())
            {
                object valueExistingEmployee = typeof(T).GetProperty(property.Name).GetValue(existingEntity);
                object valueExistingEmployeeModified = typeof(T).GetProperty(property.Name).GetValue(modifiedExistingEntity);

                if (valueExistingEmployee != null && !valueExistingEmployee.Equals(valueExistingEmployeeModified)
                    && IsNotReferenceProperty(property))
                {
                    if (context.Entry(existingEntity).State == EntityState.Detached)
                        context.Attach(existingEntity);

                    context.Entry(existingEntity).Property(property.Name).CurrentValue = valueExistingEmployeeModified;
                }
            }

            
        }


        /// <summary>
        /// Not taking into consideration if the reference property Departemnt has changed since only the foreign key prop is relevant.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsNotReferenceProperty(System.Reflection.PropertyInfo property)
        {
            return (property.PropertyType.IsPrimitive || property.PropertyType.IsEnum || property.PropertyType == typeof(string) || property.PropertyType.IsValueType);
        }
    }
}
