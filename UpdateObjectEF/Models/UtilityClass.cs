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

        /// <summary>
        /// 
        /// Modified existing entity's proprieites and changes are reflected when SaveChanges is called.Long story short : Overrides the existing entity with the new entity given as parameter.
        /// Only applies to Value type and string proprieties.
        
        /// </summary>
        /// <param name="existingEntity">Entity that was retrieved from DB (actual values live in DB).If it is not tracked by the context already, it will be tracked after calling this method</param>
        /// <param name="modifiedExistingEntity">Entity recieved in http PUT endpoint or which the client knows exist in DB</param>
        public  void UpdateIfModified<T>(T existingEntity, T modifiedExistingEntity)
        {
            foreach (var property in existingEntity.GetType().GetProperties())
            {
                object valueExistingEmployee = typeof(T).GetProperty(property.Name).GetValue(existingEntity);
                object valueExistingEmployeeModified = typeof(T).GetProperty(property.Name).GetValue(modifiedExistingEntity);

                if ( !valueExistingEmployee.Equals(valueExistingEmployeeModified)
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
            return (property.PropertyType == typeof(string) || property.PropertyType.IsValueType);
        }
    }
}
