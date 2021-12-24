using System;
using System.Linq;
using System.Reflection;

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
        public void UpdateIfModified<T>(T existingEntity, T modifiedExistingEntity, string nameOfPrimaryKeyProperty) where T : class
        {
            var proprieties = GetProprietiesExceptPrimaryKey(typeof(T), nameOfPrimaryKeyProperty);


            foreach (var property in proprieties)
            {
                if (IsValueTypeOrString(property) == false)
                    continue;

                object valueExistingEmployee = GetValueForProperty(property, existingEntity);
                object valueExistingEmployeeModified = GetValueForProperty(property, modifiedExistingEntity);

                if (valueExistingEmployee == null || (valueExistingEmployee != null && !valueExistingEmployee.Equals(valueExistingEmployeeModified)))
                    SetValueForPropertyOfEntity(valueExistingEmployeeModified, property, existingEntity);
            }
        }

        PropertyInfo[] GetProprietiesExceptPrimaryKey(Type type, string nameOfPrimaryKeyProperty)
        {
            PropertyInfo[] propritiesOfExistingEntity = type.GetProperties();

            return propritiesOfExistingEntity.Where(property => property.Name != nameOfPrimaryKeyProperty).ToArray();
        }

        object GetValueForProperty<T>(PropertyInfo property, T entity) where T : class
        {
            return typeof(T).GetProperty(property.Name).GetValue(entity);
        }
        void SetValueForPropertyOfEntity<T>(object value, PropertyInfo property, T entity) where T : class
        {
            context.Entry(entity).Property(property.Name).CurrentValue = value;
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
