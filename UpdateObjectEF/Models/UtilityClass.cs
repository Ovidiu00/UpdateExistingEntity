﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public  void UpdateIfModified<T>(T existingEntity, T modifiedExistingEntity,string nameOfPrimaryKeyProperty) where T:class
        {
            var proprieties = GetProprietiesExceptPrimaryKey(existingEntity,nameOfPrimaryKeyProperty);


            foreach (var property in proprieties)
            {
                if (IsValueTypeOrString(property) == false)
                    continue;


                object valueExistingEmployee = GetValueForProperty(property, existingEntity);
                object valueExistingEmployeeModified = GetValueForProperty(property, modifiedExistingEntity);

                if (valueExistingEmployee == null)
                    context.Entry(existingEntity).Property(property.Name).CurrentValue = valueExistingEmployeeModified;       
                
              
                if (valueExistingEmployee != null && !valueExistingEmployee.Equals(valueExistingEmployeeModified))
                    context.Entry(existingEntity).Property(property.Name).CurrentValue = valueExistingEmployeeModified;
                
            }

            
        }

        PropertyInfo[] GetProprietiesExceptPrimaryKey<T>(T existingEntity,string nameOfPrimaryKeyProperty) where T:class
        {
            PropertyInfo[] propritiesOfExistingEntity = existingEntity.GetType().GetProperties();

            return propritiesOfExistingEntity.Where(property => property.Name != nameOfPrimaryKeyProperty).ToArray();
        }

        object GetValueForProperty<T>(PropertyInfo property , T entity) where T : class
        {
            return typeof(T).GetProperty(property.Name).GetValue(entity);
        }

        /// <summary>
        /// Not taking into consideration if the reference property Departemnt has changed since only the foreign key prop is relevant.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsValueTypeOrString(System.Reflection.PropertyInfo property)
        {
            return (property.PropertyType == typeof(string) || property.PropertyType.IsValueType);
        }
    }
}
