using AutoMapper;
using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.CacheModels;
using DatabaseAbstractions.Models.DatabaseModels;
using Extensions.Models;
using System.Reflection;

namespace DatabaseAbstractions.Models.Communication
{

    /// <summary>
    /// Конвертер сущностей отпечатка базы данных в сущности базы данных и обратно.
    /// </summary>
    /// <typeparam name="T">Тип конвертера.</typeparam>
    public class CacheConverter
    {
        /// <summary>
        /// Поставщик конвертера.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор по умолчанию конвертера сущностей отпечатка базы данных в сущности базы данных и наоборот.
        /// </summary>
        public CacheConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Конвертация сущности отпечатка базы данных в сущность базы данных.
        /// </summary>
        /// <param name="entity">Сущность отпечатка базы данных.</param>
        /// <returns>Сущность базы данных.</returns>
        public BaseEntity ConvertToEntity(CacheEntity entity)
        {
            var methods = _mapper.GetType().GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "Map" &&
                    m.IsGenericMethod &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1);

            if (methods == null)
            {
                throw new NullDataException();
            }

            var methodAttribute = entity.GetType().GetCustomAttribute<AssignedTypeAttribute>();

            if (methodAttribute == null)
            {
                throw new NullDataException();
            }

            var generic = methods.MakeGenericMethod(methodAttribute.Type);

            var result = (BaseEntity?)generic.Invoke(_mapper, [entity]);

            return result ?? throw new ConvertationException();
        }

        /// <summary>
        /// Конвертация сущности базы данных в сущность отпечатка базы данных.
        /// </summary>
        /// <param name="entity">Сущность базы данных.</param>
        /// <returns>Сущность отпечатка базы данных.</returns>
        /// <exception cref="ConvertationException">Если конвертация завершилась с ошибкой.</exception>
        public CacheEntity ConvertToFingerprintEntity(BaseEntity entity)
        {
            var methods = _mapper.GetType().GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "Map" &&
                    m.IsGenericMethod &&
                    m.GetGenericArguments().Length == 1 &&
                    m.GetParameters().Length == 1) ?? throw new NullDataException();

            var methodAttribute = entity.GetType().GetCustomAttribute<AssignedTypeAttribute>() ?? throw new NullDataException();

            var generic = methods.MakeGenericMethod(methodAttribute.Type);

            var result = (CacheEntity?)generic.Invoke(_mapper, [entity]);

            return result ?? throw new ConvertationException();
        }
    }
}
